using Core;
using ExtendedNumerics;
using System.Numerics;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests
{
    public class FileHelperTests
    {
        [Fact]
        public void GetProjectPath_WhenCalled_ReturnsValidPath()
        {
            // Act
            var result = FileHelper.GetProjectPath();

            // Assert
            Assert.NotNull(result);
            Assert.True(Directory.Exists(result));
        }

        //[Theory]
        //[InlineData(0, "0 bytes")]
        //[InlineData(1, "1 bytes")]
        //[InlineData(1023, "1023 bytes")]
        //public void ConvertFileSizeToString_WhenSizeLessThan1024_ReturnsBytesString(int size, string expected)
        //{
        //    // Act
        //    var result = FileHelper.ConvertFileSizeToString(size);

        //    // Assert
        //    Assert.Equal(expected, result);
        //}

        //[Theory]
        //[InlineData(1024, "1 Kb")]
        //[InlineData(2048, "2 Kb")]
        //[InlineData(1048575, "1024 Kb")]
        //public void ConvertFileSizeToString_WhenSizeBetween1024And1048576_ReturnsKbString(int size, string expected)
        //{
        //    // Act
        //    var result = FileHelper.ConvertFileSizeToString(size);

        //    // Assert
        //    Assert.Equal(expected, result);
        //}

        //[Theory]
        //[InlineData(1048576, "1 Mb")]
        //[InlineData(2097152, "2 Mb")]
        //[InlineData(1048775, "1 Mb")]
        //public void ConvertFileSizeToString_WhenSizeGreaterThanOrEqual1048576_ReturnsMbString(int size, string expected)
        //{
        //    // Act
        //    var result = FileHelper.ConvertFileSizeToString(size);

        //    // Assert
        //    Assert.Equal(expected, result);
        //}

        //[Fact]
        //public void ConvertFileSizeToString_WhenSizeIsNegative_ReturnsCorrectValue()
        //{
        //    // Act
        //    var result = FileHelper.ConvertFileSizeToString(-1024);

        //    // Assert
        //    Assert.Equal("-1 Kb", result);
        //}

        //[Fact]
        //public void ConvertFileSizeToString_WhenSizeIsLarge_ReturnsCorrectMbValue()
        //{
        //    // Act
        //    var result = FileHelper.ConvertFileSizeToString(20971520); // 20MB

        //    // Assert
        //    Assert.Equal("20 Mb", result);
        //}

        //[Fact]
        //public void ConvertFileSizeToString_WhenSizeIsExactly1048576_ReturnsOneMb()
        //{
        //    // Act
        //    var result = FileHelper.ConvertFileSizeToString(1048576);

        //    // Assert
        //    Assert.Equal("1 Mb", result);
        //}

        //[Fact]
        //public void ConvertFileSizeToString_WhenSizeIsExactly1024_ReturnsOneKb()
        //{
        //    // Act
        //    var result = FileHelper.ConvertFileSizeToString(1024);

        //    // Assert
        //    Assert.Equal("1 Kb", result);
        //}
    }
}
