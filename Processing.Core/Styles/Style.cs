using System.Drawing;

namespace Processing.Core.Styles
{
    public class Style : IStyle
    {
        public Font Font { get; set; }
        public float? FontSize { get; set; }
        public Color? Fill { get; set; }
        public Color? Stroke { get; set; }
        public float? StrokeWeight { get; set; }

        public static Style Default()
        {
            return new Style
            {
                Font = new Font(FontFamily.GenericMonospace, 12),
                FontSize = 12,
                Fill = Color.White,
                Stroke = Color.Black,
                StrokeWeight = 1
            };
        }
}
}
