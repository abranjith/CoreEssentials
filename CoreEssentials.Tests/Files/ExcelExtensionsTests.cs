using System;
using CoreEssentials.Files;
using Xunit;

namespace CoreEssentials.Tests.Files
{
    public class ExcelExtensionsTests
    {
        #region ToExcelColumn Method Tests

        [Fact]
        public void ToExcelColumn_WithColumnNumber1_ReturnsA()
        {
            // Act
            string result = 1.ToExcelColumn();
            
            // Assert
            Assert.Equal("A", result);
        }

        [Fact]
        public void ToExcelColumn_WithColumnNumber26_ReturnsZ()
        {
            // Act
            string result = 26.ToExcelColumn();
            
            // Assert
            Assert.Equal("Z", result);
        }

        [Fact]
        public void ToExcelColumn_WithColumnNumber27_ReturnsAA()
        {
            // Act
            string result = 27.ToExcelColumn();
            
            // Assert
            Assert.Equal("AA", result);
        }

        [Fact]
        public void ToExcelColumn_WithColumnNumber52_ReturnsAZ()
        {
            // Act
            string result = 52.ToExcelColumn();
            
            // Assert
            Assert.Equal("AZ", result);
        }

        [Fact]
        public void ToExcelColumn_WithColumnNumber53_ReturnsBA()
        {
            // Act
            string result = 53.ToExcelColumn();
            
            // Assert
            Assert.Equal("BA", result);
        }

        [Fact]
        public void ToExcelColumn_WithColumnNumber702_ReturnsZZ()
        {
            // Act
            string result = 702.ToExcelColumn();
            
            // Assert
            Assert.Equal("ZZ", result);
        }

        [Fact]
        public void ToExcelColumn_WithColumnNumber703_ReturnsAAA()
        {
            // Act
            string result = 703.ToExcelColumn();
            
            // Assert
            Assert.Equal("AAA", result);
        }

        [Theory]
        [InlineData(1, "A")]
        [InlineData(2, "B")]
        [InlineData(3, "C")]
        [InlineData(25, "Y")]
        [InlineData(26, "Z")]
        [InlineData(27, "AA")]
        [InlineData(28, "AB")]
        [InlineData(51, "AY")]
        [InlineData(52, "AZ")]
        [InlineData(53, "BA")]
        [InlineData(54, "BB")]
        [InlineData(701, "ZY")]
        [InlineData(702, "ZZ")]
        [InlineData(703, "AAA")]
        [InlineData(704, "AAB")]
        public void ToExcelColumn_WithVariousNumbers_ReturnsCorrectColumnName(int columnNumber, string expected)
        {
            // Act
            string result = columnNumber.ToExcelColumn();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToExcelColumn_WithZero_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 0.ToExcelColumn());
        }

        [Fact]
        public void ToExcelColumn_WithNegativeNumber_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => (-1).ToExcelColumn());
        }

        #endregion

        #region FromExcelColumn Method Tests

        [Fact]
        public void FromExcelColumn_WithA_Returns1()
        {
            // Act
            int result = "A".FromExcelColumn();
            
            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void FromExcelColumn_WithZ_Returns26()
        {
            // Act
            int result = "Z".FromExcelColumn();
            
            // Assert
            Assert.Equal(26, result);
        }

        [Fact]
        public void FromExcelColumn_WithAA_Returns27()
        {
            // Act
            int result = "AA".FromExcelColumn();
            
            // Assert
            Assert.Equal(27, result);
        }

        [Fact]
        public void FromExcelColumn_WithAZ_Returns52()
        {
            // Act
            int result = "AZ".FromExcelColumn();
            
            // Assert
            Assert.Equal(52, result);
        }

        [Fact]
        public void FromExcelColumn_WithBA_Returns53()
        {
            // Act
            int result = "BA".FromExcelColumn();
            
            // Assert
            Assert.Equal(53, result);
        }

        [Fact]
        public void FromExcelColumn_WithZZ_Returns702()
        {
            // Act
            int result = "ZZ".FromExcelColumn();
            
            // Assert
            Assert.Equal(702, result);
        }

        [Fact]
        public void FromExcelColumn_WithAAA_Returns703()
        {
            // Act
            int result = "AAA".FromExcelColumn();
            
            // Assert
            Assert.Equal(703, result);
        }

        [Theory]
        [InlineData("A", 1)]
        [InlineData("B", 2)]
        [InlineData("C", 3)]
        [InlineData("Y", 25)]
        [InlineData("Z", 26)]
        [InlineData("AA", 27)]
        [InlineData("AB", 28)]
        [InlineData("AY", 51)]
        [InlineData("AZ", 52)]
        [InlineData("BA", 53)]
        [InlineData("BB", 54)]
        [InlineData("ZY", 701)]
        [InlineData("ZZ", 702)]
        [InlineData("AAA", 703)]
        [InlineData("AAB", 704)]
        [InlineData("ABC", 731)]
        [InlineData("XFD", 16384)] // Excel's maximum column
        public void FromExcelColumn_WithVariousColumnNames_ReturnsCorrectNumber(string columnName, int expected)
        {
            // Act
            int result = columnName.FromExcelColumn();
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void FromExcelColumn_WithLowercase_ReturnsCorrectNumber()
        {
            // Act
            int result = "abc".FromExcelColumn();
            
            // Assert
            Assert.Equal(731, result);
        }

        [Fact]
        public void FromExcelColumn_WithMixedCase_ReturnsCorrectNumber()
        {
            // Act
            int result = "aBc".FromExcelColumn();
            
            // Assert
            Assert.Equal(731, result);
        }

        [Fact]
        public void FromExcelColumn_WithNullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ((string)null!).FromExcelColumn());
        }

        [Fact]
        public void FromExcelColumn_WithEmptyString_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => "".FromExcelColumn());
            Assert.Contains("Column name cannot be empty", exception.Message);
        }

        [Fact]
        public void FromExcelColumn_WithInvalidCharacters_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => "A1".FromExcelColumn());
            Assert.Contains("contains invalid characters", exception.Message);
        }

        [Fact]
        public void FromExcelColumn_WithSpecialCharacters_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => "A@".FromExcelColumn());
            Assert.Contains("contains invalid characters", exception.Message);
        }

        [Fact]
        public void FromExcelColumn_WithSpaces_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => "A B".FromExcelColumn());
            Assert.Contains("contains invalid characters", exception.Message);
        }

        #endregion

        #region Round-trip Tests

        [Theory]
        [InlineData(1)]
        [InlineData(26)]
        [InlineData(27)]
        [InlineData(52)]
        [InlineData(53)]
        [InlineData(702)]
        [InlineData(703)]
        [InlineData(1000)]
        [InlineData(16384)] // Excel's maximum column
        public void RoundTrip_ToExcelColumnAndFromExcelColumn_ReturnsOriginalValue(int originalNumber)
        {
            // Act
            string columnName = originalNumber.ToExcelColumn();
            int resultNumber = columnName.FromExcelColumn();
            
            // Assert
            Assert.Equal(originalNumber, resultNumber);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("Z")]
        [InlineData("AA")]
        [InlineData("AZ")]
        [InlineData("BA")]
        [InlineData("ZZ")]
        [InlineData("AAA")]
        [InlineData("XFD")]
        public void RoundTrip_FromExcelColumnAndToExcelColumn_ReturnsOriginalValue(string originalColumnName)
        {
            // Act
            int columnNumber = originalColumnName.FromExcelColumn();
            string resultColumnName = columnNumber.ToExcelColumn();
            
            // Assert
            Assert.Equal(originalColumnName.ToUpperInvariant(), resultColumnName);
        }

        [Fact]
        public void RoundTrip_CaseInsensitive_WorksCorrectly()
        {
            // Arrange
            string lowerCase = "abc";
            string upperCase = "ABC";
            
            // Act
            int fromLower = lowerCase.FromExcelColumn();
            int fromUpper = upperCase.FromExcelColumn();
            string backToUpper = fromLower.ToExcelColumn();
            
            // Assert
            Assert.Equal(fromLower, fromUpper);
            Assert.Equal(upperCase, backToUpper);
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public void EdgeCase_LargeColumnNumber_WorksCorrectly()
        {
            // Arrange - Test with a large number that would create a long column name
            int largeNumber = 100000;
            
            // Act
            string columnName = largeNumber.ToExcelColumn();
            int backToNumber = columnName.FromExcelColumn();
            
            // Assert
            Assert.Equal(largeNumber, backToNumber);
            Assert.True(columnName.Length > 3); // Should be a multi-character column name
        }

        [Fact]
        public void EdgeCase_SingleLetterColumns_AllWork()
        {
            // Act & Assert - Test all single letter columns A-Z
            for (int i = 1; i <= 26; i++)
            {
                string columnName = i.ToExcelColumn();
                int backToNumber = columnName.FromExcelColumn();
                
                Assert.Equal(i, backToNumber);
                Assert.Single(columnName); // Should be exactly one character
                Assert.True(columnName[0] >= 'A' && columnName[0] <= 'Z');
            }
        }

        [Fact]
        public void EdgeCase_DoubleLetterColumns_AllWork()
        {
            // Act & Assert - Test double letter columns AA-AZ, BA-BZ, etc.
            for (int i = 27; i <= 52; i++)
            {
                string columnName = i.ToExcelColumn();
                int backToNumber = columnName.FromExcelColumn();
                
                Assert.Equal(i, backToNumber);
                Assert.Equal(2, columnName.Length); // Should be exactly two characters
            }
        }

        #endregion
    }
}