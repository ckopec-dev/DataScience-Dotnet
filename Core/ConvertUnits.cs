
namespace Core
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/International_System_of_Units
    /// </summary>
    public class ConvertUnits
    {
        #region Public methods

        public static decimal ToFahrenheit(decimal celsius)
        {
            return 0.9m / 0.5m * celsius + 32m;
        }

        public static decimal ToCelsius(decimal fahrenheit)
        {
            return 0.5m / 0.9m * (fahrenheit - 32m);
        }

        public static decimal ToInches(decimal millimeters)
        {
            return millimeters / 25.4m;
        }

        public static decimal ToMillimeters(decimal inches)
        {
            return inches * 25.4m;
        }

        public static decimal ToFeet(decimal meters)
        {
            return meters / 0.3048m;
        }

        public static decimal ToMeters(decimal feet)
        {
            return feet * 0.3048m;
        }

        public static decimal ToMiles(decimal kilometers)
        {
            return kilometers / 1.609344m;
        }

        public static decimal ToKilometersFromMeters(decimal meters)
        {
            return meters / 1000m;
        }

        public static decimal ToKilometers(decimal miles)
        {
            return miles * 1.609344m;
        }

        public static decimal ToMilesFromLightYears(decimal lightYears)
        {
            return lightYears * 5.87E12m;
        }

        public static decimal ToLightYears(decimal miles)
        {
            return miles / 5.87E12m;
        }

        public static decimal ToParsecs(decimal lightYears)
        {
            return lightYears * 0.3068m;
        }

        public static decimal ToLightYearsFromParsecs(decimal parsecs)
        {
            return parsecs / 0.3068m;
        }

        public static decimal ToMilesFromAU(decimal AU)
        {
            return AU * 9.29E7m;
        }

        public static decimal ToAU(decimal miles)
        {
            return miles / 9.29E7m;
        }

        public static decimal ToMetricFactor(MetricPrefix prefix, decimal value)
        {
            return prefix switch
            {
                MetricPrefix.Micro => value * 0.000001m,
                MetricPrefix.Milli => value * 0.001m,
                MetricPrefix.Centi => value * 0.01m,
                MetricPrefix.Kilo => value * 1000m,
                MetricPrefix.Mega => value * 1000000m,
                _ => throw new NotImplementedException(),
            };
        }

        public static decimal ToDegrees(decimal seconds)
        {
            // There are 360 degrees to 1 day. 24 hours to a day. 86400 seconds to a day. Therefore 1 degree = 240 seconds.
            return seconds / 240m;
        }

        public static decimal ToSeconds(decimal degrees)
        {
            return degrees * 240m;
        }

        public static decimal ToSecondsFromRadians(decimal radians)
        {
            return radians * 13752m;
        }

        public static decimal ToRadians(decimal seconds)
        {
            return seconds / 13752m;
        }

        public static decimal ToDecimalDegrees(bool isPositive, int degrees, int minutes, decimal seconds)
        {
            decimal sign = 1m;

            if (!isPositive)
                sign = -1m;

            decimal dd = Math.Abs(degrees);
            decimal dm = seconds / 60m;
            decimal tm = dm + minutes;
            decimal dec = tm / 60m;
            dd += dec;
            dd *= sign;

            return dd;
        }

        #endregion
    }
}
