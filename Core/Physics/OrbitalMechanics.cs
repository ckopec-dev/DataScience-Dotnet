
namespace Core.Physics
{
    /// <summary>
    /// Main orbital mechanics class containing calculations and utilities
    /// </summary>
    public class OrbitalMechanics
    {
        // Constants
        public const double GM_EARTH = 3.986004418e5; // km³/s² (Earth's gravitational parameter)
        public const double EARTH_RADIUS = 6371.0;    // km
        public const double DEG_TO_RAD = Math.PI / 180.0;
        public const double RAD_TO_DEG = 180.0 / Math.PI;

        /// <summary>
        /// Calculates orbital period using Kepler's Third Law
        /// </summary>
        /// <param name="semiMajorAxis">Semi-major axis in km</param>
        /// <param name="mu">Gravitational parameter (default: Earth's GM)</param>
        /// <returns>Orbital period in seconds</returns>
        public static double CalculateOrbitalPeriod(double semiMajorAxis, double mu = GM_EARTH)
        {
            return 2 * Math.PI * Math.Sqrt(Math.Pow(semiMajorAxis, 3) / mu);
        }

        /// <summary>
        /// Calculates orbital velocity at any point in the orbit
        /// </summary>
        /// <param name="r">Current distance from center of mass (km)</param>
        /// <param name="a">Semi-major axis (km)</param>
        /// <param name="mu">Gravitational parameter (default: Earth's GM)</param>
        /// <returns>Orbital velocity in km/s</returns>
        public static double CalculateOrbitalVelocity(double r, double a, double mu = GM_EARTH)
        {
            return Math.Sqrt(mu * (2.0 / r - 1.0 / a));
        }

        /// <summary>
        /// Calculates circular orbital velocity
        /// </summary>
        /// <param name="r">Orbital radius (km)</param>
        /// <param name="mu">Gravitational parameter (default: Earth's GM)</param>
        /// <returns>Circular velocity in km/s</returns>
        public static double CalculateCircularVelocity(double r, double mu = GM_EARTH)
        {
            return Math.Sqrt(mu / r);
        }

        /// <summary>
        /// Calculates escape velocity
        /// </summary>
        /// <param name="r">Distance from center of mass (km)</param>
        /// <param name="mu">Gravitational parameter (default: Earth's GM)</param>
        /// <returns>Escape velocity in km/s</returns>
        public static double CalculateEscapeVelocity(double r, double mu = GM_EARTH)
        {
            return Math.Sqrt(2 * mu / r);
        }

        /// <summary>
        /// Calculates apoapsis distance
        /// </summary>
        /// <param name="a">Semi-major axis (km)</param>
        /// <param name="e">Eccentricity</param>
        /// <returns>Apoapsis distance in km</returns>
        public static double CalculateApoapsis(double a, double e)
        {
            return a * (1 + e);
        }

        /// <summary>
        /// Calculates periapsis distance
        /// </summary>
        /// <param name="a">Semi-major axis (km)</param>
        /// <param name="e">Eccentricity</param>
        /// <returns>Periapsis distance in km</returns>
        public static double CalculatePeriapsis(double a, double e)
        {
            return a * (1 - e);
        }

        /// <summary>
        /// Converts orbital elements to Cartesian state vector
        /// </summary>
        /// <param name="elements">Orbital elements</param>
        /// <param name="mu">Gravitational parameter (default: Earth's GM)</param>
        /// <returns>Tuple containing position and velocity vectors</returns>
        public static (Vector3D position, Vector3D velocity) ElementsToStateVector(OrbitalElements elements, double mu = GM_EARTH)
        {
            double a = elements.SemiMajorAxis;
            double e = elements.Eccentricity;
            double i = elements.Inclination;
            double omega = elements.LongitudeOfAscendingNode;
            double w = elements.ArgumentOfPeriapsis;
            double nu = elements.TrueAnomaly;

            // Calculate position and velocity in orbital plane
            double r = a * (1 - e * e) / (1 + e * Math.Cos(nu));
            double h = Math.Sqrt(mu * a * (1 - e * e));

            // Position in orbital plane
            double x_orb = r * Math.Cos(nu);
            double y_orb = r * Math.Sin(nu);

            // Velocity in orbital plane
            double vx_orb = -(mu / h) * Math.Sin(nu);
            double vy_orb = (mu / h) * (e + Math.Cos(nu));

            // Rotation matrices
            double cos_omega = Math.Cos(omega);
            double sin_omega = Math.Sin(omega);
            double cos_i = Math.Cos(i);
            double sin_i = Math.Sin(i);
            double cos_w = Math.Cos(w);
            double sin_w = Math.Sin(w);

            // Transform to inertial frame
            Vector3D position = new (
                (cos_omega * cos_w - sin_omega * sin_w * cos_i) * x_orb + (-cos_omega * sin_w - sin_omega * cos_w * cos_i) * y_orb,
                (sin_omega * cos_w + cos_omega * sin_w * cos_i) * x_orb + (-sin_omega * sin_w + cos_omega * cos_w * cos_i) * y_orb,
                (sin_w * sin_i) * x_orb + (cos_w * sin_i) * y_orb
            );

            Vector3D velocity = new(
                (cos_omega * cos_w - sin_omega * sin_w * cos_i) * vx_orb + (-cos_omega * sin_w - sin_omega * cos_w * cos_i) * vy_orb,
                (sin_omega * cos_w + cos_omega * sin_w * cos_i) * vx_orb + (-sin_omega * sin_w + cos_omega * cos_w * cos_i) * vy_orb,
                (sin_w * sin_i) * vx_orb + (cos_w * sin_i) * vy_orb
            );

            return (position, velocity);
        }

        /// <summary>
        /// Converts Cartesian state vector to orbital elements
        /// </summary>
        /// <param name="position">Position vector (km)</param>
        /// <param name="velocity">Velocity vector (km/s)</param>
        /// <param name="mu">Gravitational parameter (default: Earth's GM)</param>
        /// <returns>Orbital elements</returns>
        public static OrbitalElements StateVectorToElements(Vector3D position, Vector3D velocity, double mu = GM_EARTH)
        {
            Vector3D r = position;
            Vector3D v = velocity;
            double r_mag = r.Magnitude;
            double v_mag = v.Magnitude;

            // Angular momentum vector
            Vector3D h = Vector3D.Cross(r, v);
            double h_mag = h.Magnitude;

            // Eccentricity vector
            Vector3D e_vec = Vector3D.Cross(v, h) / mu - r.Normalized;
            double e = e_vec.Magnitude;

            // Semi-major axis
            double energy = v_mag * v_mag / 2 - mu / r_mag;
            double a = -mu / (2 * energy);

            // Inclination
            double i = Math.Acos(h.Z / h_mag);

            // Node vector
            Vector3D n = Vector3D.Cross(new Vector3D(0, 0, 1), h);
            double n_mag = n.Magnitude;

            // Longitude of ascending node
            double omega = 0;
            if (n_mag > 1e-10)
            {
                omega = Math.Acos(n.X / n_mag);
                if (n.Y < 0) omega = 2 * Math.PI - omega;
            }

            // Argument of periapsis
            double w = 0;
            if (n_mag > 1e-10 && e > 1e-10)
            {
                w = Math.Acos(Vector3D.Dot(n, e_vec) / (n_mag * e));
                if (e_vec.Z < 0) w = 2 * Math.PI - w;
            }

            // True anomaly
            double nu = 0;
            if (e > 1e-10)
            {
                nu = Math.Acos(Vector3D.Dot(e_vec, r) / (e * r_mag));
                if (Vector3D.Dot(r, v) < 0) nu = 2 * Math.PI - nu;
            }

            return new OrbitalElements(a, e, i, omega, w, nu);
        }

        /// <summary>
        /// Solves Kepler's equation using Newton-Raphson method
        /// </summary>
        /// <param name="M">Mean anomaly (radians)</param>
        /// <param name="e">Eccentricity</param>
        /// <param name="tolerance">Convergence tolerance</param>
        /// <returns>Eccentric anomaly in radians</returns>
        public static double SolveKeplersEquation(double M, double e, double tolerance = 1e-12)
        {
            double E = M; // Initial guess
            double deltaE;

            do
            {
                deltaE = (E - e * Math.Sin(E) - M) / (1 - e * Math.Cos(E));
                E -= deltaE;
            } while (Math.Abs(deltaE) > tolerance);

            return E;
        }

        /// <summary>
        /// Converts eccentric anomaly to true anomaly
        /// </summary>
        /// <param name="E">Eccentric anomaly (radians)</param>
        /// <param name="e">Eccentricity</param>
        /// <returns>True anomaly in radians</returns>
        public static double EccentricToTrueAnomaly(double E, double e)
        {
            return 2 * Math.Atan2(Math.Sqrt(1 + e) * Math.Sin(E / 2), Math.Sqrt(1 - e) * Math.Cos(E / 2));
        }

        /// <summary>
        /// Calculates delta-V required for Hohmann transfer
        /// </summary>
        /// <param name="r1">Initial circular orbit radius (km)</param>
        /// <param name="r2">Final circular orbit radius (km)</param>
        /// <param name="mu">Gravitational parameter (default: Earth's GM)</param>
        /// <returns>Total delta-V in km/s</returns>
        public static double HohmannTransferDeltaV(double r1, double r2, double mu = GM_EARTH)
        {
            double v1 = Math.Sqrt(mu / r1);
            double v2 = Math.Sqrt(mu / r2);

            double a_transfer = (r1 + r2) / 2;
            double v1_transfer = Math.Sqrt(mu * (2 / r1 - 1 / a_transfer));
            double v2_transfer = Math.Sqrt(mu * (2 / r2 - 1 / a_transfer));

            double deltaV1 = Math.Abs(v1_transfer - v1);
            double deltaV2 = Math.Abs(v2 - v2_transfer);

            return deltaV1 + deltaV2;
        }

        /// <summary>
        /// Calculates the sphere of influence radius for a celestial body
        /// </summary>
        /// <param name="a">Semi-major axis of the body's orbit around primary (km)</param>
        /// <param name="m">Mass of the secondary body</param>
        /// <param name="M">Mass of the primary body</param>
        /// <returns>Sphere of influence radius in km</returns>
        public static double SphereOfInfluence(double a, double m, double M)
        {
            return a * Math.Pow(m / M, 2.0 / 5.0);
        }
    }
}
