using System;

namespace CoreEssentials.Common
{
    [Flags]
    public enum PadOptions
    {
        None = 0,
        Numbers = 1 << 0,
        LowerCaseAlphabets = 1 << 1,
        UpperCaseAlphabets = 1 << 2,
        Whitespace = 1 << 3,
        SpecialCharacters = 1 << 4,
    }
}
