using System;
using MORT.Model;
using MORT.Service.Overlay.Restoration;
using System.Collections.Generic;
using System.Drawing;

namespace MORT.Service.Overlay
{
    //저품질 여덟 방향 복원과 고품질 복원을 같은 입력/출력 버퍼 경로에 연결한다.
    internal sealed class OverlayTextRestorationService : IDisposable
    {
        private readonly object _gate = new();
        private readonly OverlayRestorationInput _input = new();
        private readonly LegacyBackgroundRestoration.Workspace _workspace = new();
        private readonly HighQualityBackgroundRestoration _highQuality = new();
        internal bool UseHighQuality { get; set; }
        internal int BackgroundStrength { get; set; } = 3;
        public void Dispose() { lock(_gate) _highQuality.Dispose(); }

        public OverlayBlurFrame CreateFrame(byte[] source, int width, int height, int channels, IReadOnlyList<Rectangle> wordRects)
        {
            using var timing = OverlayPerformanceProfiler.Measure(OverlayPerformanceStage.RestoreBackground, width, height);
            lock(_gate)
            {
                int quality = AdvencedOptionManager.OverlayBackgroundQuality;
                int strength = AdvencedOptionManager.OverlayBackgroundStrength;
                if(quality > 0 && source != null && width > 0 && height > 0 && channels >= 3
                    && (long)width * height <= int.MaxValue / 4 && source.LongLength >= (long)width * height * channels && wordRects != null)
                    return _highQuality.CreateFrame(source, width, height, channels, wordRects, false, null, strength);
                return LegacyBackgroundRestoration.CreateFrame(source, width, height, channels, wordRects, workspace: _workspace, strength: strength);
            }
        }
        public OverlayBlurFrame CreatePatch(byte[] source, int width, int height, int channels, IReadOnlyList<Rectangle> wordRects)
        {
            lock(_gate)
                return CreatePatchCore(source, width, height, channels, wordRects, null);
        }
        //반환 프레임의 초기 소유권은 호출자가 저장하거나 ReleasePixels로 해제한다.
        internal OverlayBlurFrame CreatePooledPatch(byte[] source, int width, int height, int channels,
            IReadOnlyList<Rectangle> wordRects, OverlayFrameBufferPool pool)
        {
            lock(_gate)
                return CreatePatchCore(source, width, height, channels, wordRects, pool);
        }
        private OverlayBlurFrame CreatePatchCore(byte[] source, int width, int height, int channels,
            IReadOnlyList<Rectangle> words, OverlayFrameBufferPool pool)
        {
            if(source == null || width <= 0 || height <= 0 || channels < 3 || words == null
                || (long)width * height > int.MaxValue / 4
                || source.LongLength < (long)width * height * channels) return null;
            Size target = OverlayRestorationInput.ProcessingSize(width, height);
            var input = _input.Prepare(source, width, height, channels, words, target, out var scaledWords, UseHighQuality, BackgroundStrength);
            if(UseHighQuality)
                return _highQuality.CreateFrame(input, target.Width, target.Height, channels, scaledWords, true, pool, BackgroundStrength);
            //SourceRect/SourceSize는 작업 해상도 좌표계다. 렌더러가 원본 캡처 위치/크기에 맞춰 표시한다.
            return LegacyBackgroundRestoration.CreateFrame(input, target.Width, target.Height, channels,
                scaledWords, compactOutput: true, workspace: _workspace, outputPool: pool, strength: BackgroundStrength);
        }

        internal static Rectangle[] GetComparisonRegions(int width, int height, IReadOnlyList<Rectangle> words, bool highQuality = false, int strength = 3)
        {
            Size source = new Size(width, height), target = OverlayRestorationInput.ProcessingSize(width, height);
            if(target == source) return highQuality ? HighQualityBackgroundRestoration.GetInputRegions(width, height, words)
                : LegacyBackgroundRestoration.GetInputRegions(width, height, words, strength);
            var scaledWords = new Rectangle[words.Count];
            for(int i = 0; i < words.Count; i++) scaledWords[i] = OverlayRestorationInput.ScaleWord(words[i], source, target);
            var regions = highQuality ? HighQualityBackgroundRestoration.GetInputRegions(target.Width, target.Height, scaledWords)
                : LegacyBackgroundRestoration.GetInputRegions(target.Width, target.Height, scaledWords, strength);
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
