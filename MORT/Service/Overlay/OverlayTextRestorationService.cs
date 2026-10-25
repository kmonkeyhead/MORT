using MORT.Model;
using MORT.Service.Overlay.Restoration;
using System.Collections.Generic;
using System.Drawing;

namespace MORT.Service.Overlay
{
    //OCR 영역 주변의 색을 여덟 방향으로 연결하는 기존 적응형 복원을 사용한다.
    internal sealed class OverlayTextRestorationService
    {
        public OverlayBlurFrame CreateFrame(byte[] source, int width, int height, int channels, IReadOnlyList<Rectangle> wordRects, int restorationLevel = 2)
        {
            return LegacyBackgroundRestoration.CreateFrame(source, width, height, channels, wordRects, restorationLevel);
        }
    }
}
