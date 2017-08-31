using OpenTK.Graphics;

namespace Processing.OpenTk.Core.Extensions
{
    public static class Color4Extensions
    {
        public static int ToRgbaIntegerFormat(this Color4 color)
        {
            byte ToByte(float f) => (byte)(0xFF * f);

            return ToByte(color.R) << 24 | ToByte(color.G) << 16 | ToByte(color.B) << 8 | ToByte(color.A);
        }
    }
}
