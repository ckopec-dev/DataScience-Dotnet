
using Core;

namespace UnitTests
{
    [TestClass]
    public class StringExtensionsUnitTests
    {
        [TestMethod]
        public void TestLeft()
        {
            string s = "HelloWorld";
            Assert.AreEqual(s.Left(2), "He");
        }

        [TestMethod]
        public void TestRight()
        {
            string s = "HelloWorld";
            Assert.AreEqual(s.Right(2), "ld");
        }
    }
}
