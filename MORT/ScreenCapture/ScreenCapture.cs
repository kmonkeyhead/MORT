using System;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;

namespace MORT.ScreenCapture
{
    public class ScreenCapture : IDisposable
    {
        private IDirect3DDevice device;
        private SharpDX.Direct3D11.Device d3dDevice;

        private volatile ScreenCaptureProcesser capture;
        private bool _adaptiveCapture;
        private int _adaptiveTargetFps = 60;

        internal void SetAdaptiveCapture(bool enabled, int targetFps = 60)
        {
            _adaptiveCapture = enabled;
            _adaptiveTargetFps = Math.Clamp(targetFps, 1, 120);
            var current = capture;
            if(current != null)
            {
                current.AdaptiveTargetFps = _adaptiveTargetFps;
                current.AdaptiveCapture = enabled;
            }
        }

        internal MORT.Model.OverlayCaptureFrame? LatestAdaptiveFrame => capture?.LatestAdaptiveFrame;

        public ScreenCapture()
        {
            d3dDevice = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware, SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);

            device = Direct3D11Helper.CreateDirect3DDeviceFromSharpDXDevice(d3dDevice);
        }


        public void Dispose()
        {
            StopCapture();
            device.Dispose();
        }

        public void StartCaptureFromItem(GraphicsCaptureItem item, IntPtr hWnd, bool enableYellowBoard, out BorderStateType borderStateType)
        {
            StopCapture();

            capture = new ScreenCaptureProcesser(d3dDevice, device, item, hWnd);
            capture.AdaptiveTargetFps = _adaptiveTargetFps;
            capture.AdaptiveCapture = _adaptiveCapture;
            capture.SetRequireBoard(enableYellowBoard);
            capture.StartCapture();

            borderStateType = capture.BorderStateType;
        }

        public void PrepareStart()
        {
            if (capture != null)
            {
                capture.isDataSuccess = false;
                capture.isWait = false;
                capture.isStartCapture = false;
            }
        }

        public void StartDataCapture()
        {
            var current = capture;
            if(current != null) current.isStartCapture = true;
        }

        public bool GetData(ref byte[] array, ref int x, ref int y, ref int positionX, ref int positionY)
        {
            bool isSuccess = false;

            var current = capture;
            if(current == null)
            {
                return false;
            }

            if (current.AvailableGetData)
            {
                isSuccess = true;
                current.GetLastData(ref x, ref y, ref positionX, ref positionY, ref array);

            }

            Console.WriteLine($"GetData: isSuccess={isSuccess}");
            return isSuccess;
        }

        public void StopCapture()
        {
            var current = capture;
            capture = null;
            current?.Dispose();
        }

    }
}
