using OpenTK.Graphics;
using TrueTypeSharp;
using Processing.OpenTk.Core.Extensions;
using Processing.OpenTk.Core.Textures;

namespace Processing.OpenTk.Core
{
    public class FontGenerator
    {
        private readonly TrueTypeFont ttfont;

        public FontGenerator(TrueTypeFont font)
        {
            ttfont = font;
        }

        public Texture2d this[char ch, float size, Color4 color]
        {
            get
            {
                
                float scale = ttfont.GetScaleForPixelHeight(size);
                int width, height, xOffset, yOffset;
                byte[] data = ttfont.GetCodepointBitmap(ch, scale, scale, out width, out height, out xOffset, out yOffset);

                int[,] pixelData = new int[width, height];

                int FlatIndex(int x, int y) => y * width + x;

                float OpacityAt(int x, int y) => color.A * (data[FlatIndex(x, y)] / 255f);

                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        pixelData[x, y] =  new Color4(color.R, color.G, color.B, OpacityAt(x, y)).ToRgbaIntegerFormat();

                Texture2d texture = new Texture2d(pixelData);

                return texture;
            }
        }
    }
}
