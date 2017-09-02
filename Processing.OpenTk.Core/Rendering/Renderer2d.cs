using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using static System.Math;
using Processing.OpenTk.Core.Math;
using Processing.OpenTk.Core.Textures;

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

        public void Image(Texture2d image, PVector position)
        {
            GL.PushMatrix();

            GL.Translate(position.ToVector3());
            
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);            
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);            
            GL.Enable(EnableCap.DepthTest);

            GL.Color3(1, 0, 0);
            GL.BindTexture(TextureTarget.Texture2D, image.Handle);
            GL.Begin(PrimitiveType.Quads);            
            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);            
            GL.TexCoord2(0, 1);
            GL.Vertex2(0, image.Height);            
            GL.TexCoord2(1, 1);
            GL.Vertex2(image.Width, image.Height);            
            GL.TexCoord2(1, 0);
            GL.Vertex2(image.Width, 0);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Disable(EnableCap.Texture2D);

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
