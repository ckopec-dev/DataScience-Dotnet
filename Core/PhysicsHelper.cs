
namespace Core
{
    public static class PhysicsHelper
    {
        public const double G = 6.674E-11;          // m^3/kg*s^2
        public const double EARTH_MASS = 5.9736E24; // in kg
        public const double MOON_MASS = .0735E24;   // in kg
        public const double DAY_SECONDS = 86400d;   // seconds in a day

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
    }
}
