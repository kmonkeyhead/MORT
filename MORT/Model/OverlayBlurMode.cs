namespace MORT.Model
{
    public enum OverlayBlurMode
    {
        None = 0,
        Window = 1,
        Restore = 3
    }

    //OCR 원본 버퍼와 수명을 공유하지 않는 PArgb 배경. 복원 모드는 수정할 글자 부분만 불투명하다.
    public sealed record OverlayBlurFrame(byte[] Pixels, int Width, int Height)
    {
        //실시간 풀 프레임의 Pixels는 소유권/읽기 대여 기간에만 접근한다.
        //배열 용량은 Width * Height * 4보다 클 수 있다.
        internal MORT.Service.Overlay.OverlayFrameBufferPool.Lease PixelLease { get; init; }
        internal void RetainPixels() => PixelLease?.Retain();
        internal void ReleasePixels() => PixelLease?.Release();
        public OverlayBlurMode Mode { get; init; } = OverlayBlurMode.Restore;
        //실시간 복원은 전체 캡처 대신 패치만 보관한다. 기존 프레임은 Empty로 전체 크기를 뜻한다.
        public System.Drawing.Rectangle SourceRect { get; init; }
        public System.Drawing.Size SourceSize { get; init; }
    }
}
