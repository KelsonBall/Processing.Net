using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Processing.Core.Renderers;
using Processing.Core.Styles;

namespace Processing.Core
{
    public abstract class Canvas : IActionDispatchee
    {
        private ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();
        private Action<Bitmap> SetImage;
        private IRenderer _renderer;
        private Bitmap image;

        private void DrawToForm()
        {
            SetImage(image);
        }

        private int _frameRate = 60;
        private float _millisecondsFrameLength = 1000f/60;

        private bool Running { get; set; } = true;

        public void SetConnections(Action<Bitmap> imageSetter,
            Action<MouseEventHandler> subcribeToMouseDown,
            Action<MouseEventHandler> subscribeToMouseUp,
            Action<MouseEventHandler> subcribeToMouseMoved)
        {
            SetImage = imageSetter;
            subcribeToMouseDown(HandleMousePressed);
            subscribeToMouseUp(HandleMouseUp);
            subcribeToMouseMoved(HandleMouseMoved);
        }

        public void Size(int width, int height)
        {
            Width = width;
            Height = height;
            image = new Bitmap(width, height);
            _renderer = new GdiRenderer();
            _renderer.ApplyStyle(Style.Default());
        }


        #region Event Listeners

        private void HandleMousePressed(object sender, MouseEventArgs args)
        {
            MouseButton button;
            switch (args.Button)
            {
                case MouseButtons.Left:
                    button = MouseButton.Left;
                    break;
                case MouseButtons.Middle:
                    button = MouseButton.Middle;
                    break;
                case MouseButtons.Right:
                    button = MouseButton.Right;
                    break;
                default:
                    button = MouseButton.None;
                    break;
            }
            Invoke(() =>
            {
                MousePressed(button);
                mousePressedInvokee = null;
            });
        }

        private void HandleMouseMoved(object sender, MouseEventArgs args)
        {
            MouseX = args.X;
            MouseY = args.Y;
            Invoke(() =>
            {
                MouseMoved();
                mouseMovedInvokee = null;
            });
        }

        private void HandleMouseUp(object sender, MouseEventArgs args)
        {
            MouseButton button;
            switch (args.Button)
            {
                case MouseButtons.Left:
                    button = MouseButton.Left;
                    break;
                case MouseButtons.Middle:
                    button = MouseButton.Middle;
                    break;
                case MouseButtons.Right:
                    button = MouseButton.Right;
                    break;
                default:
                    button = MouseButton.None;
                    break;
            }
            Invoke(() =>
            {
                MouseReleased(button);
                mouseReleasedInvokee = null;
            });
        }

        #endregion

        #region Virtual Methods
        public enum MouseButton
        {
            None = 0,
            Left,
            Middle,
            Right
        }

        public virtual void Setup()
        {

        }

        public virtual void Draw()
        {

        }

        private Action mousePressedInvokee;
        public virtual void MousePressed(MouseButton button)
        {

        }

        private Action mouseMovedInvokee;
        public virtual void MouseMoved()
        {

        }

        private Action mouseReleasedInvokee;
        public virtual void MouseReleased(MouseButton button)
        {

        }

        #endregion

        public void Start()
        {
            Thread thread = new Thread(MainLoop);
            thread.Start();
        }

        public void Stop()
        {
            Running = false;
        }

        #region Loop Implementation

        private void MainLoop()
        {
            Stopwatch clock = new Stopwatch();
            try
            {
                InvokeSetup();
                while (Running)
                {
                    clock.Start();
                    InvokeDraw();
                    if (_millisecondsFrameLength > clock.ElapsedMilliseconds)
                        Thread.Sleep((int)(_millisecondsFrameLength - clock.ElapsedMilliseconds));
                    clock.Reset();
                }
            }
            catch (ObjectDisposedException disposed)
            {
                Running = false;
            }
        }

        private void InvokeSetup()
        {
            _renderer.BeginDraw(image);
            Setup();
            _renderer.EndDraw();
            DrawToForm();
        }

        private void InvokeDraw()
        {
            _renderer.BeginDraw(image);
            if (Loop)
                Draw();

            Action action;
            while (actions.TryDequeue(out action))
                action?.Invoke();

            _renderer.EndDraw();
            FrameCount++;
            DrawToForm();
        }

        #endregion

        #region Processing API


        public int FrameRate
        {
            get { return _frameRate; }
            set
            {
                _frameRate = value;
                _millisecondsFrameLength = 1000f / value;
            }
        }
        public ulong FrameCount { get; private set; }

        public bool Loop { get; set; } = true;

        public int Width { get; private set; }
        public int Height { get; private set; }

        private float _pmouseX;
        private float _mouseX;
        public float MouseX
        {
            get { return _mouseX; }
            set
            {
                _pmouseX = _mouseX;
                _mouseX = value;
            }
        }
        public float PMouseX { get { return _pmouseX; } }

        private float _pmouseY;
        private float _mouseY;
        public float MouseY
        {
            get { return _mouseY; }
            set
            {
                _pmouseY = _mouseY;
                _mouseY = value;
            }
        }
        public float PMouseY { get { return _pmouseY; } }

        private Color _fill;
        public Color Fill
        {
            get
            {
                return _fill;
            }
            set
            {
                _fill = value;
                _renderer.ApplyStyle(new Style { Fill = value } );
            }
        }

        private Color _stroke;
        private Brush _strokeBrush;
        public Color Stroke
        {
            get
            {
                return _stroke;
            }
            set
            {
                _stroke = value;
                _renderer.ApplyStyle(new Style { Stroke = value });
            }
        }


        public void Background(Color color)
        {
            _renderer.Background(color);
        }

        public void Background(byte r, byte g, byte b)
        {
            _renderer.Background(Color.FromArgb(0xFF, r, g, b));
        }

        public void Background(byte v)
        {
            _renderer.Background(Color.FromArgb(255, v, v, v));
        }

        public void Background(byte a, byte r, byte g, byte b)
        {
            _renderer.Background(Color.FromArgb(a, r, g, b));
        }

        public void Ellipse(float x, float y, float width, float height)
        {
            _renderer.Ellipse(x, y, width, height);
        }

        public void Rectangle(float x, float y, float width, float height)
        {
            if (width < 0)
            {
                x = x + width;
                width = -width;
            }
            if (height < 0)
            {
                y = y + height;
                height = -height;
            }
            _renderer.Rectangle(x, y, width, height);
        }

        public void Line(float x1, float y1, float x2, float y2)
        {
            _renderer.Line(x1, y1, x2, y2);
        }

        public void Text(string text, float x, float y)
        {
            _renderer.Text(text, x, y);
        }

        public void Image(Bitmap image, float x, float y)
        {
            _renderer.Image(image, x, y);
        }

        private bool shapeStarted = false;
        private Queue<PointF> shapeVertecies;
        public void StartShape()
        {
            if (shapeStarted)
                throw new Exception("Shape already started.");
            shapeStarted = true;
            shapeVertecies = new Queue<PointF>();
        }

        public void Vertex(float x, float y)
        {
            if (!shapeStarted)
                throw new Exception("No shape started.");
            shapeVertecies.Enqueue(new PointF(x, y));
        }

        public void EndShape()
        {
            if (!shapeStarted)
                throw new Exception("No shape started.");
            _renderer.Shape(shapeVertecies.ToArray(), 0, 0);
            shapeStarted = false;
            shapeVertecies = null;
        }
        #endregion

        public void Invoke(Action action)
        {
            actions.Enqueue(action);
        }
    }
}
