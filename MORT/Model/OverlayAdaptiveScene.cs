#nullable enable
using System.Collections.Generic;
using System.Drawing;

namespace MORT.Model
{
    internal sealed record OverlayAdaptiveArea(int Index, Rectangle ScreenRect, Rectangle[] Words);
    internal sealed record OverlayAdaptiveScene(OverlayAdaptiveArea[] Areas, int Level,
        ScreenCapture.ScreenCapture? Capture, Rectangle DesktopBounds, int TargetFps = 60);
    internal sealed record OverlayAdaptiveOutput(OverlayAdaptiveScene Scene, Dictionary<int, OverlayBlurFrame> Frames);

}
