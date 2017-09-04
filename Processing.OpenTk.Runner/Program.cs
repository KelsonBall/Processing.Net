using Processing.OpenTk.Core;
using Processing.OpenTk.Core.Textures;
using System;

namespace Processing.OpenTk.Runner
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {                                    
            using (Canvas canvas = new Canvas(1024, 1024))
            {
                PImage squid = default(PImage);
                
                canvas.Setup += _ =>
                {                    
                    squid = PImage.FromFile("squid.jpg").Resize(256, 256);
                };

                canvas.Draw += _ =>
                {                     
                    canvas.Image(squid, (canvas.MouseX, canvas.MouseY));
                };

                canvas.Run(60f);
            }                
        }
    }
}