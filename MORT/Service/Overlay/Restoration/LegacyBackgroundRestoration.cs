using MORT.Model;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace MORT.Service.Overlay.Restoration
{
    //모든 OCR 글자를 제외한 주변 샘플로 배경을 연결한다. 복원 중인 픽셀은 참조하지 않는다.
    internal static class LegacyBackgroundRestoration
    {
        private static readonly (int X, int Y)[] Directions = { (1, 0), (0, 1), (1, 1), (1, -1), (2, 1), (1, 2), (2, -1), (1, -2) };

        //호출자는 작업 공간을 단독 소유한다. UI에 게시한 출력 배열은 여기에 넣지 않는다.
        internal sealed class Workspace
        {
            internal byte[] Mask = Array.Empty<byte>();
            internal byte[] Background = Array.Empty<byte>();
            internal float[] Scores = Array.Empty<float>();
            private Rectangle[] _localWords = Array.Empty<Rectangle>();
            private Rectangle[] _words = Array.Empty<Rectangle>();
            private int _wordCount, _width, _height, _level, _sourceWidth, _sourceHeight;

            internal IReadOnlyList<Rectangle> LocalWords(IReadOnlyList<Rectangle> words, Rectangle bounds, Point origin)
            {
                if(_localWords.Length < words.Count) _localWords = new Rectangle[Math.Max(words.Count, _localWords.Length * 2)];
                int count = 0;
                for(int i = 0; i < words.Count; i++)
                {
                    Rectangle word = words[i];
                    if(word.Width <= 0 || word.Height <= 0) continue;
                    Rectangle clipped = Rectangle.Intersect(word, bounds);
                    if(clipped.Width <= 0 || clipped.Height <= 0) continue;
                    clipped.Offset(-origin.X, -origin.Y);
                    _localWords[count++] = clipped;
                }
                return new ArraySegment<Rectangle>(_localWords, 0, count);
            }

            internal bool Prepare(int width, int height, IReadOnlyList<Rectangle> words, int level, int sourceWidth, int sourceHeight)
            {
                bool resized = _sourceWidth != sourceWidth || _sourceHeight != sourceHeight;
                bool changed = resized || _width != width || _height != height || _level != level || _wordCount != words.Count;
                for(int i = 0; !changed && i < words.Count; i++) changed = _words[i] != words[i];
                if(!changed) return false;
                int count = width * height;
                if(resized || Mask.Length < count)
                {
                    //감지 글자 크기는 배열의 수명 기준이 아니다. 필요한 용량이 커질 때만 확장한다.
                    int capacity = resized ? count : (int)Math.Max(count, Math.Min((long)sourceWidth * sourceHeight, (long)Mask.Length * 2));
                    Mask = new byte[capacity];
                    Background = new byte[capacity * 4];
                    Scores = new float[capacity];
                }
                else
                {
                    //이전 글자가 썼던 행 구간만 지운다. stride가 바뀌어도 이전 좌표로 제거한다.
                    var oldBounds = new Rectangle(0, 0, _width, _height);
                    for(int i = 0; i < _wordCount; i++)
                    {
                        Rectangle outer = GetMaskBounds(_words[i], _level, oldBounds);
                        for(int y = outer.Top; y < outer.Bottom; y++)
                            Mask.AsSpan(y * _width + outer.Left, outer.Width).Clear();
                    }
                }
                if(_words.Length < words.Count) _words = new Rectangle[Math.Max(words.Count, _words.Length * 2)];
                for(int i = 0; i < words.Count; i++) _words[i] = words[i];
                _wordCount = words.Count;
                _width = width;
                _height = height;
                _level = level;
                _sourceWidth = sourceWidth;
                _sourceHeight = sourceHeight;
                return true;
            }
        }

        private static Rectangle GetMaskBounds(Rectangle word, int level, Rectangle bounds)
        {
            if(word.Width <= 0 || word.Height <= 0) return Rectangle.Empty;
            Rectangle clipped = Rectangle.Intersect(word, bounds);
            if(clipped.Width <= 0 || clipped.Height <= 0) return Rectangle.Empty;
            Rectangle core = Rectangle.Intersect(Rectangle.Inflate(clipped, GetPadding(clipped, level), GetPadding(clipped, level)), bounds);
            return Rectangle.Intersect(Rectangle.Inflate(core, 2, 2), bounds);
        }

        //변경 감지는 각 글자의 복원 샘플 범위만 사용한다. 떨어진 글자 사이의 빈 공간은 포함하지 않는다.
        internal static Rectangle[] GetInputRegions(int width, int height, IReadOnlyList<Rectangle> words, int level)
        {
            var bounds = new Rectangle(0, 0, width, height);
            var regions = new List<Rectangle>();
            foreach(var word in words)
            {
                if(word.Width <= 0 || word.Height <= 0) continue;
                var clipped = Rectangle.Intersect(word, bounds);
                if(clipped.Width <= 0 || clipped.Height <= 0) continue;
                regions.Add(GetProbeBounds(clipped, level, bounds));
            }
            return regions.ToArray();
        }

        private static Rectangle GetProbeBounds(Rectangle word, int level, Rectangle bounds) =>
            Rectangle.Intersect(Rectangle.Inflate(word, GetPadding(word, level) + 8, GetPadding(word, level) + 8), bounds);

        public static OverlayBlurFrame CreateFrame(byte[] source, int width, int height, int channels, IReadOnlyList<Rectangle> wordRects, int restorationLevel = 2, bool compactOutput = false, Workspace workspace = null, OverlayFrameBufferPool outputPool = null)
        {
            if(source == null || width <= 0 || height <= 0 || channels < 3
                || (long)width * height > int.MaxValue / 4
                || source.LongLength < (long)width * height * channels || wordRects == null) return null;
            int sourceWidth = width, sourceHeight = height;
            Rectangle sourceBounds = new Rectangle(0, 0, width, height);
            Rectangle work = Rectangle.Empty;
            foreach(Rectangle word in wordRects)
            {
                if(word.Width <= 0 || word.Height <= 0) continue;
                Rectangle clipped = Rectangle.Intersect(word, sourceBounds);
                if(clipped.Width <= 0 || clipped.Height <= 0) continue;
                //색상 힌트의 padding + 8, 광선 양 끝과 3x3 샘플을 모두 보존한다.
                Rectangle probe = GetProbeBounds(clipped, restorationLevel, sourceBounds);
                work = work.IsEmpty ? probe : Rectangle.Union(work, probe);
            }
            if(work.IsEmpty && compactOutput) return null;
            if(work.IsEmpty)
                return new OverlayBlurFrame(new byte[width * height * 4], width, height) { Mode = OverlayBlurMode.Restore };
            //화면 대부분을 쓰면 좌표 목록을 만들지 않고 기존 전체 범위를 사용한다.
            if((long)work.Width * work.Height * 5 >= (long)width * height * 4)
                work = sourceBounds;
            else
            {
                workspace ??= new Workspace();
                wordRects = workspace.LocalWords(wordRects, sourceBounds, work.Location);
                width = work.Width;
                height = work.Height;
            }
            //원본은 복사하지 않고 원래 stride로 참조한다. 작업 배열은 영역 크기에 맞춰 재사용한다.
            //일반 출력은 프레임이 소유한다. 실시간 출력은 읽기 대여가 끝난 풀 배열만 재사용한다.
            workspace ??= new Workspace();
            bool rebuildMask = workspace.Prepare(width, height, wordRects, restorationLevel, sourceWidth, sourceHeight);
            byte[] mask = workspace.Mask;
            byte[] background = workspace.Background;
            float[] scores = workspace.Scores;

            Rectangle bounds = new Rectangle(0, 0, width, height);
            Rectangle active = Rectangle.Empty;
            foreach(Rectangle word in wordRects)
            {
                if(word.Width <= 0 || word.Height <= 0) continue;
                Rectangle clipped = Rectangle.Intersect(word, bounds);
                if(clipped.Width <= 0 || clipped.Height <= 0) continue;
                //획 검출만으로 남던 저대비 글자·외곽선·그림자를 모두 제외한다. 페더는 원문 밖에만 둔다.
                int padding = GetPadding(clipped, restorationLevel);
                Rectangle core = Rectangle.Intersect(Rectangle.Inflate(clipped, padding, padding), bounds);
                Rectangle outer = Rectangle.Intersect(Rectangle.Inflate(core, 2, 2), bounds);
                active = active.IsEmpty ? outer : Rectangle.Union(active, outer);
                if(!rebuildMask) continue;
                for(int y = outer.Top; y < outer.Bottom; y++)
                {
                    for(int x = outer.Left; x < outer.Right; x++)
                    {
                        int distance = Math.Max(Math.Max(core.Left - x, x - core.Right + 1), Math.Max(core.Top - y, y - core.Bottom + 1));
                        byte alpha = distance <= 0 ? (byte)255 : distance == 1 ? (byte)170 : (byte)85;
                        int pixel = y * width + x;
                        mask[pixel] = Math.Max(mask[pixel], alpha);
                    }
                }
            }
            int outputWidth = compactOutput ? width : sourceWidth;
            int outputHeight = compactOutput ? height : sourceHeight;
            Point outputOrigin = compactOutput ? Point.Empty : work.Location;
            int outputLength = outputWidth * outputHeight * 4;
            var lease = outputPool?.Rent(outputLength, sourceWidth * sourceHeight * 4);
            if(outputPool != null && lease == null) return null;
            bool transferred = false;
            try
            {
                byte[] pixels = lease?.Pixels ?? new byte[outputLength];
                if(lease != null)
                {
                    lease.SetStride(outputWidth);
                    for(int i = 0; i < wordRects.Count; i++)
                    {
                        var dirty = GetMaskBounds(wordRects[i], restorationLevel, bounds);
                        dirty.Offset(outputOrigin);
                        lease.AddDirty(dirty);
                    }
                }
                //색상 힌트/점수는 현재 글자가 사용하는 연속 행 구간만 초기화한다.
                //글자 사이의 빈 공간과 작업 배열의 여유 용량은 건드리지 않는다.
                for(int i = 0; i < wordRects.Count; i++)
                {
                    Rectangle outer = GetMaskBounds(wordRects[i], restorationLevel, bounds);
                    for(int y = outer.Top; y < outer.Bottom; y++)
                    {
                        int offset = y * width + outer.Left;
                        background.AsSpan(offset * 4, outer.Width * 4).Clear();
                        scores.AsSpan(offset, outer.Width).Fill(float.PositiveInfinity);
                    }
                }
                FindBackgroundHints(source, channels, width, height, mask, wordRects, background, restorationLevel, sourceWidth, work.Location);

                //여덟 방향 중 양 끝 배경색이 잘 이어지는 방향을 선택해 대각선 경계의 번짐을 줄인다.
                foreach(var direction in Directions)
                    FillDirection(source, channels, width, height, mask, background, pixels, scores, direction.X, direction.Y, sourceWidth, work.Location, (long)sourceWidth * sourceHeight >= 65536, outputWidth, outputOrigin);
                for(int y = active.Top; y < active.Bottom; y++)
                {
                    int end = y * width + active.Right;
                    int outputDelta = ((y + outputOrigin.Y) * outputWidth + outputOrigin.X - y * width) * 4;
                    for(int i = y * width + active.Left; i < end; i++)
                    {
                        if(mask[i] == 0) continue;
                        int alpha = float.IsPositiveInfinity(scores[i]) ? 0 : mask[i];
                        int output = i * 4 + outputDelta;
                        for(int c = 0; c < 3; c++) pixels[output + c] = (byte)((pixels[output + c] * alpha + 127) / 255);
                        pixels[output + 3] = (byte)alpha;
                    }
                }
                var result = new OverlayBlurFrame(pixels, outputWidth, outputHeight)
                {
                    PixelLease = lease,
                    Mode = OverlayBlurMode.Restore,
                    SourceRect = compactOutput ? work : Rectangle.Empty,
                    SourceSize = new System.Drawing.Size(sourceWidth, sourceHeight)
                };
                transferred = true;
                return result;
            }
            finally { if(!transferred) lease?.Release(); }
        }

        private static void FillDirection(byte[] source, int channels, int width, int height, byte[] mask, byte[] background,
            byte[] pixels, float[] scores, int dx, int dy, int sourceWidth, Point origin, bool parallel, int outputWidth, Point outputOrigin)
        {
            bool Inside(int x, int y) => (uint)x < (uint)width && (uint)y < (uint)height;
            float stepLength = MathF.Sqrt(dx * dx + dy * dy);
            int edgeRows = Math.Min(Math.Abs(dy), height);
            int sideColumns = Math.Min(dx, width);
            int edgeStarts = edgeRows * width;
            int startCount = edgeStarts + (height - edgeRows) * sideColumns;
            int pixelStep = dy * width + dx;
            int outputStep = (dy * outputWidth + dx) * 4;
            void FillRay(int start)
            {
                int startX, startY;
                if(start < edgeStarts)
                {
                    startX = start % width;
                    startY = start / width + (dy < 0 ? height - edgeRows : 0);
                }
                else
                {
                    start -= edgeStarts;
                    startX = start % sideColumns;
                    startY = start / sideColumns + (dy < 0 ? 0 : edgeRows);
                }
                int x = startX, y = startY;
                while(Inside(x, y))
                {
                    if(mask[y * width + x] == 0) { x += dx; y += dy; continue; }
                    int runX = x, runY = y, count = 0;
                    while(Inside(x, y) && mask[y * width + x] != 0) { x += dx; y += dy; count++; }
                    bool hasBefore = Inside(runX - dx, runY - dy), hasAfter = Inside(x, y);
                    if(!hasBefore && !hasAfter) continue;
                    Color first = hasBefore ? Sample(source, channels, width, height, mask, runX - dx, runY - dy, sourceWidth, origin) : Sample(source, channels, width, height, mask, x, y, sourceWidth, origin);
                    Color last = hasAfter ? Sample(source, channels, width, height, mask, x, y, sourceWidth, origin) : first;
                    float contrast = Math.Max(Math.Abs(first.R - last.R), Math.Max(Math.Abs(first.G - last.G), Math.Abs(first.B - last.B)));
                    float score = contrast + count * stepLength * 0.08f + (hasBefore && hasAfter ? 0 : 300);
                    int firstPixel = runY * width + runX;
                    int firstOutput = ((runY + outputOrigin.Y) * outputWidth + runX + outputOrigin.X) * 4;
                    for(int i = 0, pixel = firstPixel, output = firstOutput; i < count; i++, pixel += pixelStep, output += outputStep)
                    {
                        float t = (i + 1f) / (count + 1f);
                        int b = (int)MathF.Round(first.B + (last.B - first.B) * t);
                        int g = (int)MathF.Round(first.G + (last.G - first.G) * t);
                        int r = (int)MathF.Round(first.R + (last.R - first.R) * t);
                        int offset = pixel * 4;
                        //패널 테두리 양쪽의 같은 색을 잘못 연결하지 않도록 단어 안팎에서 공통인 배경색을 우선한다.
                        float candidateScore = score;
                        if(background[offset + 3] != 0)
                            candidateScore += 2 * Math.Max(Math.Abs(b - background[offset]), Math.Max(Math.Abs(g - background[offset + 1]), Math.Abs(r - background[offset + 2])));
                        if(candidateScore >= scores[pixel]) continue;
                        scores[pixel] = candidateScore;
                        pixels[output] = (byte)b; pixels[output + 1] = (byte)g; pixels[output + 2] = (byte)r;
                    }
                }
            }
            //한 방향의 광선은 서로 다른 픽셀만 쓴다. 방향 사이에는 완료를 기다려
            //동점 우선순위를 유지한다. 게임에 CPU 여유를 남기도록 최대 4개만 사용한다.
            int workers = Math.Clamp(Environment.ProcessorCount / 4, 1, 4);
            //영역이 줄어도 실제 글자 복원량은 같다. 원본 크기의 기존 병렬 기준을 유지한다.
            if(workers > 1 && parallel)
                Parallel.For(0, startCount, new ParallelOptions { MaxDegreeOfParallelism = workers }, FillRay);
            else
                for(int start = 0; start < startCount; start++) FillRay(start);
        }

        private static int GetPadding(Rectangle rect, int level)
        {
            int padding = Math.Clamp(Math.Min(rect.Width, rect.Height) / 4 + 4, 5, 20);
            //3은 기존 출력과 동일하다. 원문은 항상 덮고 외곽선·그림자 제거 범위만 조절한다.
            return Math.Max(1, (padding * (Math.Clamp(level, 1, 5) + 2) + 2) / 5);
        }

        private static void FindBackgroundHints(byte[] source, int channels, int width, int height, byte[] mask, IReadOnlyList<Rectangle> words, byte[] hints, int restorationLevel, int sourceWidth, Point origin)
        {
            Rectangle bounds = new Rectangle(0, 0, width, height);
            int Key(int offset) => (source[offset] >> 4) | ((source[offset + 1] >> 4) << 4) | ((source[offset + 2] >> 4) << 8);
            //호출마다 배열을 대여한다. 동시 OCR 작업은 서로 다른 배열을 사용한다.
            int[] histogram = ArrayPool<int>.Shared.Rent(4096 * 7);
            try
            {
                Array.Clear(histogram, 0, 4096 * 7);
                Span<int> inner = histogram.AsSpan(0, 4096), outer = histogram.AsSpan(4096, 4096);
                Span<int> blue = histogram.AsSpan(8192, 4096), green = histogram.AsSpan(12288, 4096), red = histogram.AsSpan(16384, 4096);
                Span<int> touched = histogram.AsSpan(20480, 4096), seen = histogram.AsSpan(24576, 4096);
                int touchedCount = 0;
                foreach(Rectangle word in words)
                {
                    if(word.Width <= 0 || word.Height <= 0) continue;
                    Rectangle rect = Rectangle.Intersect(word, bounds);
                    if(rect.Width <= 0 || rect.Height <= 0) continue;
                    int padding = GetPadding(rect, restorationLevel);
                    Rectangle probe = GetProbeBounds(rect, restorationLevel, bounds);
                    for(int index = 0; index < touchedCount; index++)
                    {
                        int key = touched[index];
                        inner[key] = outer[key] = blue[key] = green[key] = red[key] = seen[key] = 0;
                    }
                    touchedCount = 0;
                    int outerCount = 0;
                    for(int y = rect.Top; y < rect.Bottom; y++)
                    {
                        for(int x = rect.Left; x < rect.Right; x++)
                        {
                            int key = Key(((y + origin.Y) * sourceWidth + x + origin.X) * channels);
                            if(seen[key] == 0) { seen[key] = 1; touched[touchedCount++] = key; }
                            inner[key]++;
                        }
                    }
                    for(int y = probe.Top; y < probe.Bottom; y++)
                    {
                        for(int x = probe.Left; x < probe.Right; x++)
                        {
                            int pixel = y * width + x;
                            if(mask[pixel] != 0) continue;
                            int offset = ((y + origin.Y) * sourceWidth + x + origin.X) * channels, key = Key(offset);
                            if(seen[key] == 0) { seen[key] = 1; touched[touchedCount++] = key; }
                            outer[key]++; outerCount++;
                            blue[key] += source[offset]; green[key] += source[offset + 1]; red[key] += source[offset + 2];
                        }
                    }
                    //동점이면 작은 색상 키를 선택해 기존 4096칸 순회와 같은 결과를 유지한다.
                    int best = 0;
                    for(int index = 0; index < touchedCount; index++)
                    {
                        int key = touched[index];
                        long score = (long)outer[key] * inner[key], bestScore = (long)outer[best] * inner[best];
                        if(score > bestScore || (score == bestScore && key < best)) best = key;
                    }
                    if(outer[best] < 4 || outer[best] < outerCount * 0.25 || inner[best] < rect.Width * (long)rect.Height * 0.1)
                    {
                        //빽빽한 문단은 가까운 바깥 샘플도 다른 OCR 마스크에 가려진다.
                        //내부 최빈색이 충분히 반복될 때 방향 선택의 힌트로만 쓰고, 복원 색은 여전히 바깥에서 얻는다.
                        best = 0;
                        for(int index = 0; index < touchedCount; index++)
                        {
                            int key = touched[index];
                            if(inner[key] > inner[best] || (inner[key] == inner[best] && key < best)) best = key;
                        }
                        if(inner[best] < rect.Width * (long)rect.Height * 0.35) continue;
                        outer[best] = blue[best] = green[best] = red[best] = 0;
                        for(int y = rect.Top; y < rect.Bottom; y++)
                            for(int x = rect.Left; x < rect.Right; x++)
                            {
                                int offset = ((y + origin.Y) * sourceWidth + x + origin.X) * channels;
                                if(Key(offset) != best) continue;
                                outer[best]++;
                                blue[best] += source[offset]; green[best] += source[offset + 1]; red[best] += source[offset + 2];
                            }
                    }
                    byte hintBlue = (byte)(blue[best] / outer[best]);
                    byte hintGreen = (byte)(green[best] / outer[best]);
                    byte hintRed = (byte)(red[best] / outer[best]);
                    Rectangle area = Rectangle.Intersect(Rectangle.Inflate(rect, padding + 2, padding + 2), bounds);
                    for(int y = area.Top; y < area.Bottom; y++)
                        for(int x = area.Left; x < area.Right; x++)
                        {
                            int offset = (y * width + x) * 4;
                            hints[offset] = hintBlue;
                            hints[offset + 1] = hintGreen;
                            hints[offset + 2] = hintRed;
                            hints[offset + 3] = 255;
                        }
                }
            }
            finally { ArrayPool<int>.Shared.Return(histogram); }
        }

        private static Color Sample(byte[] source, int channels, int width, int height, byte[] mask, int x, int y, int sourceWidth, Point origin)
        {
            //인접한 OCR 줄이나 단어도 제외한 경계 픽셀의 중앙값을 사용한다.
            Span<byte> red = stackalloc byte[9], green = stackalloc byte[9], blue = stackalloc byte[9];
            int count = 0;
            for(int ny = Math.Max(0, y - 1); ny <= Math.Min(height - 1, y + 1); ny++)
            {
                for(int nx = Math.Max(0, x - 1); nx <= Math.Min(width - 1, x + 1); nx++)
                {
                    int pixel = ny * width + nx;
                    if(mask[pixel] != 0) continue;
                    int offset = ((ny + origin.Y) * sourceWidth + nx + origin.X) * channels;
                    blue[count] = source[offset]; green[count] = source[offset + 1]; red[count] = source[offset + 2]; count++;
                }
            }
            red = red[..count]; green = green[..count]; blue = blue[..count];
            red.Sort(); green.Sort(); blue.Sort();
            return Color.FromArgb(red[count / 2], green[count / 2], blue[count / 2]);
        }
    }
}
