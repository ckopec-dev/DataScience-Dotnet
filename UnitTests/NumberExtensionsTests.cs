using Core;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests
{
    public class NumberExtensionsTests
    {
        #region Int Extensions

        [Fact]
        public void ToInt32_From_String()
        {
            Assert.Equal(123, "123456".ToInt32(0, 3));
        }

        [Fact]
        public void ToInt32_From_Long()
        {
            Assert.Equal(123, 123456L.ToInt32(0, 3));
        }

        [Fact]
        public void IsPerfectSquare_True()
        {
            Assert.True(9.IsPerfectSquare());
        }

        [Fact]
        public void IsPerfectSquare_False()
        {
            Assert.False(8.IsPerfectSquare());
        }

        [Fact]
        public void IsPerfectCube_True()
        {
            Assert.True(27.IsPerfectCube());
        }

        [Fact]
        public void IsPerfectCube_False()
        {
            Assert.False(26.IsPerfectCube());
        }

        [Fact]
        public void AreDigitsInAscendingOrder_True()
        {
            Assert.True(12345.AreDigitsInAscendingOrder());
        }

        [Fact]
        public void AreDigitsInAscendingOrder_False()
        {
            Assert.False(12321.AreDigitsInAscendingOrder());
        }

        [Fact]
        public void AreDigitsInDescendingOrder_True()
        {
            Assert.True(54321.AreDigitsInDescendingOrder());
        }

        [Fact]
        public void AreDigitsInDescendingOrder_False()
        {
            Assert.False(12345.AreDigitsInDescendingOrder());
        }

        [Fact]
        public void Min_Array_Returns_Min_Value()
        {
            var arr = new int[] { 3, 1, 4, 1, 5 };
            Assert.Equal(1, arr.Min());
        }

        [Fact]
        public void Min_Array_With_Index_Returns_Min_Value_And_Index()
        {
            var arr = new int[] { 3, 1, 4, 1, 5 };
            Assert.Equal(1, arr.Min(out int index));
            Assert.Equal(1, index);
        }

        [Fact]
        public void Max_Array_Returns_Max_Value()
        {
            var arr = new int[] { 3, 1, 4, 1, 5 };
            Assert.Equal(5, arr.Max());
        }

        [Fact]
        public void Identical_List_True()
        {
            Assert.True(new List<int> { 1, 1, 1 }.Identical());
        }

        [Fact]
        public void Identical_List_False()
        {
            Assert.False(new List<int> { 1, 2, 1 }.Identical());
        }

        [Fact]
        public void HasDupes_True()
        {
            Assert.True(new List<int> { 1, 2, 2 }.HasDupes());
        }

        [Fact]
        public void HasDupes_False()
        {
            Assert.False(new List<int> { 1, 2, 3 }.HasDupes());
        }

        #endregion

        #region List Extensions

        [Fact]
        public void ToIntListArray_Should_Parse_Correctly()
        {
            var data = "1,2\n3,4";
            var result = data.ToIntListArray('\n', ',');
            Assert.Equal(2, result.Count);
            Assert.Equal([1, 2], result[0]);
            Assert.Equal([3, 4], result[1]);
        }

        [Fact]
        public void ToDoubleListArray_Should_Parse_Correctly()
        {
            var data = "1.1,2.2\n3.3,4.4";
            var result = data.ToDoubleListArray('\n', ',');
            Assert.Equal(2, result.Count);
            Assert.Equal([1.1, 2.2], result[0]);
            Assert.Equal([3.3, 4.4], result[1]);
        }

        #endregion

        #region Trig Extensions

        [Fact]
        public void ToRadians_Double()
        {
            Assert.Equal(Math.PI / 2, 90d.ToRadians(), 1e-10);
        }

        [Fact]
        public void ToDegrees_Double()
        {
            Assert.Equal(90d, (Math.PI / 2).ToDegrees(), 1e-10);
        }

        #endregion

        #region Pandigital & XOR

        [Fact]
        public void PandigitalMultiple_Returns_Correct_Value()
        {
            var result = 192.PandigitalMultiple([1, 2, 3]);
            Assert.Equal(192384576, result);
        }

        [Fact]
        public void EncryptXOR_DecryptXOR_Should_Be_Reverse()
        {
            var message = "Hello World";
            var password = "secret";

            var encrypted = message.EncryptXOR(password);
            var decrypted = encrypted.DecryptXOR(password);

            Assert.Equal(message, decrypted);
        }

        #endregion

        #region Char Array / Int Array

        [Fact]
        public void ToCharArray_Returns_Correct_Char_Array()
        {
            var intArray = new int[] { 65, 66, 67 };
            var charArray = intArray.ToCharArray();
            Assert.Equal(['A', 'B', 'C'], charArray);
        }

        [Fact]
        public void ToLongArray_Returns_Correct_Long_Array()
        {
            var stringArray = new string[] { "1", "2", "3" };
            var longArray = stringArray.ToLongArray();
            Assert.Equal([1L, 2L, 3L], longArray);
        }

        #endregion

        #region String to Int Array

        [Fact]
        public void ToArray_Returns_Correct_Int_Array()
        {
            var stringArray = new string[] { "1", "2", "3" };
            var intArray = stringArray.ToArray();
            Assert.Equal([1, 2, 3], intArray);
        }

        #endregion

        #region Number Conversion / Word Value / String Parsing

        [Fact]
        public void ToInt32_Throws_Exception_For_Invalid_String()
        {
            Assert.Throws<FormatException>(() => "abc".ToInt32(0, 1));
        }

        #endregion
    }
}
