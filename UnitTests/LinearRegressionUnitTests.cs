using Core.Maths;

namespace UnitTests
{
    [TestClass]
    public class LinearRegressionUnitTests
    {
        [TestMethod]
        public void SlopeTest()
        {
            decimal slope = LinearRegression.Slope([5m, 7m, 12m, 16m, 20m], [40m, 120m, 180m, 210m, 240m]);

            Assert.AreEqual(12.207792207792207792207792208m, slope);
        }

        [TestMethod]
        public void InterceptTest()
        {
            decimal intercept = LinearRegression.Intercept([5m, 7m, 12m, 16m, 20m], [40m, 120m, 180m, 210m, 240m]);

            Assert.AreEqual(11.50649350649350649350649350m, intercept);
        }
    }
}

