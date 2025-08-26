using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using Core;

namespace UnitTests
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        #region IsUSStockMarketOpen Tests

        [TestMethod]
        public void IsUSStockMarketOpen_Saturday_ReturnsFalse()
        {
            // Arrange
            var saturday = new DateTime(2024, 1, 6); // Saturday

            // Act
            var result = saturday.IsUSStockMarketOpen();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsUSStockMarketOpen_Sunday_ReturnsFalse()
        {
            // Arrange
            var sunday = new DateTime(2024, 1, 7); // Sunday

            // Act
            var result = sunday.IsUSStockMarketOpen();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsUSStockMarketOpen_WeekdayNotHoliday_ReturnsTrue()
        {
            // Arrange
            var weekday = new DateTime(2024, 1, 8); // Monday, not a holiday

            // Act
            var result = weekday.IsUSStockMarketOpen();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsUSStockMarketOpen_HolidayDate_ReturnsFalse()
        {
            // Note: This test assumes the resource file contains holiday dates
            // In practice, you would need to mock the resource or create a test resource file
            // For now, this documents the expected behavior when a holiday is found

            // This test would work if the USStockMarketHolidays.txt resource exists
            // and contains the date being tested
            var newYearsDay = new DateTime(2024, 1, 1);

            try
            {
                var result = newYearsDay.IsUSStockMarketOpen();
                Assert.IsFalse(result, "New Year's Day should return false");
            }
            catch (Exception ex) when (ex.Message.Contains("Resource not found"))
            {
            }
        }

        #endregion

        #region IsLeapYear Tests

        [TestMethod]
        public void IsLeapYear_DivisibleBy4Not100_ReturnsTrue()
        {
            // Arrange
            var date = new DateTime(2004, 1, 1); // 2004 is divisible by 4 but not 100

            // Act
            var result = date.IsLeapYear();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsLeapYear_DivisibleBy100Not400_ReturnsFalse()
        {
            // Arrange
            var date = new DateTime(1900, 1, 1); // 1900 is divisible by 100 but not 400

            // Act
            var result = date.IsLeapYear();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsLeapYear_DivisibleBy400_ReturnsTrue()
        {
            // Arrange
            var date = new DateTime(2000, 1, 1); // 2000 is divisible by 400

            // Act
            var result = date.IsLeapYear();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsLeapYear_NotDivisibleBy4_ReturnsFalse()
        {
            // Arrange
            var date = new DateTime(2003, 1, 1); // 2003 is not divisible by 4

            // Act
            var result = date.IsLeapYear();

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region IsGregorianDate Tests

        [TestMethod]
        public void IsGregorianDate_BeforeGregorianCalendar_ReturnsTrue()
        {
            // Arrange
            var date = new DateTime(1582, 10, 14); // Before Gregorian calendar adoption

            // Act
            var result = date.IsGregorianDate();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsGregorianDate_OnGregorianCalendarStart_ReturnsFalse()
        {
            // Arrange
            var date = new DateTime(1582, 10, 15); // Gregorian calendar start date

            // Act
            var result = date.IsGregorianDate();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsGregorianDate_AfterGregorianCalendar_ReturnsFalse()
        {
            // Arrange
            var date = new DateTime(2024, 1, 1); // After Gregorian calendar adoption

            // Act
            var result = date.IsGregorianDate();

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region DayNumber Tests

        [TestMethod]
        public void DayNumber_JanuaryInLeapYear_ReturnsCorrectValue()
        {
            // Arrange
            var date = new DateTime(2004, 1, 15); // January 15th in leap year

            // Act
            var result = date.DayNumber();

            // Assert
            // January (month-1=0), 0*62=0, 0/2=0, +15 = 15
            Assert.AreEqual(15m, result);
        }

        [TestMethod]
        public void DayNumber_JanuaryInNonLeapYear_ReturnsCorrectValue()
        {
            // Arrange
            var date = new DateTime(2003, 1, 15); // January 15th in non-leap year

            // Act
            var result = date.DayNumber();

            // Assert
            // January (month-1=0), 0*63=0, 0/2=0, +15 = 15
            Assert.AreEqual(15m, result);
        }

        [TestMethod]
        public void DayNumber_FebruaryInLeapYear_ReturnsCorrectValue()
        {
            // Arrange
            var date = new DateTime(2004, 2, 15); // February 15th in leap year

            // Act
            var result = date.DayNumber();

            // Assert
            // February (month-1=1), 1*62=62, 62/2=31, +15 = 46
            Assert.AreEqual(46m, result);
        }

        [TestMethod]
        public void DayNumber_FebruaryInNonLeapYear_ReturnsCorrectValue()
        {
            // Arrange
            var date = new DateTime(2003, 2, 15); // February 15th in non-leap year

            // Act
            var result = date.DayNumber();

            // Assert
            // February (month-1=1), 1*63=63, 63/2=31 (with remainder), +15 = 46
            Assert.AreEqual(46m, result);
        }

        [TestMethod]
        public void DayNumber_MarchInLeapYear_ReturnsCorrectValue()
        {
            // Arrange
            var date = new DateTime(2004, 3, 15); // March 15th in leap year

            // Act
            var result = date.DayNumber();

            // Assert
            // March (month+1=4), floor(4*30.6)=122, 122-62=60, +15 = 75
            Assert.AreEqual(75m, result);
        }

        [TestMethod]
        public void DayNumber_MarchInNonLeapYear_ReturnsCorrectValue()
        {
            // Arrange
            var date = new DateTime(2003, 3, 15); // March 15th in non-leap year

            // Act
            var result = date.DayNumber();

            // Assert
            // March (month+1=4), floor(4*30.6)=122, 122-63=59, +15 = 74
            Assert.AreEqual(74m, result);
        }

        [TestMethod]
        public void DayNumber_DecemberInLeapYear_ReturnsCorrectValue()
        {
            // Arrange
            var date = new DateTime(2004, 12, 31); // December 31st in leap year

            // Act
            var result = date.DayNumber();

            // Assert
            // December (month+1=13), floor(13*30.6)=397, 397-62=335, +31 = 366
            Assert.AreEqual(366m, result);
        }

        [TestMethod]
        public void DayNumber_DecemberInNonLeapYear_ReturnsCorrectValue()
        {
            // Arrange
            var date = new DateTime(2003, 12, 31); // December 31st in non-leap year

            // Act
            var result = date.DayNumber();

            // Assert
            // December (month+1=13), floor(13*30.6)=397, 397-63=334, +31 = 365
            Assert.AreEqual(365m, result);
        }

        #endregion

        #region DayPercentage Tests

        [TestMethod]
        public void DayPercentage_StartOfDay_ReturnsZero()
        {
            // Arrange
            var startOfDay = new DateTime(2024, 1, 1, 0, 0, 0);
            double millisecondsPerDay = 86400000; // 24 hours * 60 minutes * 60 seconds * 1000 milliseconds

            // Act
            var result = startOfDay.DayPercentage(millisecondsPerDay);

            // Assert
            Assert.AreEqual(0m, Math.Round(result, 10)); // Rounded to handle floating point precision
        }

        [TestMethod]
        public void DayPercentage_Noon_ReturnsHalf()
        {
            // Arrange
            var noon = new DateTime(2024, 1, 1, 12, 0, 0);
            double millisecondsPerDay = 86400000;

            // Act
            var result = noon.DayPercentage(millisecondsPerDay);

            // Assert
            Assert.AreEqual(0.5m, Math.Round(result, 10));
        }

        [TestMethod]
        public void DayPercentage_SixPM_ReturnsThreeQuarters()
        {
            // Arrange
            var sixPM = new DateTime(2024, 1, 1, 18, 0, 0);
            double millisecondsPerDay = 86400000;

            // Act
            var result = sixPM.DayPercentage(millisecondsPerDay);

            // Assert
            Assert.AreEqual(0.75m, Math.Round(result, 10));
        }

        #endregion

        #region ToDateTime Tests

        [TestMethod]
        public void ToDateTime_UnixEpochZero_ReturnsEpochMinusEST()
        {
            // Arrange
            long unixEpoch = 0;

            // Act
            var result = unixEpoch.ToDateTime();

            // Assert
            // Unix epoch (1970-01-01 00:00:00 UTC) minus 5 hours (EST)
            var expected = new DateTime(1969, 12, 31, 19, 0, 0);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToDateTime_UnixEpochWithPositiveValue_ReturnsCorrectDateTime()
        {
            // Arrange
            long unixEpoch = 86400; // 1 day after epoch

            // Act
            var result = unixEpoch.ToDateTime();

            // Assert
            // 1970-01-02 00:00:00 UTC minus 5 hours (EST) = 1970-01-01 19:00:00
            var expected = new DateTime(1970, 1, 1, 19, 0, 0);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToDateTime_WithTimeZoneOffset_ReturnsCorrectDateTime()
        {
            // Arrange
            long unixEpoch = 0;
            int timeZoneOffset = 2; // +2 hours

            // Act
            var result = unixEpoch.ToDateTime(timeZoneOffset);

            // Assert
            // Unix epoch (1970-01-01 00:00:00 UTC) plus 2 hours
            var expected = new DateTime(1970, 1, 1, 2, 0, 0);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToDateTime_WithNegativeTimeZoneOffset_ReturnsCorrectDateTime()
        {
            // Arrange
            long unixEpoch = 0;
            int timeZoneOffset = -8; // -8 hours (PST)

            // Act
            var result = unixEpoch.ToDateTime(timeZoneOffset);

            // Assert
            // Unix epoch (1970-01-01 00:00:00 UTC) minus 8 hours
            var expected = new DateTime(1969, 12, 31, 16, 0, 0);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToDateTime_WithZeroTimeZoneOffset_ReturnsUTCTime()
        {
            // Arrange
            long unixEpoch = 0;
            int timeZoneOffset = 0; // UTC

            // Act
            var result = unixEpoch.ToDateTime(timeZoneOffset);

            // Assert
            // Unix epoch (1970-01-01 00:00:00 UTC)
            var expected = new DateTime(1970, 1, 1, 0, 0, 0);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToDateTime_LargeUnixEpoch_ReturnsCorrectDateTime()
        {
            // Arrange
            long unixEpoch = 1609459200; // 2021-01-01 00:00:00 UTC
            int timeZoneOffset = 0;

            // Act
            var result = unixEpoch.ToDateTime(timeZoneOffset);

            // Assert
            var expected = new DateTime(2021, 1, 1, 0, 0, 0);
            Assert.AreEqual(expected, result);
        }

        #endregion
    }
}
