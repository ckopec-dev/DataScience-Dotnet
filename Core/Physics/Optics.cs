namespace Core.Physics
{
    public static class Optics
    {
        #region Constants

        public const decimal LIGHT_SPEED = 186000m; // Given in miles/second.

        #endregion

        #region Public methods

        public static decimal RefractiveIndex(decimal lightSpeed, decimal speedInSubstance)
        {
            // The speeds must be in the same units (e.g. miles/second).

            return lightSpeed / speedInSubstance;
        }

        public static decimal SpeedInSubstance(decimal refractiveIndex)
        {
            return LIGHT_SPEED / refractiveIndex;
        }

        public static decimal FocalRatio(decimal focalLengthMilimeters, decimal lensDiameterMilimeters)
        {
            // The init units are typically in millimeters.
            // The focal length is the distance from the center of the lens to the focal point (where the light rays converge).

            return focalLengthMilimeters / lensDiameterMilimeters;
        }

        public static decimal FocalLengthConcave(decimal convexRadius, decimal concaveRadius, decimal refractiveIndex)
        {
            // Used for refractor lens.
            // Convex part of the lens is the side closest to the light rays.
            // Concave part of the lens is the side closest to the focal point. This is given as a negative number.

            return 1m / ((refractiveIndex - 1) * (1m / convexRadius - 1m / concaveRadius));
        }

        public static decimal FocalLengthConvex(decimal concaveRadius)
        {
            // concave radius is given as a negative number.

            return -concaveRadius / 2m;
        }

        public static decimal MagnifyingPower(decimal focalLengthObjective, decimal focalLengthEyepiece)
        {
            // Parameters must be in the same units.

            return focalLengthObjective / focalLengthEyepiece;
        }

        public static decimal FocalLengthEyepiece(decimal focalLengthObjective, decimal magnifyingPower)
        {
            return focalLengthObjective / magnifyingPower;
        }

        public static decimal AiryDiscRadius(decimal wavelength, decimal lensDiameter, decimal focalLength)
        {
            // Returns radius in microns.
            // Wavelength is in nanometers.
            // Diameter is in millimeters.

            return 0.00122m * wavelength * focalLength / lensDiameter;
        }

        public static decimal TrueFieldOfView(decimal degreesApparentAngularField, decimal power)
        {
            return degreesApparentAngularField / power;
        }

        #endregion
    }
}
