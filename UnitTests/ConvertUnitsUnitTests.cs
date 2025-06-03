using Core;

namespace UnitTests
{
    [TestClass]
    public class ConvertUnitsUnitTests
    {
        [TestMethod]
        public void ToFahrenheitTest()
        {
            decimal f = ConvertUnits.ToFahrenheit(0);
            Assert.AreEqual(32m, f);
        }

        [TestMethod]
        public void ToCelsiusTest()
        {
            decimal c = ConvertUnits.ToCelsius(32m);
            Assert.AreEqual(0, c);
        }

        [TestMethod]
        public void ToInchesTest()
        {
            decimal i = ConvertUnits.ToInches(25.4m);
            Assert.AreEqual(1m, i);
        }

        [TestMethod]
        public void ToMillimetersTest()
        {
            decimal m = ConvertUnits.ToMillimeters(1m);
            Assert.AreEqual(25.4m, m);
        }

        //[TestMethod]
        //public void ToFeetTest()
        //{
        //    decimal f = ConvertUnits.ToFeet(304.8m);
        //    Assert.AreEqual(1m, f);
        //}
    }
}
