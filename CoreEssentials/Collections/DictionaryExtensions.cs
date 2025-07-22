using CoreEssentials.Common;
using CoreEssentials.Enumerable;
using System;
using System.Collections.Generic;


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
                if (kvp.Key != null && kvp.Key.Equals(key, compareFlags))
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
        /// <param name="compareFlags">The flags that specify how the comparison should be performed.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>The value associated with the specified key, or the default value if the key is not found.</returns>
        public static TValue GetValueOrDefault<TValue>(this IDictionary<string, TValue> dictionary, string key, CompareFlags compareFlags = CompareFlags.None, TValue defaultValue = default!)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return dictionary.TryGetValue(key, out TValue value, compareFlags) ? value : defaultValue;
        }

        /// <summary>
        /// Retrieves the value associated with the specified key from the dictionary,  or returns a default value if
        /// the key is not found.
        /// </summary>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search for the key. Cannot be null.</param>
        /// <param name="key">The key to locate in the dictionary.</param>
        /// <param name="keyProcessor">A function to process the key before searching the dictionary.</param>
        /// <param name="defaultValue">The value to return if the key is not found. Defaults to the default value of <typeparamref name="TValue"/>.</param>
        /// <returns>The value associated with the processed key if found; otherwise, <paramref name="defaultValue"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dictionary"/> is null.</exception>
        public static TValue GetValueOrDefault<TValue>(this IDictionary<string, TValue> dictionary, string key, Func<string, string> keyProcessor, TValue defaultValue = default!)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (key == null)
            {
                return defaultValue;
            }

            foreach (var kvp in dictionary)
            {
                if (kvp.Key != null && kvp.Key.Equals(key, keyProcessor!))
                {
                    return kvp.Value;
                }
            }

            return defaultValue;
        }
    }
}
