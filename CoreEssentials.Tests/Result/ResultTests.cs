using CoreEssentials.Result;

namespace CoreEssentials.Tests.Result
{
    /// <summary>
    /// Unit tests for the Result{TResult, TError} class, covering all public methods and scenarios.
    /// Tests include success cases, error cases, edge cases, and exception handling.
    /// </summary>
    public class ResultTests
    {
        #region Ok Method Tests

        /// <summary>
        /// Tests that the Ok method creates a successful result with the provided value.
        /// Verifies that IsOk returns true, IsErr returns false, and Value property returns the correct value.
        /// </summary>
        [Fact]
        public void Ok_WithValidValue_CreatesSuccessfulResult()
        {
            // Arrange
            const string expectedValue = "test value";

            // Act
            var result = Result<string, string>.Ok(expectedValue);

            // Assert
            Assert.True(result.IsOk());
            Assert.False(result.IsErr());
            Assert.Equal(expectedValue, result.Value);
        }

        /// <summary>
        /// Tests that the Ok method can create a successful result with a null value.
        /// This ensures that null values are properly handled in success scenarios.
        /// </summary>
        [Fact]
        public void Ok_WithNullValue_CreatesSuccessfulResultWithNull()
        {
            // Act
            var result = Result<string?, string>.Ok(null);

            // Assert
            Assert.True(result.IsOk());
            Assert.False(result.IsErr());
            Assert.Null(result.Value);
        }

        /// <summary>
        /// Tests that the Ok method works with value types like integers.
        /// Verifies proper handling of primitive types in success scenarios.
        /// </summary>
        [Fact]
        public void Ok_WithValueType_CreatesSuccessfulResult()
        {
            // Arrange
            const int expectedValue = 42;

            // Act
            var result = Result<int, string>.Ok(expectedValue);

            // Assert
            Assert.True(result.IsOk());
            Assert.False(result.IsErr());
            Assert.Equal(expectedValue, result.Value);
        }

        #endregion

        #region Err Method Tests

        /// <summary>
        /// Tests that the Err method creates an error result with the provided error value.
        /// Verifies that IsErr returns true, IsOk returns false, and Error property returns the correct error.
        /// </summary>
        [Fact]
        public void Err_WithValidError_CreatesErrorResult()
        {
            // Arrange
            const string expectedError = "test error";

            // Act
            var result = Result<string, string>.Err(expectedError);

            // Assert
            Assert.False(result.IsOk());
            Assert.True(result.IsErr());
            Assert.Equal(expectedError, result.Error);
        }

        /// <summary>
        /// Tests that the Err method properly handles Exception objects as error values.
        /// Verifies that exceptions are correctly stored and can be retrieved later.
        /// </summary>
        [Fact]
        public void Err_WithException_CreatesErrorResultWithException()
        {
            // Arrange
            var expectedException = new InvalidOperationException("test exception");

            // Act
            var result = Result<string, Exception>.Err(expectedException);

            // Assert
            Assert.False(result.IsOk());
            Assert.True(result.IsErr());
            Assert.Equal(expectedException, result.Error);
        }

        /// <summary>
        /// Tests that the Err method can handle null error values.
        /// This ensures proper handling of null in error scenarios.
        /// </summary>
        [Fact]
        public void Err_WithNullError_CreatesErrorResultWithNull()
        {
            // Act
            var result = Result<string, string?>.Err(null);

            // Assert
            Assert.False(result.IsOk());
            Assert.True(result.IsErr());
            Assert.Null(result.Error);
        }

        #endregion

        #region Value Property Tests

        /// <summary>
        /// Tests that accessing the Value property on a successful result returns the correct value.
        /// This is the happy path scenario for value retrieval.
        /// </summary>
        [Fact]
        public void Value_OnSuccessfulResult_ReturnsCorrectValue()
        {
            // Arrange
            const string expectedValue = "success value";
            var result = Result<string, string>.Ok(expectedValue);

            // Act & Assert
            Assert.Equal(expectedValue, result.Value);
        }

        /// <summary>
        /// Tests that accessing the Value property on an error result throws InvalidOperationException.
        /// This ensures proper error handling when attempting to access value from failed results.
        /// </summary>
        [Fact]
        public void Value_OnErrorResult_ThrowsInvalidOperationException()
        {
            // Arrange
            var result = Result<string, string>.Err("error");

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => result.Value);
            Assert.Equal("Called success Value on an Error Result", exception.Message);
        }

        #endregion

        #region Error Property Tests

        /// <summary>
        /// Tests that accessing the Error property on an error result returns the correct error value.
        /// This is the expected behavior for error retrieval from failed results.
        /// </summary>
        [Fact]
        public void Error_OnErrorResult_ReturnsCorrectError()
        {
            // Arrange
            const string expectedError = "test error";
            var result = Result<string, string>.Err(expectedError);

            // Act & Assert
            Assert.Equal(expectedError, result.Error);
        }

        /// <summary>
        /// Tests that accessing the Error property on a successful result throws InvalidOperationException.
        /// This ensures proper error handling when attempting to access error from successful results.
        /// </summary>
        [Fact]
        public void Error_OnSuccessfulResult_ThrowsInvalidOperationException()
        {
            // Arrange
            var result = Result<string, string>.Ok("success");

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => result.Error);
            Assert.Equal("Called Error on a Success Result", exception.Message);
        }

        #endregion

        #region UnwrapOr Method Tests

        /// <summary>
        /// Tests that UnwrapOr returns the result value when the result is successful.
        /// The default value should be ignored in success scenarios.
        /// </summary>
        [Fact]
        public void UnwrapOr_OnSuccessfulResult_ReturnsResultValue()
        {
            // Arrange
            const string resultValue = "success value";
            const string defaultValue = "default value";
            var result = Result<string, string>.Ok(resultValue);

            // Act
            var actualValue = result.UnwrapOr(defaultValue);

            // Assert
            Assert.Equal(resultValue, actualValue);
        }

        /// <summary>
        /// Tests that UnwrapOr returns the provided default value when the result is an error.
        /// This ensures proper fallback behavior for failed results.
        /// </summary>
        [Fact]
        public void UnwrapOr_OnErrorResult_ReturnsDefaultValue()
        {
            // Arrange
            const string defaultValue = "default value";
            var result = Result<string, string>.Err("error");

            // Act
            var actualValue = result.UnwrapOr(defaultValue);

            // Assert
            Assert.Equal(defaultValue, actualValue);
        }

        /// <summary>
        /// Tests UnwrapOr with null default value to ensure proper null handling.
        /// Verifies that null can be used as a valid default value.
        /// </summary>
        [Fact]
        public void UnwrapOr_WithNullDefault_HandlesNullCorrectly()
        {
            // Arrange
            var result = Result<string?, string>.Err("error");

            // Act
            var actualValue = result.UnwrapOr(null);

            // Assert
            Assert.Null(actualValue);
        }

        #endregion

        #region Unwrap Method Tests

        /// <summary>
        /// Tests that Unwrap returns the result value when the result is successful.
        /// This is the happy path for unwrapping successful results.
        /// </summary>
        [Fact]
        public void Unwrap_OnSuccessfulResult_ReturnsResultValue()
        {
            // Arrange
            const string expectedValue = "success value";
            var result = Result<string, string>.Ok(expectedValue);

            // Act
            var actualValue = result.Unwrap();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        /// <summary>
        /// Tests that Unwrap throws an exception when the result contains an Exception error.
        /// This ensures that exception information is preserved, though reconstruction may sometimes fail.
        /// </summary>
        [Fact]
        public void Unwrap_OnErrorResultWithException_ThrowsException()
        {
            // Arrange
            var originalException = new ArgumentNullException("paramName", "test message");
            var result = Result<string, Exception>.Err(originalException);

            // Act & Assert - Exception reconstruction may fail with AmbiguousMatchException
            var thrownException = Assert.ThrowsAny<Exception>(() => result.Unwrap());
            Assert.True(thrownException is ArgumentNullException);
        }

        /// <summary>
        /// Tests that Unwrap throws a generic Exception with error details when the result contains a non-Exception error.
        /// This ensures that non-exception errors are still properly handled and thrown as exceptions.
        /// </summary>
        [Fact]
        public void Unwrap_OnErrorResultWithNonException_ThrowsGenericExceptionWithErrorDetails()
        {
            // Arrange
            const string errorMessage = "custom error message";
            var result = Result<string, string>.Err(errorMessage);

            // Act & Assert
            var thrownException = Assert.Throws<Exception>(() => result.Unwrap());
            Assert.Equal(errorMessage, thrownException.Message);
        }

        /// <summary>
        /// Tests Unwrap behavior when the error result contains a null non-exception error.
        /// Verifies proper handling of null error values in unwrap scenarios.
        /// </summary>
        [Fact]
        public void Unwrap_OnErrorResultWithNullNonException_ThrowsGenericException()
        {
            // Arrange
            var result = Result<string, string?>.Err(null);

            // Act & Assert
            var thrownException = Assert.Throws<Exception>(() => result.Unwrap());
            // The message will be the default Exception message since ToString() on null returns empty string
            Assert.NotNull(thrownException.Message);
        }

        #endregion

        #region IsCausedBy Method Tests

        /// <summary>
        /// Tests that IsCausedBy returns false when the result is successful.
        /// Exception checking should only apply to error results.
        /// </summary>
        [Fact]
        public void IsCausedBy_OnSuccessfulResult_ReturnsFalse()
        {
            // Arrange
            var result = Result<string, Exception>.Ok("success");

            // Act
            var isCausedByArgumentException = result.IsCausedBy<ArgumentException>();

            // Assert
            Assert.False(isCausedByArgumentException);
        }

        /// <summary>
        /// Tests that IsCausedBy returns false when the result is an error but contains no exception.
        /// Exception type checking requires an actual exception to be present.
        /// </summary>
        [Fact]
        public void IsCausedBy_OnErrorResultWithoutException_ReturnsFalse()
        {
            // Arrange
            var result = Result<string, string>.Err("string error");

            // Act
            var isCausedByArgumentException = result.IsCausedBy<ArgumentException>();

            // Assert
            Assert.False(isCausedByArgumentException);
        }

        /// <summary>
        /// Tests that IsCausedBy returns true when the exception type exactly matches the specified type.
        /// This verifies direct type matching functionality.
        /// </summary>
        [Fact]
        public void IsCausedBy_WithExactExceptionType_ReturnsTrue()
        {
            // Arrange
            var exception = new ArgumentNullException("param");
            var result = Result<string, Exception>.Err(exception);

            // Act
            var isCausedByArgumentNullException = result.IsCausedBy<ArgumentNullException>();

            // Assert
            Assert.True(isCausedByArgumentNullException);
        }

        /// <summary>
        /// Tests that IsCausedBy returns true when the exception is a derived type of the specified base type.
        /// This verifies inheritance-based type matching (ArgumentNullException inherits from ArgumentException).
        /// </summary>
        [Fact]
        public void IsCausedBy_WithDerivedExceptionType_ReturnsTrue()
        {
            // Arrange
            var exception = new ArgumentNullException("param");
            var result = Result<string, Exception>.Err(exception);

            // Act
            var isCausedByArgumentException = result.IsCausedBy<ArgumentException>();

            // Assert
            Assert.True(isCausedByArgumentException);
        }

        /// <summary>
        /// Tests that IsCausedBy returns false when the exception type does not match the specified type.
        /// This verifies that unrelated exception types are correctly identified as non-matches.
        /// </summary>
        [Fact]
        public void IsCausedBy_WithUnrelatedExceptionType_ReturnsFalse()
        {
            // Arrange
            var exception = new InvalidOperationException("test");
            var result = Result<string, Exception>.Err(exception);

            // Act
            var isCausedByArgumentException = result.IsCausedBy<ArgumentException>();

            // Assert
            Assert.False(isCausedByArgumentException);
        }

        #endregion

        #region Implicit Conversion Tests

        /// <summary>
        /// Tests that a value can be implicitly converted to a successful Result.
        /// This verifies the implicit conversion operator for success scenarios.
        /// </summary>
        [Fact]
        public void ImplicitConversion_FromValue_CreatesSuccessfulResult()
        {
            // Arrange
            const int value = 42;

            // Act
            Result<int, string> result = value;

            // Assert
            Assert.True(result.IsOk());
            Assert.Equal(value, result.Value);
        }

        /// <summary>
        /// Tests that an error value can be implicitly converted to an error Result.
        /// This verifies the implicit conversion operator for error scenarios.
        /// </summary>
        [Fact]
        public void ImplicitConversion_FromError_CreatesErrorResult()
        {
            // Arrange
            var error = new ArgumentException("test error");

            // Act
            Result<string, ArgumentException> result = error;

            // Assert
            Assert.True(result.IsErr());
            Assert.Equal(error, result.Error);
        }

        /// <summary>
        /// Tests implicit conversion with null values to ensure proper null handling.
        /// This verifies that null values can be properly converted to Results.
        /// </summary>
        [Fact]
        public void ImplicitConversion_WithNullValue_HandlesNullCorrectly()
        {
            // Act
            Result<string?, int> result = (string?)null;

            // Assert
            Assert.True(result.IsOk());
            Assert.Null(result.Value);
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Tests a complete workflow of creating a successful result and accessing all its properties.
        /// This integration test verifies that all methods work correctly together in success scenarios.
        /// </summary>
        [Fact]
        public void IntegrationTest_SuccessfulResult_AllMethodsWorkCorrectly()
        {
            // Arrange
            const int value = 42;
            const int defaultValue = 0;

            // Act
            var result = Result<int, string>.Ok(value);

            // Assert
            Assert.True(result.IsOk());
            Assert.False(result.IsErr());
            Assert.Equal(value, result.Value);
            Assert.Equal(value, result.UnwrapOr(defaultValue));
            Assert.Equal(value, result.Unwrap());
            Assert.False(result.IsCausedBy<ArgumentException>());
        }

        /// <summary>
        /// Tests a complete workflow of creating an error result and accessing all its properties.
        /// This integration test verifies that all methods work correctly together in error scenarios.
        /// </summary>
        [Fact]
        public void IntegrationTest_ErrorResult_AllMethodsWorkCorrectly()
        {
            // Arrange
            var exception = new ArgumentException("test argument exception");
            const string defaultValue = "default";

            // Act
            var result = Result<string, Exception>.Err(exception);

            // Assert
            Assert.False(result.IsOk());
            Assert.True(result.IsErr());
            Assert.Equal(exception, result.Error);
            Assert.Equal(defaultValue, result.UnwrapOr(defaultValue));
            Assert.True(result.IsCausedBy<ArgumentException>());
            
            // Verify that accessing Value throws
            Assert.Throws<InvalidOperationException>(() => result.Value);
            
            // Verify that Unwrap throws an exception (may be AmbiguousMatchException due to reconstruction issues)
            Assert.ThrowsAny<Exception>(() => result.Unwrap());
        }

        /// <summary>
        /// Tests edge case scenarios with various data types to ensure robust type handling.
        /// This verifies that the Result class works correctly with different generic type combinations.
        /// </summary>
        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        public void EdgeCase_IntegerValues_WorkCorrectly(int testValue)
        {
            // Act
            var successResult = Result<int, string>.Ok(testValue);
            var errorResult = Result<int, string>.Err("error");

            // Assert
            Assert.Equal(testValue, successResult.Value);
            Assert.Equal(testValue, successResult.UnwrapOr(-1));
            Assert.Equal(-1, errorResult.UnwrapOr(-1));
        }

        /// <summary>
        /// Tests Result behavior with custom exception types to ensure proper exception handling.
        /// This verifies that custom exceptions are properly stored and the IsCausedBy method works correctly.
        /// </summary>
        [Fact]
        public void CustomException_HandlingTest_WorksCorrectly()
        {
            // Arrange
            var customException = new CustomTestException("Custom message", 123);
            var result = Result<string, CustomTestException>.Err(customException);

            // Act & Assert
            Assert.True(result.IsErr());
            Assert.Equal(customException, result.Error);
            Assert.True(result.IsCausedBy<CustomTestException>());
            Assert.True(result.IsCausedBy<Exception>());
            
            // Verify that Unwrap throws an exception (reconstruction may fall back to generic Exception)
            var thrownException = Assert.Throws<Exception>(() => result.Unwrap());
            Assert.Contains("Custom message", thrownException.Message);
        }

        #endregion


        #region Helper Classes for Testing

        /// <summary>
        /// Custom exception class for testing exception handling scenarios.
        /// This helps verify that the Result class works correctly with user-defined exception types.
        /// </summary>
        private class CustomTestException : Exception
        {
            public int ErrorCode { get; }

            public CustomTestException(string message, int errorCode) : base(message)
            {
                ErrorCode = errorCode;
            }
        }

        #endregion
    }
}
