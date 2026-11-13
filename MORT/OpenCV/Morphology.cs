using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace MORT.OpenCV
{
    //Separable rectangular min/max filters with replicated, isolated ROI borders.
    //SIMD works on contiguous pixels; scalar tails handle narrow words and edges.
    internal static class Morphology
    {
        internal static unsafe void Extreme(byte* source, int stride, byte* output, byte* temporary,
            int width, int height, int radius, bool maximum)
        {
            int lanes = Vector<byte>.Count;
            for(int y = 0; y < height; y++)
            {
                byte* row = source + y * stride, target = temporary + y * width;
                int x = 0;
                for(; x < Math.Min(radius, width); x++) target[x] = Edge(row, x, width, radius, maximum);
                for(; x <= width - radius - lanes; x += lanes)
                {
                    var value = Unsafe.ReadUnaligned<Vector<byte>>(row + x - radius);
                    for(int k = 1; k <= radius * 2; k++)
                    {
                        var next = Unsafe.ReadUnaligned<Vector<byte>>(row + x - radius + k);
                        value = maximum ? Vector.Max(value, next) : Vector.Min(value, next);
                    }
                    Unsafe.WriteUnaligned(target + x, value);
                }
                for(; x < width; x++) target[x] = Edge(row, x, width, radius, maximum);
            }
            for(int y = 0; y < height; y++)
            {
                byte* target = output + y * width;
                int firstRow = Math.Max(0, y - radius), lastRow = Math.Min(height - 1, y + radius), x = 0;
                for(; x <= width - lanes; x += lanes)
                {
                    var value = Unsafe.ReadUnaligned<Vector<byte>>(temporary + firstRow * width + x);
                    for(int k = firstRow + 1; k <= lastRow; k++)
                    {
                        var next = Unsafe.ReadUnaligned<Vector<byte>>(temporary + k * width + x);
                        value = maximum ? Vector.Max(value, next) : Vector.Min(value, next);
                    }
                    Unsafe.WriteUnaligned(target + x, value);
                }
                for(; x < width; x++)
                {
                    int value = temporary[firstRow * width + x];
                    for(int k = firstRow + 1; k <= lastRow; k++)
                        value = maximum ? Math.Max(value, temporary[k * width + x]) : Math.Min(value, temporary[k * width + x]);
                    target[x] = (byte)value;
                }
            }
        }

        private static unsafe byte Edge(byte* row, int x, int width, int radius, bool maximum)
        {
            int left = Math.Max(0, x - radius), right = Math.Min(width - 1, x + radius), value = row[left];
            for(int k = left + 1; k <= right; k++) value = maximum ? Math.Max(value, row[k]) : Math.Min(value, row[k]);
            return (byte)value;
        }
    }
}
