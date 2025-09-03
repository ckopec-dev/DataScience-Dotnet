using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace UnitTests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void ToCsvValue_NullValue_ReturnsEmptyString()
        {
            // Arrange
            string? nullString = null;

            // Act
            var result = nullString.ToCsvValue();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ToCsvValue_EmptyString_ReturnsEmptyString()
        {
            // Arrange
            var emptyString = string.Empty;

            // Act
            var result = emptyString.ToCsvValue();

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ToCsvValue_SimpleString_ReturnsUnmodifiedString()
        {
            // Arrange
            var simpleString = "Hello";

            // Act
            var result = simpleString.ToCsvValue();

            // Assert
            Assert.AreEqual("Hello", result);
        }

        [TestMethod]
        public void ToCsvValue_StringWithComma_ReturnsQuotedString()
        {
            // Arrange
            var stringWithComma = "Hello, World";

            // Act
            var result = stringWithComma.ToCsvValue();

            // Assert
            Assert.AreEqual("\"Hello, World\"", result);
        }

        [TestMethod]
        public void ToCsvValue_StringWithDoubleQuote_ReturnsEscapedAndQuotedString()
        {
            // Arrange
            var stringWithQuote = "Say \"Hello\"";

            // Act
            var result = stringWithQuote.ToCsvValue();

            // Assert
            Assert.AreEqual("\"Say \"\"Hello\"\"\"", result);
        }

        [TestMethod]
        public void ToCsvValue_StringWithNewline_ReturnsQuotedString()
        {
            // Arrange
            var stringWithNewline = "Line 1\nLine 2";

            // Act
            var result = stringWithNewline.ToCsvValue();

            // Assert
            Assert.AreEqual("\"Line 1\nLine 2\"", result);
        }

        [TestMethod]
        public void ToCsvValue_StringWithCarriageReturn_ReturnsQuotedString()
        {
            // Arrange
            var stringWithCR = "Line 1\rLine 2";

            // Act
            var result = stringWithCR.ToCsvValue();

            // Assert
            Assert.AreEqual("\"Line 1\rLine 2\"", result);
        }

        [TestMethod]
        public void ToCsvValue_StringWithLeadingSpace_ReturnsQuotedString()
        {
            // Arrange
            var stringWithLeadingSpace = " Hello";

            // Act
            var result = stringWithLeadingSpace.ToCsvValue();

            // Assert
            Assert.AreEqual("\" Hello\"", result);
        }

        [TestMethod]
        public void ToCsvValue_StringWithTrailingSpace_ReturnsQuotedString()
        {
            // Arrange
            var stringWithTrailingSpace = "Hello ";

            // Act
            var result = stringWithTrailingSpace.ToCsvValue();

            // Assert
            Assert.AreEqual("\"Hello \"", result);
        }

        [TestMethod]
        public void ToCsvValue_Integer_ReturnsStringRepresentation()
        {
            // Arrange
            var number = 42;

            // Act
            var result = number.ToCsvValue();

            // Assert
            Assert.AreEqual("42", result);
        }

        [TestMethod]
        public void ToCsvValue_Decimal_ReturnsStringRepresentation()
        {
            // Arrange
            var price = 29.99m;

            // Act
            var result = price.ToCsvValue();

            // Assert
            Assert.AreEqual("29.99", result);
        }

        [TestMethod]
        public void ToCsvValue_Boolean_ReturnsStringRepresentation()
        {
            // Arrange & Act
            var trueResult = true.ToCsvValue();
            var falseResult = false.ToCsvValue();

            // Assert
            Assert.AreEqual("True", trueResult);
            Assert.AreEqual("False", falseResult);
        }

        [TestMethod]
        public void ToCsvValue_DateTime_ReturnsStringRepresentation()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25, 10, 30, 0);

            // Act
            var result = date.ToCsvValue();

            // Assert
            Assert.AreEqual("12/25/2023 10:30:00 AM", result);
        }

        [TestMethod]
        public void ToCsvValue_StringWithMultipleSpecialChars_ReturnsProperlyEscaped()
        {
            // Arrange
            var complexString = "Name: \"John, Jr.\"\nAge: 25";

            // Act
            var result = complexString.ToCsvValue();

            // Assert
            Assert.AreEqual("\"Name: \"\"John, Jr.\"\"\nAge: 25\"", result);
        }

        [TestMethod]
        public void ToCsvValue_StringWithOnlyDoubleQuote_ReturnsEscapedAndQuoted()
        {
            // Arrange
            var singleQuote = "\"";

            // Act
            var result = singleQuote.ToCsvValue();

            // Assert
            Assert.AreEqual("\"\"\"\"", result);
        }

        [TestMethod]
        public void ToCsvValue_StringWithOnlyComma_ReturnsQuoted()
        {
            // Arrange
            var singleComma = ",";

            // Act
            var result = singleComma.ToCsvValue();

            // Assert
            Assert.AreEqual("\",\"", result);
        }

        [TestMethod]
        public void ToCsvValue_CustomObject_ReturnsToStringResult()
        {
            // Arrange
            var customObject = new TestObjectCsv { Name = "Test", Value = 123 };

            // Act
            var result = customObject.ToCsvValue();

            // Assert
            Assert.AreEqual("Test:123", result);
        }

        [TestMethod]
        public void ToCsvValue_CustomObjectWithSpecialChars_ReturnsQuotedToStringResult()
        {
            // Arrange
            var customObject = new TestObjectCsv { Name = "Test, Object", Value = 123 };

            // Act
            var result = customObject.ToCsvValue();

            // Assert
            Assert.AreEqual("\"Test, Object:123\"", result);
        }

        // Helper class for testing custom objects
        private class TestObjectCsv
        {
            public string? Name { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return $"{Name}:{Value}";
            }
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithUnixLineEndings_ReturnsCorrectList()
        {
            // Arrange
            string input = "line1\nline2\nline3";
            var expected = new List<string> { "line1", "line2", "line3" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithWindowsLineEndings_ReturnsCorrectList()
        {
            // Arrange
            string input = "line1\r\nline2\r\nline3";
            var expected = new List<string> { "line1", "line2", "line3" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithMacLineEndings_ReturnsCorrectList()
        {
            // Arrange
            string input = "line1\rline2\rline3";
            var expected = new List<string> { "line1", "line2", "line3" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithMixedLineEndings_ReturnsCorrectList()
        {
            // Arrange
            string input = "line1\nline2\r\nline3\rline4";
            var expected = new List<string> { "line1", "line2", "line3", "line4" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithEmptyString_ReturnsListWithEmptyString()
        {
            // Arrange
            string input = "";
            var expected = new List<string> { "" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithSingleLine_ReturnsSingleItemList()
        {
            // Arrange
            string input = "single line";
            var expected = new List<string> { "single line" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithEmptyLines_IncludesEmptyLines()
        {
            // Arrange
            string input = "line1\n\nline3\n";
            var expected = new List<string> { "line1", "", "line3", "" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithEmptyLinesAndRemoveEmptyEntries_ExcludesEmptyLines()
        {
            // Arrange
            string input = "line1\n\nline3\n";
            var expected = new List<string> { "line1", "line3" };

            // Act
            var result = input.ParseRowDelimitedString(true);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithWhitespaceLines_PreservesWhitespace()
        {
            // Arrange
            string input = "line1\n   \nline3";
            var expected = new List<string> { "line1", "   ", "line3" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithSpecialCharacters_PreservesSpecialCharacters()
        {
            // Arrange
            string input = "line with, commas\nline with \"quotes\"\nline with\ttabs";
            var expected = new List<string> { "line with, commas", "line with \"quotes\"", "line with\ttabs" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseRowDelimitedString_WithTrailingNewline_IncludesEmptyLastEntry()
        {
            // Arrange
            string input = "line1\nline2\n";
            var expected = new List<string> { "line1", "line2", "" };

            // Act
            var result = input.ParseRowDelimitedString();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        #region LargestCommonSubstring Tests

        [TestMethod]
        public void LargestCommonSubstring_WithNullCollection_ReturnsNull()
        {
            IEnumerable<string>? strings = null;
            var result = strings!.LargestCommonSubstring();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void LargestCommonSubstring_WithEmptyCollection_ReturnsNull()
        {
            var strings = new List<string>();
            var result = strings.LargestCommonSubstring();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void LargestCommonSubstring_WithCommonSubstring_ReturnsLargestCommon()
        {
            var strings = new List<string> { "hello world", "hello there", "hello friend" };
            var result = strings.LargestCommonSubstring();
            Assert.AreEqual("hello ", result);
        }

        [TestMethod]
        public void LargestCommonSubstring_WithNoCommonSubstring_ReturnsNull()
        {
            var strings = new List<string> { "abc", "def", "ghi" };
            var result = strings.LargestCommonSubstring();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void LargestCommonSubstring_WithSingleString_ReturnsEntireString()
        {
            var strings = new List<string> { "hello" };
            var result = strings.LargestCommonSubstring();
            Assert.AreEqual("hello", result);
        }

        [TestMethod]
        public void LargestCommonSubstring_ListOverload_ReturnsLargestCommon()
        {
            var strings = new List<string> { "testing123", "testing456", "testing789" };
            var result = strings.LargestCommonSubstring();
            Assert.AreEqual("testing", result);
        }

        #endregion

        #region IsSubsequence Tests

        [TestMethod]
        public void IsSubsequence_ValidSubsequence_ReturnsTrue()
        {
            var result = "ADXCPY".IsSubsequence("AXY");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSubsequence_InvalidSubsequence_ReturnsFalse()
        {
            var result = "ABC".IsSubsequence("ACB");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsSubsequence_EmptySubsequence_ReturnsTrue()
        {
            var result = "ABC".IsSubsequence("");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSubsequence_EmptyString_ReturnsFalse()
        {
            var result = "".IsSubsequence("A");
            Assert.IsFalse(result);
        }

        #endregion

        #region IsPangram Tests

        [TestMethod]
        public void IsPangram_ValidPangram_ReturnsTrue()
        {
            var result = "The quick brown fox jumps over the lazy dog".IsPangram();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPangram_InvalidPangram_ReturnsFalse()
        {
            var result = "Hello World".IsPangram();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPangram_MixedCase_ReturnsTrue()
        {
            var result = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IsPangram();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPangram_LowerCase_ReturnsTrue()
        {
            var result = "abcdefghijklmnopqrstuvwxyz".IsPangram();
            Assert.IsTrue(result);
        }

        #endregion

        #region Shift Tests

        [TestMethod]
        public void Shift_ZeroCount_ReturnsOriginalString()
        {
            var result = "Hello World!".Shift(0);
            Assert.AreEqual("Hello World!", result);
        }

        [TestMethod]
        public void Shift_NegativeCount_ShiftsLeft()
        {
            var result = "Hello World!".Shift(-3);
            Assert.AreEqual("lo World!Hel", result);
        }

        [TestMethod]
        public void Shift_PositiveCount_ShiftsRight()
        {
            var result = "Hello World!".Shift(3);
            Assert.AreEqual("ld!Hello Wor", result);
        }

        #endregion

        #region Permute Tests

        [TestMethod]
        public void Permute_SingleCharacter_ReturnsSinglePermutation()
        {
            var result = "A".Permute().ToList();
            Assert.HasCount(1, result);
            Assert.AreEqual("A", result[0]);
        }

        [TestMethod]
        public void Permute_EmptyString_ReturnsEmptyString()
        {
            var result = "".Permute().ToList();
            Assert.HasCount(1, result);
            Assert.AreEqual("", result[0]);
        }

        [TestMethod]
        public void Permute_TwoCharacters_ReturnsCorrectPermutations()
        {
            var result = "AB".Permute().ToList();
            Assert.HasCount(2, result);
            Assert.Contains("AB", result);
            Assert.Contains("A B", result);
        }

        #endregion

        //#region Between Tests

        //[TestMethod]
        //public void Between_ValidStrings_ReturnsSubstring()
        //{
        //    var result = "Hello[World]Test".Between("[", "]");
        //    Assert.AreEqual("World", result);
        //}

        //[TestMethod]
        //public void Between_StartStringNotFound_ReturnsFromBeginning()
        //{
        //    var result = "HelloWorldTest".Between("X", "T");
        //    Assert.AreEqual("HelloWorl", result);
        //}

        //#endregion

        #region Left Tests

        [TestMethod]
        public void Left_ValidLength_ReturnsLeftPortion()
        {
            var result = "helloworld".Left(2);
            Assert.AreEqual("he", result);
        }

        [TestMethod]
        public void Left_ZeroLength_ReturnsEmptyString()
        {
            var result = "helloworld".Left(0);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void Left_FullLength_ReturnsEntireString()
        {
            var result = "hello".Left(5);
            Assert.AreEqual("hello", result);
        }

        #endregion

        #region Right Tests

        [TestMethod]
        public void Right_ValidLength_ReturnsRightPortion()
        {
            var result = "helloworld".Right(2);
            Assert.AreEqual("ld", result);
        }

        [TestMethod]
        public void Right_FullLength_ReturnsEntireString()
        {
            var result = "hello".Right(5);
            Assert.AreEqual("hello", result);
        }

        #endregion

        #region ToProperCase Tests

        [TestMethod]
        public void ToProperCase_LowerCaseWords_ReturnsProperCase()
        {
            var result = "hello world test".ToProperCase();
            Assert.AreEqual("Hello World Test", result);
        }

        [TestMethod]
        public void ToProperCase_UpperCaseWords_ReturnsProperCase()
        {
            var result = "HELLO WORLD TEST".ToProperCase();
            Assert.AreEqual("Hello World Test", result);
        }

        [TestMethod]
        public void ToProperCase_MixedCase_ReturnsProperCase()
        {
            var result = "hELLo WoRLd".ToProperCase();
            Assert.AreEqual("Hello World", result);
        }

        [TestMethod]
        public void ToProperCase_WithNumbers_PreservesNumbers()
        {
            var result = "hello 123 world".ToProperCase();
            Assert.AreEqual("Hello 123 World", result);
        }

        #endregion

        #region Byte Array Tests

        [TestMethod]
        public void ToUTFByteArray_ValidString_ReturnsByteArray()
        {
            var result = "Hello".ToUTFByteArray();
            var expected = Encoding.UTF8.GetBytes("Hello");
            CollectionAssert.AreEqual(expected, result);
        }

        //[TestMethod]
        //public void ToString_ByteArray_ReturnsString()
        //{
        //    var bytes = Encoding.UTF8.GetBytes("Hello");
        //    var result = bytes.ToString();
        //    Assert.AreEqual("Hello", result);
        //}

        [TestMethod]
        public void ToBytes_ValidString_ReturnsByteArray()
        {
            var result = "Hi".ToBytes();
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Length); // 2 chars * 2 bytes each
        }

        [TestMethod]
        public void ToUTF8ByteArray_ValidString_ReturnsByteArray()
        {
            var result = "Hello".ToUTF8ByteArray();
            var expected = Encoding.UTF8.GetBytes("Hello");
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ToStringFromUTF8Bytes_ValidBytes_ReturnsString()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello");
            var result = bytes.ToStringFromUTF8Bytes();
            Assert.AreEqual("Hello", result);
        }

        #endregion

        #region Reverse Tests

        [TestMethod]
        public void Reverse_ValidString_ReturnsReversedString()
        {
            var result = "Hello".Reverse();
            Assert.AreEqual("olleH", result);
        }

        [TestMethod]
        public void Reverse_EmptyString_ReturnsEmptyString()
        {
            var result = "".Reverse();
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void Reverse_SingleChar_ReturnsSameChar()
        {
            var result = "A".Reverse();
            Assert.AreEqual("A", result);
        }

        #endregion

        #region ToIntArray Tests

        [TestMethod]
        public void ToIntArray_ValidString_ReturnsIntArray()
        {
            var result = "1,2,3,4".ToIntArray(',');
            CollectionAssert.AreEqual(expectedArray0, result);
        }
        private static readonly int[] expected = [10, 20, 30];
        private static readonly int[] expectedArray = [10, 20];
        private static readonly int[] expectedArray0 = [1, 2, 3, 4];

        [TestMethod]
        public void ToIntArray_SpaceSeparated_ReturnsIntArray()
        {
            var result = "10 20 30".ToIntArray(' ');
            CollectionAssert.AreEqual(expected, result);
        }

        #endregion

        #region ToList Tests

        [TestMethod]
        public void ToList_StringArray_ReturnsListIgnoringBlanks()
        {
            var data = new string[] { "hello", "", "world", "   ", "test" };
            var result = data.ToList();
            Assert.HasCount(3, result);
            CollectionAssert.AreEqual(new List<string> { "hello", "world", "test" }, result);
        }

        //[TestMethod]
        //public void ToList_String_ReturnsListOfLines()
        //{
        //    var data = "hello\nworld\ntest";
        //    var result = data.ToList();
        //    Assert.HasCount(3, result);
        //    CollectionAssert.AreEqual(new List<string> { "hello", "world", "test" }, result);
        //}

        [TestMethod]
        public void ToList_StringWithDelimiter_ReturnsListIgnoringBlanks()
        {
            var data = "hello,world,,test,";
            var result = data.ToList(',');
            Assert.HasCount(3, result);
            CollectionAssert.AreEqual(new List<string> { "hello", "world", "test" }, result);
        }

        #endregion

        #region AllIndexesOf Tests

        [TestMethod]
        public void AllIndexesOf_MultipleOccurrences_ReturnsAllIndexes()
        {
            var result = "hello hello hello".AllIndexesOf("hello");
            CollectionAssert.AreEqual(new List<int> { 0, 6, 12 }, result);
        }

        [TestMethod]
        public void AllIndexesOf_WithBaseIndex_ReturnsAdjustedIndexes()
        {
            var result = "hello hello".AllIndexesOf("hello", 10);
            CollectionAssert.AreEqual(new List<int> { 10, 16 }, result);
        }

        [TestMethod]
        public void AllIndexesOf_NotFound_ReturnsEmptyList()
        {
            var result = "hello world".AllIndexesOf("xyz");
            Assert.IsEmpty(result);
        }

        #endregion

        #region ToCsv Tests

        [TestMethod]
        public void ToCsv_ValidObjects_ReturnsCsvString()
        {
            var items = new List<TestObject>
            {
                new() { Name = "John", Age = 30 },
                new() { Name = "Jane", Age = 25 }
            };
            var result = items.ToCsv();
            Assert.Contains("\"John\"", result);
            Assert.Contains("30", result);
        }

        [TestMethod]
        public void ToCsv_WithNullValues_HandlesBlanks()
        {
            var items = new List<TestObjectWithNulls>
            {
                new() { Name = null, Value = 10 }
            };
            var result = items.ToCsv();
            Assert.Contains("\"\"", result);
        }

        private class TestObject
        {
            public string? Name { get; set; }
            public int Age { get; set; }
        }

        private class TestObjectWithNulls
        {
            public string? Name { get; set; }
            public int Value { get; set; }
        }

        #endregion

        #region ContainsAny Tests

        [TestMethod]
        public void ContainsAny_ContainsSubstring_ReturnsTrue()
        {
            var result = "Hello World".ContainsAny("World");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsAny_CaseInsensitive_ReturnsTrue()
        {
            var result = "Hello World".ContainsAny("WORLD");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsAny_DoesNotContain_ReturnsFalse()
        {
            var result = "Hello World".ContainsAny("xyz");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsAny_NullOrWhitespace_ReturnsFalse()
        {
            Assert.IsFalse("Hello".ContainsAny(""));
            Assert.IsFalse("Hello".ContainsAny("   "));
            Assert.IsFalse("".ContainsAny("Hello"));
        }

        #endregion

        #region ToNodePair Tests

        [TestMethod]
        public void ToNodePair_ValidInput_ReturnsIntArray()
        {
            var result = "10 20".ToNodePair();
            CollectionAssert.AreEqual(expectedArray, result);
        }

        #endregion

        #region ToFriendlyDuration Tests

        [TestMethod]
        public void ToFriendlyDuration_Seconds_ReturnsSecondsFormat()
        {
            var timespan = TimeSpan.FromSeconds(30);
            var result = timespan.ToFriendlyDuration(2);
            Assert.AreEqual("30.00 seconds", result);
        }

        [TestMethod]
        public void ToFriendlyDuration_Minutes_ReturnsMinutesFormat()
        {
            var timespan = TimeSpan.FromMinutes(2.5);
            var result = timespan.ToFriendlyDuration(1);
            Assert.AreEqual("2.5 minutes", result);
        }

        [TestMethod]
        public void ToFriendlyDuration_Hours_ReturnsHoursFormat()
        {
            var timespan = TimeSpan.FromHours(3.25);
            var result = timespan.ToFriendlyDuration(2);
            Assert.AreEqual("3.25 hours", result);
        }

        [TestMethod]
        public void ToFriendlyDuration_Days_ReturnsDaysFormat()
        {
            var timespan = TimeSpan.FromDays(2.5);
            var result = timespan.ToFriendlyDuration(1);
            Assert.AreEqual("2.5 days", result);
        }

        #endregion

        #region ToString List Tests

        [TestMethod]
        public void ToString_ListWithNewLine_ReturnsStringWithNewlines()
        {
            var list = new List<string> { "line1", "line2", "line3" };
            var result = list.ToString(true);
            Assert.Contains("line1\r\n", result);
            Assert.Contains("line2\r\n", result);
            Assert.Contains("line3\r\n", result);
        }

        [TestMethod]
        public void ToString_ListWithoutNewLine_ReturnsStringWithoutNewlines()
        {
            var list = new List<string> { "part1", "part2", "part3" };
            var result = list.ToString(false);
            Assert.AreEqual("part1part2part3", result);
        }

        [TestMethod]
        public void ToString_ListWithDelimiter_ReturnsDelimitedString()
        {
            var list = new List<string> { "a", "b", "c" };
            var result = list.ToString(',');
            Assert.AreEqual("a,b,c", result);
        }

        [TestMethod]
        public void ToString_EmptyList_ReturnsNull()
        {
            var list = new List<string>();
            var result = list.ToString(',');
            Assert.IsNull(result);
        }

        #endregion

        #region RemoveUtf8ByteOrderMark Tests

        [TestMethod]
        public void RemoveUtf8ByteOrderMark_WithBOM_RemovesBOM()
        {
            var bomString = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble()) + "Hello";
            var result = bomString.RemoveUtf8ByteOrderMark();
            Assert.AreEqual("Hello", result);
        }

        #endregion

        #region Merge Tests

        [TestMethod]
        public void Merge_ValidList_ReturnsDelimitedString()
        {
            var list = new List<string> { "apple", "banana", "cherry" };
            var result = list.Merge(',');
            Assert.AreEqual("apple,banana,cherry", result);
        }

        [TestMethod]
        public void Merge_EmptyList_ReturnsNull()
        {
            var list = new List<string>();
            var result = list.Merge(',');
            Assert.IsNull(result);
        }

        #endregion

        #region Repeat Tests

        [TestMethod]
        public void Repeat_Char_ReturnsRepeatedChar()
        {
            var result = 'A'.Repeat(5);
            Assert.AreEqual("AAAAA", result);
        }

        [TestMethod]
        public void Repeat_String_ReturnsRepeatedString()
        {
            var result = "Hi".Repeat(3);
            Assert.AreEqual("HiHiHi", result);
        }

        [TestMethod]
        public void Repeat_ZeroTimes_ReturnsEmpty()
        {
            var result = "Test".Repeat(0);
            Assert.AreEqual("", result);
        }

        #endregion

        #region Compression Tests

        [TestMethod]
        public void Compressed_ValidString_ReturnsCompressedString()
        {
            var original = "This is a test string for compression";
            var compressed = original.Compressed();
            Assert.IsNotNull(compressed);
            Assert.AreNotEqual(original, compressed);
        }

        [TestMethod]
        public void Decompress_CompressedString_ReturnsOriginalString()
        {
            var original = "This is a test string for compression and decompression";
            var compressed = original.Compressed();
            var decompressed = compressed.Decompress();
            Assert.AreEqual(original, decompressed);
        }

        #endregion

        #region ToAlphabetIndexes Tests

        [TestMethod]
        public void ToAlphabetIndexes_ValidString_ReturnsCorrectIndexes()
        {
            var result = "ABC".ToAlphabetIndexes();
            CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, result);
        }

        [TestMethod]
        public void ToAlphabetIndexes_EmptyString_ReturnsEmptyList()
        {
            var result = "".ToAlphabetIndexes();
            Assert.IsEmpty(result);
        }

        #endregion

        #region IsPalindrome Tests

        [TestMethod]
        public void IsPalindrome_ValidPalindrome_ReturnsTrue()
        {
            var result = "racecar".IsPalindrome();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPalindrome_NotPalindrome_ReturnsFalse()
        {
            var result = "hello".IsPalindrome();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPalindrome_EmptyString_ReturnsTrue()
        {
            var result = "".IsPalindrome();
            Assert.IsTrue(result);
        }

        #endregion

        #region IsPermutation Tests

        [TestMethod]
        public void IsPermutation_ValidPermutation_ReturnsTrue()
        {
            var result = "abc".IsPermutation("bca");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPermutation_NotPermutation_ReturnsFalse()
        {
            var result = "abc".IsPermutation("def");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPermutation_DifferentLengths_ReturnsFalse()
        {
            var result = "abc".IsPermutation("abcd");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsPermutation_Numbers_ValidPermutation_ReturnsTrue()
        {
            var result = 123L.IsPermutation(321L);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPermutation_Numbers_NotPermutation_ReturnsFalse()
        {
            var result = 123L.IsPermutation(456L);
            Assert.IsFalse(result);
        }

        #endregion

        #region Base64 Tests

        [TestMethod]
        public void ToBase64_ValidString_ReturnsBase64String()
        {
            var result = "Hello".ToBase64();
            var expected = Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello"));
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FromBase64_ValidBase64_ReturnsOriginalString()
        {
            var original = "Hello World!";
            var base64 = original.ToBase64();
            var decoded = base64.FromBase64();
            Assert.AreEqual(original, decoded);
        }

        #endregion

        #region ToPrettyJson Tests

        [TestMethod]
        public void ToPrettyJson_ValidJson_ReturnsFormattedJson()
        {
            var uglyJson = "{\"name\":\"John\",\"age\":30}";
            var result = uglyJson.ToPrettyJson();
            Assert.Contains("  ", result); // Should have indentation
            Assert.Contains("name", result);
            Assert.Contains("John", result);
        }

        [TestMethod]
        public void ToPrettyJson_InvalidJson_ThrowsException()
        {
            var invalidJson = "{invalid json}";
            Assert.ThrowsExactly<System.Text.Json.JsonException>(() => invalidJson.ToPrettyJson());
        }

        #endregion
    }
}