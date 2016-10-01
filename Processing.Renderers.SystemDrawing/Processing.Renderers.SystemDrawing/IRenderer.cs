using System;
using System.Drawing;

namespace Application
{
	public interface IPVector
	{
		float X { get; }
		float Y { get; }
	}

	public interface IMatrix
	{
		IPVector Basis { get; set; }
		IMatrix Parent { get; set; }
		void Pop();
		void Push ();
		IPVector Transform(IPVector vector);
	}

	public interface IPShape
	{
		IPVector[] Points { get; set; }
	}

	public interface IStyle
	{
		Font Font { get; set; }
		float FontSize { get; set; }
		Color Fill { get; set; }
		Color Stroke { get; set; }
		float StrokeWeight { get; set; }
	}

	public interface IRenderer
	{
		Bitmap Triangle(Bitmap source, IStyle style, IMatrix matrix, float x1, float y1, float x2, float y2, float x3, float y3);
		Bitmap Rectangle(Bitmap source, IStyle style, IMatrix matrix, float x, float y, float width, float height);
		Bitmap Quad(Bitmap source, IStyle style, IMatrix matrix, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4);
		Bitmap Ellipse(Bitmap source, IStyle style, IMatrix matrix, float x, float y, float width, float height);
		Bitmap Line(Bitmap source, IStyle style, IMatrix matrix, float x1, float y1, float x2, float y2);
		Bitmap Arc(Bitmap source, IStyle style, IMatrix matrix, IPVector[] points);
		Bitmap Image(Bitmap source, IStyle style, IMatrix matrix, Bitmap image, float x, float y);
		Bitmap Text(Bitmap source, IStyle style, IMatrix matrix, string text, float x, float y);
		Bitmap Shape(Bitmap source, IStyle style, IMatrix matrix, IPShape shape, float x, float y);
	}
}