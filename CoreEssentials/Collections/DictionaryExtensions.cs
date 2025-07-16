using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEssentials.Collections
{
    public static class DictionaryExtensions
    {

        /// <summary>
        /// Attempts to get the value associated with the specified key, using the specified comparison flags.
        /// </summary>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <param name="compareFlags">The flags that specify how the comparison should be performed.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public static bool TryGetValue<TValue>(this IDictionary<string, TValue> dictionary, string key, out TValue value, CompareFlags compareFlags = CompareFlags.None)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (key == null)
            {
                value = default!;
                return false;
            }

            // If no flags are specified, use the default TryGetValue
            if (compareFlags == CompareFlags.None)
                return dictionary.TryGetValue(key, out value);

            foreach (var kvp in dictionary)
            {
                if (kvp.Key != null && StringEquals(kvp.Key, key, compareFlags))
                {
                    value = kvp.Value;
                    return true;
                }
            }

            value = default!;
            return false;
        }

        /// <summary>
        /// Gets the value associated with the specified key, or returns a default value if the key is not found.
        /// </summary>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="key">The key to locate.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <param name="compareFlags">The flags that specify how the comparison should be performed.</param>
        /// <returns>The value associated with the specified key, or the default value if the key is not found.</returns>
        public static TValue GetValueOrDefault<TValue>(this IDictionary<string, TValue> dictionary, string key, TValue defaultValue = default!, CompareFlags compareFlags = CompareFlags.None)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return dictionary.TryGetValue(key, out TValue value, compareFlags) ? value : defaultValue;
        }

        private static bool StringEquals(string str1, string str2, CompareFlags compareFlags)
        {
            string processedStr1 = ProcessStringWithFlags(str1, compareFlags);
            string processedStr2 = ProcessStringWithFlags(str2, compareFlags);

            return processedStr1.Equals(processedStr2, 
                compareFlags.HasFlag(CompareFlags.IgnoreCase) ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
        }

        private static string ProcessStringWithFlags(string input, CompareFlags compareFlags)
        {
            string result = input;

            if (compareFlags.HasFlag(CompareFlags.IgnoreWhitespace))
            {
                var sb = new StringBuilder();
                foreach (char c in result)
                {
                    if (!char.IsWhiteSpace(c))
                        sb.Append(c);
                }
                result = sb.ToString();
            }

            return result;
        }
    }

    [Flags]
    public enum CompareFlags
    {
        /// <summary>
        /// No additional flags.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Specifies that the comparison should ignore case differences (current culture).
        /// </summary>
        IgnoreCase = 1 << 0,

        /// <summary>
        /// Specifies that the comparison should ignore any whit space differences.
        /// </summary>
        IgnoreWhitespace = 1 << 1,
    }
}
