
namespace Core
{
    public static class PhysicsHelper
    {
        public const double G = 6.674E-11;          // m^3/kg*s^2
        public const double EARTH_MASS = 5.9736E24; // in kg
        public const double MOON_MASS = .0735E24;   // in kg
        public const double DAY_SECONDS = 86400d;   // seconds in a day
        public const double EARTH_GRAVITY = 9.8d;   // acceleration falling freely near the surface of the earth

        #region Kepler's 3rd law

        public static double OrbitalRadius(double massInKilograms, double timeInSeconds)
        {
            // cube root of (g*m*t^2 / 4*pi^2)
            // Returns radius in meters.

            double step1 = G * massInKilograms * Math.Pow(timeInSeconds, 2d);
            //Console.WriteLine("step1: {0}", step1);

            double step2 = 4d * Math.Pow(Math.PI, 2d);
            //Console.WriteLine("step2: {0}", step2);

            double step3 = step1 / step2;
            //Console.WriteLine("step3: {0}", step3);

            return Math.Pow(step3, 1d / 3d);
        }

        public static double OrbitalTime(double radiusInMeters, double massInKilograms)
        {
            // square root of (4*pi^2*r^3 / g*m)

            double step1 = 4d * Math.Pow(Math.PI, 2d) * Math.Pow(radiusInMeters, 3d);
            //Console.WriteLine("step1: {0}", step1);

            double step2 = G * massInKilograms;
            //Console.WriteLine("step2: {0}", step2);

            double step3 = step1 / step2;
            //Console.WriteLine("step3: {0}", step3);

            return Math.Sqrt(step3);
        }

        public static double OrbitalMass(double radiusInMeters, double timeInSeconds)
        {
            // 4*pi^2*r^3 / g*t^2

            double step1 = 4d * Math.Pow(Math.PI, 2d) * Math.Pow(radiusInMeters, 3d);
            Console.WriteLine("step1: {0}", step1);

            double step2 = G * Math.Pow(timeInSeconds, 2d);
            Console.WriteLine("step2: {0}", step2);

            return step1 / step2;
        }

        #endregion

        #region Radioactivity

        public static double Decay(double initialQuantity, double halfLife, double elapsedTime)
        {
            return initialQuantity * Math.Pow(0.5d, (elapsedTime / halfLife));
        }

        #endregion

        #region Astronomy

        public static DateTime Easter(int year)
        {
            Math.DivRem(year, 19, out int a);
            int b = Math.DivRem(year, 100, out int c);
            int d = Math.DivRem(b, 4, out int e);
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            Math.DivRem((19 * a) + b - d - g + 15, 30, out int h);
            int i = Math.DivRem(c, 4, out int k);
            Math.DivRem(32 + (2 * e) + (2 * i) - h - k, 7, out int l);
            int m = (a + (11 * h) + (22 * l)) / 451;
            int n = Math.DivRem(h + l - (7 * m) + 114, 31, out int p);

            int day = p + 1;
            int month = n;

            return new DateTime(year, month, day);
        }

        #endregion

        #region Motion

        public static double Velocity(double meters, double seconds)
        {
            return meters / seconds;
        }

        public static double Acceleration(double velocityEnd, double velocityStart, double seconds)
        {
            return (velocityEnd - velocityStart) / seconds;
        }

        public static double FallDistance(double originalSpeed, double timeElapsed, double acceleration)
        {
            return originalSpeed * timeElapsed + acceleration * timeElapsed * timeElapsed / 2d;
        }

        #endregion

        #region Gravity

        public static double Gravity(double mass1, double mass2, double distance)
        {
            // Mass1: in kilograms.
            // Mass2: in kilograms.,
            // Distance between the center of the two masses. In meters.
            
            // Returns newtons.
            return G * (mass1 * mass2) / (distance * distance);
        }

        #endregion
    }
}
