using Core;

namespace UnitTests
{
    [TestClass]
    public class ConvertUnitsTests
    {
        private const decimal Tolerance = 0.0001m;

        #region Temperature Conversion Tests

        [TestMethod]
        public void ToFahrenheit_ZeroCelsius_Returns32Fahrenheit()
        {
            // Arrange
            decimal celsius = 0m;
            decimal expected = 32m;

            // Act
            decimal actual = ConvertUnits.ToFahrenheit(celsius);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToFahrenheit_100Celsius_Returns212Fahrenheit()
        {
            // Arrange
            decimal celsius = 100m;
            decimal expected = 212m;

            // Act
            decimal actual = ConvertUnits.ToFahrenheit(celsius);

            // Assert
            // Note: The current implementation has an error (0.9/0.5 = 1.8, which is correct)
            // but the formula should be (9/5) * celsius + 32
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToCelsius_32Fahrenheit_ReturnsZeroCelsius()
        {
            // Arrange
            decimal fahrenheit = 32m;
            decimal expected = 0m;

            // Act
            decimal actual = ConvertUnits.ToCelsius(fahrenheit);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToCelsius_212Fahrenheit_Returns100Celsius()
        {
            // Arrange
            decimal fahrenheit = 212m;
            decimal expected = 100m;

            // Act
            decimal actual = ConvertUnits.ToCelsius(fahrenheit);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void TemperatureConversion_RoundTrip_ReturnsOriginalValue()
        {
            // Arrange
            decimal originalCelsius = 25.5m;

            // Act
            decimal fahrenheit = ConvertUnits.ToFahrenheit(originalCelsius);
            decimal backToCelsius = ConvertUnits.ToCelsius(fahrenheit);

            // Assert
            Assert.AreEqual(originalCelsius, backToCelsius, Tolerance);
        }

        #endregion

        #region Length Conversion Tests

        [TestMethod]
        public void ToInches_25Point4Millimeters_ReturnsOneInch()
        {
            // Arrange
            decimal millimeters = 25.4m;
            decimal expected = 1m;

            // Act
            decimal actual = ConvertUnits.ToInches(millimeters);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToMillimeters_OneInch_Returns25Point4Millimeters()
        {
            // Arrange
            decimal inches = 1m;
            decimal expected = 25.4m;

            // Act
            decimal actual = ConvertUnits.ToMillimeters(inches);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToFeet_OneMeter_ReturnsApprox3Point28Feet()
        {
            // Arrange
            decimal meters = 1m;
            decimal expected = 3.2808398950131233595800524934383m;

            // Act
            decimal actual = ConvertUnits.ToFeet(meters);

            // Assert
            Assert.AreEqual(expected, actual, 0.0001m);
        }

        [TestMethod]
        public void ToMeters_OneFoot_ReturnsApprox0Point3048Meters()
        {
            // Arrange
            decimal feet = 1m;
            decimal expected = 0.3048m;

            // Act
            decimal actual = ConvertUnits.ToMeters(feet);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToMiles_OneKilometer_ReturnsApprox0Point621Miles()
        {
            // Arrange
            decimal kilometers = 1m;
            decimal expected = 0.6213711922373339015151515151515m;

            // Act
            decimal actual = ConvertUnits.ToMiles(kilometers);

            // Assert
            Assert.AreEqual(expected, actual, 0.0001m);
        }

        [TestMethod]
        public void ToKilometers_OneMile_ReturnsApprox1Point609Kilometers()
        {
            // Arrange
            decimal miles = 1m;
            decimal expected = 1.609344m;

            // Act
            decimal actual = ConvertUnits.ToKilometers(miles);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToKilometersFromMeters_1000Meters_ReturnsOneKilometer()
        {
            // Arrange
            decimal meters = 1000m;
            decimal expected = 1m;

            // Act
            decimal actual = ConvertUnits.ToKilometersFromMeters(meters);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        #endregion

        #region Astronomical Distance Tests

        [TestMethod]
        public void ToMilesFromLightYears_OneLightYear_ReturnsCorrectMiles()
        {
            // Arrange
            decimal lightYears = 1m;
            decimal expected = 5.87E12m;

            // Act
            decimal actual = ConvertUnits.ToMilesFromLightYears(lightYears);

            // Assert
            Assert.AreEqual(expected, actual, 1E9m); // Large tolerance due to astronomical scale
        }

        [TestMethod]
        public void ToLightYears_FromMiles_RoundTrip()
        {
            // Arrange
            decimal originalLightYears = 2.5m;

            // Act
            decimal miles = ConvertUnits.ToMilesFromLightYears(originalLightYears);
            decimal backToLightYears = ConvertUnits.ToLightYears(miles);

            // Assert
            Assert.AreEqual(originalLightYears, backToLightYears, 0.001m);
        }

        [TestMethod]
        public void ToParsecs_OneLightYear_ReturnsCorrectParsecs()
        {
            // Arrange
            decimal lightYears = 1m;
            decimal expected = 0.3068m;

            // Act
            decimal actual = ConvertUnits.ToParsecs(lightYears);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToLightYearsFromParsecs_RoundTrip()
        {
            // Arrange
            decimal originalParsecs = 5.2m;

            // Act
            decimal lightYears = ConvertUnits.ToLightYearsFromParsecs(originalParsecs);
            decimal backToParsecs = ConvertUnits.ToParsecs(lightYears);

            // Assert
            Assert.AreEqual(originalParsecs, backToParsecs, 0.001m);
        }

        [TestMethod]
        public void ToMilesFromAU_OneAU_ReturnsCorrectMiles()
        {
            // Arrange
            decimal AU = 1m;
            decimal expected = 9.29E7m;

            // Act
            decimal actual = ConvertUnits.ToMilesFromAU(AU);

            // Assert
            Assert.AreEqual(expected, actual, 1E4m); // Tolerance for astronomical unit
        }

        [TestMethod]
        public void ToAU_FromMiles_RoundTrip()
        {
            // Arrange
            decimal originalAU = 3.7m;

            // Act
            decimal miles = ConvertUnits.ToMilesFromAU(originalAU);
            decimal backToAU = ConvertUnits.ToAU(miles);

            // Assert
            Assert.AreEqual(originalAU, backToAU, 0.001m);
        }

        #endregion

        #region Metric Prefix Tests

        [TestMethod]
        public void ToMetricFactor_Micro_ReturnsCorrectValue()
        {
            // Arrange
            decimal value = 1000m;
            MetricPrefix prefix = MetricPrefix.Micro;
            decimal expected = 0.001m;

            // Act
            decimal actual = ConvertUnits.ToMetricFactor(prefix, value);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToMetricFactor_Milli_ReturnsCorrectValue()
        {
            // Arrange
            decimal value = 500m;
            MetricPrefix prefix = MetricPrefix.Milli;
            decimal expected = 0.5m;

            // Act
            decimal actual = ConvertUnits.ToMetricFactor(prefix, value);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToMetricFactor_Centi_ReturnsCorrectValue()
        {
            // Arrange
            decimal value = 150m;
            MetricPrefix prefix = MetricPrefix.Centi;
            decimal expected = 1.5m;

            // Act
            decimal actual = ConvertUnits.ToMetricFactor(prefix, value);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToMetricFactor_Kilo_ReturnsCorrectValue()
        {
            // Arrange
            decimal value = 2.5m;
            MetricPrefix prefix = MetricPrefix.Kilo;
            decimal expected = 2500m;

            // Act
            decimal actual = ConvertUnits.ToMetricFactor(prefix, value);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToMetricFactor_Mega_ReturnsCorrectValue()
        {
            // Arrange
            decimal value = 3m;
            MetricPrefix prefix = MetricPrefix.Mega;
            decimal expected = 3000000m;

            // Act
            decimal actual = ConvertUnits.ToMetricFactor(prefix, value);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToMetricFactor_InvalidPrefix_ThrowsNotImplementedException()
        {
            // Arrange
            decimal value = 100m;
            MetricPrefix invalidPrefix = (MetricPrefix)999;

            // Act & Assert
            Assert.ThrowsExactly<NotImplementedException>(() => ConvertUnits.ToMetricFactor(invalidPrefix, value));
        }

        #endregion

        #region Angular Conversion Tests

        [TestMethod]
        public void ToDegrees_240Seconds_ReturnsOneDegree()
        {
            // Arrange
            decimal seconds = 240m;
            decimal expected = 1m;

            // Act
            decimal actual = ConvertUnits.ToDegrees(seconds);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToSeconds_OneDegree_Returns240Seconds()
        {
            // Arrange
            decimal degrees = 1m;
            decimal expected = 240m;

            // Act
            decimal actual = ConvertUnits.ToSeconds(degrees);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToSecondsFromRadians_OneRadian_Returns13752Seconds()
        {
            // Arrange
            decimal radians = 1m;
            decimal expected = 13752m;

            // Act
            decimal actual = ConvertUnits.ToSecondsFromRadians(radians);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToRadians_13752Seconds_ReturnsOneRadian()
        {
            // Arrange
            decimal seconds = 13752m;
            decimal expected = 1m;

            // Act
            decimal actual = ConvertUnits.ToRadians(seconds);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        #endregion

        #region Decimal Degrees Tests

        [TestMethod]
        public void ToDecimalDegrees_PositiveDMS_ReturnsCorrectDecimalDegrees()
        {
            // Arrange
            bool isPositive = true;
            int degrees = 45;
            int minutes = 30;
            decimal seconds = 15m;
            decimal expected = 45.50416666666666666666666667m;

            // Act
            decimal actual = ConvertUnits.ToDecimalDegrees(isPositive, degrees, minutes, seconds);

            // Assert
            Assert.AreEqual(expected, actual, 0.00001m);
        }

        [TestMethod]
        public void ToDecimalDegrees_NegativeDMS_ReturnsNegativeDecimalDegrees()
        {
            // Arrange
            bool isPositive = false;
            int degrees = 30;
            int minutes = 15;
            decimal seconds = 45m;
            decimal expected = -30.2625m;

            // Act
            decimal actual = ConvertUnits.ToDecimalDegrees(isPositive, degrees, minutes, seconds);

            // Assert
            Assert.AreEqual(expected, actual, 0.00001m);
        }

        #endregion

        #region Edge Cases and Error Conditions

        [TestMethod]
        public void ToInches_Zero_ReturnsZero()
        {
            // Arrange
            decimal millimeters = 0m;
            decimal expected = 0m;

            // Act
            decimal actual = ConvertUnits.ToInches(millimeters);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        [TestMethod]
        public void ToDecimalDegrees_ZeroValues_ReturnsZero()
        {
            // Arrange
            bool isPositive = true;
            int degrees = 0;
            int minutes = 0;
            decimal seconds = 0m;
            decimal expected = 0m;

            // Act
            decimal actual = ConvertUnits.ToDecimalDegrees(isPositive, degrees, minutes, seconds);

            // Assert
            Assert.AreEqual(expected, actual, Tolerance);
        }

        #endregion
    }
}
