using System.Drawing;

namespace Processing.Core.Styles
{
    public interface IStyle
    {
        Font Font { get; set; }
        float? FontSize { get; set; }
        Color? Fill { get; set; }
        Color? Stroke { get; set; }
        float? StrokeWeight { get; set; }
    }
}
