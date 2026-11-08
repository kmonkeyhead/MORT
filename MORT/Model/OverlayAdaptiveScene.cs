#nullable enable
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MORT.Model
{
    internal sealed record OverlayAdaptiveMask(int Index, Size ImageSize, Rectangle[] Words)
    {
        internal static OverlayAdaptiveMask FromOcr(OCRDataManager.ResultData data) =>
            new(data.Index, data.OcrImageSize, GetWords(data.LineDataList));

        internal static Rectangle[] GetWords(IEnumerable<OCRDataManager.LineData> lines) => lines
            .SelectMany(line => line.wordRectList.Count > 0 ? line.wordRectList : new List<Rectangle> { line.lineRect })
            .Where(rect => rect.Width > 0 && rect.Height > 0)
            .Distinct().ToArray();
    }

    internal sealed record OverlayAdaptiveArea(int Index, Rectangle ScreenRect, Rectangle[] Words);
    internal sealed record OverlayAdaptiveScene(OverlayAdaptiveArea[] Areas, int Level,
        ScreenCapture.ScreenCapture? Capture, Rectangle DesktopBounds, int TargetFps = 60);
    internal sealed record OverlayAdaptiveOutput(OverlayAdaptiveScene Scene, Dictionary<int, OverlayBlurFrame> Frames);

}
