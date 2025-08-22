using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GeneralAlgebra
{
    /// <summary>
    /// A comprehensive general algebra class providing various mathematical operations and utilities
    /// </summary>
    public static class Algebra
    {
        #region Basic Arithmetic Operations

        /// <summary>
        /// Adds two numbers
        /// </summary>
        public static double Add(double a, double b) => a + b;

        /// <summary>
        /// Subtracts two numbers
        /// </summary>
        public static double Subtract(double a, double b) => a - b;

        /// <summary>
        /// Multiplies two numbers
        /// </summary>
        public static double Multiply(double a, double b) => a * b;

        /// <summary>
        /// Divides two numbers with division by zero check
        /// </summary>
        public static double Divide(double a, double b)
        {
            if (Math.Abs(b) < double.Epsilon)
                throw new DivideByZeroException("Cannot divide by zero");
            return a / b;
        }

        #endregion

        #region Power and Root Operations

        /// <summary>
        /// Raises a number to a power
        /// </summary>
        public static double Power(double baseNum, double exponent) => Math.Pow(baseNum, exponent);

        /// <summary>
        /// Calculates square root
        /// </summary>
        public static double SquareRoot(double number)
        {
            if (number < 0)
                throw new ArgumentException("Cannot calculate square root of negative number");
            return Math.Sqrt(number);
        }

        /// <summary>
        /// Calculates nth root of a number
        /// </summary>
        public static double NthRoot(double number, double n)
        {
            if (n == 0)
                throw new ArgumentException("Root degree cannot be zero");
            return Math.Pow(number, 1.0 / n);
        }

        #endregion

        #region Linear Equations

        /// <summary>
        /// Solves linear equation ax + b = 0
        /// Returns the value of x
        /// </summary>
        public static double SolveLinearEquation(double a, double b)
        {
            if (Math.Abs(a) < double.Epsilon)
            {
                if (Math.Abs(b) < double.Epsilon)
                    throw new InvalidOperationException("Infinite solutions (0 = 0)");
                else
                    throw new InvalidOperationException("No solution (contradiction)");
            }
            return -b / a;
        }

        /// <summary>
        /// Solves system of two linear equations using Cramer's rule
        /// a1*x + b1*y = c1
        /// a2*x + b2*y = c2
        /// </summary>
        public static (double x, double y) SolveLinearSystem(double a1, double b1, double c1,
                                                             double a2, double b2, double c2)
        {
            double det = a1 * b2 - a2 * b1;

            if (Math.Abs(det) < double.Epsilon)
                throw new InvalidOperationException("System has no unique solution (determinant is zero)");

            double x = (c1 * b2 - c2 * b1) / det;
            double y = (a1 * c2 - a2 * c1) / det;

            return (x, y);
        }

        #endregion

        #region Quadratic Equations

        /// <summary>
        /// Represents the result of solving a quadratic equation
        /// </summary>
        public class QuadraticResult
        {
            public bool HasRealSolutions { get; set; }
            public double? Root1 { get; set; }
            public double? Root2 { get; set; }
            public double Discriminant { get; set; }

            public override string ToString()
            {
                if (!HasRealSolutions)
                    return $"No real solutions (Discriminant: {Discriminant:F2})";

                if (Math.Abs(Root1!.Value - Root2!.Value) < double.Epsilon)
                    return $"One solution: x = {Root1:F2}";

                return $"Two solutions: x₁ = {Root1:F2}, x₂ = {Root2:F2}";
            }
        }

        /// <summary>
        /// Solves quadratic equation ax² + bx + c = 0
        /// </summary>
        public static QuadraticResult SolveQuadratic(double a, double b, double c)
        {
            if (Math.Abs(a) < double.Epsilon)
                throw new ArgumentException("Coefficient 'a' cannot be zero for quadratic equation");

            double discriminant = b * b - 4 * a * c;
            var result = new QuadraticResult { Discriminant = discriminant };

            if (discriminant < 0)
            {
                result.HasRealSolutions = false;
                return result;
            }

            result.HasRealSolutions = true;
            double sqrtDiscriminant = Math.Sqrt(discriminant);
            result.Root1 = (-b + sqrtDiscriminant) / (2 * a);
            result.Root2 = (-b - sqrtDiscriminant) / (2 * a);

            return result;
        }

        #endregion

        #region Polynomial Operations

        /// <summary>
        /// Represents a polynomial with coefficients
        /// </summary>
        public class Polynomial
        {
            public double[] Coefficients { get; private set; }
            public int Degree => Coefficients.Length - 1;

            public Polynomial(params double[] coefficients)
            {
                if (coefficients == null || coefficients.Length == 0)
                    throw new ArgumentException("Polynomial must have at least one coefficient");

                Coefficients = [.. coefficients];
            }

            /// <summary>
            /// Evaluates polynomial at given x value
            /// </summary>
            public double Evaluate(double x)
            {
                double result = 0;
                for (int i = 0; i < Coefficients.Length; i++)
                {
                    result += Coefficients[i] * Math.Pow(x, Coefficients.Length - 1 - i);
                }
                return result;
            }

            /// <summary>
            /// Adds two polynomials
            /// </summary>
            public static Polynomial Add(Polynomial p1, Polynomial p2)
            {
                int maxLength = Math.Max(p1.Coefficients.Length, p2.Coefficients.Length);
                double[] result = new double[maxLength];

                for (int i = 0; i < maxLength; i++)
                {
                    double coeff1 = i < p1.Coefficients.Length ? p1.Coefficients[p1.Coefficients.Length - 1 - i] : 0;
                    double coeff2 = i < p2.Coefficients.Length ? p2.Coefficients[p2.Coefficients.Length - 1 - i] : 0;
                    result[maxLength - 1 - i] = coeff1 + coeff2;
                }

                return new Polynomial(result);
            }

            public override string ToString()
            {
                var terms = new List<string>();
                for (int i = 0; i < Coefficients.Length; i++)
                {
                    int power = Coefficients.Length - 1 - i;
                    double coeff = Coefficients[i];

                    if (Math.Abs(coeff) < double.Epsilon) continue;

                    string term;
                    if (power == 0)
                        term = coeff.ToString("F1");
                    else if (power == 1)
                        term = $"{coeff:F1}x";
                    else
                        term = $"{coeff:F1}x^{power}";

                    terms.Add(term);
                }

                return terms.Count == 0 ? "0" : string.Join(" + ", terms).Replace("+ -", "- ");
            }
        }

        #endregion

        #region Matrix Operations

        /// <summary>
        /// Represents a matrix and basic operations
        /// </summary>
        public class Matrix(double[,] data)
        {
            public double[,] Data { get; private set; } = (double[,])data.Clone();
            public int Rows { get; private set; } = data.GetLength(0);
            public int Columns { get; private set; } = data.GetLength(1);

            public double this[int row, int col]
            {
                get => Data[row, col];
                set => Data[row, col] = value;
            }

            /// <summary>
            /// Adds two matrices
            /// </summary>
            public static Matrix Add(Matrix m1, Matrix m2)
            {
                if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
                    throw new ArgumentException("Matrices must have same dimensions for addition");

                double[,] result = new double[m1.Rows, m1.Columns];
                for (int i = 0; i < m1.Rows; i++)
                    for (int j = 0; j < m1.Columns; j++)
                        result[i, j] = m1[i, j] + m2[i, j];

                return new Matrix(result);
            }

            /// <summary>
            /// Multiplies two matrices
            /// </summary>
            public static Matrix Multiply(Matrix m1, Matrix m2)
            {
                if (m1.Columns != m2.Rows)
                    throw new ArgumentException("First matrix columns must equal second matrix rows");

                double[,] result = new double[m1.Rows, m2.Columns];
                for (int i = 0; i < m1.Rows; i++)
                {
                    for (int j = 0; j < m2.Columns; j++)
                    {
                        for (int k = 0; k < m1.Columns; k++)
                        {
                            result[i, j] += m1[i, k] * m2[k, j];
                        }
                    }
                }

                return new Matrix(result);
            }

            /// <summary>
            /// Calculates determinant for 2x2 matrix
            /// </summary>
            public double Determinant()
            {
                if (Rows != 2 || Columns != 2)
                    throw new InvalidOperationException("Determinant calculation implemented only for 2x2 matrices");

                return Data[0, 0] * Data[1, 1] - Data[0, 1] * Data[1, 0];
            }

            public override string ToString()
            {
                var result = new System.Text.StringBuilder();
                for (int i = 0; i < Rows; i++)
                {
                    result.Append('[');
                    for (int j = 0; j < Columns; j++)
                    {
                        result.Append($"{Data[i, j]:F2}");
                        if (j < Columns - 1) result.Append(", ");
                    }
                    result.AppendLine("]");
                }
                return result.ToString();
            }
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Calculates greatest common divisor using Euclidean algorithm
        /// </summary>
        public static long GCD(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Calculates least common multiple
        /// </summary>
        public static long LCM(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }

        /// <summary>
        /// Calculates factorial of a number
        /// </summary>
        public static long Factorial(int n)
        {
            if (n < 0)
                throw new ArgumentException("Factorial is not defined for negative numbers");

            if (n == 0 || n == 1)
                return 1;

            long result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;

            return result;
        }

        /// <summary>
        /// Checks if a number is prime
        /// </summary>
        public static bool IsPrime(long number)
        {
            if (number < 2) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            long sqrt = (long)Math.Sqrt(number);
            for (long i = 3; i <= sqrt; i += 2)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Calculates binomial coefficient (n choose k)
        /// </summary>
        public static long BinomialCoefficient(int n, int k)
        {
            if (k > n || k < 0)
                return 0;

            if (k == 0 || k == n)
                return 1;

            // Use symmetry property: C(n,k) = C(n,n-k)
            if (k > n - k)
                k = n - k;

            long result = 1;
            for (int i = 0; i < k; i++)
            {
                result = result * (n - i) / (i + 1);
            }

            return result;
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        public static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        public static double RadiansToDegrees(double radians) => radians * 180.0 / Math.PI;

        #endregion

        #region Example Usage Methods

        /// <summary>
        /// Demonstrates various algebra operations
        /// </summary>
        public static void RunExamples()
        {
            Console.WriteLine("=== Algebra Class Examples ===\n");

            // Basic operations
            Console.WriteLine($"5 + 3 = {Add(5, 3)}");
            Console.WriteLine($"10 - 4 = {Subtract(10, 4)}");
            Console.WriteLine($"6 * 7 = {Multiply(6, 7)}");
            Console.WriteLine($"15 / 3 = {Divide(15, 3)}");
            Console.WriteLine();

            // Power operations
            Console.WriteLine($"2^8 = {Power(2, 8)}");
            Console.WriteLine($"√16 = {SquareRoot(16)}");
            Console.WriteLine($"∛27 = {NthRoot(27, 3)}");
            Console.WriteLine();

            // Linear equation: 2x + 6 = 0
            Console.WriteLine($"Solving 2x + 6 = 0: x = {SolveLinearEquation(2, 6)}");

            // System of equations
            var (x, y) = SolveLinearSystem(2, 3, 7, 1, -1, 1);
            Console.WriteLine($"System solution: x = {x:F2}, y = {y:F2}");
            Console.WriteLine();

            // Quadratic equation: x² - 5x + 6 = 0
            var quadResult = SolveQuadratic(1, -5, 6);
            Console.WriteLine($"Quadratic x² - 5x + 6 = 0: {quadResult}");
            Console.WriteLine();

            // Polynomial
            var poly = new Polynomial(1, -3, 2); // x² - 3x + 2
            Console.WriteLine($"Polynomial: {poly}");
            Console.WriteLine($"Evaluated at x=1: {poly.Evaluate(1)}");
            Console.WriteLine();

            // Matrix operations
            var matrix1 = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            var matrix2 = new Matrix(new double[,] { { 5, 6 }, { 7, 8 } });
            var matrixSum = Matrix.Add(matrix1, matrix2);
            Console.WriteLine($"Matrix addition result:\n{matrixSum}");

            // Utility functions
            Console.WriteLine($"GCD(48, 18) = {GCD(48, 18)}");
            Console.WriteLine($"LCM(4, 6) = {LCM(4, 6)}");
            Console.WriteLine($"5! = {Factorial(5)}");
            Console.WriteLine($"Is 17 prime? {IsPrime(17)}");
            Console.WriteLine($"C(5,2) = {BinomialCoefficient(5, 2)}");
        }

        #endregion
    }
}
