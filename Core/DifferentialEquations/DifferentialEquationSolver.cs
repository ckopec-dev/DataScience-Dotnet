using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DifferentialEquations
{
    /// <summary>
    /// Delegate representing a differential equation dy/dx = f(x, y)
    /// </summary>
    /// <param name="x">Independent variable</param>
    /// <param name="y">Dependent variable</param>
    /// <returns>Value of dy/dx at point (x, y)</returns>
    public delegate double DifferentialEquation(double x, double y);

    /// <summary>
    /// A comprehensive differential equation solver using various numerical methods
    /// </summary>
    public class DifferentialEquationSolver
    {
        private readonly DifferentialEquation equation;
        private readonly SolverConfig config;

        public DifferentialEquationSolver(DifferentialEquation equation, SolverConfig config)
        {
            this.equation = equation ?? throw new ArgumentNullException(nameof(equation));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            ValidateConfig();
        }

        /// <summary>
        /// Solves the differential equation and returns the solution points
        /// </summary>
        /// <returns>List of solution points</returns>
        public List<SolutionPoint> Solve()
        {
            return config.Method switch
            {
                SolutionMethod.Euler => SolveEuler(),
                SolutionMethod.ImprovedEuler => SolveImprovedEuler(),
                SolutionMethod.RungeKutta4 => SolveRungeKutta4(),
                SolutionMethod.AdaptiveRungeKutta => SolveAdaptiveRungeKutta(),
                _ => throw new NotImplementedException($"Method {config.Method} not implemented")
            };
        }

        /// <summary>
        /// Euler's method (first-order)
        /// </summary>
        private List<SolutionPoint> SolveEuler()
        {
            var solution = new List<SolutionPoint>();
            double x = config.InitialX;
            double y = config.InitialY;

            solution.Add(new SolutionPoint(x, y));

            while (x < config.FinalX)
            {
                double h = Math.Min(config.StepSize, config.FinalX - x);
                y += h * equation(x, y);
                x += h;
                solution.Add(new SolutionPoint(x, y));
            }

            return solution;
        }

        /// <summary>
        /// Improved Euler's method (Heun's method)
        /// </summary>
        private List<SolutionPoint> SolveImprovedEuler()
        {
            var solution = new List<SolutionPoint>();
            double x = config.InitialX;
            double y = config.InitialY;

            solution.Add(new SolutionPoint(x, y));

            while (x < config.FinalX)
            {
                double h = Math.Min(config.StepSize, config.FinalX - x);

                // Predictor step
                double k1 = equation(x, y);
                double yPredict = y + h * k1;

                // Corrector step
                double k2 = equation(x + h, yPredict);
                y += (h / 2.0) * (k1 + k2);

                x += h;
                solution.Add(new SolutionPoint(x, y));
            }

            return solution;
        }

        /// <summary>
        /// Fourth-order Runge-Kutta method
        /// </summary>
        private List<SolutionPoint> SolveRungeKutta4()
        {
            var solution = new List<SolutionPoint>();
            double x = config.InitialX;
            double y = config.InitialY;

            solution.Add(new SolutionPoint(x, y));

            while (x < config.FinalX)
            {
                double h = Math.Min(config.StepSize, config.FinalX - x);

                double k1 = equation(x, y);
                double k2 = equation(x + h / 2, y + h * k1 / 2);
                double k3 = equation(x + h / 2, y + h * k2 / 2);
                double k4 = equation(x + h, y + h * k3);

                y += (h / 6.0) * (k1 + 2 * k2 + 2 * k3 + k4);
                x += h;

                solution.Add(new SolutionPoint(x, y));
            }

            return solution;
        }

        /// <summary>
        /// Adaptive Runge-Kutta method with error control
        /// </summary>
        private List<SolutionPoint> SolveAdaptiveRungeKutta()
        {
            var solution = new List<SolutionPoint>();
            double x = config.InitialX;
            double y = config.InitialY;
            double h = config.StepSize;

            solution.Add(new SolutionPoint(x, y));

            while (x < config.FinalX)
            {
                if (x + h > config.FinalX)
                    h = config.FinalX - x;

                // Calculate with step size h
                double y1 = RungeKuttaStep(x, y, h);

                // Calculate with two steps of size h/2
                double yMid = RungeKuttaStep(x, y, h / 2);
                double y2 = RungeKuttaStep(x + h / 2, yMid, h / 2);

                // Estimate error
                double error = Math.Abs(y2 - y1) / 15.0; // Richardson extrapolation error estimate

                if (error <= config.Tolerance || h <= config.MinStepSize)
                {
                    // Accept the step
                    x += h;
                    y = y2; // Use the more accurate estimate
                    solution.Add(new SolutionPoint(x, y));

                    // Adjust step size for next iteration
                    if (error > 0)
                    {
                        double factor = Math.Pow(config.Tolerance / error, 0.2);
                        h = Math.Min(config.MaxStepSize, Math.Max(config.MinStepSize, 0.9 * h * factor));
                    }
                }
                else
                {
                    // Reject the step and reduce step size
                    double factor = Math.Pow(config.Tolerance / error, 0.25);
                    h = Math.Max(config.MinStepSize, 0.5 * h * factor);
                }
            }

            return solution;
        }

        /// <summary>
        /// Single Runge-Kutta step
        /// </summary>
        private double RungeKuttaStep(double x, double y, double h)
        {
            double k1 = equation(x, y);
            double k2 = equation(x + h / 2, y + h * k1 / 2);
            double k3 = equation(x + h / 2, y + h * k2 / 2);
            double k4 = equation(x + h, y + h * k3);

            return y + (h / 6.0) * (k1 + 2 * k2 + 2 * k3 + k4);
        }

        /// <summary>
        /// Validates the solver configuration
        /// </summary>
        private void ValidateConfig()
        {
            if (config.FinalX <= config.InitialX)
                throw new ArgumentException("Final X must be greater than initial X");

            if (config.StepSize <= 0)
                throw new ArgumentException("Step size must be positive");

            if (config.Tolerance <= 0)
                throw new ArgumentException("Tolerance must be positive");

            if (config.MinStepSize <= 0 || config.MinStepSize > config.MaxStepSize)
                throw new ArgumentException("Invalid step size bounds");
        }

        /// <summary>
        /// Estimates the error at a given point using Richardson extrapolation
        /// </summary>
        public double EstimateError(double x, double y, double h)
        {
            double y1 = RungeKuttaStep(x, y, h);
            double yMid = RungeKuttaStep(x, y, h / 2);
            double y2 = RungeKuttaStep(x + h / 2, yMid, h / 2);

            return Math.Abs(y2 - y1) / 15.0;
        }

        /// <summary>
        /// Evaluates the differential equation at a given point
        /// </summary>
        public double EvaluateEquation(double x, double y) => equation(x, y);
    }
}
