using System.Drawing;

namespace Processing.Core.Transforms
{
    public interface IMatrix
    {
        PointF Basis { get; set; }
        IMatrix Parent { get; set; }
        void Pop();
        void Push();
        PointF Transform(PointF vector);
    }
}
