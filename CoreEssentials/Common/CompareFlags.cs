using System;

namespace CoreEssentials.Common
{

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
        
        /// <summary>
        /// Specifies that the comparison should consider only alphabetic characters.
        /// </summary>
        CompareAlphabets = 1 << 2,

        /// <summary>
        /// Compares only the numeric parts of the strings.
        /// </summary>
        CompareNumbers = 1 << 3,
    }
}
