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

            //using (Canvas canvas = new Canvas(800, 600))
            //    canvas.Run(60f);
        }
    }
}