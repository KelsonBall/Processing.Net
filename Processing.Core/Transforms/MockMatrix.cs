using System.Drawing;

namespace Processing.Core.Transforms
{
    internal class MockMatrix : IMatrix
    {
        public int Count { get; private set; }

        public PointF Basis { get; set; }
        public IMatrix Parent { get; set; }

        public void Pop()
        {
            Count--;
        }

        public void Push()
        {
            Count++;
        }

        public PointF Transform(PointF vector)
        {
            return vector;
        }
    }
}
