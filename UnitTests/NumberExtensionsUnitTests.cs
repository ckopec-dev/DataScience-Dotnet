using Core;
using System.Numerics;

namespace UnitTests
{
    [TestClass]
    public class NumberExtensionsUnitTests
    {
        [TestMethod]
        public void TestSumOfDigits()
        {
            // Byte
            byte b = 123;
            Assert.AreEqual(6, b.SumOfDigits());

            // Short
            short s = 12345;
            Assert.AreEqual(15, s.SumOfDigits());

            // Integer
            int i = 12345;
            Assert.AreEqual(15, i.SumOfDigits());

            // Long
            long l = 12345;
            Assert.AreEqual(15, l.SumOfDigits());

            // BigInteger
            BigInteger bi = new BigInteger(12345);
            Assert.AreEqual(15, bi.SumOfDigits());
        }

        [TestMethod]
        public void TestIsCyclicSet()
        {
            List<int> list = [8128, 2882, 8281];

            bool isCyclicSet = list.IsCyclicSet(2);
            Assert.IsTrue(isCyclicSet);
        }
    }
}
