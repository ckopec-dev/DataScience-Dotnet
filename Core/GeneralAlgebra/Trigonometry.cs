using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.GeneralAlgebra
{
    // <summary>
    /// Comprehensive trigonometry class providing various trigonometric functions,
    /// conversions, and utility methods.
    /// </summary>
    public static class Trigonometry
    {
        #region Constants

        /// <summary>
        /// Pi constant with high precision
        /// </summary>
        public const double PI = Math.PI;

        /// <summary>
        /// 2 * Pi constant
        /// </summary>
        public const double TWO_PI = 2.0 * Math.PI;

        /// <summary>
        /// Pi / 2 constant
        /// </summary>
        public const double HALF_PI = Math.PI / 2.0;

        /// <summary>
        /// Pi / 4 constant
        /// </summary>
        public const double QUARTER_PI = Math.PI / 4.0;

        /// <summary>
        /// Degrees to radians conversion factor
        /// </summary>
        public const double DEG_TO_RAD = Math.PI / 180.0;

        /// <summary>
        /// Radians to degrees conversion factor
        /// </summary>
        public const double RAD_TO_DEG = 180.0 / Math.PI;

        /// <summary>
        /// Gradians to radians conversion factor
        /// </summary>
        public const double GRAD_TO_RAD = Math.PI / 200.0;

        /// <summary>
        /// Radians to gradians conversion factor
        /// </summary>
        public const double RAD_TO_GRAD = 200.0 / Math.PI;

        #endregion

        #region Angle Conversion Methods

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="degrees">Angle in degrees</param>
        /// <returns>Angle in radians</returns>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * DEG_TO_RAD;
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Angle in degrees</returns>
        public static double RadiansToDegrees(double radians)
        {
            return radians * RAD_TO_DEG;
        }

        /// <summary>
        /// Converts gradians to radians
        /// </summary>
        /// <param name="gradians">Angle in gradians</param>
        /// <returns>Angle in radians</returns>
        public static double GradiansToRadians(double gradians)
        {
            return gradians * GRAD_TO_RAD;
        }

        /// <summary>
        /// Converts radians to gradians
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Angle in gradians</returns>
        public static double RadiansToGradians(double radians)
        {
            return radians * RAD_TO_GRAD;
        }

        /// <summary>
        /// Converts degrees to gradians
        /// </summary>
        /// <param name="degrees">Angle in degrees</param>
        /// <returns>Angle in gradians</returns>
        public static double DegreesToGradians(double degrees)
        {
            return degrees * 10.0 / 9.0;
        }

        /// <summary>
        /// Converts gradians to degrees
        /// </summary>
        /// <param name="gradians">Angle in gradians</param>
        /// <returns>Angle in degrees</returns>
        public static double GradiansToDegrees(double gradians)
        {
            return gradians * 9.0 / 10.0;
        }

        #endregion

        #region Basic Trigonometric Functions

        /// <summary>
        /// Calculates sine of angle in radians
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Sine value</returns>
        public static double Sin(double radians)
        {
            return Math.Sin(radians);
        }

        /// <summary>
        /// Calculates cosine of angle in radians
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Cosine value</returns>
        public static double Cos(double radians)
        {
            return Math.Cos(radians);
        }

        /// <summary>
        /// Calculates tangent of angle in radians
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Tangent value</returns>
        public static double Tan(double radians)
        {
            return Math.Tan(radians);
        }

        /// <summary>
        /// Calculates sine of angle in degrees
        /// </summary>
        /// <param name="degrees">Angle in degrees</param>
        /// <returns>Sine value</returns>
        public static double SinDeg(double degrees)
        {
            return Math.Sin(DegreesToRadians(degrees));
        }

        /// <summary>
        /// Calculates cosine of angle in degrees
        /// </summary>
        /// <param name="degrees">Angle in degrees</param>
        /// <returns>Cosine value</returns>
        public static double CosDeg(double degrees)
        {
            return Math.Cos(DegreesToRadians(degrees));
        }

        /// <summary>
        /// Calculates tangent of angle in degrees
        /// </summary>
        /// <param name="degrees">Angle in degrees</param>
        /// <returns>Tangent value</returns>
        public static double TanDeg(double degrees)
        {
            return Math.Tan(DegreesToRadians(degrees));
        }

        #endregion

        #region Reciprocal Trigonometric Functions

        /// <summary>
        /// Calculates cosecant (1/sin) of angle in radians
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Cosecant value</returns>
        /// <exception cref="DivideByZeroException">Thrown when sine is zero</exception>
        public static double Csc(double radians)
        {
            double sinValue = Math.Sin(radians);
            if (Math.Abs(sinValue) < double.Epsilon)
                throw new DivideByZeroException("Cosecant is undefined when sine equals zero");
            return 1.0 / sinValue;
        }

        /// <summary>
        /// Calculates secant (1/cos) of angle in radians
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Secant value</returns>
        /// <exception cref="DivideByZeroException">Thrown when cosine is zero</exception>
        public static double Sec(double radians)
        {
            double cosValue = Math.Cos(radians);
            if (Math.Abs(cosValue) < double.Epsilon)
                throw new DivideByZeroException("Secant is undefined when cosine equals zero");
            return 1.0 / cosValue;
        }

        /// <summary>
        /// Calculates cotangent (1/tan) of angle in radians
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Cotangent value</returns>
        /// <exception cref="DivideByZeroException">Thrown when tangent is zero</exception>
        public static double Cot(double radians)
        {
            double tanValue = Math.Tan(radians);
            if (Math.Abs(tanValue) < double.Epsilon)
                throw new DivideByZeroException("Cotangent is undefined when tangent equals zero");
            return 1.0 / tanValue;
        }

        #endregion

        #region Inverse Trigonometric Functions

        /// <summary>
        /// Calculates arcsine (inverse sine) in radians
        /// </summary>
        /// <param name="value">Input value (must be between -1 and 1)</param>
        /// <returns>Angle in radians</returns>
        public static double Asin(double value)
        {
            return Math.Asin(value);
        }

        /// <summary>
        /// Calculates arccosine (inverse cosine) in radians
        /// </summary>
        /// <param name="value">Input value (must be between -1 and 1)</param>
        /// <returns>Angle in radians</returns>
        public static double Acos(double value)
        {
            return Math.Acos(value);
        }

        /// <summary>
        /// Calculates arctangent (inverse tangent) in radians
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>Angle in radians</returns>
        public static double Atan(double value)
        {
            return Math.Atan(value);
        }

        /// <summary>
        /// Calculates arctangent of y/x using the signs to determine quadrant
        /// </summary>
        /// <param name="y">Y coordinate</param>
        /// <param name="x">X coordinate</param>
        /// <returns>Angle in radians</returns>
        public static double Atan2(double y, double x)
        {
            return Math.Atan2(y, x);
        }

        /// <summary>
        /// Calculates arcsine in degrees
        /// </summary>
        /// <param name="value">Input value (must be between -1 and 1)</param>
        /// <returns>Angle in degrees</returns>
        public static double AsinDeg(double value)
        {
            return RadiansToDegrees(Math.Asin(value));
        }

        /// <summary>
        /// Calculates arccosine in degrees
        /// </summary>
        /// <param name="value">Input value (must be between -1 and 1)</param>
        /// <returns>Angle in degrees</returns>
        public static double AcosDeg(double value)
        {
            return RadiansToDegrees(Math.Acos(value));
        }

        /// <summary>
        /// Calculates arctangent in degrees
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>Angle in degrees</returns>
        public static double AtanDeg(double value)
        {
            return RadiansToDegrees(Math.Atan(value));
        }

        /// <summary>
        /// Calculates arctangent of y/x in degrees using the signs to determine quadrant
        /// </summary>
        /// <param name="y">Y coordinate</param>
        /// <param name="x">X coordinate</param>
        /// <returns>Angle in degrees</returns>
        public static double Atan2Deg(double y, double x)
        {
            return RadiansToDegrees(Math.Atan2(y, x));
        }

        #endregion

        #region Hyperbolic Functions

        /// <summary>
        /// Calculates hyperbolic sine
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>Hyperbolic sine value</returns>
        public static double Sinh(double value)
        {
            return Math.Sinh(value);
        }

        /// <summary>
        /// Calculates hyperbolic cosine
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>Hyperbolic cosine value</returns>
        public static double Cosh(double value)
        {
            return Math.Cosh(value);
        }

        /// <summary>
        /// Calculates hyperbolic tangent
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>Hyperbolic tangent value</returns>
        public static double Tanh(double value)
        {
            return Math.Tanh(value);
        }

        #endregion

        #region Utility Functions

        /// <summary>
        /// Normalizes an angle in radians to the range [0, 2π)
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        /// <returns>Normalized angle in radians</returns>
        public static double NormalizeRadians(double radians)
        {
            radians %= TWO_PI;
            if (radians < 0)
                radians += TWO_PI;
            return radians;
        }

        /// <summary>
        /// Normalizes an angle in degrees to the range [0, 360)
        /// </summary>
        /// <param name="degrees">Angle in degrees</param>
        /// <returns>Normalized angle in degrees</returns>
        public static double NormalizeDegrees(double degrees)
        {
            degrees %= 360.0;
            if (degrees < 0)
                degrees += 360.0;
            return degrees;
        }

        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        /// <param name="x1">X coordinate of first point</param>
        /// <param name="y1">Y coordinate of first point</param>
        /// <param name="x2">X coordinate of second point</param>
        /// <param name="y2">Y coordinate of second point</param>
        /// <returns>Distance between the points</returns>
        public static double Distance(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Converts polar coordinates to Cartesian coordinates
        /// </summary>
        /// <param name="radius">Radius (distance from origin)</param>
        /// <param name="angleRadians">Angle in radians</param>
        /// <returns>Tuple containing (x, y) coordinates</returns>
        public static (double x, double y) PolarToCartesian(double radius, double angleRadians)
        {
            double x = radius * Math.Cos(angleRadians);
            double y = radius * Math.Sin(angleRadians);
            return (x, y);
        }

        /// <summary>
        /// Converts Cartesian coordinates to polar coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Tuple containing (radius, angle in radians)</returns>
        public static (double radius, double angleRadians) CartesianToPolar(double x, double y)
        {
            double radius = Math.Sqrt(x * x + y * y);
            double angleRadians = Math.Atan2(y, x);
            return (radius, angleRadians);
        }

        /// <summary>
        /// Checks if two angles are equivalent (differ by a multiple of 2π)
        /// </summary>
        /// <param name="angle1">First angle in radians</param>
        /// <param name="angle2">Second angle in radians</param>
        /// <param name="tolerance">Tolerance for comparison</param>
        /// <returns>True if angles are equivalent</returns>
        public static bool AreAnglesEquivalent(double angle1, double angle2, double tolerance = 1e-10)
        {
            double diff = Math.Abs(NormalizeRadians(angle1) - NormalizeRadians(angle2));
            return diff < tolerance || Math.Abs(diff - TWO_PI) < tolerance;
        }

        /// <summary>
        /// Calculates the shortest angular distance between two angles
        /// </summary>
        /// <param name="from">Starting angle in radians</param>
        /// <param name="to">Ending angle in radians</param>
        /// <returns>Shortest angular distance (-π to π)</returns>
        public static double AngularDistance(double from, double to)
        {
            double diff = NormalizeRadians(to) - NormalizeRadians(from);
            if (diff > PI)
                diff -= TWO_PI;
            else if (diff < -PI)
                diff += TWO_PI;
            return diff;
        }

        /// <summary>
        /// Linearly interpolates between two angles taking the shortest path
        /// </summary>
        /// <param name="from">Starting angle in radians</param>
        /// <param name="to">Ending angle in radians</param>
        /// <param name="t">Interpolation parameter (0 to 1)</param>
        /// <returns>Interpolated angle in radians</returns>
        public static double LerpAngle(double from, double to, double t)
        {
            double distance = AngularDistance(from, to);
            return from + distance * t;
        }

        #endregion

        #region Triangle Functions

        /// <summary>
        /// Calculates the area of a triangle using two sides and the included angle
        /// </summary>
        /// <param name="sideA">Length of first side</param>
        /// <param name="sideB">Length of second side</param>
        /// <param name="angleRadians">Included angle in radians</param>
        /// <returns>Area of the triangle</returns>
        public static double TriangleAreaSAS(double sideA, double sideB, double angleRadians)
        {
            return 0.5 * sideA * sideB * Math.Sin(angleRadians);
        }

        /// <summary>
        /// Calculates the third side of a triangle using the law of cosines
        /// </summary>
        /// <param name="sideA">Length of first known side</param>
        /// <param name="sideB">Length of second known side</param>
        /// <param name="angleRadians">Angle between the known sides in radians</param>
        /// <returns>Length of the third side</returns>
        public static double LawOfCosines(double sideA, double sideB, double angleRadians)
        {
            double cosAngle = Math.Cos(angleRadians);
            return Math.Sqrt(sideA * sideA + sideB * sideB - 2 * sideA * sideB * cosAngle);
        }

        /// <summary>
        /// Calculates an angle of a triangle using the law of cosines
        /// </summary>
        /// <param name="sideA">Length of first side</param>
        /// <param name="sideB">Length of second side</param>
        /// <param name="sideC">Length of third side</param>
        /// <returns>Angle opposite to sideC in radians</returns>
        public static double LawOfCosinesAngle(double sideA, double sideB, double sideC)
        {
            double cosC = (sideA * sideA + sideB * sideB - sideC * sideC) / (2 * sideA * sideB);
            return Math.Acos(Math.Max(-1.0, Math.Min(1.0, cosC))); // Clamp to valid range
        }

        #endregion

        public static void RunExamples()
        {
            // Example usage of the Trigonometry class
            Console.WriteLine("Trigonometry Class Examples:");
            Console.WriteLine();
            
            // Angle conversions
            double degrees = 45.0;
            double radians = Trigonometry.DegreesToRadians(degrees);
            Console.WriteLine($"{degrees}° = {radians} radians");
            Console.WriteLine($"{radians} radians = {Trigonometry.RadiansToDegrees(radians)}°");
            Console.WriteLine();
            
            // Basic trigonometric functions
            Console.WriteLine($"sin(45°) = {Trigonometry.SinDeg(45):F6}");
            Console.WriteLine($"cos(45°) = {Trigonometry.CosDeg(45):F6}");
            Console.WriteLine($"tan(45°) = {Trigonometry.TanDeg(45):F6}");
            Console.WriteLine();
            
            // Polar to Cartesian conversion
            var(x, y) = Trigonometry.PolarToCartesian(1.0, Trigonometry.QUARTER_PI);
            Console.WriteLine($"Polar (1, π/4) = Cartesian ({x:F6}, {y:F6})");
            
            // Cartesian to Polar conversion
            var(r, angle) = Trigonometry.CartesianToPolar(x, y);
            Console.WriteLine($"Cartesian ({x:F6}, {y:F6}) = Polar ({r:F6}, {angle:F6})");
            Console.WriteLine();
            
            // Triangle calculations
            double area = Trigonometry.TriangleAreaSAS(3, 4, Trigonometry.HALF_PI);
            Console.WriteLine($"Area of triangle with sides 3, 4 and 90° angle: {area}");
            
            double thirdSide = Trigonometry.LawOfCosines(3, 4, Trigonometry.HALF_PI);
            Console.WriteLine($"Third side of triangle: {thirdSide:F6}");
        }
    }
}
