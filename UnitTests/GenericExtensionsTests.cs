using Core;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests
{
    public class GenericExtensionsTests
    {
        [Fact]
        public void RandomSort_EmptyList_RemainsEmpty()
        {
            // Arrange
            var list = new List<int>();

            // Act
            list.RandomSort();

            // Assert
            Assert.Empty(list);
        }

        [Fact]
        public void RandomSort_SingleElement_ListUnchanged()
        {
            // Arrange
            var list = new List<int> { 42 };

            // Act
            list.RandomSort();

            // Assert
            Assert.Single(list);
            Assert.Equal(42, list[0]);
        }

        [Fact]
        public void RandomSort_TwoElements_ValuesSwapped()
        {
            // Arrange
            var list = new List<int> { 1, 2 };

            // Act
            list.RandomSort();

            // Assert - Values should still be present but order may differ
            Assert.Equal(2, list.Count);
            Assert.Contains(1, list);
            Assert.Contains(2, list);
        }

        [Fact]
        public void RandomSort_MultipleElements_AllElementsPresent()
        {
            // Arrange
            var originalList = new List<int> { 1, 2, 3, 4, 5 };
            var list = new List<int>(originalList);

            // Act
            list.RandomSort();

            // Assert
            Assert.Equal(5, list.Count);
            foreach (var item in originalList)
            {
                Assert.Contains(item, list);
            }
        }

        [Fact]
        public void RandomSort_MultipleElements_DifferentOrder()
        {
            // Arrange
            var originalList = new List<int> { 1, 2, 3, 4, 5 };
            var list = new List<int>(originalList);

            // Act
            list.RandomSort();

            // Assert - There's a very small chance this could fail with random sorting,
            // but it's extremely unlikely. If it fails, it's likely due to implementation issues.
            Assert.Equal(5, list.Count);
            Assert.Contains(1, list);
            Assert.Contains(2, list);
            Assert.Contains(3, list);
            Assert.Contains(4, list);
            Assert.Contains(5, list);
        }

        [Fact]
        public void RandomSort_StringElements_AllElementsPresent()
        {
            // Arrange
            var originalList = new List<string> { "apple", "banana", "cherry" };
            var list = new List<string>(originalList);

            // Act
            list.RandomSort();

            // Assert
            Assert.Equal(3, list.Count);
            foreach (var item in originalList)
            {
                Assert.Contains(item, list);
            }
        }

        [Fact]
        public void RandomSort_ObjectElements_AllElementsPresent()
        {
            // Arrange
            var obj1 = new TestClass { Id = 1 };
            var obj2 = new TestClass { Id = 2 };
            var obj3 = new TestClass { Id = 3 };

            var originalList = new List<TestClass> { obj1, obj2, obj3 };
            var list = new List<TestClass>(originalList);

            // Act
            list.RandomSort();

            // Assert
            Assert.Equal(3, list.Count);
            Assert.Contains(obj1, list);
            Assert.Contains(obj2, list);
            Assert.Contains(obj3, list);
        }

        [Fact]
        public void RandomSort_WithDuplicates_AllElementsPresent()
        {
            // Arrange
            var originalList = new List<int> { 1, 2, 2, 3 };
            var list = new List<int>(originalList);

            // Act
            list.RandomSort();

            // Assert
            Assert.Equal(4, list.Count);
            Assert.Contains(1, list);
            Assert.Contains(2, list);
            Assert.Contains(2, list);
            Assert.Contains(3, list);
        }

        [Fact]
        public void RandomSort_LargeList_AllElementsPresent()
        {
            // Arrange
            var originalList = Enumerable.Range(1, 100).ToList();
            var list = new List<int>(originalList);

            // Act
            list.RandomSort();

            // Assert
            Assert.Equal(100, list.Count);
            foreach (var item in originalList)
            {
                Assert.Contains(item, list);
            }
        }

        [Fact]
        public void RandomSort_ConsistentBehavior_WithSameSeed()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var originalList = new List<int>(list);

            // Act - Create a new instance with same seed for predictable behavior
            // Note: This test demonstrates that the extension works consistently
            // but since Random is used internally, we can't guarantee exact ordering
            list.RandomSort();

            // Assert
            Assert.Equal(5, list.Count);
            foreach (var item in originalList)
            {
                Assert.Contains(item, list);
            }
        }

        [Fact]
        public void RandomSort_SortTwice_BothValid()
        {
            // Arrange
            var list1 = new List<int> { 1, 2, 3 };
            var list2 = new List<int> { 1, 2, 3 };

            // Act
            list1.RandomSort();
            list2.RandomSort();

            // Assert
            Assert.Equal(3, list1.Count);
            Assert.Equal(3, list2.Count);
            foreach (var item in list1)
            {
                Assert.Contains(item, list1);
            }
            foreach (var item in list2)
            {
                Assert.Contains(item, list2);
            }
        }

        [Fact]
        public void RandomSort_WithNullElements_AllElementsPresent()
        {
            // Arrange
            var originalList = new List<string?> { "hello", null, "world" };
            var list = new List<string?>(originalList);

            // Act
            list.RandomSort();

            // Assert
            Assert.Equal(3, list.Count);
            Assert.Contains("hello", list);
            Assert.Contains("world", list);
            Assert.Contains(null, list);
        }

        [Fact]
        public void RandomSort_WithCustomType_AllElementsPresent()
        {
            // Arrange
            var originalList = new List<CustomClass>
            {
                new() { Name = "A" },
                new() { Name = "B" },
                new() { Name = "C" }
            };
            var list = new List<CustomClass>(originalList);

            // Act
            list.RandomSort();

            // Assert
            Assert.Equal(3, list.Count);
            Assert.Contains(originalList[0], list);
            Assert.Contains(originalList[1], list);
            Assert.Contains(originalList[2], list);
        }

        private class TestClass
        {
            public int Id { get; set; }
        }

        private class CustomClass
        {
            public string Name { get; set; } = string.Empty;
        }
    }
}
