using System;
using MORT.Model;
using MORT.Service.Overlay.Restoration;
using System.Collections.Generic;
using System.Drawing;

namespace MORT.Service.Overlay
{
    //OCR 영역 주변의 색을 여덟 방향으로 연결하는 기존 적응형 복원을 사용한다.
    internal sealed class OverlayTextRestorationService
    {
        private readonly object _gate = new();
        private readonly OverlayRestorationInput _input = new();
        private readonly LegacyBackgroundRestoration.Workspace _workspace = new();

        public OverlayBlurFrame CreateFrame(byte[] source, int width, int height, int channels, IReadOnlyList<Rectangle> wordRects, int restorationLevel = 2)
        {
            using var timing = OverlayPerformanceProfiler.Measure(OverlayPerformanceStage.RestoreBackground, width, height);
            lock(_gate)
                return LegacyBackgroundRestoration.CreateFrame(source, width, height, channels, wordRects, restorationLevel, workspace: _workspace);
        }
        public OverlayBlurFrame CreatePatch(byte[] source, int width, int height, int channels, IReadOnlyList<Rectangle> wordRects, int restorationLevel = 2)
        {
            lock(_gate)
                return CreatePatchCore(source, width, height, channels, wordRects, restorationLevel, null);
        }
        //반환 프레임의 초기 소유권은 호출자가 저장하거나 ReleasePixels로 해제한다.
        internal OverlayBlurFrame CreatePooledPatch(byte[] source, int width, int height, int channels,
            IReadOnlyList<Rectangle> wordRects, int restorationLevel, OverlayFrameBufferPool pool)
        {
            lock(_gate)
                return CreatePatchCore(source, width, height, channels, wordRects, restorationLevel, pool);
        }
        private OverlayBlurFrame CreatePatchCore(byte[] source, int width, int height, int channels,
            IReadOnlyList<Rectangle> words, int level, OverlayFrameBufferPool pool)
        {
            if(source == null || width <= 0 || height <= 0 || channels < 3 || words == null
                || (long)width * height > int.MaxValue / 4
                || source.LongLength < (long)width * height * channels) return null;
            Size target = OverlayRestorationInput.ProcessingSize(width, height);
            var input = _input.Prepare(source, width, height, channels, words, target, out var scaledWords, level);
            //SourceRect/SourceSize는 작업 해상도 좌표계다. 렌더러가 원본 캡처 위치/크기에 맞춰 표시한다.
            return LegacyBackgroundRestoration.CreateFrame(input, target.Width, target.Height, channels,
                scaledWords, level, compactOutput: true, workspace: _workspace, outputPool: pool);
        }

        internal static Rectangle[] GetComparisonRegions(int width, int height, IReadOnlyList<Rectangle> words, int level)
        {
            Size source = new Size(width, height), target = OverlayRestorationInput.ProcessingSize(width, height);
            if(target == source) return LegacyBackgroundRestoration.GetInputRegions(width, height, words, level);
            var scaledWords = new Rectangle[words.Count];
            for(int i = 0; i < words.Count; i++) scaledWords[i] = OverlayRestorationInput.ScaleWord(words[i], source, target);
            var regions = LegacyBackgroundRestoration.GetInputRegions(target.Width, target.Height, scaledWords, level);
            for(int i = 0; i < regions.Length; i++)
            {
                var r = regions[i];
                //축소 필터가 참조하는 인접 원본 픽셀까지 포함한다. OCR 영역 밖의 화면 전체 비교는 하지 않는다.
                regions[i] = Rectangle.Intersect(new Rectangle(Point.Empty, source), Rectangle.FromLTRB(
                    (int)Math.Floor(r.Left * (double)width / target.Width) - 2,
                    (int)Math.Floor(r.Top * (double)height / target.Height) - 2,
                    (int)Math.Ceiling(r.Right * (double)width / target.Width) + 2,
                    (int)Math.Ceiling(r.Bottom * (double)height / target.Height) + 2));
            }
            return regions;
        }
    }
}
