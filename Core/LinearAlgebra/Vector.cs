namespace Core.LinearAlgebra
{
    // Vector class for n-dimensional vectors
    public class Vector
    {
        private readonly double[] _elements;

        public int Size => _elements.Length;
        public double[] Elements => (double[])_elements.Clone();

        // Constructors
        public Vector(params double[] elements)
        {
            _elements = (double[])elements.Clone();
        }

        public Vector(int size)
        {
            _elements = new double[size];
        }

        // Indexer
        public double this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value;
        }

        // Static factory methods
        public static Vector Zero(int size) => new(new double[size]);
        public static Vector Ones(int size) => new([.. Enumerable.Repeat(1.0, size)]);
        public static Vector Unit(int size, int index)
        {
            var v = Zero(size);
            v[index] = 1.0;
            return v;
        }

        // Basic operations
        public static Vector operator +(Vector a, Vector b)
        {
            if (a.Size != b.Size) throw new LinearAlgebraException("Vector dimensions must match");
            var result = new Vector(a.Size);
            for (int i = 0; i < a.Size; i++)
                result[i] = a[i] + b[i];
            return result;
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.Size != b.Size) throw new LinearAlgebraException("Vector dimensions must match");
            var result = new Vector(a.Size);
            for (int i = 0; i < a.Size; i++)
                result[i] = a[i] - b[i];
            return result;
        }

        public static Vector operator *(double scalar, Vector v)
        {
            var result = new Vector(v.Size);
            for (int i = 0; i < v.Size; i++)
                result[i] = scalar * v[i];
            return result;
        }

        public static Vector operator *(Vector v, double scalar) => scalar * v;

        public static Vector operator /(Vector v, double scalar)
        {
            if (Math.Abs(scalar) < double.Epsilon) throw new LinearAlgebraException("Division by zero");
            return (1.0 / scalar) * v;
        }

        // Vector operations
        public double Dot(Vector other)
        {
            if (Size != other.Size) throw new LinearAlgebraException("Vector dimensions must match");
            double sum = 0;
            for (int i = 0; i < Size; i++)
                sum += _elements[i] * other._elements[i];
            return sum;
        }

        public Vector Cross(Vector other)
        {
            if (Size != 3 || other.Size != 3) throw new LinearAlgebraException("Cross product is only defined for 3D vectors");
            return new Vector(
                _elements[1] * other._elements[2] - _elements[2] * other._elements[1],
                _elements[2] * other._elements[0] - _elements[0] * other._elements[2],
                _elements[0] * other._elements[1] - _elements[1] * other._elements[0]
            );
        }

        public double Magnitude => Math.Sqrt(Dot(this));
        public double MagnitudeSquared => Dot(this);

        public Vector Normalize()
        {
            double mag = Magnitude;
            if (mag < double.Epsilon) throw new LinearAlgebraException("Cannot normalize zero vector");
            return this / mag;
        }

        public double DistanceTo(Vector other) => (this - other).Magnitude;

        public double AngleTo(Vector other)
        {
            double cosTheta = Dot(other) / (Magnitude * other.Magnitude);
            return Math.Acos(Math.Max(-1, Math.Min(1, cosTheta)));
        }

        public Vector ProjectOnto(Vector other) => (Dot(other) / other.MagnitudeSquared) * other;

        // String representation
        public override string ToString()
        {
            return "[" + string.Join(", ", _elements.Select(x => x.ToString("F3"))) + "]";
        }
    }
}
