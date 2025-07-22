using CoreEssentials.Collections;
using CoreEssentials.Common;

namespace CoreEssentials.Tests.Collections
{
    public class DictionaryExtensionsTests
    {
        #region TryGetValue Tests

        [Fact]
        public void TryGetValue_WithExactKey_ReturnsTrueAndValue()
        {
            var dict = new Dictionary<string, int> { { "Key", 42 } };
            Assert.True(dict.TryGetValue("Key", out int value));
            Assert.Equal(42, value);
        }

        [Fact]
        public void TryGetValue_WithMissingKey_ReturnsFalseAndDefault()
        {
            var dict = new Dictionary<string, int> { { "Key", 42 } };
            Assert.False(dict.TryGetValue("Missing", out int value));
            Assert.Equal(default, value);
        }

        [Fact]
        public void TryGetValue_WithIgnoreCaseFlag_FindsKeyCaseInsensitive()
        {
            var dict = new Dictionary<string, int> { { "Key", 42 } };
            Assert.True(dict.TryGetValue("key", out int value, CompareFlags.IgnoreCase));
            Assert.Equal(42, value);
        }

        [Fact]
        public void TryGetValue_WithIgnoreWhitespaceFlag_FindsKeyIgnoringWhitespace()
        {
            var dict = new Dictionary<string, int> { { "My Key", 99 } };
            Assert.True(dict.TryGetValue("MyKey", out int value, CompareFlags.IgnoreWhitespace));
            Assert.Equal(99, value);
        }

        [Fact]
        public void TryGetValue_WithIgnoreCaseAndWhitespaceFlags_FindsKey()
        {
            var dict = new Dictionary<string, int> { { "My Key", 99 } };
            Assert.True(dict.TryGetValue("mykey", out int value, CompareFlags.IgnoreCase | CompareFlags.IgnoreWhitespace));
            Assert.Equal(99, value);
        }

        [Fact]
        public void TryGetValue_WithNullKey_ReturnsFalseAndDefault()
        {
            var dict = new Dictionary<string, int> { { "Key", 42 } };
            Assert.False(dict.TryGetValue(null!, out int value, CompareFlags.None));
            Assert.Equal(default, value);
        }

        [Fact]
        public void TryGetValue_WithNullDictionary_ThrowsArgumentNullException()
        {
            Dictionary<string, int> dict = null!;
            Assert.Throws<ArgumentNullException>(() => dict.TryGetValue("Key", out int _, CompareFlags.None));
        }

        #endregion

        #region GetValueOrDefault with CompareFlags Tests

        [Fact]
        public void GetValueOrDefault_WithExactKey_ReturnsValue()
        {
            var dict = new Dictionary<string, string> { { "A", "Alpha" } };
            Assert.Equal("Alpha", dict.GetValueOrDefault("A"));
        }

        [Fact]
        public void GetValueOrDefault_WithMissingKey_ReturnsDefaultValue()
        {
            var dict = new Dictionary<string, string> { { "A", "Alpha" } };
            Assert.Equal("Default", dict.GetValueOrDefault("B", CompareFlags.None, "Default"));
        }

        [Fact]
        public void GetValueOrDefault_WithIgnoreCaseFlag_ReturnsValue()
        {
            var dict = new Dictionary<string, string> { { "A", "Alpha" } };
            Assert.Equal("Alpha", dict.GetValueOrDefault("a", CompareFlags.IgnoreCase));
        }

        [Fact]
        public void GetValueOrDefault_WithIgnoreWhitespaceFlag_ReturnsValue()
        {
            var dict = new Dictionary<string, string> { { "A B", "AB" } };
            Assert.Equal("AB", dict.GetValueOrDefault("AB", CompareFlags.IgnoreWhitespace));
        }

        [Fact]
        public void GetValueOrDefault_WithNullKey_ReturnsDefaultValue()
        {
            var dict = new Dictionary<string, string> { { "A", "Alpha" } };
            Assert.Equal("Default", dict.GetValueOrDefault(null, CompareFlags.None, "Default"));
        }

        [Fact]
        public void GetValueOrDefault_WithNullDictionary_ThrowsArgumentNullException()
        {
            Dictionary<string, string> dict = null!;
            Assert.Throws<ArgumentNullException>(() => dict.GetValueOrDefault("A"));
        }

        #endregion

        #region GetValueOrDefault with keyProcessor Tests

        [Fact]
        public void GetValueOrDefault_WithKeyProcessor_NormalizesKeyAndFindsValue()
        {
            var dict = new Dictionary<string, string> { { "abc123", "Found" } };
            string key = "ABC-123";
            Func<string, string> processor = s => s.Replace("-", "").ToLowerInvariant();
            Assert.Equal("Found", dict.GetValueOrDefault(key, processor));
        }

        [Fact]
        public void GetValueOrDefault_WithKeyProcessor_MissingKey_ReturnsDefaultValue()
        {
            var dict = new Dictionary<string, string> { { "abc", "Found" } };
            Func<string, string> processor = s => s.ToUpperInvariant();
            Assert.Equal("Default", dict.GetValueOrDefault("def", processor, "Default"));
        }

        [Fact]
        public void GetValueOrDefault_WithKeyProcessor_NullKey_ReturnsDefaultValue()
        {
            var dict = new Dictionary<string, string> { { "abc", "Found" } };
            Func<string, string> processor = s => s.ToUpperInvariant();
            Assert.Equal("Default", dict.GetValueOrDefault(null, processor, "Default"));
        }

        [Fact]
        public void GetValueOrDefault_WithKeyProcessor_NullDictionary_ThrowsArgumentNullException()
        {
            Dictionary<string, string> dict = null!;
            Func<string, string> processor = s => s;
            Assert.Throws<ArgumentNullException>(() => dict.GetValueOrDefault("abc", processor));
        }

        #endregion
    }
}
