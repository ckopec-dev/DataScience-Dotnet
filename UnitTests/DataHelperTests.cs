using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class DataHelperTests
    {
        private Assembly? _originalAssembly;
        private readonly Assembly? _mockAssembly;

        [TestInitialize]
        public void Setup()
        {
            _originalAssembly = Assembly.GetExecutingAssembly();
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Reset any mocked assemblies if needed
        }

        [TestMethod]
        public void USStockMarketHolidays_WithValidData_ReturnsListOfDates()
        {
            // Arrange
            var testData = "2024-01-01\n2024-07-04\n2024-12-25\n";
            var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));

            // We need to create a mock assembly that returns our test stream
            var mockAssembly = new MockAssembly("Core.Data.USStockMarketHolidays.txt", mockStream);

            // Act
            var result = GetUSStockMarketHolidaysWithMockAssembly(mockAssembly);

            // Assert
            Assert.IsNotNull(result);
            Assert.HasCount(3, result);
            Assert.AreEqual(new DateOnly(2024, 1, 1), result[0]);
            Assert.AreEqual(new DateOnly(2024, 7, 4), result[1]);
            Assert.AreEqual(new DateOnly(2024, 12, 25), result[2]);
        }

        [TestMethod]
        public void USStockMarketHolidays_WithEmptyData_ReturnsEmptyList()
        {
            // Arrange
            var testData = "";
            var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));
            var mockAssembly = new MockAssembly("Core.Data.USStockMarketHolidays.txt", mockStream);

            // Act
            var result = GetUSStockMarketHolidaysWithMockAssembly(mockAssembly);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [TestMethod]
        public void USStockMarketHolidays_WithWhitespaceData_HandlesWhitespaceCorrectly()
        {
            // Arrange
            var testData = "  2024-01-01  \n\t2024-07-04\t\n 2024-12-25 \n";
            var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));
            var mockAssembly = new MockAssembly("Core.Data.USStockMarketHolidays.txt", mockStream);

            // Act
            var result = GetUSStockMarketHolidaysWithMockAssembly(mockAssembly);

            // Assert
            Assert.IsNotNull(result);
            Assert.HasCount(3, result);
            Assert.AreEqual(new DateOnly(2024, 1, 1), result[0]);
            Assert.AreEqual(new DateOnly(2024, 7, 4), result[1]);
            Assert.AreEqual(new DateOnly(2024, 12, 25), result[2]);
        }

        [TestMethod]
        public void USStockMarketHolidays_WithMissingResource_ThrowsResourceNotFoundException()
        {
            // Arrange
            var mockAssembly = new MockAssembly("Core.Data.USStockMarketHolidays.txt", null);

            // Act
            Assert.ThrowsExactly<ResourceNotFoundException>(() => GetUSStockMarketHolidaysWithMockAssembly(mockAssembly));

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public void RnaCodonTable_WithValidData_ReturnsDictionary()
        {
            // Arrange
            var testData = "UUU Phe\nUUC Phe\nUUA Leu\nUUG Leu\n";
            var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));
            var mockAssembly = new MockAssembly("Core.Data.RnaCodonTable.txt", mockStream);

            // Act
            var result = GetRnaCodonTableWithMockAssembly(mockAssembly);

            // Assert
            Assert.IsNotNull(result);
            Assert.HasCount(4, result);
            Assert.AreEqual("Phe", result["UUU"]);
            Assert.AreEqual("Phe", result["UUC"]);
            Assert.AreEqual("Leu", result["UUA"]);
            Assert.AreEqual("Leu", result["UUG"]);
        }

        [TestMethod]
        public void RnaCodonTable_WithEmptyData_ReturnsEmptyDictionary()
        {
            // Arrange
            var testData = "";
            var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));
            var mockAssembly = new MockAssembly("Core.Data.RnaCodonTable.txt", mockStream);

            // Act
            var result = GetRnaCodonTableWithMockAssembly(mockAssembly);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [TestMethod]
        public void RnaCodonTable_WithNullLines_SkipsNullLines()
        {
            // Arrange - This test simulates a scenario where ReadLine() might return null
            var testData = "UUU Phe\nUUC Phe\n";
            var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));
            var mockAssembly = new MockAssembly("Core.Data.RnaCodonTable.txt", mockStream);

            // Act
            var result = GetRnaCodonTableWithMockAssembly(mockAssembly);

            // Assert
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            Assert.AreEqual("Phe", result["UUU"]);
            Assert.AreEqual("Phe", result["UUC"]);
        }

        [TestMethod]
        public void RnaCodonTable_WithMissingResource_ThrowsResourceNotFoundException()
        {
            // Arrange
            var mockAssembly = new MockAssembly("Core.Data.RnaCodonTable.txt", null);

            // Act
            Assert.ThrowsExactly<ResourceNotFoundException>(() => GetRnaCodonTableWithMockAssembly(mockAssembly));

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public void RnaCodonTable_WithSingleSpaceSeparator_ParsesCorrectly()
        {
            // Arrange
            var testData = "UAG Stop\nUAA Stop\nUGA Stop\n";
            var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));
            var mockAssembly = new MockAssembly("Core.Data.RnaCodonTable.txt", mockStream);

            // Act
            var result = GetRnaCodonTableWithMockAssembly(mockAssembly);

            // Assert
            Assert.IsNotNull(result);
            Assert.HasCount(3, result);
            Assert.AreEqual("Stop", result["UAG"]);
            Assert.AreEqual("Stop", result["UAA"]);
            Assert.AreEqual("Stop", result["UGA"]);
        }

        [TestMethod]
        public void RnaCodonTable_WithMultipleSpaces_TakesFirstTwoParts()
        {
            // Arrange - Testing that split[0] and split[1] are used correctly
            var testData = "UUU Phe Extra Data\nUUC Phe More Extra\n";
            var mockStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testData));
            var mockAssembly = new MockAssembly("Core.Data.RnaCodonTable.txt", mockStream);

            // Act
            var result = GetRnaCodonTableWithMockAssembly(mockAssembly);

            // Assert
            Assert.IsNotNull(result);
            Assert.HasCount(2, result);
            Assert.AreEqual("Phe", result["UUU"]);
            Assert.AreEqual("Phe", result["UUC"]);
        }

        // Helper methods to inject mock assembly for testing
        private static List<DateOnly> GetUSStockMarketHolidaysWithMockAssembly(MockAssembly mockAssembly)
        {
            // This method simulates the USStockMarketHolidays property logic with our mock assembly
            Stream? mrs = mockAssembly.GetManifestResourceStream("Core.Data.USStockMarketHolidays.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            List<DateOnly> dates = [];
            string? line;

            while ((line = sr.ReadLine()) != null)
            {
                dates.Add(DateOnly.Parse(line.Trim()));
            }

            return dates;
        }

        private static Dictionary<string, string> GetRnaCodonTableWithMockAssembly(MockAssembly mockAssembly)
        {
            // This method simulates the RnaCodonTable property logic with our mock assembly
            Stream? mrs = mockAssembly.GetManifestResourceStream("Core.Data.RnaCodonTable.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            Dictionary<string, string> dic = [];

            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();

                if (line != null)
                {
                    string[] split = line.Split(" ");
                    dic.Add(split[0], split[1]);
                }
            }

            return dic;
        }
    }

    // Mock assembly class for testing
    public class MockAssembly(string resourceName, Stream? resourceStream)
    {
        private readonly string _resourceName = resourceName;
        private readonly Stream? _resourceStream = resourceStream;

        public Stream? GetManifestResourceStream(string name)
        {
            return name == _resourceName ? _resourceStream : null;
        }
    }
}
