namespace Core.DifferentialEquations
{
    /// <summary>
    /// Configuration for the differential equation solver
    /// </summary>
    public class SolverConfig
    {
        public double InitialX { get; set; }
        public double InitialY { get; set; }
        public double FinalX { get; set; }
        public double StepSize { get; set; } = 0.1;
        public SolutionMethod Method { get; set; } = SolutionMethod.RungeKutta4;
        public double Tolerance { get; set; } = 1e-6; // For adaptive methods
        public double MinStepSize { get; set; } = 1e-8;
        public double MaxStepSize { get; set; } = 1.0;
    }
}
