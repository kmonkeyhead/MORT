using System;
using System.Threading;

namespace MORT.Service.Overlay
{
    //UI 스레드에서 생성/해제한다. 요청하는 작업 스레드는 플래그만 변경한다.
    //BeginInvoke는 콜백 실행 중 추가된 콜백도 같은 루프에서 소진하므로,
    //연속 프레임이 그리기보다 빠르면 입력/타이머 메시지가 처리되지 않을 수 있다.
    internal sealed class OverlayPaintScheduler : IDisposable
    {
        private readonly System.Windows.Forms.Timer _timer;
        private readonly Action _paint;
        private int _pending;
        private int _disposed;

        internal OverlayPaintScheduler(Action paint)
        {
            _paint = paint;
            //WM_TIMER는 입력보다 우선순위가 낮다. 매 tick 한 번만 그리며 밀린 프레임은 쌓지 않는다.
            //실제 표시 주기는 UI 부하/Windows 타이머 정밀도에 따라 목표 FPS보다 낮을 수 있다.
            _timer = new System.Windows.Forms.Timer { Interval = 1 };
            _timer.Tick += OnTick;
            _timer.Start();
        }

        internal void Request()
        {
            if(Volatile.Read(ref _disposed) == 0) Interlocked.Exchange(ref _pending, 1);
        }

        internal void Clear() => Interlocked.Exchange(ref _pending, 0);

        private void OnTick(object sender, EventArgs e)
        {
            if(Volatile.Read(ref _disposed) == 0 && Interlocked.Exchange(ref _pending, 0) != 0)
                _paint();
        }

        public void Dispose()
        {
            if(Interlocked.Exchange(ref _disposed, 1) != 0) return;
            _timer.Stop();
            _timer.Tick -= OnTick;
            _timer.Dispose();
            Clear();
        }
    }
}
