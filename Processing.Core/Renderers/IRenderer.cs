using System.Drawing;
using Processing.Core.Styles;
using Processing.Core.Transforms;

namespace Processing.Core.Renderers
{
	public interface IRenderer
	{
	    IRenderer BeginDraw(Bitmap source);
	    IRenderer EndDraw();

	    IRenderer Background(Color color);
        IRenderer Triangle(float x1, float y1, float x2, float y2, float x3, float y3);
        IRenderer Rectangle(float x, float y, float width, float height);
        IRenderer Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4);
        IRenderer Ellipse(float x, float y, float width, float height);
        IRenderer Line(float x1, float y1, float x2, float y2);
        IRenderer Arc(float x, float y, float width, float height, float startAngle, float sweepAngle);
        IRenderer Image(Bitmap image, float x, float y);
        IRenderer Text(string text, float x, float y);
        IRenderer Shape(PointF[] vertecies, float x, float y);

	    IRenderer ApplyStyle(IStyle style);
	    IRenderer ApplyTransform(IMatrix matrix);
	}
}