using CoreEssentials.Common;
using System;
using System.Text;

namespace CoreEssentials.Enumerable
{
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether two specified strings are equal, using the specified comparison options.
        /// </summary>
        /// <remarks>The comparison is influenced by the <paramref name="compareFlags"/> parameter, which
        /// can include options such as ignoring case. If <paramref name="str2"/> is <see langword="null"/>, the method
        /// returns <see langword="false"/> unless <paramref name="str1"/> is also <see langword="null"/>.</remarks>
        /// <param name="str1">The first string to compare. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="str2">The second string to compare. This parameter can be <see langword="null"/>.</param>
        /// <param name="compareFlags">A set of flags that specify the comparison options, such as case sensitivity.</param>
        /// <returns><see langword="true"/> if the strings are considered equal based on the specified comparison options;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool Equals(this string? str1, string? str2, CompareFlags compareFlags)
        {
            if (str1 == null && str2 == null)
                return true;
            if (str1 == null || str2 == null)
                return false;
            var processedStr1 = ProcessStringWithFlags(str1, compareFlags);
            var processedStr2 = ProcessStringWithFlags(str2, compareFlags);
            
            return (processedStr1?.Equals(processedStr2,
                compareFlags.HasFlag(CompareFlags.IgnoreCase) ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture) == true);
        }

        /// <summary>
        /// Determines whether two specified strings are equal after processing them with a given function.
        /// </summary>
        /// <remarks>This method applies the <paramref name="strProcessor"/> function to both strings
        /// before comparing them.  It is useful for scenarios where strings need to be normalized or transformed in a
        /// specific way before comparison.</remarks>
        /// <param name="str1">The first string to compare.</param>
        /// <param name="str2">The second string to compare, which can be null.</param>
        /// <param name="strProcessor">A function that processes each string before comparison. This function must handle null values
        /// appropriately.</param>
        /// <returns><see langword="true"/> if the processed strings are equal; otherwise, <see langword="false"/>.</returns>
        public static bool Equals(this string? str1, string? str2, Func<string?,string?> strProcessor)
        {
            if (str1 == null && str2 == null)
                return true;
            if (str1 == null || str2 == null)
                return false;

            var processedStr1 = strProcessor(str1);
            var processedStr2 = strProcessor(str2);

            return (processedStr1?.Equals(processedStr2) == true);
        }

        private static string? ProcessStringWithFlags(string? input, CompareFlags compareFlags)
        {
            if(input == null)
                return null;

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
}
