using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processing.Core
{
    public interface IColorable
    {
        Color Fill { get; set; }
        Color Stroke { get; set; }
        float StrokeWeight { get; set; }
    }
}
