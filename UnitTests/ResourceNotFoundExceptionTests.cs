using Core;
using System;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests
{
    public class ResourceNotFoundExceptionTests
    {
        [Fact]
        public void Constructor_WithNoParameters_ShouldSetDefaultMessage()
        {
            // Act
            var exception = new ResourceNotFoundException();

            // Assert
            Assert.Equal("Resource not found", exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithMessageParameter_ShouldSetCustomMessage()
        {
            // Arrange
            const string customMessage = "Custom resource not found message";

            // Act
            var exception = new ResourceNotFoundException(customMessage);

            // Assert
            Assert.Equal(customMessage, exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            const string customMessage = "Custom error message";
            var innerException = new InvalidOperationException("Inner exception message");

            // Act
            var exception = new ResourceNotFoundException(customMessage, innerException);

            // Assert
            Assert.Equal(customMessage, exception.Message);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void InheritsFromException()
        {
            // Act
            var exception = new ResourceNotFoundException();

            // Assert
            Assert.IsType<Exception>(exception, exactMatch: false);
        }

        [Fact]
        public void Constructor_WithEmptyMessage_ShouldSetEmptyString()
        {
            // Act
            var exception = new ResourceNotFoundException(string.Empty);

            // Assert
            Assert.Equal(string.Empty, exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithInnerException_ShouldBeSerializable()
        {
            // Arrange
            var innerException = new ArgumentException("Inner argument exception");

            // Act
            var exception = new ResourceNotFoundException("Test message", innerException);

            // Assert
            Assert.Equal("Test message", exception.Message);
            Assert.Same(innerException, exception.InnerException);
        }

        [Fact]
        public void Exception_InheritanceChain_ShouldBeCorrect()
        {
            // Act
            var exception = new ResourceNotFoundException();

            // Assert
            Assert.IsType<ResourceNotFoundException>(exception);
            Assert.IsType<Exception>(exception, exactMatch: false);
            Assert.True(exception.GetType().IsSubclassOf(typeof(Exception)));
        }

        [Fact]
        public void Constructor_WithVeryLongMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var longMessage = new string('A', 1000);

            // Act
            var exception = new ResourceNotFoundException(longMessage);

            // Assert
            Assert.Equal(longMessage, exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void Constructor_WithSpecialCharactersInMessage_ShouldHandleCorrectly()
        {
            // Arrange
            const string specialMessage = "Resource not found: \n\t\r\"'<>";

            // Act
            var exception = new ResourceNotFoundException(specialMessage);

            // Assert
            Assert.Equal(specialMessage, exception.Message);
            Assert.Null(exception.InnerException);
        }
    }
}
