using OpenTK.Graphics;
using TrueTypeSharp;
using Processing.OpenTk.Core.Extensions;
using Processing.OpenTk.Core.Textures;
using ImageSharp;
using System;

namespace Processing.OpenTk.Core
{
    public class FontGenerator
    {
        private readonly TrueTypeFont ttfont;

        public FontGenerator(TrueTypeFont font)
        {
            ttfont = font;
        }

        public PImage this[char ch, float size, Color4 color]
        {
            get
            {                
                float scale = ttfont.GetScaleForPixelHeight(size);
                int width, height, xOffset, yOffset;
                byte[] data = ttfont.GetCodepointBitmap(ch, scale, scale, out width, out height, out xOffset, out yOffset);

                int[] pixelData = new int[width * height * 4];

                int FlatIndex(int x, int y) => y * width + x;

                byte scalarAt(int x, int y) => data[FlatIndex(x, y)];

                float alphaAt(int x, int y) => (scalarAt(x, y) * color.A) / 255f;

                Color4 colorAt(int x, int y) => new Color4(color.R, color.G, color.B, alphaAt(x, y));

                int packedColorAt(int x, int y) => colorAt(x, y).ToRgbaIntegerFormat();

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var index = x + (y * height);
                        if (data[FlatIndex(x, y)] == 0)
                            pixelData[index] = Color4.Transparent.ToRgbaIntegerFormat();
                        else
                        {
                            var scalar = scalarAt(x, y);
                            var alpha = alphaAt(x, y);
                            var localColor = colorAt(x, y);
                            pixelData[index] = packedColorAt(x, y);
                        }
                    }
                }
                
                return PImage.FromImage(Image<Rgba32>.FromInts(pixelData, width, height));                
            }
        }
    }
}
