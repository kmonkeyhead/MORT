using System.Drawing;

namespace MORT.Model
{
    //게시 후 픽셀을 수정하지 않는다. OCR의 GetData 소비 상태와 별개로 최신 프레임을 공유한다.
    internal sealed record OverlayCaptureFrame(byte[] Pixels, int Width, int Height, Rectangle ScreenBounds);
}
