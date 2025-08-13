
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
            Assert.AreEqual("He", s.Left(2));
        }

        [TestMethod]
        public void TestRight()
        {
            string s = "HelloWorld";
            Assert.AreEqual("ld", s.Right(2));
        }
    }
}
