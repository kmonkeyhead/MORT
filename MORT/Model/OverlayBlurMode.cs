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
        public OverlayBlurMode Mode { get; init; } = OverlayBlurMode.Restore;
        //실시간 복원은 전체 캡처 대신 패치만 보관한다. 기존 프레임은 Empty로 전체 크기를 뜻한다.
        public System.Drawing.Rectangle SourceRect { get; init; }
        public System.Drawing.Size SourceSize { get; init; }
    }
}
