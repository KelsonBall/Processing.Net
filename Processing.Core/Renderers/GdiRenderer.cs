using System;
using System.Drawing;
using System.Threading;
using Processing.Core.Styles;
using Processing.Core.Transforms;

namespace Processing.Core.Renderers
{
    internal class GdiRenderer : IRenderer, IDisposable
    {
        private IStyle _style;
        private Pen _pen;
        private Brush _brush;
        private Graphics _canvas;

        public IRenderer BeginDraw(Bitmap source)
        {
            if (_style == null)
                ApplyDefaultStyle();

            // This worst line of code ever written:
            while (_canvas != null) Thread.Sleep(1);

            try
            {
                _canvas = Graphics.FromImage(source);
            }
            catch (InvalidOperationException opException)
            {
                if (!opException.Message.Equals("Object is currently in use elsewhere."))
                    throw opException;
                else
                    _canvas = Graphics.FromImage(new Bitmap(1, 1));
            }

            return this;
        }

        public IRenderer EndDraw()
        {
            if (_canvas != null)
            {
                _canvas.Dispose();
                _canvas = null;
            }
            return this;
        }

        public IRenderer Background(Color color)
        {
            _canvas.Clear(color);
            return this;
        }

        public IRenderer Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            PointF[] points = { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3) };
            _canvas.FillPolygon(_brush, points);
            if (_style.StrokeWeight > 0)
                _canvas.DrawPolygon(_pen, points);
            return this;
        }

        public IRenderer Rectangle(float x, float y, float width, float height)
        {
            _canvas.FillRectangle(_brush, x, y, width, height);
            if (_style.StrokeWeight > 0)
                _canvas.DrawRectangle(_pen, x, y, width, height);
            return this;
        }

        public IRenderer Quad(float x1, float y1, float x2, float y2, float x3, float y3,
            float x4, float y4)
        {
            PointF[] points = { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3), new PointF(x4, y4) };
            _canvas.FillPolygon(_brush, points);
            if (_style.StrokeWeight > 0)
                _canvas.DrawPolygon(_pen, points);
            return this;
        }

        public IRenderer Ellipse(float x, float y, float width, float height)
        {
            _canvas.FillEllipse(_brush, x, y, width, height);
            if (_style.StrokeWeight > 0)
                _canvas.DrawEllipse(_pen, x, y, width, height);
            return this;
        }

        public IRenderer Line(float x1, float y1, float x2, float y2)
        {
            _canvas.DrawLine(_pen, x1, y1, x2, y2);
            return this;
        }

        public IRenderer Arc(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            _canvas.DrawArc(_pen, x, y, width, height, startAngle, sweepAngle);
            return this;
        }

        public IRenderer Image(Bitmap image, float x, float y)
        {
            _canvas.DrawImage(image, x, y);
            return this;
        }

        public IRenderer Text(string text, float x, float y)
        {
            _canvas.DrawString(text, _style.Font, _brush, x, y);
            return this;
        }

        public IRenderer Shape(PointF[] vertecies, float x, float y)
        {
            for (int i = 0; i < vertecies.Length; i++)
                vertecies[i] = new PointF(vertecies[i].X + x, vertecies[i].Y + y);
            _canvas.FillPolygon(_brush, vertecies);
            _canvas.DrawPolygon(_pen, vertecies);
            return this;
        }

        public IRenderer ApplyStyle(IStyle style)
        {
            if (style.Fill != null)
                _brush = new SolidBrush((Color)style.Fill);

            if (style.Stroke != null)
                _pen = new Pen((Color)style.Stroke, style.StrokeWeight ?? 1);

            if (style.Font != null)
                style.Font = new Font(style.Font.FontFamily, style.FontSize ?? 12);

            this._style = style;

            return this;
        }

        private void ApplyDefaultStyle()
        {
            ApplyStyle(new Style
            {
                Stroke = Color.Black,
                Fill = Color.White,
                Font =  SystemFonts.DefaultFont,
                FontSize = 12,
                StrokeWeight = 1
            });
        }

        public IRenderer ApplyTransform(IMatrix matrix)
        {
            throw new NotImplementedException();
        }



        public void Dispose()
        {
            EndDraw();
        }

        ~GdiRenderer()
        {
            Dispose();
        }
    }
}
