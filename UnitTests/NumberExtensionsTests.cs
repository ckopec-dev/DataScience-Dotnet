using Core;
using ExtendedNumerics;
using System.Numerics;
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

        [Fact]
        public void Factorial_Zero_ReturnsOne()
        {
            var result = 0.Factorial();
            Assert.Equal(1, result);
        }

        [Fact]
        public void Factorial_PositiveNumber_ReturnsCorrectValue()
        {
            var result = 5.Factorial();
            Assert.Equal(120, result);
        }

        [Fact]
        public void Factorial_Negative_ThrowsException()
        {
            Assert.Throws<Exception>(() => (-1).Factorial());
        }

        [Fact]
        public void CommonDigits_SameDigits_ReturnsCommonDigits()
        {
            var result = 234.CommonDigits(328);
            Assert.Contains(2, result);
            Assert.Contains(3, result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void CommonDigits_NoCommonDigits_ReturnsEmptyList()
        {
            var result = 123.CommonDigits(456);
            Assert.Empty(result);
        }

        [Fact]
        public void ReplaceDigit_ReplaceMiddleDigit_ReturnsNewNumber()
        {
            var result = 12345.ReplaceDigit(2, 7);
            Assert.Equal(12745, result);
        }

        [Fact]
        public void RemoveDigit_RemovesAllOccurrences_ReturnsNewNumber()
        {
            var result = 1234423.RemoveDigit(2);
            Assert.Equal(13443, result);
        }

        [Fact]
        public void GetDigit_IndexOutOfBounds_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 123.GetDigit(5));
        }

        [Fact]
        public void GetDigit_ValidIndex_ReturnsCorrectDigit()
        {
            var result = 12345.GetDigit(2);
            Assert.Equal(3, result);

            var longResult = 12345L.GetDigit(2);
            Assert.Equal(3, longResult);
        }

        [Fact]
        public void ToStringOfOrderedDigits_ReturnsSortedString()
        {
            var result = 3142.ToStringOfOrderedDigits();
            Assert.Equal("1234", result);
        }

        [Fact]
        public void SameDigits_True_ReturnsTrue()
        {
            var numbers = new List<int> { 123, 321, 132 };
            var result = 123.SameDigits(numbers);
            Assert.True(result);
        }

        [Fact]
        public void SameDigits_False_ReturnsFalse()
        {
            var numbers = new List<int> { 123, 456 };
            var result = 123.SameDigits(numbers);
            Assert.False(result);
        }

        [Fact]
        public void Product_List_ReturnsProduct()
        {
            var numbers = new List<int> { 2, 3, 4 };
            var result = numbers.Product();
            Assert.Equal(24, result);
        }

        [Fact]
        public void Sum_List_ReturnsSum()
        {
            var numbers = new List<int> { 1, 2, 3 };
            var result = numbers.Sum();
            Assert.Equal(6, result);
        }

        [Fact]
        public void Sum_Int_ReturnsSumOfSequence()
        {
            var result = 5.Sum();
            Assert.Equal(15, result); // 1 + 2 + 3 + 4 + 5 = 15
        }

        [Fact]
        public void IsPerfect_True_ReturnsTrue()
        {
            var result = 6.IsPerfect();
            Assert.True(result);
        }

        [Fact]
        public void IsPerfect_False_ReturnsFalse()
        {
            var result = 8.IsPerfect();
            Assert.False(result);
        }

        [Fact]
        public void ProperDivisors_Int_ReturnsCorrectList()
        {
            var result = 6.ProperDivisors();
            Assert.Equal([1, 2, 3], result);
        }

        [Fact]
        public void ProperDivisors_Long_ReturnsCorrectList()
        {
            var result = 6L.ProperDivisors();
            Assert.Equal([1, 2, 3], result);
        }

        [Fact]
        public void IsPerfect_SmallNumber_ReturnsCorrectResult()
        {
            Assert.True(6.IsPerfect());
            Assert.False(10.IsPerfect());
        }

        [Fact]
        public void Reverse_Short_ReturnsReversedValue()
        {
            short input = 1234;
            short expected = 4321;
            var result = input.Reverse();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Reverse_UShort_ReturnsReversedValue()
        {
            ushort input = 1234;
            ushort expected = 4321;
            var result = input.Reverse();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Reverse_Int_ReturnsReversedValue()
        {
            int input = 12345;
            int expected = 54321;
            var result = input.Reverse();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Reverse_UInt_ReturnsReversedValue()
        {
            uint input = 12345;
            uint expected = 54321;
            var result = input.Reverse();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Reverse_Long_ReturnsReversedValue()
        {
            long input = 123456789L;
            long expected = 987654321L;
            var result = input.Reverse();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Reverse_ULong_ReturnsReversedValue()
        {
            ulong input = 123456789UL;
            ulong expected = 987654321UL;
            var result = input.Reverse();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SumOfDigits_Byte_ReturnsCorrectSum()
        {
            byte input = 123;
            long expected = 6;
            var result = input.SumOfDigits();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SumOfDigits_Short_ReturnsCorrectSum()
        {
            short input = 123;
            long expected = 6;
            var result = input.SumOfDigits();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SumOfDigits_Int_ReturnsCorrectSum()
        {
            int input = 123;
            long expected = 6;
            var result = input.SumOfDigits();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SumOfDigits_Long_ReturnsCorrectSum()
        {
            long input = 12345L;
            long expected = 15;
            var result = input.SumOfDigits();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SumOfDigits_BigInteger_ReturnsCorrectSum()
        {
            BigInteger input = new(123456789);
            BigInteger expected = 45;
            var result = input.SumOfDigits();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SumOfDigits_Zero_ReturnsZero()
        {
            long input = 0;
            long expected = 0;
            var result = input.SumOfDigits();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SumOfDecimalDigits_ReturnsCorrectSum()
        {
            var bd = BigDecimal.Parse("123.456");
            var result = bd.SumOfDecimalDigits();
            Assert.Equal(21, result); // 1+2+3+4+5+6
        }

        [Fact]
        public void IsArmstrong_WhenTrue_ReturnsTrue()
        {
            Assert.True(9474.IsArmstrong());
        }

        [Fact]
        public void IsArmstrong_WhenFalse_ReturnsFalse()
        {
            Assert.False(123.IsArmstrong());
        }

        [Fact]
        public void GetSquareRootDigits_ReturnsCorrectDigits()
        {
            var digits = 20.GetSquareRootDigits(5);
            Assert.Equal([4, 4, 7, 2, 1], digits);
        }

        [Fact]
        public void SquareDigitChain_EndsAt1Or89()
        {
            Assert.Equal(1, 44.SquareDigitChain());
            Assert.Equal(89, 85.SquareDigitChain());
        }

        [Fact]
        public void IsPerfectSquare_Perfect_ReturnsTrue()
        {
            Assert.True(16.IsPerfectSquare());
        }

        [Fact]
        public void IsPerfectSquare_NotPerfect_ReturnsFalse()
        {
            Assert.False(15.IsPerfectSquare());
        }

        [Fact]
        public void IsSquare_DoubleIsInteger_ReturnsTrue()
        {
            Assert.True(25.0.IsSquare());
        }

        [Fact]
        public void IsSquare_DoubleNotInteger_ReturnsFalse()
        {
            Assert.False(24.0.IsSquare());
        }

        [Fact]
        public void PrettyPrint_IntList_ReturnsFormattedString()
        {
            var list = new List<int> { 1, 2, 3 };
            Assert.Equal("{ 1, 2, 3 }", list.PrettyPrint());
        }

        [Fact]
        public void Concatenate_ReturnsConcatenatedNumber()
        {
            Assert.Equal(12345, 123.Concatenate(45));
        }

        [Fact]
        public void HasSameDigits_True_ReturnsTrue()
        {
            Assert.True(123.HasSameDigits(321));
        }

        [Fact]
        public void HasSameDigits_False_ReturnsFalse()
        {
            Assert.False(123.HasSameDigits(456));
        }

        [Fact]
        public void ToDelimitedString_IntArray_ReturnsString()
        {
            int[] arr = [1, 2, 3];
            Assert.Equal("1,2,3", arr.ToDelimitedString(','));
        }

        [Fact]
        public void ToDelimitedString_DoubleArray_ReturnsString()
        {
            double[] arr = [1.0, 2.0, 3.0];
            Assert.Equal("1,2,3", arr.ToDelimitedString(','));
        }

        [Fact]
        public void ToDecimalArray_ReturnsCorrectArray()
        {
            string[] input = ["1.0", "2.5"];
            var result = input.ToDecimalArray();
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void ToIntList_ReturnsCorrectList()
        {
            var list = "1,2,3".ToIntList(',');
            Assert.Equal([1, 2, 3], list);
        }

        [Fact]
        public void ToBinary_ReturnsBinaryString()
        {
            Assert.Equal("1010", 10.ToBinary());
        }

        [Fact]
        public void ToInt32FromBinary_ReturnsCorrectInt()
        {
            Assert.Equal(10, "1010".ToInt32FromBinary());
        }

        [Fact]
        public void ToHex_ReturnsHex()
        {
            Assert.Equal("A", 10.ToHex());
        }

        [Fact]
        public void ToInt32FromHex_ReturnsCorrectInt()
        {
            Assert.Equal(10, "A".ToInt32FromHex());
        }

        [Fact]
        public void ToNibbleList_ReturnsNibbles()
        {
            var result = 123.ToNibbleList();
            Assert.Equal(["0001", "0010", "0011"], result);
        }

        [Fact]
        public void ToInt32FromNibbleList_ReturnsCorrectNumber()
        {
            var list = new List<string> { "0001", "0010", "0011" };
            Assert.Equal(123, list.ToInt32FromNibbleList());
        }

        [Fact]
        public void IsPositive_ShouldReturnTrue_WhenNumberIsGreaterThanZero()
        {
            ulong n = 5;
            Assert.True(n.IsPositive());
        }

        [Fact]
        public void IsPositive_ShouldReturnFalse_WhenNumberIsZero()
        {
            ulong n = 0;
            Assert.False(n.IsPositive());
        }

        [Fact]
        public void IsPerfectSquare_ShouldReturnTrue_ForPerfectSquareNumbers()
        {
            Assert.True(16ul.IsPerfectSquare());
            Assert.True(100ul.IsPerfectSquare());
            Assert.True(0ul.IsPerfectSquare());
        }

        [Fact]
        public void IsPerfectSquare_ShouldReturnFalse_ForNonPerfectSquares()
        {
            Assert.False(15ul.IsPerfectSquare());
            Assert.False(20ul.IsPerfectSquare());
        }

        [Fact]
        public void IsSquare_ShouldReturnTrue_WhenBigDecimalIsPerfectSquare()
        {
            var bigDecimal = new BigDecimal(16);
            Assert.True(bigDecimal.IsSquare(2));
        }

        [Fact]
        public void IsSquare_ShouldReturnFalse_WhenBigDecimalIsNotPerfectSquare()
        {
            var bigDecimal = new BigDecimal(15);
            Assert.False(bigDecimal.IsSquare(2));
        }

        [Fact]
        public void ContinuedFraction_ShouldReturnCorrectRootAndRepeatList()
        {
            ulong root = 23ul.ContinuedFraction(out List<ulong> repeat);

            Assert.Equal(4ul, root);
            Assert.Contains(1ul, repeat);
            Assert.Contains(3ul, repeat);
            Assert.Contains(1ul, repeat);
            Assert.Contains(8ul, repeat);
        }

        [Fact]
        public void PrettyPrint_ShouldReturnFormattedString()
        {
            var list = new List<long> { 1, 2, 3 };
            string result = list.PrettyPrint();
            Assert.Equal("{ 1, 2, 3 }", result);
        }

        [Fact]
        public void HasSameDigits_ReturnsTrue_WhenNumbersHaveSameDigits()
        {
            Assert.True(123456789UL.HasSameDigits(987654321UL));
            Assert.True(123UL.HasSameDigits(321UL));
            Assert.True(1122UL.HasSameDigits(2211UL));
        }

        [Fact]
        public void HasSameDigits_ReturnsFalse_WhenNumbersHaveDifferentLength()
        {
            Assert.False(123UL.HasSameDigits(1234UL));
        }

        [Fact]
        public void HasSameDigits_ReturnsFalse_WhenNumbersDoNotHaveSameDigits()
        {
            Assert.False(123UL.HasSameDigits(456UL));
        }

        [Fact]
        public void Factorial_ReturnsCorrectResult()
        {
            Assert.Equal(BigInteger.One, 0.Factorial());
            Assert.Equal(new BigInteger(120), 5.Factorial());
            Assert.Equal(new BigInteger(3628800), 10.Factorial());
        }

        [Fact]
        public void ToMicrons_ReturnsCorrectValue()
        {
            Assert.Equal(0.0001m, 1m.ToMicrons());
            Assert.Equal(0.0002m, 2m.ToMicrons());
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, false)]
        [InlineData(9, false)]
        [InlineData(17, true)]
        public void IsProbablyPrime_ReturnsCorrectValue(int number, bool expected)
        {
            Assert.Equal(expected, number.IsProbablyPrime());
        }

        [Theory]
        [InlineData(2, 5, true)]
        [InlineData(4, 5, false)]
        public void IsProbablyPrime_WithCertainty_ReturnsCorrectValue(int number, int certainty, bool expected)
        {
            Assert.Equal(expected, number.IsProbablyPrime(certainty));
        }

        [Theory]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(4, true)]
        [InlineData(5, false)]
        public void IsEven_ReturnsCorrectValue(int number, bool expected)
        {
            Assert.Equal(expected, number.IsEven());
        }

        [Fact]
        public void IsEven_BigInteger_ReturnsCorrectValue()
        {
            Assert.True(new BigInteger(2).IsEven());
            Assert.False(new BigInteger(3).IsEven());
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        [InlineData(3, true)]
        [InlineData(4, false)]
        public void IsOdd_ReturnsCorrectValue(int number, bool expected)
        {
            Assert.Equal(expected, number.IsOdd());
        }

        [Fact]
        public void IsOdd_BigInteger_ReturnsCorrectValue()
        {
            Assert.True(new BigInteger(3).IsOdd());
            Assert.False(new BigInteger(2).IsOdd());
        }

        [Theory]
        [InlineData(121, true)]
        [InlineData(123, false)]
        [InlineData(0, true)]
        public void IsPalindrome_ReturnsCorrectValue(int number, bool expected)
        {
            Assert.Equal(expected, number.IsPalindrome());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 3)]
        [InlineData(3, 6)]
        [InlineData(4, 10)]
        [InlineData(5, 15)]
        public void TriangleNumber_ReturnsCorrectValue(int input, int expected)
        {
            Assert.Equal(expected, input.TriangleNumber());
        }

        [Fact]
        public void TriangleNumber_ThrowsException_WhenNegative()
        {
            Assert.Throws<NotSupportedException>(() => (-1).TriangleNumber());
        }
    }
}
