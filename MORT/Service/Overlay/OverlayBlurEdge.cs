using System;
using System.Drawing;

namespace MORT.Service.Overlay
{
    //블러의 색은 바꾸지 않고 가장자리만 원래 화면으로 자연스럽게 이어 준다.
    internal static class OverlayBlurEdge
    {
        public static float Feather(Size size) => Math.Clamp(Math.Min(size.Width, size.Height) * 0.12f, 1f, 6f);
        public static float Radius(Size size) => Math.Min(Math.Min(size.Width, size.Height) / 2f, Feather(size) * 1.5f);

        public static float Smooth(float value)
        {
            value = Math.Clamp(value, 0f, 1f);
            return value * value * (3f - 2f * value);
        }

        public static float Opacity(float x, float y, Size size)
        {
            float radius = Radius(size);
            float dx = Math.Abs(x - size.Width / 2f) - (size.Width / 2f - radius);
            float dy = Math.Abs(y - size.Height / 2f) - (size.Height / 2f - radius);
            float outsideX = Math.Max(dx, 0);
            float outsideY = Math.Max(dy, 0);
            float distance = MathF.Sqrt(outsideX * outsideX + outsideY * outsideY)
                + Math.Min(Math.Max(dx, dy), 0) - radius;
            return Smooth(-distance / Feather(size));
        }
    }
}
