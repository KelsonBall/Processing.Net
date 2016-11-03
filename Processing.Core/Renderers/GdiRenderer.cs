using System;
using System.Drawing;
using System.Threading;
using Processing.Core.Styles;
using Processing.Core.Transforms;

namespace Processing.Core.Renderers
{
    internal class GdiRenderer : IRenderer<Bitmap>, IDisposable
    {
        //private IStyle style;
        //private Pen pen;
        //private Brush brush;
        private Graphics _canvas;

        public IRenderer<Bitmap> BeginDraw(Bitmap source)
        {            
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

        public IRenderer<Bitmap> EndDraw()
        {
            if (_canvas != null)
            {
                _canvas.Dispose();
                _canvas = null;
            }
            return this;
        }

        public IRenderer<Bitmap> Background(Color color)
        {
            _canvas.Clear(color);
            return this;
        }

        public IRenderer<Bitmap> Triangle(float x1, float y1, float x2, float y2, float x3, float y3, IStyle style, IMatrix matrix)
        {
            ApplyMatrix(matrix);
            PointF[] points = { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3) };
            using (var brush = GetBrush(style) )
                _canvas.FillPolygon(brush, points);
            if (style.StrokeWeight > 0)
                using (var pen = GetPen(style))
                    _canvas.DrawPolygon(pen, points);
            _canvas.ResetTransform();
            return this;
        }

        public IRenderer<Bitmap> Rectangle(float x, float y, float width, float height, IStyle style, IMatrix matrix)
        {
            ApplyMatrix(matrix);
            using (var brush = GetBrush(style))
                _canvas.FillRectangle(brush, x, y, width, height);
            if (style.StrokeWeight > 0)
                using (var pen = GetPen(style))
                    _canvas.DrawRectangle(pen, x, y, width, height);
            _canvas.ResetTransform();
            return this;
        }

        public IRenderer<Bitmap> Quad(float x1, float y1, float x2, float y2, float x3, float y3,
            float x4, float y4, IStyle style, IMatrix matrix)
        {
            ApplyMatrix(matrix);
            PointF[] points = { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y3), new PointF(x4, y4) };
            using (var brush = GetBrush(style))
                _canvas.FillPolygon(brush, points);
            if (style.StrokeWeight > 0)
                using (var pen = GetPen(style))
                    _canvas.DrawPolygon(pen, points);
            _canvas.ResetTransform();
            return this;
        }

        public IRenderer<Bitmap> Ellipse(float x, float y, float width, float height, IStyle style, IMatrix matrix)
        {
            ApplyMatrix(matrix);
            using (var brush = GetBrush(style))
                _canvas.FillEllipse(brush, x, y, width, height);
            if (style.StrokeWeight > 0)
                using (var pen = GetPen(style))
                    _canvas.DrawEllipse(pen, x, y, width, height);
            _canvas.ResetTransform();
            return this;
        }

        public IRenderer<Bitmap> Line(float x1, float y1, float x2, float y2, IStyle style, IMatrix matrix)
        {
            ApplyMatrix(matrix);
            using (var pen = GetPen(style))
                _canvas.DrawLine(pen, x1, y1, x2, y2);
            _canvas.ResetTransform();
            return this;
        }

        public IRenderer<Bitmap> Arc(float x, float y, float width, float height, float startAngle, float sweepAngle, IStyle style, IMatrix matrix)
        {
            ApplyMatrix(matrix);
            using (var pen = GetPen(style))
                _canvas.DrawArc(pen, x, y, width, height, startAngle, sweepAngle);
            _canvas.ResetTransform();
            return this;
        }

        public IRenderer<Bitmap> Image(Bitmap image, float x, float y, IStyle style, IMatrix matrix)
        {
            ApplyMatrix(matrix);
            _canvas.DrawImage(image, x, y);
            _canvas.ResetTransform();
            return this;
        }

        public IRenderer<Bitmap> Text(string text, float x, float y, IStyle style, IMatrix matrix)
        {
            ApplyMatrix(matrix);
            using (var brush = GetBrush(style))
                _canvas.DrawString(text, style.Font, brush, x, y);
            _canvas.ResetTransform();
            return this;
        }

        public IRenderer<Bitmap> Shape(PointF[] vertecies, float x, float y, IStyle style, IMatrix matrix)
        {
            ApplyMatrix(matrix);
            for (int i = 0; i < vertecies.Length; i++)
                vertecies[i] = new PointF(vertecies[i].X + x, vertecies[i].Y + y);
            using (var brush = GetBrush(style))
                _canvas.FillPolygon(brush, vertecies);
            using (var pen = GetPen(style))
                _canvas.DrawPolygon(pen, vertecies);
            _canvas.ResetTransform();
            return this;
        }
        

        private Brush GetBrush(IStyle style)
        {
            return new SolidBrush(style.Fill ?? Color.White);
        }

        private Pen GetPen(IStyle style)
        {
            return new Pen(new SolidBrush(style.Stroke ?? Color.Black), style.StrokeWeight ?? 1f);
        }

        private void ApplyMatrix(IMatrix matrix)
        {
            if (matrix.Parent != null)
                ApplyMatrix(matrix.Parent);
            _canvas.RotateTransform((float)matrix.Rotation);
            _canvas.TranslateTransform((float)matrix.Translation.X, (float)matrix.Translation.Y);            
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

    internal static class ColorExtensions
    {

    }
}
