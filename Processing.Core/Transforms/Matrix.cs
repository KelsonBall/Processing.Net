namespace Processing.Core.Transforms
{
    public class Matrix : IMatrix
    {
        public IMatrix Parent { get; set; }

        public PVector LookVector
        {
            get
            {
                return PVector.FromAngle(Rotation);
            }
        }

        public double Rotation { get; set; } = 0;
        public double TotalRotation
        {
            get
            {
                return (Parent?.TotalRotation ?? 0) + Rotation;
            }
        }

        public double Scale { get; set; } = 1;

        private PVector _translation = new PVector(0, 0);
        public PVector Translation
        {
            get { return _translation; }
            set { _translation = value; }
        }

        public Matrix(PVector translation = default(PVector), double rotation = 0)
        {
            Rotation = rotation;
            Translation = translation;
        }

        public PVector Calculate()
        {
            PVector origin = Parent?.Calculate() ?? PVector.O;
            PVector rotated = Translation.Rotate(Rotation);
            PVector scaled = rotated * Scale;
            PVector result = origin + scaled;
            return result;
        }
    }
}
