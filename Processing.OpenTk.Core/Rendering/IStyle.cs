using OpenTK.Graphics;
using TrueTypeSharp;

namespace Processing.OpenTk.Core.Rendering
{
    public interface IStyle
    {
        TrueTypeFont Font { get; set; }
        float? FontSize { get; set; }
        Color4? Fill { get; set; }
        Color4? Stroke { get; set; }
        float? StrokeWeight { get; set; }
        Orientation NormalOrientation { get; set; }
    }
}
