
namespace Core.Physics
{
    /// <summary>
    /// Represents a complex number with quantum-specific operations
    /// </summary>
    public readonly struct QuantumComplex(double real, double imaginary = 0)
    {
        public double Real { get; } = real;
        public double Imaginary { get; } = imaginary;

        public double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);
        public double Phase => Math.Atan2(Imaginary, Real);
        public QuantumComplex Conjugate => new(Real, -Imaginary);

        public static QuantumComplex operator +(QuantumComplex a, QuantumComplex b) =>
            new(a.Real + b.Real, a.Imaginary + b.Imaginary);

        public static QuantumComplex operator -(QuantumComplex a, QuantumComplex b) =>
            new(a.Real - b.Real, a.Imaginary - b.Imaginary);

        public static QuantumComplex operator *(QuantumComplex a, QuantumComplex b) =>
            new(
                a.Real * b.Real - a.Imaginary * b.Imaginary,
                a.Real * b.Imaginary + a.Imaginary * b.Real
            );

        public static QuantumComplex operator *(double scalar, QuantumComplex complex) =>
            new(scalar * complex.Real, scalar * complex.Imaginary);

        public override string ToString() =>
            Imaginary >= 0 ? $"{Real:F3} + {Imaginary:F3}i" : $"{Real:F3} - {Math.Abs(Imaginary):F3}i";
    }
}
