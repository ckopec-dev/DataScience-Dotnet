using Core;

namespace UnitTests
{
    [TestClass]
    public class NumberExtensionsUnitTests
    {
        [TestMethod]
        public void TestIsCyclicSet()
        {
            List<int> list = [8128, 2882, 8281];

            bool isCyclicSet = list.IsCyclicSet(2);
            Assert.IsTrue(isCyclicSet);
        }
    }
}
