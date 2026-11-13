using System;
using System.Collections.Generic;
using System.Drawing;
using MORT.Model;
using MORT.OpenCV;

namespace MORT.Service.Overlay.Restoration
{
    //OCR 글자 마스크를 OpenCV 기반 C# 코드로 복원하고 MORT 출력 풀에 직접 기록한다.
    internal sealed class HighQualityBackgroundRestoration : IDisposable
    {
        //사용자 단계는 MORT에서 실제 처리값으로 변환한다. OpenCV 처리부에는 픽셀 수만 전달한다.
        private static readonly int[] MaskExpansionByStrength = { 2, 3, 4, 5, 6 };
        private Rectangle[] _words = Array.Empty<Rectangle>();
        private Rectangle[] _regions = Array.Empty<Rectangle>();
        private BackgroundRestorer.Region[][] _text = Array.Empty<BackgroundRestorer.Region[]>();
        private BackgroundRestorer? _restorer;
        private int[] _rowSpans = Array.Empty<int>();
        private Size _size;
        private int _count;

        //형태학 커널/마스크 확장/복원 경계가 참조하는 여백.
        internal static Rectangle[] GetInputRegions(int width, int height, IReadOnlyList<Rectangle> words)
        {
            var bounds = new Rectangle(0, 0, width, height);
            var regions = new List<Rectangle>();
            foreach(var word in words)
            {
                var clipped = Rectangle.Intersect(word, bounds);
                if(clipped.Width <= 0 || clipped.Height <= 0) continue;
                var region = Rectangle.Intersect(Rectangle.Inflate(clipped, BackgroundRestorer.InputPadding, BackgroundRestorer.InputPadding), bounds);
                for(int i = 0; i < regions.Count;)
                {
                    if(region.IntersectsWith(regions[i]))
                    {
                        region = Rectangle.Union(region, regions[i]);
                        regions.RemoveAt(i);
                        i = 0;
                    }
                    else i++;
                }
                regions.Add(region);
            }
            return regions.ToArray();
        }

        private void Configure(int width, int height, IReadOnlyList<Rectangle> words)
        {
            bool changed = _size != new Size(width, height) || _count != words.Count;
            for(int i = 0; !changed && i < words.Count; i++) changed = _words[i] != words[i];
            if(!changed) return;
            _size = new Size(width, height);
            _count = words.Count;
            if(_words.Length < _count) _words = new Rectangle[_count];
            for(int i = 0; i < _count; i++) _words[i] = words[i];
            _regions = GetInputRegions(width, height, words);
            _text = new BackgroundRestorer.Region[_regions.Length][];
            var bounds = new Rectangle(Point.Empty, _size);
            for(int i = 0; i < _regions.Length; i++)
            {
                var text = new List<BackgroundRestorer.Region>();
                foreach(var word in words)
                {
                    var clipped = Rectangle.Intersect(word, bounds);
                    if(clipped.Width <= 0 || clipped.Height <= 0 || !_regions[i].IntersectsWith(clipped)) continue;
                    text.Add(new BackgroundRestorer.Region(clipped.X - _regions[i].X, clipped.Y - _regions[i].Y,
                        clipped.Width, clipped.Height));
                }
                _text[i] = text.ToArray();
            }
        }

        internal unsafe OverlayBlurFrame CreateFrame(byte[] source, int width, int height, int channels,
            IReadOnlyList<Rectangle> words, bool compact, OverlayFrameBufferPool pool, int strength = 3)
        {
            int maskExpansion = MaskExpansionByStrength[Math.Clamp(strength, 1, 5) - 1];
            Configure(width, height, words);
            if(_regions.Length == 0) return null;
            var work = _regions[0];
            for(int i = 1; i < _regions.Length; i++) work = Rectangle.Union(work, _regions[i]);
            if(!compact) work = new Rectangle(0, 0, width, height);
            var lease = pool?.Rent(checked(work.Width * work.Height * 4), checked(width * height * 4));
            if(pool != null && lease == null) return null;
            bool transferred = false;
            try
            {
                var pixels = lease?.Pixels ?? new byte[work.Width * work.Height * 4];
                lease?.SetStride(work.Width);
                _restorer ??= new BackgroundRestorer();
                if(_rowSpans.Length < work.Height * 2) _rowSpans = new int[work.Height * 2];
                fixed(byte* sourcePtr = source, outputPtr = pixels)
                for(int i = 0; i < _regions.Length; i++)
                {
                    var region = _regions[i];
                    var dirty = region;
                    dirty.Offset(-work.X, -work.Y);
                    _restorer.Restore(sourcePtr + (region.Y * width + region.X) * channels,
                        region.Width, region.Height, channels, width * channels, _text[i],
                        outputPtr + (dirty.Y * work.Width + dirty.X) * 4, work.Width * 4, _rowSpans, maskExpansion);
                    for(int y = 0; y < region.Height; y++)
                    {
                        int first = _rowSpans[y * 2], end = _rowSpans[y * 2 + 1];
                        if(end > first) lease?.AddDirty(new Rectangle(dirty.X + first, dirty.Y + y, end - first, 1));
                    }
                }
                var result = new OverlayBlurFrame(pixels, work.Width, work.Height)
                {
                    PixelLease = lease, Mode = OverlayBlurMode.Restore,
                    SourceRect = compact ? work : Rectangle.Empty, SourceSize = new Size(width, height)
                };
                transferred = true;
                return result;
            }
            finally { if(!transferred) lease?.Release(); }
        }
        public void Dispose() { _restorer?.Dispose(); _restorer = null; }
    }
}
