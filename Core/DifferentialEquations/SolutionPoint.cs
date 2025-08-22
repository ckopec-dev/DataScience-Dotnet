namespace Core.DifferentialEquations
{
    /// <summary>
    /// Represents a point in the solution of a differential equation
    /// </summary>
    public struct SolutionPoint(double x, double y)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;

        public override readonly string ToString() => $"({X:F4}, {Y:F4})";
    }
}
