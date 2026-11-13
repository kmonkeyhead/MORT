using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MORT.Service.Overlay
{
    //UI 스레드 전용. GDI+가 쓰는 픽셀과 UpdateLayeredWindow가 읽는 DIB를 공유한다.
    internal sealed class LayeredWindowSurface : IDisposable
    {
        internal Bitmap Image { get; private set; }
        internal IntPtr DeviceContext { get; private set; }
        internal IntPtr Pixels { get; private set; }
        private IntPtr _bitmap, _previous;

        internal LayeredWindowSurface(int width, int height)
        {
            if(width <= 0 || height <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            int stride = checked(width * 4);
            var info = new BitmapInfo { Size = 40, Width = width, Height = -height,
                Planes = 1, BitCount = 32, ImageSize = checked((uint)((long)stride * height)) };
            try
            {
                DeviceContext = CreateCompatibleDC(IntPtr.Zero);
                if(DeviceContext == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
                _bitmap = CreateDIBSection(DeviceContext, ref info, 0, out var pixels, IntPtr.Zero, 0);
                if(_bitmap == IntPtr.Zero) throw new Win32Exception(Marshal.GetLastWin32Error());
                Pixels = pixels;
                _previous = SelectObject(DeviceContext, _bitmap);
                if(_previous == IntPtr.Zero || _previous == new IntPtr(-1))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                Image = new Bitmap(width, height, stride, PixelFormat.Format32bppPArgb, pixels);
            }
            catch { Dispose(); throw; }
        }

        public void Dispose()
        {
            Image?.Dispose();
            Image = null;
            if(DeviceContext != IntPtr.Zero && _previous != IntPtr.Zero && _previous != new IntPtr(-1))
                SelectObject(DeviceContext, _previous);
            if(_bitmap != IntPtr.Zero) DeleteObject(_bitmap);
            if(DeviceContext != IntPtr.Zero) DeleteDC(DeviceContext);
            _bitmap = _previous = IntPtr.Zero;
            DeviceContext = IntPtr.Zero;
            Pixels = IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BitmapInfo
        {
            public uint Size;
            public int Width, Height;
            public ushort Planes, BitCount;
            public uint Compression, ImageSize;
            public int XPelsPerMeter, YPelsPerMeter;
            public uint ColorsUsed, ColorsImportant;
        }
        [DllImport("gdi32.dll", SetLastError = true)] private static extern IntPtr CreateCompatibleDC(IntPtr dc);
        [DllImport("gdi32.dll", SetLastError = true)] private static extern IntPtr CreateDIBSection(IntPtr dc, ref BitmapInfo info, uint usage, out IntPtr bits, IntPtr section, uint offset);
        [DllImport("gdi32.dll", SetLastError = true)] private static extern IntPtr SelectObject(IntPtr dc, IntPtr value);
        [DllImport("gdi32.dll")] private static extern bool DeleteObject(IntPtr value);
        [DllImport("gdi32.dll")] private static extern bool DeleteDC(IntPtr dc);
    }
}
