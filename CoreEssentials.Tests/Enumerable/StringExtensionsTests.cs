using CoreEssentials.Enumerable;
using CoreEssentials.Common;

namespace CoreEssentials.Tests.Enumerable
{
    public class StringExtensionsTests
    {
        #region Equals with CompareFlags

        [Fact]
        public void Equals_WithIdenticalStrings_ReturnsTrue()
        {
            Assert.True("hello".Equals("hello", CompareFlags.None));
        }

        [Fact]
        public void Equals_WithDifferentStrings_ReturnsFalse()
        {
            Assert.False("hello".Equals("world", CompareFlags.None));
        }

        [Fact]
        public void Equals_WithIgnoreCaseFlag_ReturnsTrueForCaseDifference()
        {
            Assert.True("Hello".Equals("hello", CompareFlags.IgnoreCase));
        }

        [Fact]
        public void Equals_WithIgnoreWhitespaceFlag_ReturnsTrueForWhitespaceDifference()
        {
            Assert.True("h e l l o".Equals("hello", CompareFlags.IgnoreWhitespace));
        }

        [Fact]
        public void Equals_WithIgnoreCaseAndWhitespaceFlags_ReturnsTrueForBothDifferences()
        {
            Assert.True(" H e L l o ".Equals("hello", CompareFlags.IgnoreCase | CompareFlags.IgnoreWhitespace));
        }

        [Fact]
        public void Equals_WithNullSecondString_ReturnsFalseUnlessBothNull()
        {
            Assert.False("hello".Equals(null, CompareFlags.None));
            string? str1 = null;
            string? str2 = null;
            Assert.True(str1.Equals(str2, CompareFlags.None));
        }

        [Fact]
        public void Equals_WithEmptyStrings_ReturnsTrue()
        {
            Assert.True("".Equals("", CompareFlags.None));
        }

        #endregion

        #region Equals with String Processor

        [Fact]
        public void Equals_WithProcessor_IdenticalStrings_ReturnsTrue()
        {
            Assert.True("abc".Equals("abc", s => s));
        }

        [Fact]
        public void Equals_WithProcessor_DifferentStrings_ReturnsFalse()
        {
            Assert.False("abc".Equals("def", s => s));
        }

        [Fact]
        public void Equals_WithProcessor_TrimmedStrings_ReturnsTrue()
        {
            Assert.True("  abc  ".Equals("abc", s => s?.Trim()));
        }

        [Fact]
        public void Equals_WithProcessor_NullSecondString_ReturnsFalseUnlessBothNull()
        {
            Assert.False("abc".Equals(null, s => s));
            string? str1 = null;
            string? str2 = null;
            Assert.True(str1.Equals(str2, s => s));
        }

        [Fact]
        public void Equals_WithProcessor_EmptyStrings_ReturnsTrue()
        {
            Assert.True("".Equals("", s => s));
        }

        [Fact]
        public void Equals_WithProcessor_CustomNormalization_ReturnsTrue()
        {
            // Remove all digits before comparison
            Func<string?, string?> removeDigits = s => s == null ? null : System.Text.RegularExpressions.Regex.Replace(s, "\\d", "");
            Assert.True("abc123".Equals("abc", removeDigits));
        }

        #endregion

        #region Equals with CompareAlphabets and CompareNumbers

        [Fact]
        public void Equals_WithCompareAlphabets_OnlyAlphabetsCompared()
        {
            Assert.True("abc123".Equals("abc", CompareFlags.CompareAlphabets));
            Assert.True("a b c 1 2 3".Equals("abc", CompareFlags.CompareAlphabets | CompareFlags.IgnoreWhitespace));
            Assert.False("abc123".Equals("abd", CompareFlags.CompareAlphabets));
        }

        [Fact]
        public void Equals_WithCompareNumbers_OnlyNumbersCompared()
        {
            Assert.True("abc123".Equals("123", CompareFlags.CompareNumbers));
            Assert.True("a1b2c3".Equals("123", CompareFlags.CompareNumbers));
            Assert.False("abc123".Equals("124", CompareFlags.CompareNumbers));
        }

        [Fact]
        public void Equals_WithCompareAlphabetsAndIgnoreCase_AlphabetsCaseInsensitive()
        {
            Assert.True("AbC123".Equals("aBc", CompareFlags.CompareAlphabets | CompareFlags.IgnoreCase));
        }

        [Fact]
        public void Equals_WithCompareNumbersAndIgnoreWhitespace_NumbersWhitespaceIgnored()
        {
            Assert.True("1 2 3".Equals("123", CompareFlags.CompareNumbers | CompareFlags.IgnoreWhitespace));
        }

        [Fact]
        public void Equals_WithCompareAlphabetsAndNumbers_AlphabetsAndNumbersCompared()
        {
            Assert.True("abc123".Equals("abc123", CompareFlags.CompareAlphabets | CompareFlags.CompareNumbers));
            Assert.False("a1b2c3".Equals("abc123", CompareFlags.CompareAlphabets | CompareFlags.CompareNumbers));
            Assert.False("abc123".Equals("abd123", CompareFlags.CompareAlphabets | CompareFlags.CompareNumbers));
            Assert.False("abc123".Equals("abc124", CompareFlags.CompareAlphabets | CompareFlags.CompareNumbers));
        }

        [Fact]
        public void Equals_WithCompareAlphabetsNumbersAndIgnoreCase_AllFlagsCombined()
        {
            Assert.True("A1B2C3".Equals("a1b2c3", CompareFlags.CompareAlphabets | CompareFlags.CompareNumbers | CompareFlags.IgnoreCase));
            Assert.True("A 1 B 2 C 3".Equals("a1b2c3", CompareFlags.CompareAlphabets | CompareFlags.CompareNumbers | CompareFlags.IgnoreCase | CompareFlags.IgnoreWhitespace));
        }

        [Fact]
        public void Equals_WithCompareAlphabetsNumbersAndWhitespace_NonAlphaNumIgnored()
        {
            Assert.True("a!1@b#2$c%3".Equals("a1b2c3", CompareFlags.CompareAlphabets | CompareFlags.CompareNumbers));
        }

        #endregion

        #region ToCamelCase and ToPascalCase Tests

        [Theory]
        [InlineData("hello world", "helloWorld")]
        [InlineData("HELLO WORLD", "helloWorld")]
        [InlineData("Hello World", "helloWorld")]
        [InlineData("hello_world", "helloWorld")]
        [InlineData("hello-world", "helloWorld")]
        [InlineData("helloWorld", "helloworld")]
        [InlineData("HelloWorld", "helloworld")]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData("   spaced   words   ", "spacedWords")]
        [InlineData("multiple   spaces   between", "multipleSpacesBetween")]
        [InlineData("mixed_separators-in one_string", "mixedSeparatorsInOneString")]
        public void ToCamelCase_WithVariousInputs_ReturnsCamelCasedString(string input, string expected)
        {
            // Act
            string result = input.ToCamelCase();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("hello world", "HelloWorld")]
        [InlineData("HELLO WORLD", "HelloWorld")]
        [InlineData("Hello World", "HelloWorld")]
        [InlineData("hello_world", "HelloWorld")]
        [InlineData("hello-world", "HelloWorld")]
        [InlineData("helloWorld", "Helloworld")]
        [InlineData("HelloWorld", "Helloworld")]
        [InlineData("", "")]
        [InlineData(null, null)]
        [InlineData("   spaced   words   ", "SpacedWords")]
        [InlineData("multiple   spaces   between", "MultipleSpacesBetween")]
        [InlineData("mixed_separators-in one_string", "MixedSeparatorsInOneString")]
        public void ToPascalCase_WithVariousInputs_ReturnsPascalCasedString(string input, string expected)
        {
            // Act
            string result = input.ToPascalCase();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("A", "a")]
        [InlineData("1", "1")]
        [InlineData("_", "")]
        [InlineData("-", "")]
        [InlineData(" ", "")]
        public void ToCamelCase_WithSingleCharInputs_HandlesEdgeCases(string input, string expected)
        {
            // Act
            string result = input.ToCamelCase();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("a", "A")]
        [InlineData("A", "A")]
        [InlineData("1", "1")]
        [InlineData("_", "")]
        [InlineData("-", "")]
        [InlineData(" ", "")]
        public void ToPascalCase_WithSingleCharInputs_HandlesEdgeCases(string input, string expected)
        {
            // Act
            string result = input.ToPascalCase();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToCamelCase_WithComplexString_ReturnsCamelCaseString()
        {
            // Arrange
            string input = "This_is-a complex   string WITH_mixed Separators123";
            string expected = "thisIsAComplexStringWithMixedSeparators123";
            
            // Act
            string result = input.ToCamelCase();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToPascalCase_WithComplexString_ReturnsPascalCaseString()
        {
            // Arrange
            string input = "This_is-a complex   string WITH_mixed Separators123";
            string expected = "ThisIsAComplexStringWithMixedSeparators123";
            
            // Act
            string result = input.ToPascalCase();
            
            // Assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
