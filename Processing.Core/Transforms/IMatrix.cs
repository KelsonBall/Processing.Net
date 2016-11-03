using System.Drawing;

namespace Processing.Core.Transforms
{
    public interface IMatrix
    {        
        IMatrix Parent { get; set; }        
        double Scale { get; set; }
        double Rotation { get; set; }
        double TotalRotation { get; }       
        PVector Translation { get; set; }
        PVector Calculate();        
    }
}
