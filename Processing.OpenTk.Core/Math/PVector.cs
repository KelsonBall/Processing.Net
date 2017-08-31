using static System.Math;

namespace Processing.OpenTk.Core.Math
{
    public struct PVector
    {
        public readonly double X;
        public readonly double Y;

        public PVector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static PVector î => new PVector(1, 0);

        public static PVector ĵ => new PVector(0, 1);

        public static PVector O => new PVector(0, 0);

        public static PVector FromAngle(double angle)
        {
            return new PVector(Sin(angle), Cos(angle));
        }

        public static PVector operator +(PVector a, PVector b)
        {
            return a.Add(b);
        }

        public static PVector operator -(PVector a, PVector b)
        {
            return a.Add(new PVector(-b.X, -b.Y));
        }

        public static PVector operator *(PVector a, double b)
        {
            return a.Scale(b);
        }

        public static PVector operator *(PVector a, int b)
        {
            return a.Scale(b);
        }

        public static PVector operator *(double a, PVector b)
        {
            return b.Scale(a);
        }

        public static PVector operator *(int a, PVector b)
        {
            return b.Scale(a);
        }

        public static PVector operator /(PVector a, double b)
        {
            return a.Scale(1 / b);
        }

        public PVector Add(PVector to)
        {
            return new PVector(X + to.X, Y + to.Y);
        }

        public PVector Scale(double scalar)
        {
            return new PVector(X * scalar, Y * scalar);
        }

        public double Dot(PVector by)
        {
            return X * by.X + Y + by.Y;
        }

        public double MagnitudeSquared()
        {
            return X * X + Y * Y;
        }

        public double Magnitude()
        {
            return Sqrt(MagnitudeSquared());
        }

        public double Angle()
        {
            return Atan2(Y, X);
        }

        public PVector Unit()
        {
            return this / Magnitude();
        }

        public PVector Rotate(double angle)
        {
            return FromAngle(Angle() + angle) * Magnitude();
        }

        public override string ToString()
        {
            return $"{{{X}, {Y}}}";
        }
    }
}
