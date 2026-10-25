#nullable enable
using MORT.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.Service.Overlay
{
    //OCR/번역 대기와 무관하게 실행한다. 처리 작업 하나와 최신 완성 결과 하나만 보관한다.
    internal sealed class OverlayAdaptiveRefreshService : IDisposable
    {
        internal const int TargetFramesPerSecond = 60;
        private readonly object _gate = new();
        private readonly CancellationTokenSource _stop = new();
        private readonly SemaphoreSlim _wake = new(0, 1);
        private readonly OverlayTextRestorationService _restoration = new();
        private readonly Dictionary<int, CaptureBuffer> _buffers = new();
        private readonly Func<OverlayAdaptiveScene, OverlayAdaptiveArea, OverlayCaptureFrame?, byte[]?>? _captureOverride;
        private OverlayAdaptiveScene? _scene;
        private OverlayAdaptiveOutput? _latest;
        private Task? _worker;
        private Action? _requestPaint;
        private bool _disposed;
        internal OverlayAdaptiveOutput? Latest => Volatile.Read(ref _latest);
        internal long CompletedFrames;

        public OverlayAdaptiveRefreshService() { }

        //화면을 건드리지 않는 프레임 재생/수명 검증에서도 실제 스케줄러와 복원을 사용한다.
        internal OverlayAdaptiveRefreshService(Func<OverlayAdaptiveScene, OverlayAdaptiveArea, OverlayCaptureFrame?, byte[]?> capture)
        {
            _captureOverride = capture;
        }

        internal void SetScene(OverlayAdaptiveScene? scene, Action? requestPaint)
        {
            lock(_gate)
            {
                if(_disposed || ReferenceEquals(_scene, scene)) return;
                if(!ReferenceEquals(_scene?.Capture, scene?.Capture))
                {
                    _scene?.Capture?.SetAdaptiveCapture(false);
                }
                scene?.Capture?.SetAdaptiveCapture(true, Math.Clamp(scene.TargetFps, 1, 120));
                _requestPaint = requestPaint;
                Volatile.Write(ref _latest, null);
                Volatile.Write(ref _scene, scene);
                if(scene != null) _worker ??= Task.Factory.StartNew(Run, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                if(_wake.CurrentCount == 0) _wake.Release();
            }
        }

        private void Run()
        {
            var token = _stop.Token;
            using var clock = new OverlayFrameClock(token);
            OverlayCaptureFrame? lastCapture = null;
            OverlayAdaptiveScene? lastScene = null;
            long lastError = 0;
            try
            {
                while(!token.IsCancellationRequested)
                {
                    var scene = Volatile.Read(ref _scene);
                    if(scene == null)
                    {
                        ClearBuffers();
                        lastScene = null;
                        lastCapture = null;
                        _wake.Wait(token);
                        continue;
                    }
                    long start = Stopwatch.GetTimestamp();
                    try
                    {
                        var capture = scene.Capture?.LatestAdaptiveFrame;
                        //WGC의 같은 최신 프레임은 재처리하지 않는다. OCR 소비 플래그와 분리한다.
                        if(scene.Capture == null || !ReferenceEquals(scene, lastScene) || !ReferenceEquals(capture, lastCapture))
                        {
                            var frames = new Dictionary<int, OverlayBlurFrame>();
                            foreach(var area in scene.Areas)
                            {
                                if(token.IsCancellationRequested || !ReferenceEquals(scene, Volatile.Read(ref _scene))) break;
                                byte[]? pixels = _captureOverride != null ? _captureOverride(scene, area, capture) : Capture(scene, area, capture);
                                if(pixels == null) continue;
                                var patch = _restoration.CreatePatch(pixels, area.ScreenRect.Width, area.ScreenRect.Height, 4, area.Words, scene.Level);
                                if(patch != null) frames[area.Index] = patch;
                            }
                            Action? notify = null;
                            lock(_gate)
                            {
                                if(!_disposed && ReferenceEquals(scene, _scene))
                                {
                                    Volatile.Write(ref _latest, new OverlayAdaptiveOutput(scene, frames));
                                    Interlocked.Increment(ref CompletedFrames);
                                    notify = _requestPaint;
                                }
                            }
                            notify?.Invoke();
                            lastScene = scene;
                            lastCapture = capture;
                            foreach(int index in _buffers.Keys.ToArray())
                                if(!scene.Areas.Any(area => area.Index == index)) { _buffers[index].Dispose(); _buffers.Remove(index); }
                        }
                    }
                    catch(Exception ex) when(ex is not OperationCanceledException)
                    {
                        //실패한 캡처는 재사용하지 않는다. 정지/설정 변경 시 UI를 기다리지 않는다.
                        Action? notify = null;
                        lock(_gate)
                        {
                            if(!_disposed && ReferenceEquals(scene, _scene))
                            {
                                Volatile.Write(ref _latest, new OverlayAdaptiveOutput(scene, new()));
                                notify = _requestPaint;
                            }
                        }
                        notify?.Invoke();
                        if(Environment.TickCount64 - lastError > 1000)
                        {
                            lastError = Environment.TickCount64;
                            Util.ShowLog($"Adaptive background refresh failed: {ex.Message}");
                        }
                        clock.Wait(100);
                    }
                    //사용자가 지정한 FPS 목표. 늦었을 때 따라잡기 루프/프레임 큐를 만들지 않고 다음 최신 화면으로 진행한다.
                    double remaining = 1000.0 / Math.Clamp(scene.TargetFps, 1, 120) - Stopwatch.GetElapsedTime(start).TotalMilliseconds;
                    if(remaining > 0) clock.Wait(remaining);
                }
            }
            catch(OperationCanceledException) when(token.IsCancellationRequested) { }
            finally { ClearBuffers(); }
        }

        private byte[]? Capture(OverlayAdaptiveScene scene, OverlayAdaptiveArea area, OverlayCaptureFrame? frame)
        {
            if(!_buffers.TryGetValue(area.Index, out var buffer) || buffer.Image.Size != area.ScreenRect.Size)
            {
                buffer?.Dispose();
                buffer = new CaptureBuffer(area.ScreenRect.Size);
                _buffers[area.Index] = buffer;
            }
            if(scene.Capture != null)
            {
                if(frame == null || !frame.ScreenBounds.Contains(area.ScreenRect)) return null;
                int x = area.ScreenRect.X - frame.ScreenBounds.X, y = area.ScreenRect.Y - frame.ScreenBounds.Y;
                for(int row = 0; row < area.ScreenRect.Height; row++)
                    Buffer.BlockCopy(frame.Pixels, ((y + row) * frame.Width + x) * 4,
                        buffer.Pixels, row * area.ScreenRect.Width * 4, area.ScreenRect.Width * 4);
                return buffer.Pixels;
            }
            Rectangle visible = Rectangle.Intersect(area.ScreenRect, scene.DesktopBounds);
            if(visible.Width <= 0 || visible.Height <= 0) return null;
            using(var graphics = Graphics.FromImage(buffer.Image))
            {
                if(visible != area.ScreenRect) graphics.Clear(Color.Black);
                graphics.CopyFromScreen(visible.Location,
                    new Point(visible.X - area.ScreenRect.X, visible.Y - area.ScreenRect.Y), visible.Size, CopyPixelOperation.SourceCopy);
            }
            var bits = buffer.Image.LockBits(new Rectangle(Point.Empty, buffer.Image.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                for(int row = 0; row < buffer.Image.Height; row++)
                    Marshal.Copy(bits.Scan0 + row * bits.Stride, buffer.Pixels, row * buffer.Image.Width * 4, buffer.Image.Width * 4);
            }
            finally { buffer.Image.UnlockBits(bits); }
            //GDI의 예약 바이트는 알파가 아니다.
            for(int i = 3; i < buffer.Pixels.Length; i += 4) buffer.Pixels[i] = 255;
            return buffer.Pixels;
        }

        private void ClearBuffers()
        {
            foreach(var buffer in _buffers.Values) buffer.Dispose();
            _buffers.Clear();
        }

        public void Dispose()
        {
            Task? worker;
            lock(_gate)
            {
                if(_disposed) return;
                _disposed = true;
                _requestPaint = null;
                _scene?.Capture?.SetAdaptiveCapture(false);
                Volatile.Write(ref _scene, null);
                Volatile.Write(ref _latest, null);
                _stop.Cancel();
                worker = _worker;
            }
            //UI는 worker를 Join하지 않는다. 작업이 끝난 뒤 대기 리소스를 해제한다.
            if(worker == null) { _stop.Dispose(); _wake.Dispose(); }
            else _ = worker.ContinueWith(_ => { _stop.Dispose(); _wake.Dispose(); }, TaskScheduler.Default);
        }

        private sealed class CaptureBuffer : IDisposable
        {
            internal readonly Bitmap Image;
            internal readonly byte[] Pixels;
            internal CaptureBuffer(Size size)
            {
                Image = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
                Pixels = new byte[checked(size.Width * size.Height * 4)];
            }
            public void Dispose() => Image.Dispose();
        }
    }
}
