//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using SharpDX.Direct3D11;
using System;

using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;
using SharpDX.DXGI;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using System.Runtime.InteropServices;
using Windows.Graphics.Imaging;
using MORT.OcrApi.WindowOcr;
using System.Security.Cryptography.Xml;
using WinRT;
using System.Windows.Media.Media3D;
using System.Linq;

namespace MORT.ScreenCapture
{
    public enum BorderStateType
    {
        None,
        Enable,
        Disable,
        ForceEnable
    }
    public class ScreenCaptureProcesser : IDisposable
    {
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIInspectable)]
        [Guid("f2cdd966-22ae-5ea1-9596-3a289344c3be")]
        public interface IBorderRequired
        {
            bool IsBorderRequired { get; set; }
        }
        public BorderStateType BorderStateType { get; private set; }

        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int attribute, ref Rect rectangle, int size);

        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;

        public static bool TryGetCaptureBounds(IntPtr hwnd, out Rect rectangle)
        {
            rectangle = new Rect();
            if(hwnd == IntPtr.Zero)
            {
                return false;
            }

            int result = DwmGetWindowAttribute(hwnd, DWMWA_EXTENDED_FRAME_BOUNDS, ref rectangle, Marshal.SizeOf<Rect>());
            if(result == 0 && rectangle.Right > rectangle.Left && rectangle.Bottom > rectangle.Top)
            {
                return true;
            }

            rectangle = new Rect();
            return GetWindowRect(hwnd, ref rectangle)
                && rectangle.Right > rectangle.Left
                && rectangle.Bottom > rectangle.Top;
        }


        private GraphicsCaptureItem item;
        private Direct3D11CaptureFramePool framePool;
        private GraphicsCaptureSession session;
        private SizeInt32 lastSize;

        private IDirect3DDevice _device;
        private SharpDX.Direct3D11.Device _d3dDevice;
        private SharpDX.DXGI.SwapChain1 swapChain;

        public byte[] array = default(byte[]);
        public int lastX;
        public int lastY;

        public int lastPositionX;
        public int lastPositionY;


        IntPtr hWnd = IntPtr.Zero;

        public ScreenCaptureProcesser(SharpDX.Direct3D11.Device d3dDevice, IDirect3DDevice device, GraphicsCaptureItem i, IntPtr hWnd)
        {
            this.hWnd = hWnd;
            item = i;

            this._device = device;
            this._d3dDevice = d3dDevice;

            //this.d3dDevice = Direct3D11Helper.CreateSharpDXDevice(device);


            var dxgiFactory = new SharpDX.DXGI.Factory2();
            var description = new SharpDX.DXGI.SwapChainDescription1()
            {

                Width = item.Size.Width,
                Height = item.Size.Height,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Stereo = false,
                SampleDescription = new SharpDX.DXGI.SampleDescription()
                {
                    Count = 1,
                    Quality = 0
                },
                Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                BufferCount = 2,
                Scaling = SharpDX.DXGI.Scaling.Stretch,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
                AlphaMode = SharpDX.DXGI.AlphaMode.Premultiplied,
                Flags = SharpDX.DXGI.SwapChainFlags.None
            };
            swapChain = new SharpDX.DXGI.SwapChain1(dxgiFactory, _d3dDevice, ref description);

            try
            {
                framePool = Direct3D11CaptureFramePool.Create(_device, DirectXPixelFormat.B8G8R8A8UIntNormalized, 2, i.Size);
                session = framePool.CreateCaptureSession(i);
                session.IsCursorCaptureEnabled = false;

                lastSize = i.Size;

                framePool.FrameArrived += OnFrameArrived;
            }
            catch(Exception ex) { }

        }

        public void SetRequireBoard(bool enableBoard)
        {
            BorderStateType = enableBoard ? BorderStateType.Enable : BorderStateType.Disable;
            try
            {


                session.IsBorderRequired = enableBoard;
                /*
                var pUnk = Marshal.GetIUnknownForObject(session);
                if (pUnk != IntPtr.Zero)
                {
                    var borderRequired = Marshal.GetObjectForIUnknown(pUnk) as IBorderRequired;

                    if (borderRequired != null)
                    {
                        borderRequired.IsBorderRequired = enableBoard;
                    }
                    else
                    {
                        BorderStateType = BorderStateType.ForceEnable;
                    }
                }
                */

            }
            catch
            {
                BorderStateType = BorderStateType.ForceEnable;
            }
        }

        private bool _disposed;

        public void Dispose()
        {
            _disposed = true;
            isStartCapture = false;
            _deferTimer?.Dispose();
            System.Threading.Interlocked.Exchange(ref _pendingFrame, null)?.Dispose();
            session?.Dispose();
            framePool?.Dispose();
            swapChain?.Dispose();
        }

        public void StartCapture()
        {
            session.StartCapture();
        }

        internal volatile bool AdaptiveCapture;
        internal volatile int AdaptiveTargetFps = 60;
        private int _lastAdaptiveFps;
        private long _nextAdaptiveCopy;
        private MORT.Model.OverlayCaptureFrame? _adaptiveFrame;
        internal MORT.Model.OverlayCaptureFrame? LatestAdaptiveFrame => System.Threading.Volatile.Read(ref _adaptiveFrame);

        //OCR 스레드와 프레임 처리 스레드가 함께 읽고 쓴다. 최적화 빌드에서도 최신 값을 읽도록 volatile로 둔다.
        public volatile bool isWait = false;
        public volatile bool isStartCapture = false;
        public volatile bool isDataSuccess = false;

        //프레임 도착 순번. OCR 결과가 요청 시점보다 오래된 화면인지 시간 대신 순번으로 판단한다.
        //복사 시간·OCR 확인 간격은 PC마다 다르므로 특정 시간 안에 처리된다고 가정하지 않는다.
        private long _arrivedSequence;
        //이번 OCR 주기에서 넘겨야 하는 최소 순번. -1은 아직 요청이 없다는 뜻이다.
        private long _requiredSequence = -1;
        //마지막으로 복사를 마친 프레임의 순번. _adaptiveFrame을 게시한 뒤에 기록한다.
        private long _copiedSequence;

        public bool AvailableGetData
        {
            get
            {
                long required = System.Threading.Volatile.Read(ref _requiredSequence);
                long copied = System.Threading.Volatile.Read(ref _copiedSequence);
                //요청 시점에 도착해 있던 프레임 이상을 복사했으면 넘긴다.
                //요청 뒤에 도착한 프레임은 기준을 올리지 않으므로 화면이 계속 바뀌어도 결과가 확정된다.
                if(required >= 0 && copied < required)
                {
                    return false;
                }

                return LatestAdaptiveFrame != null;
            }
        }

        //이번 OCR 주기를 시작한다. 기준 순번은 첫 요청에서 정한다.
        internal void PrepareDataCapture()
        {
            isDataSuccess = false;
            isWait = false;
            isStartCapture = false;
            System.Threading.Volatile.Write(ref _requiredSequence, -1);
        }

        public void GetLastData(ref int x, ref int y, ref int positionX, ref int positionY, ref byte[] dataArray)
        {
            isDataSuccess = false;
            //복사 스레드가 다음 프레임을 쓰는 중이어도 크기·위치·픽셀이 한 프레임에서 나오도록 게시된 묶음을 읽는다.
            var frame = LatestAdaptiveFrame;
            if(frame != null)
            {
                x = frame.Width;
                y = frame.Height;
                positionX = frame.ScreenBounds.X;
                positionY = frame.ScreenBounds.Y;
                dataArray = frame.Pixels;
            }
            _remainbackupFrameCount = 5;
        }


        public int TestIndex = 0;
        private int _remainbackupFrameCount = 5;
        private int _autoCaptureFrameCounter = 0;

        //아직 복사하지 않은 프레임 중 가장 최신 한 장. 복사 조건이 아니어도 버리지 않고 들고 있다가
        //조건이 되면(진행 중 복사 완료, 적응형 주기 도래, OCR 요청) 처리한다.
        //화면이 바뀔 때만 프레임을 보내는 앱은 마지막 프레임 뒤로 다음 프레임이 오지 않기 때문이다.
        //최신 한 장만 들고 있으므로 버퍼 2개짜리 풀에서 새 프레임을 받을 자리는 항상 남는다.
        private PendingCaptureFrame? _pendingFrame;
        //프레임 처리는 FrameArrived 를 받는 스레드(UI)에서 한다. 타이머/OCR 요청도 이 컨텍스트로 돌려보낸다.
        private readonly System.Threading.SynchronizationContext? _frameContext = System.Threading.SynchronizationContext.Current;
        private System.Threading.Timer? _deferTimer;
        private int _pumping;
        private int _pumpRequested;
        private int _pumpPosted;
        private bool _autoCaptureRequested;

        //프레임과 도착 순번을 함께 보관한다.
        private sealed class PendingCaptureFrame : IDisposable
        {
            internal PendingCaptureFrame(Direct3D11CaptureFrame frame, long sequence)
            {
                Frame = frame;
                Sequence = sequence;
            }

            internal Direct3D11CaptureFrame Frame { get; }
            internal long Sequence { get; }

            public void Dispose()
            {
                Frame.Dispose();
            }
        }

        //OCR이 새 화면을 요청한다. 들고 있는 최신 프레임이 있으면 바로 복사한다.
        internal void RequestDataCapture()
        {
            isStartCapture = true;
            //주기의 첫 요청 때만 기준을 정한다. OCR이 반복 확인할 때마다 기준을 올리면
            //프레임을 계속 보내는 앱에서 복사가 끝나기 전에 기준이 앞서 나가 결과를 받지 못한다.
            System.Threading.Interlocked.CompareExchange(ref _requiredSequence,
                System.Threading.Volatile.Read(ref _arrivedSequence), -1);
            RequestPump();
        }

        private void RequestPump()
        {
            if(_disposed)
            {
                return;
            }

            var context = _frameContext;
            if(context != null)
            {
                //처리되지 않은 게시가 있으면 더 쌓지 않는다. 느린 PC에서 UI 메시지 큐가 밀리지 않게 한다.
                if(System.Threading.Interlocked.Exchange(ref _pumpPosted, 1) != 0)
                {
                    return;
                }

                context.Post(_ =>
                {
                    System.Threading.Volatile.Write(ref _pumpPosted, 0);
                    PumpFrames();
                }, null);
            }
            else
            {
                PumpFrames();
            }
        }

        private void OnFrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            if(_disposed)
            {
                return;
            }

            Direct3D11CaptureFrame? frame;
            try
            {
                frame = sender.TryGetNextFrame();
            }
            catch(Exception ex)
            {
                if(!_disposed)
                {
                    Util.ShowLog($"Screen capture frame failed: {ex.Message}");
                }
                return;
            }

            if(frame == null)
            {
                return;
            }

            long sequence = System.Threading.Interlocked.Increment(ref _arrivedSequence);
            //Console.WriteLine($"arrive : {TestIndex} isStartCapture {isStartCapture} isDataSuccess {isDataSuccess}");
            if(!isStartCapture)
            {
                _autoCaptureFrameCounter++;
                if(_autoCaptureFrameCounter >= 10)
                {
                    _autoCaptureRequested = true;
                    _autoCaptureFrameCounter = 0;
                }
            }
            else
            {
                _autoCaptureFrameCounter = 0;
            }

            //복사 중이어도 프레임은 반드시 꺼낸다. 꺼내지 않고 돌아가면 버퍼 2개짜리 풀이 막히고,
            //화면이 바뀔 때만 프레임을 보내는 앱에서는 바뀌기 전 화면이 OCR에 계속 전달된다.
            //새 프레임이 우선이며 들고 있던 이전 프레임은 버린다.
            System.Threading.Interlocked.Exchange(ref _pendingFrame, new PendingCaptureFrame(frame, sequence))?.Dispose();
            if(_disposed)
            {
                System.Threading.Interlocked.Exchange(ref _pendingFrame, null)?.Dispose();
                return;
            }

            PumpFrames();
        }

        private async void PumpFrames()
        {
            System.Threading.Volatile.Write(ref _pumpRequested, 1);
            while(!_disposed && System.Threading.Volatile.Read(ref _pumpRequested) == 1)
            {
                //이미 처리 중이면 그 루프가 요청 플래그를 보고 이어서 처리한다.
                if(System.Threading.Interlocked.CompareExchange(ref _pumping, 1, 0) != 0)
                {
                    return;
                }

                try
                {
                    await PumpPendingFramesAsync();
                }
                catch(Exception ex)
                {
                    if(!_disposed)
                    {
                        Util.ShowLog($"Screen capture frame failed: {ex.Message}");
                    }
                }
                finally
                {
                    System.Threading.Volatile.Write(ref _pumping, 0);
                }
            }

            if(_disposed)
            {
                System.Threading.Interlocked.Exchange(ref _pendingFrame, null)?.Dispose();
            }
        }

        private async System.Threading.Tasks.Task PumpPendingFramesAsync()
        {
            while(!_disposed)
            {
                System.Threading.Volatile.Write(ref _pumpRequested, 0);
                if(System.Threading.Volatile.Read(ref _pendingFrame) == null)
                {
                    return;
                }

                if(!TryBeginCopy(out int retryMs))
                {
                    //적응형 주기 전이면 그 시각에 다시 처리한다. 그 외에는 다음 프레임이나 OCR 요청을 기다린다.
                    if(retryMs > 0)
                    {
                        ScheduleDeferredPump(retryMs);
                    }
                    return;
                }

                var frame = System.Threading.Interlocked.Exchange(ref _pendingFrame, null);
                if(frame == null)
                {
                    return;
                }

                await CopyFrameAsync(frame);
            }
        }

        //지금 복사할지 정한다. 조건은 기존과 같다: OCR 요청, 읽은 뒤 백업 프레임, 10프레임마다 자동, 적응형 주기.
        private bool TryBeginCopy(out int retryMs)
        {
            retryMs = 0;
            bool adaptive = AdaptiveCapture;
            int adaptiveFps = Math.Clamp(AdaptiveTargetFps, 1, 120);
            if(adaptiveFps != _lastAdaptiveFps)
            {
                _nextAdaptiveCopy = 0;
                _lastAdaptiveFps = adaptiveFps;
            }

            long frequency = System.Diagnostics.Stopwatch.Frequency;
            long copyTick = System.Diagnostics.Stopwatch.GetTimestamp();
            bool adaptiveDue = adaptive && copyTick + frequency / 1000 >= _nextAdaptiveCopy;
            bool autoCapture = _autoCaptureRequested && !adaptive;
            //요청 당시 복사 중이던 프레임이 기준보다 오래됐으면 OCR의 다음 확인을 기다리지 않고 이어서 복사한다.
            bool requiredPending = System.Threading.Volatile.Read(ref _copiedSequence)
                < System.Threading.Volatile.Read(ref _requiredSequence);
            if(!(isStartCapture || requiredPending || _remainbackupFrameCount >= 0 || autoCapture || adaptiveDue))
            {
                if(adaptive)
                {
                    long wait = _nextAdaptiveCopy - copyTick;
                    retryMs = (int)Math.Clamp(wait * 1000 / frequency + 1, 1, 1000);
                }
                return false;
            }

            _autoCaptureRequested = false;
            _remainbackupFrameCount--;
            if(_remainbackupFrameCount < -1)
            {
                _remainbackupFrameCount = -1;
            }

            if(adaptive)
            {
                _nextAdaptiveCopy += frequency / adaptiveFps;
                if(_nextAdaptiveCopy <= copyTick)
                {
                    _nextAdaptiveCopy = copyTick + frequency / adaptiveFps;
                }
            }
            return true;
        }

        private void ScheduleDeferredPump(int delayMs)
        {
            if(_disposed)
            {
                return;
            }

            var timer = _deferTimer;
            if(timer == null)
            {
                timer = new System.Threading.Timer(_ => RequestPump(), null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                _deferTimer = timer;
            }

            try
            {
                timer.Change(delayMs, System.Threading.Timeout.Infinite);
            }
            catch(ObjectDisposedException)
            {
                //정지 중에는 예약하지 않는다.
            }
        }

        private async System.Threading.Tasks.Task CopyFrameAsync(PendingCaptureFrame capturedFrame)
        {
            bool ownsCopy = false;
            try
            {
                var newSize = false;

                using(var frame = capturedFrame.Frame)
                {
                    if(!_disposed)
                    {
                        isWait = true;
                        ownsCopy = true;
                        if(frame.ContentSize.Width != lastSize.Width || frame.ContentSize.Height != lastSize.Height)
                        {
                            // The thing we have been capturing has changed size.
                            // We need to resize the swap chain first, then blit the pixels.
                            // After we do that, retire the frame and then recreate the frame pool.
                            newSize = true;
                            lastSize = frame.ContentSize;

                            swapChain.ResizeBuffers(
                                2,
                                lastSize.Width,
                                lastSize.Height,
                                SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                                SharpDX.DXGI.SwapChainFlags.None);

                        }
                        using(var readbackTiming = MORT.Service.Overlay.OverlayPerformanceProfiler.Measure(
                            MORT.Service.Overlay.OverlayPerformanceStage.WgcReadback, frame.ContentSize.Width, frame.ContentSize.Height))
                        using(var backBuffer = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0))
                        {
                            using(var softwareBitmap = await SoftwareBitmap.CreateCopyFromSurfaceAsync(frame.Surface))
                            {
                                //GPU 복사 완료를 비동기로 기다린다. 대기 중 닫힌 캡처 소스에는 접근하지 않는다.
                                if(_disposed) return;
                                using(var buffer = softwareBitmap.LockBuffer(BitmapBufferAccessMode.ReadWrite))
                                {
                                    using(var reference = buffer.CreateReference())
                                    {
                                        unsafe
                                        {
                                            byte* data;
                                            uint capacity;
                                            var desc = buffer.GetPlaneDescription(0);
                                            var memoryBuffer = reference.As<IMemoryBufferByteAccess>();

                                            memoryBuffer.GetBuffer(out data, out capacity);

                                            byte[] arr = new byte[softwareBitmap.PixelWidth * softwareBitmap.PixelHeight * 4];
                                            for(int row = 0; row < softwareBitmap.PixelHeight; row++)
                                                Marshal.Copy((IntPtr)data + desc.StartIndex + row * desc.Stride,
                                                    arr, row * softwareBitmap.PixelWidth * 4, softwareBitmap.PixelWidth * 4);

                                            array = arr;
                                            lastX = softwareBitmap.PixelWidth;
                                            lastY = softwareBitmap.PixelHeight;

                                            try
                                            {
                                                if(TryGetCaptureBounds(hWnd, out Rect rect))
                                                {
                                                    lastPositionX = rect.Left;
                                                    lastPositionY = rect.Top;
                                                }
                                                TestIndex++;
                                                //Console.WriteLine("Capture : " + _remainbackupFrameCount + "/" + DateTime.Now.ToString());
                                            }
                                            catch
                                            {

                                            }


                                            System.Threading.Volatile.Write(ref _adaptiveFrame, new MORT.Model.OverlayCaptureFrame(
                                                arr, lastX, lastY, new System.Drawing.Rectangle(lastPositionX, lastPositionY, lastX, lastY)));
                                            //프레임을 먼저 게시하고 순번을 기록한다. 순번을 본 OCR은 그 이상의 프레임을 읽는다.
                                            System.Threading.Volatile.Write(ref _copiedSequence, capturedFrame.Sequence);
                                            isDataSuccess = true;

                                        }
                                    }
                                }

                            }

                        }

                        isStartCapture = false;

                        swapChain.Present(0, SharpDX.DXGI.PresentFlags.None);

                        if(newSize)
                        {
                            framePool.Recreate(
                                _device,
                                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                                2,
                                lastSize);
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                if(!_disposed) Util.ShowLog($"Screen capture frame failed: {ex.Message}");
            }
            finally
            {
                //실패한 복사도 대기 플래그를 풀어 다음 프레임에서 재시도한다.
                if(ownsCopy) isWait = false;
            }
        }



        private void CopyBitmap(IDirect3DSurface surface, SharpDX.Direct3D11.Texture2D screenTexture2D, int Width, int Height)
        {
            //isWait = true;
            try
            {                //screenTexture 카피대상
                //screenTexture2D 화면

                var textureDesc = new Texture2DDescription
                {
                    CpuAccessFlags = CpuAccessFlags.Read,
                    BindFlags = BindFlags.None,
                    Format = Format.B8G8R8A8_UNorm,
                    Width = Width,
                    Height = Height,
                    OptionFlags = ResourceOptionFlags.None,
                    MipLevels = 1,
                    ArraySize = 1,
                    SampleDescription = { Count = 1, Quality = 0 },
                    Usage = ResourceUsage.Staging
                };


                using(var screenTexture = new Texture2D(_d3dDevice, textureDesc))
                {
                    _d3dDevice.ImmediateContext.CopyResource(screenTexture2D, screenTexture);

                    var mapSource = _d3dDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);
                    //var bitmap = new System.Drawing.Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                    var boundsRect = new System.Drawing.Rectangle(0, 0, Width, Height);
                    ;

                    //var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    var sourcePtr = mapSource.DataPointer;
                    //var destPtr = mapDest.Scan0;


                    byte[] managedArray = new byte[Width * Height * 4];


                    for(int y = 0; y < Height; y++)
                    {
                        System.Runtime.InteropServices.Marshal.Copy(sourcePtr, managedArray, y * Width * 4, Width * 4);
                        sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                    }

                    //bitmap.UnlockBits(mapDest);
                    _d3dDevice.ImmediateContext.UnmapSubresource(screenTexture, 0);

                    //array = ConvertBitmapToByteArray(bitmap);
                    array = managedArray;
                    lastX = Width;
                    lastY = Height;


                    if(hWnd != IntPtr.Zero)
                    {
                        if(TryGetCaptureBounds(hWnd, out Rect rect))
                        {
                            lastPositionX = rect.Left;
                            lastPositionY = rect.Top;

                            Console.WriteLine(rect.Left + " / " + rect.Right + " / " + rect.Top + " / " + rect.Bottom);
                        }
                    }

                    screenTexture.Dispose();
                    //bitmap.Save("./result.png");
                    //bitmap.Dispose();

                    isDataSuccess = true;
                }

            }
            catch(Exception e)
            {
                Console.WriteLine("Error - " + e.ToString());
            }
            isWait = false;

        }
    }
}
