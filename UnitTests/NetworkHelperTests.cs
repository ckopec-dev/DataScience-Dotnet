using Core.Internet;
using Xunit;
using Xunit.Sdk;
using Assert = Xunit.Assert;

namespace UnitTests
{
    public class NetworkHelperTests
    {
        [Fact]
        public async Task GetHtmlAsync_WithValidUrl_ReturnsContent()
        {
            // Arrange
            var url = "https://en.wikipedia.org";

            // Act
            var result = await NetworkHelper.GetHtmlAsync(url);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void ProjectPath_WithValidDirectoryStructure_ReturnsProjectDirectory()
        {
            // Arrange & Act
            var result = NetworkHelper.ProjectPath();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(Directory.Exists(result));
        }

        [Fact]
        public void RandomSleep_WithValidParameters_DoesNotThrow()
        {
            // Arrange
            int min = 10;
            int max = 100;

            // Act & Assert
            Assert.Null(Record.Exception(() => NetworkHelper.RandomSleep(min, max)));
        }

        [Theory]
        [InlineData(0x12345678, 0x78563412)]
        [InlineData(0x00000000, 0x00000000)]
        [InlineData(0xFFFFFFFF, 0xFFFFFFFF)]
        [InlineData(0x10203040, 0x40302010)]
        public void SwapEndianness_WithVariousInputs_ReturnsCorrectResult(uint input, uint expected)
        {
            // Act
            var result = NetworkHelper.SwapEndianness(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SwapEndianness_WithZeroInput_ReturnsZero()
        {
            // Act
            var result = NetworkHelper.SwapEndianness(0);

            // Assert
            Assert.Equal(0u, result);
        }

        [Fact]
        public void SwapEndianness_WithMaxUintInput_ReturnsCorrectResult()
        {
            // Act
            var result = NetworkHelper.SwapEndianness(uint.MaxValue);

            // Assert
            Assert.Equal(uint.MaxValue, result);
        }
    }
}
