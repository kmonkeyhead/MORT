using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Windows.System;
using Windows.UI.Composition;
using Windows.UI.Composition.Desktop;
using WinRT;

namespace MORT.Service.Overlay
{
    //배경 픽셀을 보관하지 않는다. DWM이 현재 창 뒤 화면을 합성할 때마다 처리한다.
    internal sealed class OverlayBackdropWindow : NativeWindow, IDisposable
    {
        [ThreadStatic] private static DispatcherQueueController _dispatcher;
        private Compositor _compositor;
        private DesktopWindowTarget _target;
        private ContainerVisual _root;
        private CompositionBackdropBrush _backdrop;
        private readonly List<CompositionObject> _regionObjects = new();
        private Rectangle[] _regions = Array.Empty<Rectangle>();
        private Size _size;
        private bool _failed;
        private bool? _excludedFromCapture;

        public bool TryInitialize()
        {
            if(_failed) return false;
            if(Handle != IntPtr.Zero) return true;
            try
            {
                if(!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000))
                    throw new NotSupportedException("Window backdrop blur requires Windows 11 or later.");
                if(DispatcherQueue.GetForCurrentThread() == null)
                {
                    var options = new DispatcherQueueOptions { Size = Marshal.SizeOf<DispatcherQueueOptions>(), ThreadType = 2, ApartmentType = 2 };
                    Marshal.ThrowExceptionForHR(CreateDispatcherQueueController(options, out var pointer));
                    try { _dispatcher = MarshalInterface<DispatcherQueueController>.FromAbi(pointer); }
                    finally { Marshal.Release(pointer); }
                }
                CreateHandle(new CreateParams
                {
                    Caption = "MORT live window backdrop",
                    Style = unchecked((int)0x88000000),
                    //Layered + Transparent는 다른 프로세스의 창에도 입력을 통과시킨다.
                    ExStyle = 0x08000000 | 0x80 | 0x20 | 0x00200000 | 0x00080000,
                    Width = 1, Height = 1
                });
                if(!SetLayeredWindowAttributes(Handle, 0, 255, 2)) throw new Win32Exception();
                int enabled = 1;
                Marshal.ThrowExceptionForHR(DwmSetWindowAttribute(Handle, 17, ref enabled, sizeof(int)));
                _compositor = new Compositor();
                var interop = _compositor.As<ICompositorDesktopInterop>();
                interop.CreateDesktopWindowTarget(Handle, false, out var target);
                try { _target = MarshalInterface<DesktopWindowTarget>.FromAbi(target); }
                finally { Marshal.Release(target); }
                _root = _compositor.CreateContainerVisual();
                _target.Root = _root;
                _backdrop = _compositor.CreateHostBackdropBrush();
                return true;
            }
            catch(Exception ex) { Disable(); Util.ShowLog($"Overlay window backdrop unavailable: {ex.Message}"); return false; }
        }

        public void Update(IntPtr textWindow, Rectangle bounds, IReadOnlyList<Rectangle> rectangles, bool excludeFromCapture, int blurLevel = 5)
        {
            if(rectangles.Count == 0 || bounds.Width <= 0 || bounds.Height <= 0) { Hide(); return; }
            if(!TryInitialize()) return;
            SetCaptureExclusion(excludeFromCapture);
            if(_failed) return;
            UpdateLevel(blurLevel);
            UpdateRegions(bounds.Size, rectangles);
            if(_regions.Length == 0) { Hide(); return; }
            if(!SetWindowPos(Handle, textWindow, bounds.X, bounds.Y, bounds.Width, bounds.Height, 0x10 | 0x40))
                throw new Win32Exception();
        }

        private void UpdateLevel(int blurLevel)
        {
            //HostBackdrop의 흐림 반경은 Windows가 결정한다. 수준은 합성 결과의 적용 비율이다.
            float opacity = Math.Clamp(blurLevel, 1, 5) / 5f;
            if(_root.Opacity != opacity) _root.Opacity = opacity;
        }

        private void UpdateRegions(Size size, IReadOnlyList<Rectangle> rectangles)
        {
            Rectangle viewport = new Rectangle(Point.Empty, size);
            var regions = rectangles.Where(rect => rect.Width > 0 && rect.Height > 0)
                .Select(rect => Rectangle.Intersect(viewport, rect))
                .Where(rect => rect.Width > 0 && rect.Height > 0).Distinct().ToArray();
            if(_size == size && _regions.SequenceEqual(regions)) return;
            _size = size;
            _regions = regions;
            _root.Size = new Vector2(size.Width, size.Height);
            int objectIndex = 0;
            foreach(var rect in regions)
            {
                float feather = OverlayBlurEdge.Feather(rect.Size);
                float radius = OverlayBlurEdge.Radius(rect.Size);
                //좁은 페더를 소수의 겹친 윤곽으로 표현한다.
                //누적 알파를 보정하여 단순 중첩 때문에 가장자리가 짙어지지 않게 한다.
                int steps = Math.Clamp((int)Math.Ceiling(feather), 2, 6);
                float previous = 0;
                for(int i = 0; i <= steps; i++)
                {
                    float inset = feather * i / steps;
                    float width = rect.Width - inset * 2, height = rect.Height - inset * 2;
                    if(width <= 0 || height <= 0) break;
                    float opacity = OverlayBlurEdge.Smooth(Math.Min(1, (i + 0.5f) / steps));
                    CompositionRoundedRectangleGeometry geometry;
                    SpriteVisual visual;
                    if(objectIndex < _regionObjects.Count)
                    {
                        geometry = (CompositionRoundedRectangleGeometry)_regionObjects[objectIndex];
                        visual = (SpriteVisual)_regionObjects[objectIndex + 2];
                    }
                    else
                    {
                        geometry = _compositor.CreateRoundedRectangleGeometry();
                        var clip = _compositor.CreateGeometricClip(geometry);
                        visual = _compositor.CreateSpriteVisual();
                        visual.Clip = clip;
                        visual.Brush = _backdrop;
                        _root.Children.InsertAtTop(visual);
                        _regionObjects.Add(geometry);
                        _regionObjects.Add(clip);
                        _regionObjects.Add(visual);
                    }
                    //움직임마다 전체 COM 객체를 해제/생성하지 않고 바뀐 속성만 갱신한다.
                    var layerSize = new Vector2(width, height);
                    var cornerRadius = new Vector2(Math.Max(0, radius - inset));
                    var offset = new Vector3(rect.X + inset, rect.Y + inset, 0);
                    float layerOpacity = (opacity - previous) / Math.Max(0.0001f, 1 - previous);
                    if(geometry.Size != layerSize) geometry.Size = layerSize;
                    if(geometry.CornerRadius != cornerRadius) geometry.CornerRadius = cornerRadius;
                    if(visual.Size != layerSize) visual.Size = layerSize;
                    if(visual.Offset != offset) visual.Offset = offset;
                    if(visual.Opacity != layerOpacity) visual.Opacity = layerOpacity;
                    objectIndex += 3;
                    previous = opacity;
                }
            }
            //줄이 줄어든 경우 사용하지 않는 뒤쪽 레이어를 바로 해제한다.
            for(int i = _regionObjects.Count - 3; i >= objectIndex; i -= 3)
            {
                _root.Children.Remove((SpriteVisual)_regionObjects[i + 2]);
                _regionObjects[i + 2].Dispose();
                _regionObjects[i + 1].Dispose();
                _regionObjects[i].Dispose();
            }
            if(objectIndex < _regionObjects.Count)
                _regionObjects.RemoveRange(objectIndex, _regionObjects.Count - objectIndex);
        }

        private void ClearRegions()
        {
            _root?.Children.RemoveAll();
            for(int i = _regionObjects.Count - 1; i >= 0; i--) _regionObjects[i].Dispose();
            _regionObjects.Clear();
            _regions = Array.Empty<Rectangle>();
        }

        public void SetCaptureExclusion(bool exclude)
        {
            if(Handle == IntPtr.Zero || _excludedFromCapture == exclude) return;
            if(!SetWindowDisplayAffinity(Handle, exclude ? 0x11u : 0u))
            {
                var error = new Win32Exception();
                Disable();
                Util.ShowLog($"Overlay window backdrop capture exclusion failed: {error.Message}");
                return;
            }
            _excludedFromCapture = exclude;
        }
        public void Hide() { if(Handle != IntPtr.Zero) ShowWindow(Handle, 0); }
        public void Disable() { _failed = true; Dispose(); }
        public void Dispose()
        {
            ClearRegions();
            if(_target != null) _target.Root = null;
            _backdrop?.Dispose(); _backdrop = null;
            _root?.Dispose(); _root = null;
            _target?.Dispose(); _target = null;
            _compositor?.Dispose(); _compositor = null;
            _size = Size.Empty;
            _excludedFromCapture = null;
            if(Handle != IntPtr.Zero) DestroyHandle();
        }
        protected override void WndProc(ref Message m)
        {
            if(m.Msg == 0x84) { m.Result = new IntPtr(-1); return; }
            if(m.Msg == 0x21) { m.Result = new IntPtr(3); return; }
            if(m.Msg == 0x14) { m.Result = new IntPtr(1); return; }
            base.WndProc(ref m);
        }

        [StructLayout(LayoutKind.Sequential)] private struct DispatcherQueueOptions { public int Size, ThreadType, ApartmentType; }
        [ComImport, Guid("29E691FA-4567-4DCA-B319-D0F207EB6807"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ICompositorDesktopInterop { void CreateDesktopWindowTarget(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool topmost, out IntPtr target); }
        [DllImport("CoreMessaging.dll")] private static extern int CreateDispatcherQueueController(DispatcherQueueOptions options, out IntPtr controller);
        [DllImport("dwmapi.dll")] private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);
        [DllImport("user32.dll", SetLastError = true)] private static extern bool SetWindowPos(IntPtr window, IntPtr after, int x, int y, int width, int height, uint flags);
        [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr window, int command);
        [DllImport("user32.dll", SetLastError = true)] private static extern bool SetWindowDisplayAffinity(IntPtr window, uint affinity);
        [DllImport("user32.dll", SetLastError = true)] private static extern bool SetLayeredWindowAttributes(IntPtr window, uint colorKey, byte alpha, uint flags);
    }
}
