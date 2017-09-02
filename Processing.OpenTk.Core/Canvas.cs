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

        #endregion
        public Canvas(int sizex, int sizey) : base(sizex, sizey, GraphicsMode.Default)
        {
            VSync = VSyncMode.On;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.CornflowerBlue);
            
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
        private readonly Renderer2d _renderer2d = new Renderer2d();
        
        public void Background(Color4 color)
        {
            _renderer2d.Background(color);
        }

        public void Triangle(PVector a, PVector b, PVector c)
        {
            _renderer2d.Triangle(a, b, c);
        }

        public void Rectangle(PVector position, PVector size)
        {
            _renderer2d.Rectangle(position, size);
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

        public void Image(Texture2d image, PVector position)
        {
            _renderer2d.Image(image, position);
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
