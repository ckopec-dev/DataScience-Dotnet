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
    }
}
