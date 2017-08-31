using OpenTK;
using OpenTK.Graphics;
using Processing.OpenTk.Core.Math;
using Processing.OpenTk.Core.Textures;

namespace Processing.OpenTk.Core.Rendering
{    
    public interface IRenderer2d
    {
        IRenderer2d BeginDraw();
        IRenderer2d EndDraw();

        IStyle Style { get; set; }

        IRenderer2d Background(Color4 color);
        IRenderer2d Triangle(PVector a, PVector b, PVector c);
        IRenderer2d Rectangle(PVector position, PVector size);
        IRenderer2d Quad(PVector a, PVector b, PVector c, PVector d);
        IRenderer2d Ellipse(PVector position, PVector size);
        IRenderer2d Line(PVector a, PVector b);
        IRenderer2d Arc(PVector position, PVector size, float startAngle, float sweepAngle);
        IRenderer2d Image(Texture2d image, PVector position);
        IRenderer2d Text(string text, PVector position);
        IRenderer2d Shape(PVector position, params PVector[] points);

    }
}
