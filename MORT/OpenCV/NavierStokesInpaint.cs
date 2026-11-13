// C# adaptation of OpenCV 4.14.0 modules/photo/src/inpaint.cpp (8-bit BGR, NS radius 2).
// Original copyright/license text is retained in SOURCE-LICENSES.txt; see NOTICE.txt.
// MORT changes: reusable flat buffers/heap, BGRA working stride, precomputed disk stencil.
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace MORT.OpenCV
{
    internal sealed class NavierStokesInpaint : IDisposable
    {
        private const byte Known = 0, Band = 1, Inside = 2;
        private byte[] _state = Array.Empty<byte>(), _result = Array.Empty<byte>();
        private float[] _time = Array.Empty<float>();
        private Entry[] _heap = Array.Empty<Entry>();
        private int _count, _order;
        private struct Entry { internal int Index, Order; internal float Distance; }
        //k/l scan order must match upstream, including ties in the marching heap.
        private static readonly (int Y, int X, float Weight)[] Stencil =
        {
            (-2,0,1f/17),(-1,-1,1f/5),(-1,0,1f/2),(-1,1,1f/5),
            (0,-2,1f/17),(0,-1,1f/2),(0,1,1f/2),(0,2,1f/17),
            (1,-1,1f/5),(1,0,1f/2),(1,1,1f/5),(2,0,1f/17)
        };

        internal unsafe byte[] Restore(byte* source, int sourceStride, int channels,
            byte* mask, int maskStride, int width, int height)
        {
            int stride = width + 2, length = checked(stride * (height + 2));
            BackgroundRestorer.Ensure(ref _state, length);
            BackgroundRestorer.Ensure(ref _time, length);
            BackgroundRestorer.Ensure(ref _result, checked(width * height * 4));
            _state.AsSpan(0, length).Clear();
            _time.AsSpan(0, length).Fill(1e6f);
            _count = _order = 0;
            fixed(byte* state = _state, result = _result)
            fixed(float* time = _time)
            {
                for(int y = 0; y < height; y++)
                {
                    byte* input = source + y * sourceStride, output = result + y * width * 4;
                    byte* masked = mask + y * maskStride, target = state + (y + 1) * stride + 1;
                    if(channels == 4) Buffer.MemoryCopy(input, output, width * 4L, width * 4L);
                    for(int x = 0; x < width; x++)
                    {
                        if(channels == 3)
                        {
                            output[x*4] = input[x*3]; output[x*4+1] = input[x*3+1]; output[x*4+2] = input[x*3+2];
                        }
                        if(masked[x] != 0) target[x] = Inside;
                    }
                }
                //Upstream initializes the heap from cross-dilate(mask) minus mask,
                //in row-major order. NS then operates on mask, not the unused f Mat.
                for(int y = 1; y <= height; y++)
                for(int x = 1; x <= width; x++)
                {
                    int p = y * stride + x;
                    if(state[p] == Inside) continue;
                    if(state[p-stride] == Inside || state[p-1] == Inside || state[p+stride] == Inside || state[p+1] == Inside)
                    {
                        time[p] = 0;
                        Push(p, 0);
                    }
                }
                while(_count > 0)
                {
                    int p = Pop();
                    state[p] = Known;
                    for(int q = 0; q < 4; q++)
                    {
                        int next = p + (q == 0 ? -stride : q == 1 ? -1 : q == 2 ? stride : 1);
                        if(state[next] != Inside) continue; //The padded border is always Known.
                        float distance = Math.Min(Math.Min(Solve(next-stride,next-1,state,time), Solve(next+stride,next-1,state,time)),
                            Math.Min(Solve(next-stride,next+1,state,time), Solve(next+stride,next+1,state,time)));
                        time[next] = distance;
                        if(Sse41.IsSupported) FillVector(next / stride, next % stride, state, result, width, height, stride);
                        else Fill(next / stride, next % stride, state, result, width, height, stride);
                        state[next] = Band;
                        Push(next, distance);
                    }
                }
            }
            return _result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe float Solve(int first, int second, byte* state, float* time)
        {
            double a = time[first], b = time[second], minimum = Math.Min(a, b);
            if(state[first] != Inside)
            {
                if(state[second] == Inside) return (float)(1 + a);
                double difference = a - b;
                return (float)(Math.Abs(difference) >= 1 ? 1 + minimum : (a + b + Math.Sqrt(2 - difference * difference)) * 0.5);
            }
            return (float)(1 + (state[second] != Inside ? b : minimum));
        }

        private static unsafe void Fill(int y, int x, byte* state, byte* result, int width, int height, int stride)
        {
            float* sums = stackalloc float[6];
            sums[0] = sums[1] = sums[2] = 0;
            sums[3] = sums[4] = sums[5] = 1e-20f;
            foreach(var sample in Stencil)
            {
                int k = y + sample.Y, l = x + sample.X;
                if(k <= 0 || l <= 0 || k > height || l > width) continue;
                int position = k * stride + l;
                if(state[position] == Inside) continue;
                int km = k - 1 + (k == 1 ? 1 : 0), kp = k - 1 - (k == height ? 1 : 0);
                int lm = l - 1 + (l == 1 ? 1 : 0), lp = l - 1 - (l == width ? 1 : 0);
                int xa = (kp * width + lm) * 4, xb = ((kp + 1) * width + lm) * 4, xc = ((km - 1) * width + lm) * 4;
                int ya = (km * width + lm) * 4, yb = (km * width + lp + 1) * 4, yc = (km * width + lm - 1) * 4;
                bool down = state[position+stride] != Inside, up = state[position-stride] != Inside;
                bool right = state[position+1] != Inside, left = state[position-1] != Inside;
                int input = ((k - 1) * width + l - 1) * 4;
                float dx = sample.X, dy = sample.Y, squaredDistance = dx * dx + dy * dy;
                for(int color = 0; color < 3; color++)
                {
                    float gx = down ? (up ? Math.Abs(result[xb+color] - result[xa+color]) + Math.Abs(result[xa+color] - result[xc+color])
                        : Math.Abs(result[xb+color] - result[xa+color]) * 2) : (up ? Math.Abs(result[xa+color] - result[xc+color]) * 2 : 0);
                    float gy = right ? (left ? Math.Abs(result[yb+color] - result[ya+color]) + Math.Abs(result[ya+color] - result[yc+color])
                        : Math.Abs(result[yb+color] - result[ya+color]) * 2) : (left ? Math.Abs(result[ya+color] - result[yc+color]) * 2 : 0);
                    gx = -gx;
                    float direction = dx * gx + dy * gy;
                    direction = Math.Abs(direction) <= 0.01 ? 0.000001f
                        : Math.Abs(direction / MathF.Sqrt(squaredDistance * (gx * gx + gy * gy)));
                    float weight = sample.Weight * direction;
                    sums[color] += weight * result[input+color];
                    sums[color+3] += weight;
                }
            }
            int target = ((y - 1) * width + x - 1) * 4;
            for(int color = 0; color < 3; color++)
                result[target+color] = (byte)Math.Clamp((int)Math.Round((double)sums[color] / sums[color+3]), 0, 255);
        }

        //Each SIMD lane is one color channel; neighbor accumulation order and
        //float rounding remain identical to the scalar/OpenCV path. Alpha is ignored.
        private static unsafe void FillVector(int y, int x, byte* state, byte* result, int width, int height, int stride)
        {
            var sums = Vector128<float>.Zero;
            var weights = Vector128.Create(1e-20f);
            var sign = Vector128.Create(-0.0f);
            var two = Vector128.Create(2f);
            foreach(var sample in Stencil)
            {
                int k = y + sample.Y, l = x + sample.X;
                if(k <= 0 || l <= 0 || k > height || l > width) continue;
                int position = k * stride + l;
                if(state[position] == Inside) continue;
                int km = k - 1 + (k == 1 ? 1 : 0), kp = k - 1 - (k == height ? 1 : 0);
                int lm = l - 1 + (l == 1 ? 1 : 0), lp = l - 1 - (l == width ? 1 : 0);
                bool down = state[position+stride] != Inside, up = state[position-stride] != Inside;
                bool right = state[position+1] != Inside, left = state[position-1] != Inside;
                var gx = Vector128<float>.Zero;
                var gy = Vector128<float>.Zero;
                if(down || up)
                {
                    var center = Colors(result + (kp * width + lm) * 4);
                    if(down) gx = Sse.AndNot(sign, Sse.Subtract(Colors(result + ((kp + 1) * width + lm) * 4), center));
                    if(up) gx = Sse.Add(gx, Sse.AndNot(sign, Sse.Subtract(center, Colors(result + ((km - 1) * width + lm) * 4))));
                    if(!(down && up)) gx = Sse.Multiply(gx, two);
                }
                if(right || left)
                {
                    var center = Colors(result + (km * width + lm) * 4);
                    if(right) gy = Sse.AndNot(sign, Sse.Subtract(Colors(result + (km * width + lp + 1) * 4), center));
                    if(left) gy = Sse.Add(gy, Sse.AndNot(sign, Sse.Subtract(center, Colors(result + (km * width + lm - 1) * 4))));
                    if(!(right && left)) gy = Sse.Multiply(gy, two);
                }
                gx = Sse.Xor(gx, sign);
                var direction = Sse.Add(Sse.Multiply(gx, Vector128.Create((float)sample.X)), Sse.Multiply(gy, Vector128.Create((float)sample.Y)));
                var norm = Sse.Sqrt(Sse.Multiply(Vector128.Create((float)(sample.X * sample.X + sample.Y * sample.Y)),
                    Sse.Add(Sse.Multiply(gx, gx), Sse.Multiply(gy, gy))));
                var small = Sse.CompareLessThanOrEqual(Sse.AndNot(sign, direction), Vector128.Create(0.01f));
                direction = Sse.Or(Sse.And(small, Vector128.Create(0.000001f)), Sse.AndNot(small, Sse.AndNot(sign, Sse.Divide(direction, norm))));
                var weight = Sse.Multiply(direction, Vector128.Create(sample.Weight));
                sums = Sse.Add(sums, Sse.Multiply(weight, Colors(result + ((k - 1) * width + l - 1) * 4)));
                weights = Sse.Add(weights, weight);
            }
            int target = ((y - 1) * width + x - 1) * 4;
            for(int color = 0; color < 3; color++)
                result[target+color] = (byte)Math.Clamp((int)Math.Round((double)sums.GetElement(color) / weights.GetElement(color)), 0, 255);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe Vector128<float> Colors(byte* pixel) => Sse2.ConvertToVector128Single(Sse41.ConvertToVector128Int32(pixel));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Before(Entry a, Entry b) => a.Distance < b.Distance || (a.Distance == b.Distance && a.Order < b.Order);

        private void Push(int index, float distance)
        {
            if(_count == _heap.Length) Array.Resize(ref _heap, Math.Max(64, _heap.Length * 2));
            var entry = new Entry { Index = index, Distance = distance, Order = _order++ };
            int position = _count++;
            while(position > 0)
            {
                int parent = (position - 1) / 2;
                if(!Before(entry, _heap[parent])) break;
                _heap[position] = _heap[parent]; position = parent;
            }
            _heap[position] = entry;
        }

        private int Pop()
        {
            int result = _heap[0].Index;
            var last = _heap[--_count];
            int position = 0;
            while(position * 2 + 1 < _count)
            {
                int child = position * 2 + 1;
                if(child + 1 < _count && Before(_heap[child+1], _heap[child])) child++;
                if(!Before(_heap[child], last)) break;
                _heap[position] = _heap[child]; position = child;
            }
            if(_count > 0) _heap[position] = last;
            return result;
        }

        public void Dispose()
        {
            _state = _result = Array.Empty<byte>(); _time = Array.Empty<float>(); _heap = Array.Empty<Entry>();
            _count = _order = 0;
        }
    }
}
