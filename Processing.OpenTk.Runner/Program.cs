using System;
using System.IO;
using OpenTK.Graphics;
using Processing.OpenTk.Core;
using Processing.OpenTk.Core.Textures;

namespace Processing.OpenTk.Runner
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {            
            using (Canvas canvas = new Canvas(1024, 1024))
            {
                Font anon = default(Font);
                PImage squid = default(PImage);                

                canvas.Setup += _ =>
                {
                    anon = new Font(() => File.OpenRead("AnonymousPro.ttf"), new Color4(0.2f, 0.2f, 0.2f, 1f), 128);
                    squid = PImage.FromFile("squid.jpg").Resize(256, 256);
                };

                canvas.Draw += _ =>
                {                    
                    canvas.Image(squid, (canvas.MouseX, canvas.MouseY));
                    canvas.Image(anon['A'], (100, 100));
                };

                canvas.Run(60f);
            }                
        }
    }
}