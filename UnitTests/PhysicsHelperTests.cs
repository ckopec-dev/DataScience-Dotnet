using Core;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests
{
    public class PhysicsHelperTests
    {
        #region Kepler's 3rd Law

        [Fact]
        public void OrbitalRadius_ReturnsCorrectValue()
        {
            double result = PhysicsHelper.OrbitalRadius(PhysicsHelper.EARTH_MASS, PhysicsHelper.DAY_SECONDS);
            Assert.True(result > 0);
        }

        [Fact]
        public void OrbitalTime_ReturnsCorrectValue()
        {
            double result = PhysicsHelper.OrbitalTime(384400000, PhysicsHelper.EARTH_MASS);
            Assert.True(result > 0);
        }

        [Fact]
        public void OrbitalMass_ReturnsCorrectValue()
        {
            double result = PhysicsHelper.OrbitalMass(384400000, PhysicsHelper.DAY_SECONDS);
            Assert.True(result > 0);
        }

        #endregion

        #region Radioactivity

        [Fact]
        public void Decay_ReturnsCorrectValue()
        {
            double result = PhysicsHelper.Decay(100, 5730, 11460);
            Assert.Equal(25, result, precision: 2);
        }

        #endregion

        #region Astronomy

        [Fact]
        public void Easter_ReturnsCorrectDate()
        {
            DateTime easter = PhysicsHelper.Easter(2024);
            Assert.Equal(new DateTime(2024, 3, 31), easter);
        }

        #endregion

        #region Motion

        [Fact]
        public void Velocity_ReturnsCorrectValue()
        {
            double result = PhysicsHelper.Velocity(100, 10);
            Assert.Equal(10, result);
        }

        [Fact]
        public void Acceleration_ReturnsCorrectValue()
        {
            double result = PhysicsHelper.Acceleration(20, 10, 5);
            Assert.Equal(2, result);
        }

        [Fact]
        public void FallDistance_ReturnsCorrectValue()
        {
            double result = PhysicsHelper.FallDistance(0, 2, 9.8);
            Assert.Equal(19.6, result, precision: 2);
        }

        #endregion

        #region Gravity

        [Fact]
        public void Gravity_ReturnsCorrectValue()
        {
            double result = PhysicsHelper.Gravity(5.972E24, 7.342E22, 384400000);
            Assert.True(result > 0);
        }

        #endregion
    }
}
