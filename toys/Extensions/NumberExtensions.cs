using System.Linq;

namespace toys.Extensions
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Determine a string is number or not
        /// </summary>
        /// <param name="s">The string to check</param>
        /// <returns>True if is a number. Otherwise return false</returns>
        public static bool IsNumber(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            return s.Length > 0 && s.All(char.IsDigit);
        }

        /// <summary>
        /// try parse string to number, if fail return default value
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int ToNumber(this string s, int defaultValue = 0)
        {
            if (string.IsNullOrWhiteSpace(s))
                return defaultValue;

            return int.TryParse(s, out var number) ? number : defaultValue;
        }

        /// <summary>
        /// try parse string to number, if fail return null
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static int? ToNullableNumber(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;

            if (int.TryParse(s, out var number))
                return number;

            return null;
        }
    }
}
