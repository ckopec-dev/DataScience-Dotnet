using Core;
using System.Numerics;

namespace UnitTests
{
    [TestClass]
    public class NumberExtensionsUnitTests
    {
        [TestMethod]
        public void TestIsPrime()
        {
            // Byte
            byte b = 16;
            Assert.IsFalse(b.IsPrime());

            // Short
            short s = 16;
            Assert.IsFalse(s.IsPrime());

            // UShort
            ushort us = 16;
            Assert.IsFalse(us.IsPrime());

            // Integer
            int i = 16;
            Assert.IsFalse(i.IsPrime());

            // UInteger
            uint ui = 16;
            Assert.IsFalse(ui.IsPrime());

            // Long
            long l = 16;
            Assert.IsFalse(l.IsPrime());

            // ULong
            ulong ul = 16;
            Assert.IsFalse(ul.IsPrime());
            ul = 5153;
            Assert.IsTrue(ul.IsPrime());
            ul = 5151;
            Assert.IsFalse(ul.IsPrime());

            // BigInteger
            BigInteger bi = new(16);
            Assert.IsFalse(bi.IsPrime());
            bi = new(3);
            Assert.IsTrue(bi.IsPrime());
            bi = new(5153);
            Assert.IsTrue(bi.IsPrime());
            bi = new(5151);
            Assert.IsFalse(bi.IsPrime());
        }

        [TestMethod]
        public void TestReverse()
        {
            // Byte
            byte b = 12;
            Assert.AreEqual(21, b.Reverse());

            // Short
            short s = 123;
            Assert.AreEqual(321, s.Reverse());

            // Integer
            int i = 12345;
            Assert.AreEqual(54321, i.Reverse());

            // Long
            long l = 12345;
            Assert.AreEqual(54321, l.Reverse());

            // BigInteger
            BigInteger bi = new(12345);
            Assert.AreEqual(54321, bi.Reverse());
        }

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
            BigInteger bi = new(12345);
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
