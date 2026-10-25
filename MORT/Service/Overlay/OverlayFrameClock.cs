#nullable enable
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MORT.Service.Overlay
{
    //일반 Task.Delay의 타이머 양자화가 60Hz 주기를 늦추지 않도록 전용 worker에서만 대기한다.
    //시스템 전체 타이머 해상도를 변경하거나 CPU spin을 하지 않는다.
    internal sealed class OverlayFrameClock : WaitHandle
    {
        private readonly CancellationToken _stop;
        private readonly WaitHandle[] _handles;

        internal OverlayFrameClock(CancellationToken stop)
        {
            _stop = stop;
            SafeWaitHandle = CreateWaitableTimerExW(IntPtr.Zero, null, 0x2, 0x001F0003);
            if(SafeWaitHandle.IsInvalid)
            {
                SafeWaitHandle.Dispose();
                SafeWaitHandle = CreateWaitableTimerExW(IntPtr.Zero, null, 0, 0x001F0003);
            }
            _handles = new WaitHandle[] { stop.WaitHandle, this };
        }

        internal void Wait(double milliseconds)
        {
            _stop.ThrowIfCancellationRequested();
            long due = -Math.Max(1L, (long)(milliseconds * 10000));
            if(SafeWaitHandle.IsInvalid || !SetWaitableTimer(SafeWaitHandle, ref due, 0, IntPtr.Zero, IntPtr.Zero, false))
                _stop.WaitHandle.WaitOne(TimeSpan.FromMilliseconds(Math.Max(1, milliseconds)));
            else WaitAny(_handles);
            _stop.ThrowIfCancellationRequested();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern SafeWaitHandle CreateWaitableTimerExW(IntPtr attributes, string? name, uint flags, uint access);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWaitableTimer(SafeWaitHandle timer, ref long due, int period,
            IntPtr callback, IntPtr argument, [MarshalAs(UnmanagedType.Bool)] bool resume);
    }
}
