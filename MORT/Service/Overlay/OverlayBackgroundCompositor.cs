using System;
using System.Drawing;
using MORT.Model;

namespace MORT.Service.Overlay
{
    //UI 전용. 낮은 해상도 결과의 실제 알파 구간만 최종 PArgb DIB에 합성한다.
    //호출자는 GDI+ 작업을 Flush하고 프레임 읽기 소유권을 유지해야 한다.
    internal sealed class OverlayBackgroundCompositor
    {
        private OverlayBlurFrame _frame;
        private int[] _rows = Array.Empty<int>();
        private SpanRange[] _spans = Array.Empty<SpanRange>();
        private int _spanCount;
        private int[] _x0 = Array.Empty<int>(), _x1 = Array.Empty<int>(), _wx = Array.Empty<int>();
        private int[] _y0 = Array.Empty<int>(), _y1 = Array.Empty<int>(), _wy = Array.Empty<int>();
        private Rectangle _clip;
        private RectangleF _destination;
        private Size _sourceSize;
        private struct SpanRange { internal int Left, Right; }

        internal unsafe void Draw(LayeredWindowSurface surface, OverlayBlurFrame frame, RectangleF destination, Rectangle clip)
        {
            if(frame.Width <= 0 || frame.Height <= 0 || (long)frame.Width * frame.Height * 4 > frame.Pixels.LongLength)
                throw new ArgumentException("Invalid restoration frame.", nameof(frame));
            if(surface.Pixels == IntPtr.Zero || destination.Width <= 0 || destination.Height <= 0) return;
            clip = Rectangle.Intersect(clip, new Rectangle(Point.Empty, surface.Image.Size));
            clip = Rectangle.Intersect(clip, Rectangle.FromLTRB(
                (int)Math.Ceiling(destination.Left - 0.5), (int)Math.Ceiling(destination.Top - 0.5),
                (int)Math.Ceiling(destination.Right - 0.5), (int)Math.Ceiling(destination.Bottom - 0.5)));
            if(clip.Width <= 0 || clip.Height <= 0) return;
            if(!ReferenceEquals(_frame, frame)) BuildSpans(frame);
            var size = new Size(frame.Width, frame.Height);
            if(_sourceSize != size || _destination != destination || _clip != clip)
            {
                MakeAxis(frame.Width, destination.X, destination.Width, clip.Left, clip.Width, ref _x0, ref _x1, ref _wx);
                MakeAxis(frame.Height, destination.Y, destination.Height, clip.Top, clip.Height, ref _y0, ref _y1, ref _wy);
                _sourceSize = size; _destination = destination; _clip = clip;
            }
            fixed(byte* sourceBytes = frame.Pixels)
            {
                uint* source = (uint*)sourceBytes;
                uint* target = (uint*)surface.Pixels;
                double scaleX = destination.Width / frame.Width;
                for(int row = 0; row < clip.Height; row++)
                {
                    int top = _y0[row], bottom = _y1[row];
                    int i = top < 0 ? 0 : _rows[top], endI = top < 0 ? 0 : _rows[top + 1];
                    int j = bottom < 0 || bottom == top ? 0 : _rows[bottom];
                    int endJ = bottom < 0 || bottom == top ? 0 : _rows[bottom + 1];
                    int left = -1, right = -1;
                    while(i < endI || j < endJ)
                    {
                        var span = j >= endJ || (i < endI && _spans[i].Left <= _spans[j].Left) ? _spans[i++] : _spans[j++];
                        //필터가 참조하는 이웃까지 포함하고 겹친 구간은 한 번만 합성한다.
                        int start = Math.Max(clip.Left, (int)Math.Floor(destination.X + (span.Left - 1) * scaleX));
                        int end = Math.Min(clip.Right, (int)Math.Ceiling(destination.X + (span.Right + 1) * scaleX));
                        if(start >= end) continue;
                        if(left < 0) { left = start; right = end; }
                        else if(start <= right) right = Math.Max(right, end);
                        else
                        {
                            DrawSpan(source, target, surface.Image.Width, frame.Width, row, left, right);
                            left = start; right = end;
                        }
                    }
                    if(left >= 0) DrawSpan(source, target, surface.Image.Width, frame.Width, row, left, right);
                }
            }
        }

        private unsafe void DrawSpan(uint* source, uint* target, int targetWidth, int sourceWidth, int row, int left, int right)
        {
            int y0 = _y0[row], y1 = _y1[row], fy = _wy[row];
            uint* output = target + (_clip.Y + row) * targetWidth + left;
            for(int x = left; x < right; x++, output++)
            {
                int index = x - _clip.X, x0 = _x0[index], x1 = _x1[index], fx = _wx[index];
                uint a = y0 < 0 || x0 < 0 ? 0 : source[y0 * sourceWidth + x0];
                uint b = y0 < 0 || x1 < 0 ? 0 : source[y0 * sourceWidth + x1];
                uint c = y1 < 0 || x0 < 0 ? 0 : source[y1 * sourceWidth + x0];
                uint d = y1 < 0 || x1 < 0 ? 0 : source[y1 * sourceWidth + x1];
                uint color = Lerp(Lerp(a, b, fx), Lerp(c, d, fx), fy);
                uint alpha = color >> 24;
                if(alpha == 0) continue;
                if(alpha == 255) { *output = color; continue; }
                *output = color + Attenuate(*output, 255 - alpha);
            }
        }

        private static uint Lerp(uint a, uint b, int weight)
        {
            uint t = (uint)weight, inverse = 256 - t;
            uint rb = (((a & 0x00ff00ff) * inverse + (b & 0x00ff00ff) * t + 0x00800080) >> 8) & 0x00ff00ff;
            uint ag = ((((a >> 8) & 0x00ff00ff) * inverse + ((b >> 8) & 0x00ff00ff) * t + 0x00800080) >> 8) & 0x00ff00ff;
            return rb | (ag << 8);
        }

        private static uint Attenuate(uint color, uint alpha)
        {
            uint rb = (color & 0x00ff00ff) * alpha + 0x00800080;
            uint ag = ((color >> 8) & 0x00ff00ff) * alpha + 0x00800080;
            rb = ((rb + ((rb >> 8) & 0x00ff00ff)) >> 8) & 0x00ff00ff;
            ag = ((ag + ((ag >> 8) & 0x00ff00ff)) >> 8) & 0x00ff00ff;
            return rb | (ag << 8);
        }

        private void BuildSpans(OverlayBlurFrame frame)
        {
            if(_rows.Length < frame.Height + 1) _rows = new int[frame.Height + 1];
            _spanCount = 0;
            for(int y = 0; y < frame.Height; y++)
            {
                _rows[y] = _spanCount;
                int x = 0, row = y * frame.Width * 4;
                while(x < frame.Width)
                {
                    while(x < frame.Width && frame.Pixels[row + x * 4 + 3] == 0) x++;
                    int left = x;
                    while(x < frame.Width && frame.Pixels[row + x * 4 + 3] != 0) x++;
                    if(left == x) continue;
                    if(_spans.Length == _spanCount) Array.Resize(ref _spans, Math.Max(64, _spanCount * 2));
                    _spans[_spanCount++] = new SpanRange { Left = left, Right = x };
                }
            }
            _rows[frame.Height] = _spanCount;
            _frame = frame;
        }

        private static void MakeAxis(int sourceLength, double origin, double length, int start, int count,
            ref int[] first, ref int[] second, ref int[] weight)
        {
            if(first.Length < count) { first = new int[count]; second = new int[count]; weight = new int[count]; }
            for(int i = 0; i < count; i++)
            {
                double position = (start + i + 0.5 - origin) * sourceLength / length - 0.5;
                int index = (int)Math.Floor(position);
                first[i] = (uint)index < (uint)sourceLength ? index : -1;
                second[i] = (uint)(index + 1) < (uint)sourceLength ? index + 1 : -1;
                weight[i] = Math.Clamp((int)((position - index) * 256 + 0.5), 0, 256);
            }
        }
    }
}
