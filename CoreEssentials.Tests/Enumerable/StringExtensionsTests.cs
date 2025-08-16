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

        #region PadChars Method Tests

        [Fact]
        public void PadChars_WithNullInput_ThrowsArgumentNullException()
        {
            // Arrange
            string? input = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => input!.PadChars(10, PadOptions.Numbers));
        }

        [Fact]
        public void PadChars_WithTargetLengthSmallerThanInputLength_ReturnsOriginalString()
        {
            // Arrange
            string input = "hello world";
            int targetLength = 5;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Numbers);

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void PadChars_WithTargetLengthEqualToInputLength_ReturnsOriginalString()
        {
            // Arrange
            string input = "hello";
            int targetLength = 5;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Numbers);

            // Assert
            Assert.Equal(input, result);
            Assert.Equal(targetLength, result.Length);
        }

        [Fact]
        public void PadChars_WithPadOptionsNone_ReturnsOriginalStringRegardlessOfTargetLength()
        {
            // Arrange
            string input = "test";
            int targetLength = 10;

            // Act
            string result = input.PadChars(targetLength, PadOptions.None);

            // Assert
            Assert.Equal(input, result);
            Assert.Equal(input.Length, result.Length);
        }

        [Fact]
        public void PadChars_WithNumbersPadOption_PadsWithDigitsOnly()
        {
            // Arrange
            string input = "abc";
            int targetLength = 8;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Numbers);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Check that all padding characters are digits
            string paddingPart = result.Substring(input.Length);
            Assert.All(paddingPart, c => Assert.True(char.IsDigit(c)));
        }

        [Fact]
        public void PadChars_WithLowerCaseAlphabetsPadOption_PadsWithLowercaseLettersOnly()
        {
            // Arrange
            string input = "123";
            int targetLength = 10;

            // Act
            string result = input.PadChars(targetLength, PadOptions.LowerCaseAlphabets);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Check that all padding characters are lowercase letters
            string paddingPart = result.Substring(input.Length);
            Assert.All(paddingPart, c => Assert.True(char.IsLower(c) && char.IsLetter(c)));
        }

        [Fact]
        public void PadChars_WithUpperCaseAlphabetsPadOption_PadsWithUppercaseLettersOnly()
        {
            // Arrange
            string input = "xyz";
            int targetLength = 12;

            // Act
            string result = input.PadChars(targetLength, PadOptions.UpperCaseAlphabets);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Check that all padding characters are uppercase letters
            string paddingPart = result.Substring(input.Length);
            Assert.All(paddingPart, c => Assert.True(char.IsUpper(c) && char.IsLetter(c)));
        }

        [Fact]
        public void PadChars_WithWhitespacePadOption_PadsWithWhitespaceCharactersOnly()
        {
            // Arrange
            string input = "test";
            int targetLength = 15;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Whitespace);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Check that all padding characters are whitespace
            string paddingPart = result.Substring(input.Length);
            Assert.All(paddingPart, c => Assert.True(char.IsWhiteSpace(c) || c == ' ' || c == '\t' || c == '\n' || c == '\r'));
        }

        [Fact]
        public void PadChars_WithSpecialCharactersPadOption_PadsWithSpecialCharactersOnly()
        {
            // Arrange
            string input = "word";
            int targetLength = 20;

            // Act
            string result = input.PadChars(targetLength, PadOptions.SpecialCharacters);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Check that all padding characters are special characters
            string paddingPart = result.Substring(input.Length);
            string expectedSpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?/~`";
            Assert.All(paddingPart, c => Assert.Contains(c, expectedSpecialChars));
        }

        [Fact]
        public void PadChars_WithNumbersAndLowerCaseAlphabetsCombined_PadsWithDigitsAndLowercaseLetters()
        {
            // Arrange
            string input = "mix";
            int targetLength = 20;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Numbers | PadOptions.LowerCaseAlphabets);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Check that all padding characters are either digits or lowercase letters
            string paddingPart = result.Substring(input.Length);
            Assert.All(paddingPart, c => Assert.True(char.IsDigit(c) || (char.IsLetter(c) && char.IsLower(c))));
        }

        [Fact]
        public void PadChars_WithNumbersAndUpperCaseAlphabetsCombined_PadsWithDigitsAndUppercaseLetters()
        {
            // Arrange
            string input = "TEST";
            int targetLength = 25;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Numbers | PadOptions.UpperCaseAlphabets);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Check that all padding characters are either digits or uppercase letters
            string paddingPart = result.Substring(input.Length);
            Assert.All(paddingPart, c => Assert.True(char.IsDigit(c) || (char.IsLetter(c) && char.IsUpper(c))));
        }

        [Fact]
        public void PadChars_WithBothUpperAndLowerCaseAlphabetsCombined_PadsWithBothCaseLetters()
        {
            // Arrange
            string input = "Case";
            int targetLength = 30;

            // Act
            string result = input.PadChars(targetLength, PadOptions.LowerCaseAlphabets | PadOptions.UpperCaseAlphabets);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Check that all padding characters are letters (both upper and lower case allowed)
            string paddingPart = result.Substring(input.Length);
            Assert.All(paddingPart, c => Assert.True(char.IsLetter(c)));
        }

        [Fact]
        public void PadChars_WithAllBasicOptionsCombined_PadsWithAllCharacterTypes()
        {
            // Arrange
            string input = "all";
            int targetLength = 50;
            var allBasicOptions = PadOptions.Numbers | PadOptions.LowerCaseAlphabets | 
                                PadOptions.UpperCaseAlphabets | PadOptions.SpecialCharacters;

            // Act
            string result = input.PadChars(targetLength, allBasicOptions);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // The padding part should contain a mix of different character types
            string paddingPart = result.Substring(input.Length);
            Assert.True(paddingPart.Length > 0);
            
            // At least verify that valid characters are used (not testing randomness distribution)
            string expectedSpecialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?/~`";
            Assert.All(paddingPart, c => Assert.True(
                char.IsDigit(c) || 
                char.IsLetter(c) || 
                expectedSpecialChars.Contains(c)));
        }

        [Fact]
        public void PadChars_WithWhitespaceAndOtherOptionsCombined_IncludesWhitespaceInPadding()
        {
            // Arrange
            string input = "space";
            int targetLength = 25;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Whitespace | PadOptions.Numbers);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Check that padding contains either whitespace or digits
            string paddingPart = result.Substring(input.Length);
            Assert.All(paddingPart, c => Assert.True(char.IsDigit(c) || char.IsWhiteSpace(c)));
        }

        [Fact]
        public void PadChars_WithEmptyStringInput_PadsEntireResultWithSpecifiedCharacters()
        {
            // Arrange
            string input = "";
            int targetLength = 10;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Numbers);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.All(result, c => Assert.True(char.IsDigit(c)));
        }

        [Fact]
        public void PadChars_WithSingleCharacterInput_PreservesInputAndPadsRemainder()
        {
            // Arrange
            string input = "x";
            int targetLength = 15;

            // Act
            string result = input.PadChars(targetLength, PadOptions.LowerCaseAlphabets);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            Assert.Equal('x', result[0]);
            
            // Check that remaining characters are lowercase letters
            for (int i = 1; i < result.Length; i++)
            {
                Assert.True(char.IsLower(result[i]) && char.IsLetter(result[i]));
            }
        }

        [Fact]
        public void PadChars_WithZeroTargetLength_ReturnsOriginalString()
        {
            // Arrange
            string input = "test";
            int targetLength = 0;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Numbers);

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void PadChars_WithNegativeTargetLength_ReturnsOriginalString()
        {
            // Arrange
            string input = "negative";
            int targetLength = -5;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Numbers);

            // Assert
            Assert.Equal(input, result);
        }

        [Fact]
        public void PadChars_CallMultipleTimes_ProducesDifferentRandomResults()
        {
            // Arrange
            string input = "rand";
            int targetLength = 20;

            // Act
            string result1 = input.PadChars(targetLength, PadOptions.Numbers);
            string result2 = input.PadChars(targetLength, PadOptions.Numbers);
            string result3 = input.PadChars(targetLength, PadOptions.Numbers);

            // Assert
            Assert.Equal(targetLength, result1.Length);
            Assert.Equal(targetLength, result2.Length);
            Assert.Equal(targetLength, result3.Length);
            
            // All should start with the same input
            Assert.All(new[] { result1, result2, result3 }, r => Assert.StartsWith(input, r));
            
            // The padding portions should very likely be different due to randomness
            // Note: There's a tiny chance they could be the same, but it's astronomically small
            string padding1 = result1.Substring(input.Length);
            string padding2 = result2.Substring(input.Length);
            string padding3 = result3.Substring(input.Length);
            
            // At least one pair should be different
            bool anyDifferent = !padding1.Equals(padding2) || !padding2.Equals(padding3) || !padding1.Equals(padding3);
            Assert.True(anyDifferent, "Multiple calls should produce different random padding");
        }

        [Fact]
        public void PadChars_WithVeryLongTargetLength_HandlesLargePaddingSizes()
        {
            // Arrange
            string input = "big";
            int targetLength = 1000;

            // Act
            string result = input.PadChars(targetLength, PadOptions.Numbers);

            // Assert
            Assert.Equal(targetLength, result.Length);
            Assert.StartsWith(input, result);
            
            // Verify all padding characters are digits
            for (int i = input.Length; i < result.Length; i++)
            {
                Assert.True(char.IsDigit(result[i]));
            }
        }

        #endregion
    }
}
