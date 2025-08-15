using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreEssentials.Enumerable;
using Xunit;

namespace CoreEssentials.Tests.Enumerable
{
    public class EnumerableExtensionsTests
    {
        #region Generic GetElementType Tests

        [Fact]
        public void GetElementType_WithStringList_ReturnsStringType()
        {
            // Arrange
            var list = new List<string> { "test1", "test2" };

            // Act
            var result = list.GetElementType();

            // Assert
            Assert.Equal(typeof(string), result);
        }

        [Fact]
        public void GetElementType_WithIntArray_ReturnsIntType()
        {
            // Arrange
            var array = new int[] { 1, 2, 3 };

            // Act
            var result = array.GetElementType();

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetElementType_WithCustomObjectEnumerable_ReturnsCustomType()
        {
            // Arrange
            var customObjects = new List<DateTime> { DateTime.Now };

            // Act
            var result = customObjects.GetElementType();

            // Assert
            Assert.Equal(typeof(DateTime), result);
        }

        [Fact]
        public void GetElementType_WithEmptyEnumerable_ReturnsCorrectType()
        {
            // Arrange
            var emptyList = new List<decimal>();

            // Act
            var result = emptyList.GetElementType();

            // Assert
            Assert.Equal(typeof(decimal), result);
        }

        [Fact]
        public void GetElementType_WithLinqQueryable_ReturnsCorrectType()
        {
            // Arrange
            var query = new[] { 1, 2, 3 }.Where(x => x > 0);

            // Act
            var result = query.GetElementType();

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetElementType_WithNullableType_ReturnsNullableType()
        {
            // Arrange
            var nullableList = new List<int?> { 1, null, 3 };

            // Act
            var result = nullableList.GetElementType();

            // Assert
            Assert.Equal(typeof(int?), result);
        }

        #endregion

        #region Object GetElementType Tests

        [Fact]
        public void GetElementType_WithStringArrayObject_ReturnsStringType()
        {
            // Arrange
            object stringArray = new string[] { "a", "b", "c" };

            // Act
            var result = stringArray.GetElementType();

            // Assert
            Assert.Equal(typeof(string), result);
        }

        [Fact]
        public void GetElementType_WithIntArrayObject_ReturnsIntType()
        {
            // Arrange
            object intArray = new int[] { 1, 2, 3 };

            // Act
            var result = intArray.GetElementType();

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetElementType_WithMultidimensionalArray_ReturnsElementType()
        {
            // Arrange
            object multiArray = new int[2, 3];

            // Act
            var result = multiArray.GetElementType();

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetElementType_WithListObject_ReturnsElementType()
        {
            // Arrange
            object list = new List<double> { 1.0, 2.0, 3.0 };

            // Act
            var result = list.GetElementType();

            // Assert
            Assert.Equal(typeof(double), result);
        }

        [Fact]
        public void GetElementType_WithHashSetObject_ReturnsElementType()
        {
            // Arrange
            object hashSet = new HashSet<char> { 'a', 'b', 'c' };

            // Act
            var result = hashSet.GetElementType();

            // Assert
            Assert.Equal(typeof(char), result);
        }

        [Fact]
        public void GetElementType_WithLinkedListObject_ReturnsElementType()
        {
            // Arrange
            var linkedListTyped = new LinkedList<bool>();
            linkedListTyped.AddLast(true);
            object linkedList = linkedListTyped;
            
            // Act
            var result = linkedList.GetElementType();
            
            // Assert
            Assert.Equal(typeof(bool), result);
        }

        [Fact]
        public void GetElementType_WithQueueObject_ReturnsElementType()
        {
            // Arrange
            object queue = new Queue<DateTime>();

            // Act
            var result = queue.GetElementType();

            // Assert
            Assert.Equal(typeof(DateTime), result);
        }

        [Fact]
        public void GetElementType_WithStackObject_ReturnsElementType()
        {
            // Arrange
            object stack = new Stack<Guid>();

            // Act
            var result = stack.GetElementType();

            // Assert
            Assert.Equal(typeof(Guid), result);
        }

        [Fact]
        public void GetElementType_WithDictionaryObject_ReturnsKeyValuePairType()
        {
            // Arrange
            object dictionary = new Dictionary<string, int> { { "key", 42 } };

            // Act
            var result = dictionary.GetElementType();

            // Assert
            Assert.Equal(typeof(KeyValuePair<string, int>), result);
        }

        [Fact]
        public void GetElementType_WithArrayListObject_ReturnsObjectType()
        {
            // Arrange
            object arrayList = new ArrayList { "string", 123, true };

            // Act
            var result = arrayList.GetElementType();

            // Assert
            Assert.Equal(typeof(object), result);
        }

        [Fact]
        public void GetElementType_WithNullObject_ReturnsNull()
        {
            // Arrange
            object nullObject = null!;

            // Act
            var result = nullObject.GetElementType();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetElementType_WithNonEnumerableObject_ReturnsNull()
        {
            // Arrange
            object nonEnumerable = new object();

            // Act
            var result = nonEnumerable.GetElementType();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetElementType_WithStringObject_ReturnsCharType()
        {
            // Arrange
            object stringObj = "hello";
            
            // Act
            var result = stringObj.GetElementType();
            
            // Assert
            Assert.Equal(typeof(char), result);
        }

        [Fact]
        public void GetElementType_WithIntObject_ReturnsNull()
        {
            // Arrange
            object intObj = 42;

            // Act
            var result = intObj.GetElementType();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetElementType_WithCustomNonEnumerableClass_ReturnsNull()
        {
            // Arrange
            object customObj = new CustomNonEnumerableClass();

            // Act
            var result = customObj.GetElementType();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetElementType_WithCustomEnumerableClass_ReturnsElementType()
        {
            // Arrange
            object customEnumerable = new CustomEnumerableClass();

            // Act
            var result = customEnumerable.GetElementType();

            // Assert
            Assert.Equal(typeof(string), result);
        }

        [Fact]
        public void GetElementType_WithEmptyArray_ReturnsElementType()
        {
            // Arrange
            object emptyArray = new float[0];

            // Act
            var result = emptyArray.GetElementType();

            // Assert
            Assert.Equal(typeof(float), result);
        }

        [Fact]
        public void GetElementType_WithJaggedArray_ReturnsArrayElementType()
        {
            // Arrange
            object jaggedArray = new int[3][];

            // Act
            var result = jaggedArray.GetElementType();

            // Assert
            Assert.Equal(typeof(int[]), result);
        }

        [Fact]
        public void GetElementType_WithEnumerableRange_ReturnsIntType()
        {
            // Arrange
            object enumerableRange = System.Linq.Enumerable.Range(1, 5);

            // Act
            var result = enumerableRange.GetElementType();

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetElementType_WithEnumerableRepeat_ReturnsElementType()
        {
            // Arrange
            object enumerableRepeat = System.Linq.Enumerable.Repeat("test", 3);

            // Act
            var result = enumerableRepeat.GetElementType();

            // Assert
            Assert.Equal(typeof(string), result);
        }

        #endregion

        #region Helper Classes for Testing

        private class CustomNonEnumerableClass
        {
            public int Value { get; set; } = 42;
        }

        private class CustomEnumerableClass : IEnumerable<string>
        {
            private readonly List<string> _items = new List<string> { "item1", "item2" };

            public IEnumerator<string> GetEnumerator() => _items.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        #endregion
    }
}
