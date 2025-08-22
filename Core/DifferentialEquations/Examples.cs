
namespace Core.DifferentialEquations
{
    /// <summary>
    /// Example usage and test cases
    /// </summary>
    public static class Examples
    {
        /// <summary>
        /// Solves dy/dx = y with initial condition y(0) = 1
        /// Analytical solution: y = e^x
        /// </summary>
        public static void ExponentialGrowth()
        {
            var config = new SolverConfig
            {
                InitialX = 0,
                InitialY = 1,
                FinalX = 2,
                StepSize = 0.1,
                Method = SolutionMethod.RungeKutta4
            };

            var solver = new DifferentialEquationSolver((x, y) => y, config);
            var solution = solver.Solve();

            Console.WriteLine("Exponential Growth: dy/dx = y, y(0) = 1");
            Console.WriteLine("Numerical vs Analytical (e^x):");

            foreach (var point in solution.Take(11))
            {
                double analytical = Math.Exp(point.X);
                double error = Math.Abs(point.Y - analytical);
                Console.WriteLine($"x={point.X:F1}: Numerical={point.Y:F6}, Analytical={analytical:F6}, Error={error:E2}");
            }
        }

        /// <summary>
        /// Solves dy/dx = -2xy with initial condition y(0) = 1
        /// Analytical solution: y = e^(-x²)
        /// </summary>
        public static void GaussianDecay()
        {
            var config = new SolverConfig
            {
                InitialX = 0,
                InitialY = 1,
                FinalX = 2,
                StepSize = 0.1,
                Method = SolutionMethod.AdaptiveRungeKutta,
                Tolerance = 1e-8
            };

            var solver = new DifferentialEquationSolver((x, y) => -2 * x * y, config);
            var solution = solver.Solve();

            Console.WriteLine("\nGaussian Decay: dy/dx = -2xy, y(0) = 1");
            Console.WriteLine("Numerical vs Analytical (e^(-x²)):");

            foreach (var point in solution.Where((p, i) => i % 5 == 0).Take(11))
            {
                double analytical = Math.Exp(-point.X * point.X);
                double error = Math.Abs(point.Y - analytical);
                Console.WriteLine($"x={point.X:F1}: Numerical={point.Y:F6}, Analytical={analytical:F6}, Error={error:E2}");
            }
        }

        /// <summary>
        /// Solves the logistic equation dy/dx = ry(1-y/K) with r=2, K=10, y(0)=1
        /// </summary>
        public static void LogisticGrowth()
        {
            double r = 2.0;  // growth rate
            double K = 10.0; // carrying capacity

            var config = new SolverConfig
            {
                InitialX = 0,
                InitialY = 1,
                FinalX = 3,
                StepSize = 0.05,
                Method = SolutionMethod.RungeKutta4
            };

            var solver = new DifferentialEquationSolver((x, y) => r * y * (1 - y / K), config);
            var solution = solver.Solve();

            Console.WriteLine($"\nLogistic Growth: dy/dx = {r}y(1-y/{K}), y(0) = 1");
            Console.WriteLine("t\tPopulation");

            foreach (var point in solution.Where((p, i) => i % 10 == 0))
            {
                Console.WriteLine($"{point.X:F1}\t{point.Y:F3}");
            }
        }
    }
}
