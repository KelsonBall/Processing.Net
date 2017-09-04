using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using static System.Math;
using Processing.OpenTk.Core.Rendering;
using Processing.OpenTk.Core.Math;
using Processing.OpenTk.Core.Textures;

namespace Processing.OpenTk.Core
{
    public class Canvas : GameWindow, ICanvas, IRenderer2d
    {
        #region ICanvas

        public IStyle Style { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ulong FrameCount { get; set; } = 0;

        public event Action<Canvas> Setup;

        public event Action<Canvas> Draw;

        public int MouseX { get; protected set; }
        public int MouseY { get; protected set; }

        #endregion
        public Canvas(int sizex, int sizey) : base(sizex, sizey, GraphicsMode.Default, "Image Render")
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            GL.ClearColor(Color.CornflowerBlue);
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            Setup?.Invoke(this);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);                    
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            MouseX = Mouse.X;
            MouseY = Mouse.Y;

            if (Keyboard[Key.Escape])
                Exit();
        }        

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            FrameCount++;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Draw?.Invoke(this);

            SwapBuffers();
        }

        #region Renderer 2d        
        
        public void Background(Color4 color)
        {
            GL.ClearColor(color);
        }

        public void Triangle(PVector a, PVector b, PVector c)
        {
            throw new NotImplementedException();
        }

        public void Rectangle(PVector position, PVector size)
        {
            throw new NotImplementedException();
        }

        public void Quad(PVector a, PVector b, PVector c, PVector d)
        {
            throw new NotImplementedException();
        }

        public void Ellipse(PVector position, PVector size)
        {
            throw new NotImplementedException();
        }

        public void Line(PVector a, PVector b)
        {
            throw new NotImplementedException();
        }

        public void Arc(PVector position, PVector size, float startAngle, float sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void Image(PImage image, PVector position)
        {            
            GL.PushMatrix();
            {
                GL.LoadIdentity();
                GL.Ortho(0, Width, Height, 0, 0, 1);
                GL.Translate(position.ToVector3());
                GL.Disable(EnableCap.Lighting);
                GL.Enable(EnableCap.Texture2D);

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

        public void Text(string text, PVector position)
        {
            throw new NotImplementedException();
        }

        public void Shape(PVector position, params PVector[] points)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
