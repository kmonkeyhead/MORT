#nullable enable
using MORT.Model;
using MORT.Service.Overlay.Restoration;
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
        private readonly Dictionary<int, CaptureBuffer> _buffers = new();
        private readonly Dictionary<int, RestoredInput> _restoredInputs = new();
        private readonly Func<OverlayAdaptiveScene, OverlayAdaptiveArea, OverlayCaptureFrame?, byte[]?>? _captureOverride;
        private OverlayAdaptiveScene? _scene;
        private OverlayAdaptiveOutput? _latest;
        private Task? _worker;
        private Action? _requestPaint;
        private bool _disposed;
        internal OverlayAdaptiveOutput.ReadLease AcquireLatest()
        {
            lock(_gate) return _latest?.Acquire() ?? default;
        }
        //호출자는 _gate를 소유한다. UI 읽기 대여가 있으면 이전 배열은 아직 반환되지 않는다.
        private void ReplaceLatest(OverlayAdaptiveOutput? output)
        {
            var previous = _latest;
            _latest = output;
            previous?.Release();
        }
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
                //마스크/영역 갱신 중에는 마지막 완성 배경을 유지한다. 수명이나 캡처 소스가 바뀌면 해제한다.
                if(scene == null || _scene == null || !ReferenceEquals(_scene.Capture, scene.Capture)
                    || _scene.DesktopBounds != scene.DesktopBounds)
                    ReplaceLatest(null);
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
                        //OCR 결과가 바뀌어도 캡처 영역별 작업 버퍼는 유지한다.
                        var capture = scene.Capture?.LatestAdaptiveFrame;
                        //WGC의 같은 최신 프레임은 재처리하지 않는다. OCR 소비 플래그와 분리한다.
                        if(scene.Capture == null || !ReferenceEquals(scene, lastScene) || !ReferenceEquals(capture, lastCapture))
                        {
                            var frames = new Dictionary<int, OverlayBlurFrame>();
                            bool retryOutput = false;
                            foreach(var area in scene.Areas)
                            {
                                if(token.IsCancellationRequested || !ReferenceEquals(scene, Volatile.Read(ref _scene))) break;
                                byte[]? pixels;
                                using(OverlayPerformanceProfiler.Measure(OverlayPerformanceStage.CaptureArea, area.ScreenRect.Width, area.ScreenRect.Height))
                                    pixels = _captureOverride != null ? _captureOverride(scene, area, capture) : Capture(scene, area, capture);
                                if(pixels == null) continue;
                                _restoredInputs.TryGetValue(area.Index, out var previous);
                                if(previous == null)
                                {
                                    previous = new RestoredInput();
                                    _restoredInputs[area.Index] = previous;
                                }
                                if(!ReferenceEquals(previous.Scene, scene))
                                    previous.Configure(area, scene);
                                bool unchanged;
                                using(OverlayPerformanceProfiler.Measure(OverlayPerformanceStage.ComparePixels))
                                    unchanged = previous.Matches(pixels, area.ScreenRect.Width);
                                var patch = previous.Patch;
                                if(!unchanged)
                                {
                                    using(OverlayPerformanceProfiler.Measure(OverlayPerformanceStage.RestoreBackground, area.ScreenRect.Width, area.ScreenRect.Height))
                                        patch = previous.Restoration.CreatePooledPatch(pixels, area.ScreenRect.Width, area.ScreenRect.Height, 4, area.Words, previous.OutputPool);
                                    retryOutput |= patch == null;
                                }
                                if(patch != null && !ReferenceEquals(patch, previous.Patch))
                                {
                                    using var saveTiming = OverlayPerformanceProfiler.Measure(OverlayPerformanceStage.SaveComparison);
                                    try { previous.Save(pixels, area.ScreenRect.Width, patch); }
                                    catch { patch.ReleasePixels(); throw; }
                                    _restoredInputs[area.Index] = previous;
                                }
                                if(patch != null) frames[area.Index] = patch;
                            }
                            Action? notify = null;
                            lock(_gate)
                            {
                                //일부 영역의 캡처가 준비되지 않았으면 이전 완성 결과 전체를 유지한다.
                                if(!_disposed && ReferenceEquals(scene, _scene)
                                    && frames.Count > 0 && frames.Count == scene.Areas.Length)
                                {
                                    var latest = _latest;
                                    //정지 화면은 복원뿐 아니라 layered window 재그리기도 생략한다.
                                    if(latest == null || !ReferenceEquals(latest.Scene, scene)
                                        || frames.Any(frame => !latest.Frames.TryGetValue(frame.Key, out var old)
                                            || !ReferenceEquals(frame.Value, old)))
                                    {
                                        ReplaceLatest(new OverlayAdaptiveOutput(scene, frames));
                                        Interlocked.Increment(ref CompletedFrames);
                                        notify = _requestPaint;
                                    }
                                }
                            }
                            notify?.Invoke();
                            //출력 배열을 모두 읽고 있었으면 같은 WGC 프레임도 반환 후 다시 시도한다.
                            //정지 화면에서 다음 캡처가 올 때까지 갱신이 막히지 않도록 한다.
                            lastScene = retryOutput ? null : scene;
                            lastCapture = capture;
                            foreach(int index in _restoredInputs.Keys.ToArray())
                                if(!scene.Areas.Any(area => area.Index == index)) { _restoredInputs[index].Dispose(); _restoredInputs.Remove(index); }
                            foreach(int index in _buffers.Keys.ToArray())
                                if(!scene.Areas.Any(area => area.Index == index)) { _buffers[index].Dispose(); _buffers.Remove(index); }
                        }
                    }
                    catch(Exception ex) when(ex is not OperationCanceledException)
                    {
                        //실패한 입력을 게시하지 않는다. 마지막 완성 배경은 다음 성공까지 그대로 둔다.
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
            foreach(var input in _restoredInputs.Values) input.Dispose();
            _restoredInputs.Clear();
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
                ReplaceLatest(null);
                _stop.Cancel();
                worker = _worker;
            }
            //UI는 worker를 Join하지 않는다. 작업이 끝난 뒤 대기 리소스를 해제한다.
            if(worker == null) { _stop.Dispose(); _wake.Dispose(); }
            else _ = worker.ContinueWith(_ => { _stop.Dispose(); _wake.Dispose(); }, TaskScheduler.Default);
        }

        private sealed class RestoredInput : IDisposable
        {
            internal readonly OverlayTextRestorationService Restoration = new();
            internal readonly OverlayFrameBufferPool OutputPool = new();
            private Rectangle[] _regions = Array.Empty<Rectangle>();
            private byte[] _pixels = Array.Empty<byte>();
            private Size _captureSize;
            private bool _valid, _packed;
            internal OverlayAdaptiveScene? Scene { get; private set; }
            internal OverlayBlurFrame Patch { get; private set; } = null!;

            internal void Configure(OverlayAdaptiveArea area, OverlayAdaptiveScene scene)
            {
                Restoration.UseHighQuality = scene.UseHighQuality;
                Restoration.BackgroundStrength = scene.BackgroundStrength;
                _regions = OverlayTextRestorationService.GetComparisonRegions(area.ScreenRect.Width, area.ScreenRect.Height, area.Words, scene.UseHighQuality, scene.BackgroundStrength);
                long sampleBytes = 0;
                foreach(var rect in _regions) sampleBytes += (long)rect.Width * rect.Height * 4;
                int fullBytes = checked(area.ScreenRect.Width * area.ScreenRect.Height * 4);
                _packed = sampleBytes < fullBytes;
                int length = (int)Math.Min(sampleBytes, fullBytes);
                bool resized = _captureSize != area.ScreenRect.Size;
                if(resized) OutputPool.Reset();
                if(resized || _pixels.Length < length)
                    _pixels = new byte[resized ? length : (int)Math.Min(fullBytes, Math.Max(length, (long)_pixels.Length * 2))];
                _captureSize = area.ScreenRect.Size;
                _valid = false;
                Scene = scene;
            }

            internal bool Matches(byte[] source, int width)
            {
                if(!_valid) return false;
                int offset = 0;
                foreach(var rect in _regions)
                {
                    int rowBytes = rect.Width * 4;
                    for(int row = 0; row < rect.Height; row++)
                    {
                        int sourceOffset = ((rect.Y + row) * width + rect.X) * 4;
                        if(!source.AsSpan(sourceOffset, rowBytes)
                            .SequenceEqual(_pixels.AsSpan(_packed ? offset : sourceOffset, rowBytes))) return false;
                        if(_packed) offset += rowBytes;
                    }
                }
                return true;
            }

            internal void Save(byte[] source, int width, OverlayBlurFrame patch)
            {
                //비교 픽셀은 하나의 연속 버퍼에 모은다. 글자마다 큰 배열을 다시 만들지 않는다.
                int offset = 0;
                foreach(var rect in _regions)
                {
                    int rowBytes = rect.Width * 4;
                    for(int row = 0; row < rect.Height; row++)
                    {
                        int sourceOffset = ((rect.Y + row) * width + rect.X) * 4;
                        source.AsSpan(sourceOffset, rowBytes)
                            .CopyTo(_pixels.AsSpan(_packed ? offset : sourceOffset, rowBytes));
                        if(_packed) offset += rowBytes;
                    }
                }
                var previous = Patch;
                Patch = patch;
                _valid = true;
                previous?.ReleasePixels();
            }
            public void Dispose()
            {
                Patch?.ReleasePixels();
                Patch = null!;
                OutputPool.Dispose();
                Restoration.Dispose();
            }
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
