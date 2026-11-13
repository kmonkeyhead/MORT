using System;
using System.Drawing;

namespace MORT.OpenCV
{
    //MORT's OCR-mask pipeline. All image operations run in C#; no native binding.
    //OpenCV 4.14 arithmetic/provenance and notices: NOTICE.txt, SOURCE-LICENSES.txt.
    internal sealed class BackgroundRestorer : IDisposable
    {
        //Covers 2 * max morphology radius + detection padding (16).
        //Mask expansion must also leave enough detection/repair context within this padding.
        internal const int InputPadding = 16;
        private const int DetectionPadding = 2, RepairPadding = 6;
        internal struct Region
        {
            internal int X, Y, Width, Height;
            internal Region(int x, int y, int width, int height) { X = x; Y = y; Width = width; Height = height; }
        }

        private byte[] _gray = Array.Empty<byte>(), _mask = Array.Empty<byte>(), _expanded = Array.Empty<byte>();
        private byte[] _temporary = Array.Empty<byte>(), _intermediate = Array.Empty<byte>();
        private byte[] _opened = Array.Empty<byte>(), _closed = Array.Empty<byte>(), _detail = Array.Empty<byte>();
        private readonly int[] _histogram = new int[256];
        private readonly NavierStokesInpaint _inpaint = new();

        internal static void Ensure<T>(ref T[] buffer, int length)
        {
            if(buffer.Length < length)
            {
                //Amortize small successive OCR-context growth instead of allocating
                //a slightly larger array on every change; only scratch data is lost.
                int capacity = (int)Math.Max(length, Math.Min(Array.MaxLength, buffer.Length + (long)buffer.Length / 2));
                buffer = new T[capacity];
            }
        }

        internal unsafe void Restore(byte* source, int width, int height, int channels, int sourceStride,
            Region[] words, byte* output, int outputStride, int[] rowSpans, int maskExpansion)
        {
            if(maskExpansion < 1 || maskExpansion > InputPadding - DetectionPadding - RepairPadding)
                throw new ArgumentOutOfRangeException(nameof(maskExpansion));
            if(width <= 0 || height <= 0 || (channels != 3 && channels != 4)
                || (long)sourceStride < (long)width * channels || (long)outputStride < (long)width * 4
                || rowSpans.Length < (long)height * 2)
                throw new ArgumentException("Invalid background image layout.");
            int length = checked(width * height);
            Ensure(ref _gray, length); Ensure(ref _mask, length); Ensure(ref _expanded, length);
            Ensure(ref _temporary, length);
            _mask.AsSpan(0, length).Clear(); //Content changes each frame; retain the allocation.
            fixed(byte* gray = _gray, mask = _mask, expanded = _expanded)
            {
                for(int y = 0; y < height; y++)
                {
                    byte* input = source + y * sourceStride, target = gray + y * width;
                    for(int x = 0; x < width; x++, input += channels)
                        target[x] = (byte)((input[0] * 3735 + input[1] * 19235 + input[2] * 9798 + 16384) >> 15);
                }
                var bounds = new Rectangle(0, 0, width, height);
                foreach(var word in words)
                {
                    if(word.Width <= 0 || word.Height <= 0) continue;
                    var box = Rectangle.Intersect(new Rectangle(word.X, word.Y, word.Width, word.Height), bounds);
                    if(box.Width <= 0 || box.Height <= 0) continue;
                    int radius = Math.Clamp((box.Height / 3) | 1, 3, 15) / 2;
                    int contextPadding = radius * 2 + DetectionPadding;
                    var context = Rectangle.Intersect(Rectangle.Inflate(box, contextPadding, contextPadding), bounds);
                    int count = context.Width * context.Height;
                    Ensure(ref _intermediate, count); Ensure(ref _opened, count); Ensure(ref _closed, count);
                    fixed(byte* temporary = _temporary, middle = _intermediate, opened = _opened, closed = _closed)
                    {
                        byte* local = gray + context.Y * width + context.X;
                        Morphology.Extreme(local, width, middle, temporary, context.Width, context.Height, radius, false);
                        Morphology.Extreme(middle, context.Width, opened, temporary, context.Width, context.Height, radius, true);
                        Morphology.Extreme(local, width, middle, temporary, context.Width, context.Height, radius, true);
                        Morphology.Extreme(middle, context.Width, closed, temporary, context.Width, context.Height, radius, false);
                        var detection = Rectangle.Intersect(Rectangle.Inflate(box, DetectionPadding, DetectionPadding), bounds);
                        Ensure(ref _detail, detection.Width * detection.Height);
                        Array.Clear(_histogram);
                        int index = 0;
                        for(int y = detection.Top; y < detection.Bottom; y++)
                        {
                            int offset = (y - context.Y) * context.Width + detection.X - context.X;
                            byte* original = gray + y * width + detection.X;
                            for(int x = 0; x < detection.Width; x++, offset++)
                            {
                                int detail = Math.Max(0, Math.Max(original[x] - opened[offset], closed[offset] - original[x]));
                                _detail[index++] = (byte)detail;
                                _histogram[detail]++;
                            }
                        }
                        int threshold = Math.Max(8, Otsu(_histogram, index) / 2);
                        index = 0;
                        for(int y = detection.Top; y < detection.Bottom; y++)
                        {
                            byte* target = mask + y * width + detection.X;
                            for(int x = 0; x < detection.Width; x++)
                                if(_detail[index++] > threshold) target[x] = 255;
                        }
                    }
                }
                fixed(byte* temporary = _temporary)
                    Morphology.Extreme(mask, width, expanded, temporary, width, height, maskExpansion, true);
                new Span<byte>(expanded, width).Clear();
                new Span<byte>(expanded + (height - 1) * width, width).Clear();
                int left = width, right = 0, top = height, bottom = 0;
                for(int y = 1; y < height - 1; y++)
                {
                    byte* row = expanded + y * width;
                    row[0] = row[width - 1] = 0;
                    for(int x = 1; x < width - 1; x++)
                        if(row[x] != 0) { left = Math.Min(left, x); right = Math.Max(right, x + 1); top = Math.Min(top, y); bottom = y + 1; }
                }
                Array.Clear(rowSpans, 0, height * 2);
                if(right == 0) return;
                var repair = Rectangle.Intersect(Rectangle.FromLTRB(left - RepairPadding, top - RepairPadding,
                    right + RepairPadding, bottom + RepairPadding), bounds);
                byte[] result = _inpaint.Restore(source + repair.Y * sourceStride + repair.X * channels,
                    sourceStride, channels, expanded + repair.Y * width + repair.X, width, repair.Width, repair.Height);
                //All computation completes before touching the published/pool output.
                fixed(byte* restored = result)
                for(int y = top; y < bottom; y++)
                {
                    int first = width, end = 0;
                    byte* target = output + y * outputStride, row = expanded + y * width;
                    byte* input = restored + (y - repair.Y) * repair.Width * 4;
                    for(int x = left; x < right; x++)
                    {
                        if(row[x] == 0) continue;
                        byte* pixel = input + (x - repair.X) * 4;
                        target[x * 4] = pixel[0]; target[x * 4 + 1] = pixel[1]; target[x * 4 + 2] = pixel[2]; target[x * 4 + 3] = 255;
                        first = Math.Min(first, x); end = x + 1;
                    }
                    rowSpans[y * 2] = end == 0 ? 0 : first; rowSpans[y * 2 + 1] = end;
                }
            }
        }

        //OpenCV thresh.cpp's 8-bit Otsu arithmetic, with an already-built histogram.
        private static int Otsu(int[] histogram, int count)
        {
            const double epsilon = 1.1920928955078125e-7; //FLT_EPSILON
            double scale = 1.0 / count, mean = 0;
            for(int i = 0; i < 256; i++) mean += i * (double)histogram[i];
            mean *= scale;
            double firstMean = 0, firstWeight = 0, bestVariance = 0;
            int threshold = 0;
            for(int i = 0; i < 256; i++)
            {
                double probability = histogram[i] * scale;
                firstMean *= firstWeight;
                firstWeight += probability;
                double secondWeight = 1 - firstWeight;
                if(Math.Min(firstWeight, secondWeight) < epsilon || Math.Max(firstWeight, secondWeight) > 1 - epsilon) continue;
                firstMean = (firstMean + i * probability) / firstWeight;
                double secondMean = (mean - firstWeight * firstMean) / secondWeight;
                double variance = firstWeight * secondWeight * (firstMean - secondMean) * (firstMean - secondMean);
                if(variance > bestVariance) { bestVariance = variance; threshold = i; }
            }
            return threshold;
        }

        public void Dispose()
        {
            _gray = _mask = _expanded = _temporary = _intermediate = _opened = _closed = _detail = Array.Empty<byte>();
            _inpaint.Dispose();
        }
    }
}
