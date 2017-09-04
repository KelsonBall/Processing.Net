using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using static System.Math;
using Processing.OpenTk.Core.Math;
using Processing.OpenTk.Core.Textures;
using OpenTK;

namespace Processing.OpenTk.Core.Rendering
{
    public class Renderer2d : IRenderer2d
    {
        public IStyle Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Arc(PVector position, PVector size, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void Background(Color4 color)
        {
            GL.ClearColor(color);
        }

        public void Ellipse(PVector position, PVector size)
        {
            var perimiter = 2 * PI * Sqrt(size.MagnitudeSquared() / 2);

            throw new NotImplementedException();
        }

        public void Image(PImage image, PVector position)
        {
            GL.PushMatrix();
            {
                GL.LoadIdentity();
                GL.Ortho(0, DisplayDevice.Default.Width, DisplayDevice.Default.Height, 0, -1, 1);
                GL.Translate(position.ToVector3());
                GL.Disable(EnableCap.Lighting);

                GL.BindTexture(TextureTarget.Texture2D, image);
                {
                    GL.Begin(PrimitiveType.Quads);
                    {
                        GL.TexCoord2(1f, 1f);
                        GL.Vertex2(image.Width, image.Height);
                        GL.TexCoord2(0f, 1f);
                        GL.Vertex2(0, image.Height);
                        GL.TexCoord2(0f, 0f);
                        GL.Vertex2(0, 0);
                        GL.TexCoord2(1.0f, 0.0f);
                        GL.Vertex2(image.Width, 0);
                    }
                    GL.End();
                }
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }     
            GL.PopMatrix();
        }

        public void Line(PVector a, PVector b)
        {
            throw new NotImplementedException();
        }

        public void Quad(PVector a, PVector b, PVector c, PVector d)
        {
            throw new NotImplementedException();
        }

        public void Rectangle(PVector position, PVector size)
        {
            throw new NotImplementedException();
        }

        public void Shape(PVector position, params PVector[] points)
        {
            throw new NotImplementedException();
        }

        public void Text(string text, PVector position)
        {
            throw new NotImplementedException();
        }

        public void Triangle(PVector a, PVector b, PVector c)
        {
            throw new NotImplementedException();
        }
    }
}
