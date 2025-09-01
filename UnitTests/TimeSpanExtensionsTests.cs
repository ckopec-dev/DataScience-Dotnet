using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TimeSpanExtensionsTests
    {
        [TestMethod]
        public void ToFriendlyString_WithZeroPrecision_ReturnsCorrectFormat()
        {
            // Arrange
            var ts = TimeSpan.FromMilliseconds(500);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("500 millisecond(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_WithHighPrecision_ReturnsCorrectFormat()
        {
            // Arrange
            var ts = TimeSpan.FromMilliseconds(123.456789);
            int precision = 3;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("123.457 millisecond(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_MillisecondsUnder1000_ReturnsMilliseconds()
        {
            // Arrange
            var ts = TimeSpan.FromMilliseconds(999.99);
            int precision = 2;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("999.99 millisecond(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_ExactlyOneSecond_ReturnsSeconds()
        {
            // Arrange
            var ts = TimeSpan.FromSeconds(1);
            int precision = 1;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("1.0 second(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_SecondsUnder60_ReturnsSeconds()
        {
            // Arrange
            var ts = TimeSpan.FromSeconds(59.9);
            int precision = 1;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("59.9 second(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_FractionalSeconds_ReturnsSeconds()
        {
            // Arrange
            var ts = TimeSpan.FromSeconds(30.5);
            int precision = 2;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("30.50 second(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_ExactlyOneMinute_ReturnsMinutes()
        {
            // Arrange
            var ts = TimeSpan.FromMinutes(1);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("1 minute(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_MinutesUnder60_ReturnsMinutes()
        {
            // Arrange
            var ts = TimeSpan.FromMinutes(59.99);
            int precision = 2;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("59.99 minute(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_FractionalMinutes_ReturnsMinutes()
        {
            // Arrange
            var ts = TimeSpan.FromMinutes(15.25);
            int precision = 3;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("15.250 minute(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_ExactlyOneHour_ReturnsHours()
        {
            // Arrange
            var ts = TimeSpan.FromHours(1);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("1 hour(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_MultipleHours_ReturnsHours()
        {
            // Arrange
            var ts = TimeSpan.FromHours(24.5);
            int precision = 1;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("24.5 hour(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_VeryLargeTimeSpan_ReturnsHours()
        {
            // Arrange
            var ts = TimeSpan.FromDays(365);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("8760 hour(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_BoundaryCase999Milliseconds_ReturnsMilliseconds()
        {
            // Arrange
            var ts = TimeSpan.FromMilliseconds(999);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("999 millisecond(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_BoundaryCase1000Milliseconds_ReturnsSeconds()
        {
            // Arrange
            var ts = TimeSpan.FromMilliseconds(1000);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("1 second(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_BoundaryCase59Seconds_ReturnsSeconds()
        {
            // Arrange
            var ts = TimeSpan.FromSeconds(59);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("59 second(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_BoundaryCase60Seconds_ReturnsMinutes()
        {
            // Arrange
            var ts = TimeSpan.FromSeconds(60);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("1 minute(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_BoundaryCase59Minutes_ReturnsMinutes()
        {
            // Arrange
            var ts = TimeSpan.FromMinutes(59);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("59 minute(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_BoundaryCase60Minutes_ReturnsHours()
        {
            // Arrange
            var ts = TimeSpan.FromMinutes(60);
            int precision = 0;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("1 hour(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_MaxPrecision_ReturnsCorrectFormat()
        {
            // Arrange
            var ts = TimeSpan.FromMilliseconds(123.456789);
            int precision = 10;

            // Act
            string result = ts.ToFriendlyString(precision);
            Assert.StartsWith("123.4567", result);
            Assert.EndsWith(" millisecond(s)", result);
        }

        [TestMethod]
        public void ToFriendlyString_ComplexTimeSpan_ReturnsCorrectUnit()
        {
            // Arrange - 1 hour, 30 minutes, 45 seconds, 123 milliseconds
            var ts = new TimeSpan(0, 1, 30, 45, 123);
            int precision = 2;

            // Act
            string result = ts.ToFriendlyString(precision);

            // Assert
            Assert.AreEqual("1.51 hour(s)", result); // Total hours = 1.5125
        }
    }
}
