using Processing.OpenTk.Core;
using Processing.OpenTk.Core.Textures;
using System;
using TrueTypeSharp;

namespace Processing.OpenTk.Runner
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            TrueTypeFont ttfont = FontPack.Repository.Load("TimesNewRoman.ttf");

            float size = 80;

            char ch = 'a';

            float scale = ttfont.GetScaleForPixelHeight(size);
            int width, height, xOffset, yOffset;
            byte[] data = ttfont.GetCodepointBitmap(ch, scale, scale, out width, out height, out xOffset, out yOffset);
            
            using (Canvas canvas = new Canvas(800, 600))
            {
                Texture2d image = default(Texture2d);

                canvas.Setup += (Canvas c) =>
                {
                    image = Texture2d.FromFile("k.png");
                };

                canvas.Draw += c =>
                {
                    c.Background(new OpenTK.Graphics.Color4(c.FrameCount % 255, 100, 100, 0));
                    c.Image(image, (-0.5, -0.5));
                };
                canvas.Run(60f);
            }
                
        }
    }
}