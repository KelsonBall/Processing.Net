using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Processing.Core.Renderers;
using Processing.Core.Styles;
using Processing.Core.Transforms;

namespace Processing.Core
{
    public abstract class Canvas : IActionDispatchee, IColorable, ITransformable
    {        
        private readonly ConcurrentQueue<Action> preBuildActions = new ConcurrentQueue<Action>();
        private readonly ConcurrentQueue<Action> drawActions = new ConcurrentQueue<Action>();
        private Action<Bitmap> SetImage;
        private IRenderer<Bitmap> _renderer;
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
            InvokeNonDrawAction(() =>
            {
                Width = width;
                Height = height;
                image = new Bitmap(width, height);
                _renderer = new GdiRenderer();                
            });
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
            InvokeDrawAction(() => MousePressed(button));
        }

        private void HandleMouseMoved(object sender, MouseEventArgs args)
        {
            MouseX = args.X;
            MouseY = args.Y;
            InvokeDrawAction(() => MouseMoved());
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
            InvokeDrawAction(() => MouseReleased(button));
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

        protected virtual void Setup()
        {

        }

        protected virtual void Draw()
        {

        }

        protected virtual void MousePressed(MouseButton button)
        {

        }

        protected virtual void MouseMoved()
        {

        }

        protected virtual void MouseReleased(MouseButton button)
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
            preBuildActions.Invoke();
            Setup();

            _renderer.BeginDraw(image);
            drawActions.Invoke();
            _renderer.EndDraw();
            DrawToForm();
        }

        private void InvokeDraw()
        {

            preBuildActions.Invoke();
            
            if (Loop)
                Draw();

            _renderer.BeginDraw(image);
            drawActions.Invoke();
            _renderer.EndDraw();
            FrameCount++;
            DrawToForm();
        }

        #endregion

        #region Processing API
        private IMatrix _matrix = new Matrix();
        public IMatrix Matrix
        {
            get { return _matrix; }
        }

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
            internal set
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
            internal set
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
                InvokeDrawAction(() => _fill = value);
            }
        }

        private Color _stroke;        
        public Color Stroke
        {
            get
            {
                return _stroke;
            }
            set
            {
                _stroke = value;
                InvokeDrawAction(() => _stroke = value);
            }
        }

        private float _strokeWeight = 1;
        public float StrokeWeight
        {
            get { return _strokeWeight; }
            set
            {
                _strokeWeight = value;
                InvokeDrawAction(() => _strokeWeight = value);
            }
        }

        private Font _font = new Font(FontFamily.GenericMonospace, 12);
        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                InvokeDrawAction(() => _font = value);
            }
        }

        private float _fontSize = 12;
        public float FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                InvokeDrawAction(() => _fontSize = value);
                Font = new Font(Font.FontFamily, value);                
            }
        }


        public void Background(Color color)
        {
            InvokeDrawAction(() =>
            {
                _renderer.Background(color);
            });
        }

        public void Background(byte r, byte g, byte b)
        {
            InvokeDrawAction(() =>
            {
                _renderer.Background(Color.FromArgb(0xFF, r, g, b));
            });
        }

        public void Background(byte v)
        {
            InvokeDrawAction(() =>
            {
                _renderer.Background(Color.FromArgb(255, v, v, v));
            });
        }

        public void Background(byte a, byte r, byte g, byte b)
        {
            InvokeDrawAction(() =>
            {
                _renderer.Background(Color.FromArgb(a, r, g, b));
            });
        }

        public void Ellipse(float x, float y, float width, float height)
        {
            InvokeDrawAction(() =>
            {
                _renderer.Ellipse(x, y, width, height, GetStyle(), _matrix);
            });
        }

        public void Rectangle(float x, float y, float width, float height)
        {
            InvokeDrawAction(() =>
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
                _renderer.Rectangle(x, y, width, height, GetStyle(), _matrix);
            });
        }

        public void Line(float x1, float y1, float x2, float y2)
        {
            InvokeDrawAction(() =>
            {
                _renderer.Line(x1, y1, x2, y2, GetStyle(), _matrix);
            });
        }

        public void Text(string text, float x, float y)
        {
            InvokeDrawAction(() =>
            {
                _renderer.Text(text, x, y, GetStyle(), _matrix);
            });
        }

        public void Image(Bitmap image, float x, float y)
        {
            InvokeDrawAction(() =>
            {
                _renderer.Image(image, x, y, GetStyle(), _matrix);
            });
        }

        private bool shapeStarted = false;
        private Queue<PointF> shapeVertecies;
        public void StartShape()
        {
            InvokeDrawAction(() =>
            {
                if (shapeStarted)
                    throw new Exception("Shape already started.");
                shapeStarted = true;
                shapeVertecies = new Queue<PointF>();
            });
        }

        public void Vertex(float x, float y)
        {
            InvokeDrawAction(() =>
            {
                if (!shapeStarted)
                    throw new Exception("No shape started.");
                shapeVertecies.Enqueue(new PointF(x, y));
            });
        }

        public void EndShape()
        {
            InvokeDrawAction(() =>
            {
                if (!shapeStarted)
                    throw new Exception("No shape started.");
                _renderer.Shape(shapeVertecies.ToArray(), 0, 0);
                shapeStarted = false;
                shapeVertecies = null;
            });
        }

        public void PushMatrix()
        {
            InvokeDrawAction(() => { 
                IMatrix matrix = new Matrix();
                matrix.Parent = _matrix;
                _matrix = matrix;
            });
        }

        public void PopMatrix()
        {
            InvokeDrawAction(() =>
            {
                if (_matrix.Parent != null)
                    _matrix = _matrix.Parent;
            });
        }

        public void Translate(float x, float y)
        {
            InvokeDrawAction(() =>
            {
                _matrix.Translation += new PVector(x, y);
            });
        }

        public void Rotate(float r)
        {
            InvokeDrawAction(() =>
            {
                _matrix.Rotation += r;
            });
        }

        public void Scale(float s)
        {
            InvokeDrawAction(() =>
            {
                _matrix.Scale *= s;
            });
        }
        #endregion

        public void InvokeNonDrawAction(Action action)
        {
            preBuildActions.Enqueue(action);
        }

        public void InvokeDrawAction(Action action)
        {
            drawActions.Enqueue(action);
        }

        private IStyle GetStyle()
        {
            return new Style { Fill = this.Fill, Font = this.Font, FontSize = this.FontSize, Stroke = this.Stroke, StrokeWeight = this.StrokeWeight };
        }

        
    }
}
