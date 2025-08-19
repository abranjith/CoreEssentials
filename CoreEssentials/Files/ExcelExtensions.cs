using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEssentials.Files
{
    public static class ExcelExtensions
    {
        /// <summary>
        /// Converts an integer to Excel column naming convention (1 = A, 2 = B, ..., 26 = Z, 27 = AA, etc.).
        /// </summary>
        /// <param name="columnNumber">The column number to convert (must be greater than 0).</param>
        /// <returns>A string representing the Excel column name in uppercase.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when columnNumber is less than or equal to 0.</exception>
        public static string ToExcelColumn(this int columnNumber)
        {
            if (columnNumber <= 0)
                throw new ArgumentOutOfRangeException(nameof(columnNumber), "Column number must be greater than 0.");

            var result = new StringBuilder();
            
            while (columnNumber > 0)
            {
                columnNumber--; // Convert to 0-based indexing
                result.Insert(0, (char)('A' + (columnNumber % 26)));
                columnNumber /= 26;
            }
            
            return result.ToString();
        }

        /// <summary>
        /// Converts an Excel column name to its corresponding integer column number (A = 1, B = 2, ..., Z = 26, AA = 27, etc.).
        /// </summary>
        /// <param name="columnName">The Excel column name to convert (case-insensitive, must contain only A-Z letters).</param>
        /// <returns>An integer representing the column number.</returns>
        /// <exception cref="ArgumentNullException">Thrown when columnName is null.</exception>
        /// <exception cref="ArgumentException">Thrown when columnName is empty or contains invalid characters.</exception>
        public static int FromExcelColumn(this string columnName)
        {
            if (columnName == null)
                throw new ArgumentNullException(nameof(columnName));
            
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException("Column name cannot be empty.", nameof(columnName));

            var upperColumnName = columnName.ToUpperInvariant();
            
            // Validate that all characters are A-Z
            foreach (char c in upperColumnName)
            {
                if (c < 'A' || c > 'Z')
                    throw new ArgumentException($"Column name '{columnName}' contains invalid characters. Only A-Z letters are allowed.", nameof(columnName));
            }

            int result = 0;
            for (int i = 0; i < upperColumnName.Length; i++)
            {
                result = result * 26 + (upperColumnName[i] - 'A' + 1);
            }
            
            return result;
        }
    }
}
