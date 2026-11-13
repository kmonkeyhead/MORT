using System;
using System.Collections.Generic;
using System.Drawing;
using MORT.Service.Overlay.Restoration;

namespace MORT.Service.Overlay
{
    //OCR 확대 배율이 아니라 원본 캡처 크기로 결정한다. 배열은 캡처 크기가 같으면 유지한다.
    internal sealed class OverlayRestorationInput
    {
        private byte[] _pixels = Array.Empty<byte>();
        private Rectangle[] _words = Array.Empty<Rectangle>();
        private Rectangle[] _regions = Array.Empty<Rectangle>();
        private int _wordCount;
        private Size _sourceSize;
        private int[] _x0 = Array.Empty<int>(), _x1 = Array.Empty<int>(), _wx = Array.Empty<int>();
        private int[] _y0 = Array.Empty<int>(), _y1 = Array.Empty<int>(), _wy = Array.Empty<int>();
        private int _channels;
        private bool _highQuality;
        private int _strength = 3;

        internal static Size ProcessingSize(int width, int height)
        {
            if((long)width * height < 1920L * 1080) return new Size(width, height);
            double scale = Math.Sqrt((1280.0 * 720) / ((double)width * height));
            return new Size(Math.Max(1, (int)(width * scale)), Math.Max(1, (int)(height * scale)));
        }

        internal static Rectangle ScaleWord(Rectangle word, Size source, Size target)
        {
            if(word.Width <= 0 || word.Height <= 0) return Rectangle.Empty;
            word = Rectangle.Intersect(word, new Rectangle(Point.Empty, source));
            if(word.Width <= 0 || word.Height <= 0) return Rectangle.Empty;
            return Rectangle.FromLTRB(
                (int)Math.Floor(word.Left * (double)target.Width / source.Width),
                (int)Math.Floor(word.Top * (double)target.Height / source.Height),
                (int)Math.Ceiling(word.Right * (double)target.Width / source.Width),
                (int)Math.Ceiling(word.Bottom * (double)target.Height / source.Height));
        }

        internal byte[] Prepare(byte[] source, int width, int height, int channels,
            IReadOnlyList<Rectangle> words, Size target, out IReadOnlyList<Rectangle> scaledWords, bool highQuality = false, int strength = 3)
        {
            var sourceSize = new Size(width, height);
            bool changed = sourceSize != _sourceSize || channels != _channels;
            if(changed)
            {
                _pixels = Array.Empty<byte>();
                _sourceSize = sourceSize;
                _channels = channels;
            }
            if(target == sourceSize) { scaledWords = words; return source; }
            int length = checked(target.Width * target.Height * channels);
            if(_pixels.Length != length)
            {
                changed = true;
                _pixels = new byte[length];
                MakeAxis(width, target.Width, channels, out _x0, out _x1, out _wx);
                MakeAxis(height, target.Height, width * channels, out _y0, out _y1, out _wy);
            }
            changed |= _wordCount != words.Count || _highQuality != highQuality || _strength != strength;
            if(_words.Length < words.Count) _words = new Rectangle[Math.Max(words.Count, _words.Length * 2)];
            for(int i = 0; i < words.Count; i++)
            {
                var word = ScaleWord(words[i], sourceSize, target);
                changed |= _words[i] != word;
                _words[i] = word;
            }
            scaledWords = new ArraySegment<Rectangle>(_words, 0, words.Count);
            if(changed) _regions = highQuality ? HighQualityBackgroundRestoration.GetInputRegions(target.Width, target.Height, scaledWords)
                : LegacyBackgroundRestoration.GetInputRegions(target.Width, target.Height, scaledWords, strength);
            _highQuality = highQuality;
            _strength = strength;
            _wordCount = words.Count;

            //좌표와 가중치는 캡처 크기가 바뀔 때만 계산한다. 반복 루프는 연속 배열과 정수 연산만 사용한다.
            //복원에서 참조하는 글자 주변만 축소한다. 나머지 화면 픽셀은 처리하지 않는다.
            foreach(var region in _regions)
            for(int y = region.Top; y < region.Bottom; y++)
            {
                int topRow = _y0[y], bottomRow = _y1[y], fy = _wy[y];
                int output = (y * target.Width + region.Left) * channels;
                for(int x = region.Left; x < region.Right; x++)
                {
                    int a = topRow + _x0[x], b = topRow + _x1[x];
                    int c = bottomRow + _x0[x], d = bottomRow + _x1[x], fx = _wx[x];
                    for(int channel = 0; channel < channels; channel++)
                    {
                        int top = source[a + channel] * (256 - fx) + source[b + channel] * fx;
                        int bottom = source[c + channel] * (256 - fx) + source[d + channel] * fx;
                        _pixels[output++] = (byte)((top * (256 - fy) + bottom * fy + 32768) >> 16);
                    }
                }
            }
            return _pixels;
        }
        private static void MakeAxis(int sourceLength, int targetLength, int stride,
            out int[] first, out int[] second, out int[] weight)
        {
            first = new int[targetLength]; second = new int[targetLength]; weight = new int[targetLength];
            for(int i = 0; i < targetLength; i++)
            {
                double position = (i + 0.5) * sourceLength / targetLength - 0.5;
                int index = Math.Clamp((int)position, 0, sourceLength - 1);
                first[i] = index * stride;
                second[i] = Math.Min(index + 1, sourceLength - 1) * stride;
                weight[i] = Math.Clamp((int)((position - index) * 256 + 0.5), 0, 256);
            }
        }
    }
}
