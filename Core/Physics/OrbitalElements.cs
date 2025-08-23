namespace Core.Physics
{
    /// <summary>
    /// Represents orbital elements that define an orbit
    /// </summary>
    public class OrbitalElements(double semiMajorAxis, double eccentricity, double inclination,
                          double longitudeOfAscendingNode, double argumentOfPeriapsis, double trueAnomaly)
    {
        public double SemiMajorAxis { get; set; } = semiMajorAxis;
        public double Eccentricity { get; set; } = eccentricity;
        public double Inclination { get; set; } = inclination;
        public double LongitudeOfAscendingNode { get; set; } = longitudeOfAscendingNode;
        public double ArgumentOfPeriapsis { get; set; } = argumentOfPeriapsis;
        public double TrueAnomaly { get; set; } = trueAnomaly;
    }
}
