#nullable enable
using System;
using System.Threading;
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
    internal sealed class OverlayAdaptiveOutput
    {
        internal OverlayAdaptiveScene Scene { get; }
        internal Dictionary<int, OverlayBlurFrame> Frames { get; }
        private int _references = 1;
        internal OverlayAdaptiveOutput(OverlayAdaptiveScene scene, Dictionary<int, OverlayBlurFrame> frames)
        {
            Scene = scene;
            Frames = frames;
            foreach(var frame in Frames.Values) frame.RetainPixels();
        }
        internal ReadLease Acquire()
        {
            Interlocked.Increment(ref _references); //게시자의 소유권이 있는 동안 서비스 잠금 안에서만 호출한다.
            return new ReadLease(this);
        }
        internal void Release()
        {
            int count = Interlocked.Decrement(ref _references);
            if(count == 0) foreach(var frame in Frames.Values) frame.ReleasePixels();
            else if(count < 0) throw new InvalidOperationException("Adaptive output released twice.");
        }
        internal readonly struct ReadLease : IDisposable
        {
            internal OverlayAdaptiveOutput? Output { get; }
            internal ReadLease(OverlayAdaptiveOutput output) => Output = output;
            public void Dispose() => Output?.Release();
        }
    }

}
