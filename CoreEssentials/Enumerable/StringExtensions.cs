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

        //TODO simplify logic here
        private static string? ProcessStringWithFlags(string? input, CompareFlags compareFlags)
        {
            if(input == null)
                return null;

            string result = input;
            var sb = new StringBuilder();
            foreach (char c in result)
            {
                if (compareFlags.HasFlag(CompareFlags.IgnoreWhitespace))
                {
                    if (char.IsWhiteSpace(c)) continue;
                }
                if (compareFlags.HasFlag(CompareFlags.CompareAlphabets))
                {
                    if (compareFlags.HasFlag(CompareFlags.CompareNumbers))
                    {
                        if (!char.IsDigit(c) && !c.IsEnglishLetter()) continue;
                    }
                    else
                    {
                        if (!c.IsEnglishLetter()) continue;
                    }
                }
                if (compareFlags.HasFlag(CompareFlags.CompareNumbers) && !compareFlags.HasFlag(CompareFlags.CompareAlphabets))
                {
                    if (!char.IsDigit(c)) continue;
                }
                sb.Append(c);
            }
            result = sb.ToString();

            return result;
        }

        /// <summary>
        /// Convert a string to PascalCase.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            var sb = new StringBuilder();
            bool capitalizeNext = true;
            foreach (char c in str)
            {
                if (char.IsWhiteSpace(c) || c == '_' || c == '-')
                {
                    capitalizeNext = true;
                }
                else
                {
                    if (capitalizeNext)
                    {
                        sb.Append(char.ToUpper(c));
                        capitalizeNext = false;
                    }
                    else
                    {
                        sb.Append(char.ToLower(c));
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Convert a string to camelCase.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            var sb = new StringBuilder();
            bool capitalizeNext = true;
            foreach (char c in str)
            {
                if (char.IsWhiteSpace(c) || c == '_' || c == '-')
                {
                    capitalizeNext = true;
                }
                else
                {
                    if (capitalizeNext)
                    {
                        var c2 = sb.Length == 0 ? char.ToLower(c) : char.ToUpper(c);
                        sb.Append(c2);
                        capitalizeNext = false;
                    }
                    else
                    {
                        sb.Append(char.ToLower(c));
                    }
                }
            }
            return sb.ToString();
        }

        #region :: Helper Methods ::

        private static bool IsEnglishLetter(this char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        #endregion
    }
}
