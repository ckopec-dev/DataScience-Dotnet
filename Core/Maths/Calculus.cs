using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Maths
{
    /// <summary>
    /// Comprehensive calculus class providing numerical methods for derivatives, integrals, limits, and series
    /// </summary>
    public class Calculus
    {
        private const double DefaultTolerance = 1e-10;
        private const int DefaultMaxIterations = 1000;

        #region Derivatives

        /// <summary>
        /// Calculates the numerical derivative using the central difference method
        /// </summary>
        /// <param name="f">Function to differentiate</param>
        /// <param name="x">Point at which to evaluate the derivative</param>
        /// <param name="h">Step size (default: 1e-8)</param>
        /// <returns>Approximate derivative value</returns>
        public static double Derivative(Func<double, double> f, double x, double h = 1e-8)
        {
            return (f(x + h) - f(x - h)) / (2 * h);
        }

        /// <summary>
        /// Calculates higher-order derivatives using recursive central difference
        /// </summary>
        /// <param name="f">Function to differentiate</param>
        /// <param name="x">Point at which to evaluate the derivative</param>
        /// <param name="order">Order of derivative (1 for first derivative, 2 for second, etc.)</param>
        /// <param name="h">Step size</param>
        /// <returns>Approximate nth derivative value</returns>
        public static double NthDerivative(Func<double, double> f, double x, int order, double h = 1e-6)
        {
            if (order <= 0) throw new ArgumentException("Order must be positive");
            if (order == 1) return Derivative(f, x, h);

            // Use recursive approach for higher orders
            return (NthDerivative(f, x + h, order - 1, h) - NthDerivative(f, x - h, order - 1, h)) / (2 * h);
        }

        /// <summary>
        /// Calculates the gradient of a multivariable function
        /// </summary>
        /// <param name="f">Function taking array of variables</param>
        /// <param name="variables">Point at which to evaluate gradient</param>
        /// <param name="h">Step size</param>
        /// <returns>Gradient vector</returns>
        public static double[] Gradient(Func<double[], double> f, double[] variables, double h = 1e-8)
        {
            double[] gradient = new double[variables.Length];

            for (int i = 0; i < variables.Length; i++)
            {
                double[] forward = (double[])variables.Clone();
                double[] backward = (double[])variables.Clone();

                forward[i] += h;
                backward[i] -= h;

                gradient[i] = (f(forward) - f(backward)) / (2 * h);
            }

            return gradient;
        }

        #endregion

        #region Integrals

        /// <summary>
        /// Numerical integration using the Trapezoidal Rule
        /// </summary>
        /// <param name="f">Function to integrate</param>
        /// <param name="a">Lower bound</param>
        /// <param name="b">Upper bound</param>
        /// <param name="n">Number of subdivisions</param>
        /// <returns>Approximate integral value</returns>
        public static double TrapezoidalRule(Func<double, double> f, double a, double b, int n = 1000)
        {
            double h = (b - a) / n;
            double sum = 0.5 * (f(a) + f(b));

            for (int i = 1; i < n; i++)
            {
                sum += f(a + i * h);
            }

            return sum * h;
        }

        /// <summary>
        /// Numerical integration using Simpson's Rule (more accurate than trapezoidal)
        /// </summary>
        /// <param name="f">Function to integrate</param>
        /// <param name="a">Lower bound</param>
        /// <param name="b">Upper bound</param>
        /// <param name="n">Number of subdivisions (must be even)</param>
        /// <returns>Approximate integral value</returns>
        public static double SimpsonsRule(Func<double, double> f, double a, double b, int n = 1000)
        {
            if (n % 2 != 0) n++; // Ensure n is even

            double h = (b - a) / n;
            double sum = f(a) + f(b);

            for (int i = 1; i < n; i++)
            {
                double x = a + i * h;
                sum += (i % 2 == 0) ? 2 * f(x) : 4 * f(x);
            }

            return sum * h / 3;
        }

        /// <summary>
        /// Adaptive integration using recursive Simpson's rule with error control
        /// </summary>
        /// <param name="f">Function to integrate</param>
        /// <param name="a">Lower bound</param>
        /// <param name="b">Upper bound</param>
        /// <param name="tolerance">Desired accuracy</param>
        /// <returns>Approximate integral value</returns>
        public static double AdaptiveIntegration(Func<double, double> f, double a, double b, double tolerance = DefaultTolerance)
        {
            return AdaptiveSimpsonsHelper(f, a, b, tolerance, SimpsonsRule(f, a, b, 2), 1);
        }

        private static double AdaptiveSimpsonsHelper(Func<double, double> f, double a, double b, double tolerance, double wholeArea, int depth)
        {
            if (depth > 20) return wholeArea; // Prevent infinite recursion

            double c = (a + b) / 2;
            double leftArea = SimpsonsRule(f, a, c, 2);
            double rightArea = SimpsonsRule(f, c, b, 2);

            if (Math.Abs(leftArea + rightArea - wholeArea) <= 15 * tolerance)
            {
                return leftArea + rightArea + (leftArea + rightArea - wholeArea) / 15;
            }

            return AdaptiveSimpsonsHelper(f, a, c, tolerance / 2, leftArea, depth + 1) +
                   AdaptiveSimpsonsHelper(f, c, b, tolerance / 2, rightArea, depth + 1);
        }

        /// <summary>
        /// Monte Carlo integration for multi-dimensional integrals
        /// </summary>
        /// <param name="f">Function to integrate</param>
        /// <param name="lowerBounds">Lower bounds for each dimension</param>
        /// <param name="upperBounds">Upper bounds for each dimension</param>
        /// <param name="samples">Number of random samples</param>
        /// <returns>Approximate integral value</returns>
        public static double MonteCarloIntegration(Func<double[], double> f, double[] lowerBounds, double[] upperBounds, int samples = 100000)
        {
            var random = new Random();
            double sum = 0;
            double volume = 1;

            for (int i = 0; i < lowerBounds.Length; i++)
            {
                volume *= (upperBounds[i] - lowerBounds[i]);
            }

            for (int i = 0; i < samples; i++)
            {
                double[] point = new double[lowerBounds.Length];
                for (int j = 0; j < lowerBounds.Length; j++)
                {
                    point[j] = lowerBounds[j] + random.NextDouble() * (upperBounds[j] - lowerBounds[j]);
                }
                sum += f(point);
            }

            return volume * sum / samples;
        }

        #endregion

        #region Limits

        /// <summary>
        /// Calculates the limit of a function as x approaches a value
        /// </summary>
        /// <param name="f">Function</param>
        /// <param name="approach">Value that x approaches</param>
        /// <param name="tolerance">Convergence tolerance</param>
        /// <returns>Limit value or NaN if limit doesn't exist</returns>
        public static double Limit(Func<double, double> f, double approach, double tolerance = DefaultTolerance)
        {
            double h = 0.1;
            double previousValue = double.NaN;

            for (int i = 0; i < DefaultMaxIterations; i++)
            {
                double leftLimit = f(approach - h);
                double rightLimit = f(approach + h);

                // Check if both sides converge to the same value
                if (Math.Abs(leftLimit - rightLimit) < tolerance)
                {
                    double currentValue = (leftLimit + rightLimit) / 2;

                    if (i > 0 && Math.Abs(currentValue - previousValue) < tolerance)
                    {
                        return currentValue;
                    }

                    previousValue = currentValue;
                }

                h /= 2;
            }

            return double.NaN; // Limit doesn't exist or doesn't converge
        }

        /// <summary>
        /// Calculates the limit approaching infinity
        /// </summary>
        /// <param name="f">Function</param>
        /// <param name="positiveInfinity">True for +∞, false for -∞</param>
        /// <param name="tolerance">Convergence tolerance</param>
        /// <returns>Limit value or NaN if limit doesn't exist</returns>
        public static double LimitAtInfinity(Func<double, double> f, bool positiveInfinity = true, double tolerance = DefaultTolerance)
        {
            double x = positiveInfinity ? 10 : -10;
            double previousValue = double.NaN;

            for (int i = 0; i < DefaultMaxIterations; i++)
            {
                double currentValue = f(x);

                if (i > 0 && Math.Abs(currentValue - previousValue) < tolerance)
                {
                    return currentValue;
                }

                previousValue = currentValue;
                x = positiveInfinity ? x * 2 : x * 2;
            }

            return double.NaN;
        }

        #endregion

        #region Series

        /// <summary>
        /// Calculates the sum of a convergent infinite series
        /// </summary>
        /// <param name="term">Function that returns the nth term of the series</param>
        /// <param name="tolerance">Convergence tolerance</param>
        /// <param name="maxTerms">Maximum number of terms to compute</param>
        /// <returns>Sum of the series or NaN if doesn't converge</returns>
        public static double InfiniteSeries(Func<int, double> term, double tolerance = DefaultTolerance, int maxTerms = DefaultMaxIterations)
        {
            double sum = 0;

            for (int n = 0; n < maxTerms; n++)
            {
                double currentTerm = term(n);
                sum += currentTerm;

                // Check for convergence
                if (Math.Abs(currentTerm) < tolerance)
                {
                    return sum;
                }
            }

            return double.NaN; // Series doesn't converge within maxTerms
        }

        /// <summary>
        /// Calculates Taylor series expansion of a function around a point
        /// </summary>
        /// <param name="f">Function to expand</param>
        /// <param name="center">Center point of expansion</param>
        /// <param name="x">Point at which to evaluate the series</param>
        /// <param name="terms">Number of terms in the expansion</param>
        /// <returns>Taylor series approximation</returns>
        public static double TaylorSeries(Func<double, double> f, double center, double x, int terms = 10)
        {
            double sum = 0;
            double factorial = 1;

            for (int n = 0; n < terms; n++)
            {
                double derivative = NthDerivative(f, center, n);
                double term = derivative * Math.Pow(x - center, n) / factorial;
                sum += term;

                factorial *= (n + 1);
            }

            return sum;
        }

        /// <summary>
        /// Calculates the power series representation around x = 0 (Maclaurin series)
        /// </summary>
        /// <param name="f">Function to expand</param>
        /// <param name="x">Point at which to evaluate</param>
        /// <param name="terms">Number of terms</param>
        /// <returns>Maclaurin series approximation</returns>
        public static double MaclaurinSeries(Func<double, double> f, double x, int terms = 10)
        {
            return TaylorSeries(f, 0, x, terms);
        }

        #endregion

        #region Special Functions and Utilities

        /// <summary>
        /// Finds critical points of a function (where derivative = 0)
        /// </summary>
        /// <param name="f">Function</param>
        /// <param name="start">Starting point for search</param>
        /// <param name="tolerance">Tolerance for zero detection</param>
        /// <returns>Critical point or NaN if not found</returns>
        public static double FindCriticalPoint(Func<double, double> f, double start, double tolerance = DefaultTolerance)
        {
            // Use Newton's method to find where f'(x) = 0
            double x = start;

            for (int i = 0; i < DefaultMaxIterations; i++)
            {
                double derivative = Derivative(f, x);
                double secondDerivative = NthDerivative(f, x, 2);

                if (Math.Abs(derivative) < tolerance)
                {
                    return x;
                }

                if (Math.Abs(secondDerivative) < tolerance)
                {
                    break; // Avoid division by zero
                }

                double newX = x - derivative / secondDerivative;

                if (Math.Abs(newX - x) < tolerance)
                {
                    return newX;
                }

                x = newX;
            }

            return double.NaN;
        }

        /// <summary>
        /// Finds the root of a function using Newton's method
        /// </summary>
        /// <param name="f">Function</param>
        /// <param name="initialGuess">Initial guess for the root</param>
        /// <param name="tolerance">Tolerance for convergence</param>
        /// <returns>Root of the function or NaN if not found</returns>
        public static double FindRoot(Func<double, double> f, double initialGuess, double tolerance = DefaultTolerance)
        {
            double x = initialGuess;

            for (int i = 0; i < DefaultMaxIterations; i++)
            {
                double fx = f(x);
                double fpx = Derivative(f, x);

                if (Math.Abs(fx) < tolerance)
                {
                    return x;
                }

                if (Math.Abs(fpx) < tolerance)
                {
                    break; // Derivative too small
                }

                double newX = x - fx / fpx;

                if (Math.Abs(newX - x) < tolerance)
                {
                    return newX;
                }

                x = newX;
            }

            return double.NaN;
        }

        /// <summary>
        /// Calculates the arc length of a curve
        /// </summary>
        /// <param name="f">Function y = f(x)</param>
        /// <param name="a">Lower bound</param>
        /// <param name="b">Upper bound</param>
        /// <param name="n">Number of subdivisions</param>
        /// <returns>Arc length</returns>
        public static double ArcLength(Func<double, double> f, double a, double b, int n = 1000)
        {
            double integrand(double x) => Math.Sqrt(1 + Math.Pow(Derivative(f, x), 2));

            return SimpsonsRule(integrand, a, b, n);
        }

        /// <summary>
        /// Calculates the area between two curves
        /// </summary>
        /// <param name="f1">First function</param>
        /// <param name="f2">Second function</param>
        /// <param name="a">Lower bound</param>
        /// <param name="b">Upper bound</param>
        /// <param name="n">Number of subdivisions</param>
        /// <returns>Area between curves</returns>
        public static double AreaBetweenCurves(Func<double, double> f1, Func<double, double> f2, double a, double b, int n = 1000)
        {
            double difference(double x) => Math.Abs(f1(x) - f2(x));
            return SimpsonsRule(difference, a, b, n);
        }

        /// <summary>
        /// Calculates the volume of revolution around the x-axis
        /// </summary>
        /// <param name="f">Function to revolve</param>
        /// <param name="a">Lower bound</param>
        /// <param name="b">Upper bound</param>
        /// <param name="n">Number of subdivisions</param>
        /// <returns>Volume of revolution</returns>
        public static double VolumeOfRevolution(Func<double, double> f, double a, double b, int n = 1000)
        {
            double integrand(double x) => Math.PI * Math.Pow(f(x), 2);
            return SimpsonsRule(integrand, a, b, n);
        }

        #endregion

        #region Common Mathematical Functions for Testing

        /// <summary>
        /// Provides common mathematical functions for testing calculus operations
        /// </summary>
        public static class CommonFunctions
        {
            public static readonly Func<double, double> Linear = x => 2 * x + 3;
            public static readonly Func<double, double> Quadratic = x => x * x - 4 * x + 3;
            public static readonly Func<double, double> Cubic = x => x * x * x - 3 * x * x + 2;
            public static readonly Func<double, double> Exponential = x => Math.Exp(x);
            public static readonly Func<double, double> Logarithmic = x => Math.Log(x);
            public static readonly Func<double, double> Sine = x => Math.Sin(x);
            public static readonly Func<double, double> Cosine = x => Math.Cos(x);
            public static readonly Func<double, double> Tangent = x => Math.Tan(x);
            public static readonly Func<double, double> SquareRoot = x => Math.Sqrt(x);
            public static readonly Func<double, double> Rational = x => 1 / (x * x + 1);
        }

        #endregion

        public static void RunExamples()
        {
            // Calculate derivative of x² at x=3
            double derivative = Calculus.Derivative(x => x * x, 3);

            // Integrate sin(x) from 0 to π
            double integral = Calculus.SimpsonsRule(Math.Sin, 0, Math.PI);

            // Find limit of (sin(x)/x) as x approaches 0
            double limit = Calculus.Limit(x => Math.Sin(x) / x, 0);

            // Calculate Taylor series for e^x around x=0
            double taylorApprox = Calculus.TaylorSeries(Math.Exp, 0, 1, 10);
        }
    }
}
