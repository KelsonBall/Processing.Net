using System.Drawing;
using Processing.Core.Styles;
using Processing.Core.Transforms;

namespace Processing.Core.Renderers
{
	public interface IRenderer<TTarget>
	{
	    IRenderer<TTarget> BeginDraw(TTarget source);
	    IRenderer<TTarget> EndDraw();

	    IRenderer<TTarget> Background(Color color);
        IRenderer<TTarget> Triangle(float x1, float y1, float x2, float y2, float x3, float y3, IStyle style = null, IMatrix matrix = null);
        IRenderer<TTarget> Rectangle(float x, float y, float width, float height, IStyle style = null, IMatrix matrix = null);
        IRenderer<TTarget> Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, IStyle style = null, IMatrix matrix = null);
        IRenderer<TTarget> Ellipse(float x, float y, float width, float height, IStyle style = null, IMatrix matrix = null);
        IRenderer<TTarget> Line(float x1, float y1, float x2, float y2, IStyle style = null, IMatrix matrix = null);
        IRenderer<TTarget> Arc(float x, float y, float width, float height, float startAngle, float sweepAngle, IStyle style = null, IMatrix matrix = null);
        IRenderer<TTarget> Image(Bitmap image, float x, float y, IStyle style = null, IMatrix matrix = null);
        IRenderer<TTarget> Text(string text, float x, float y, IStyle style = null, IMatrix matrix = null);
        IRenderer<TTarget> Shape(PointF[] vertecies, float x, float y, IStyle style = null, IMatrix matrix = null);
	    	    
	}
}