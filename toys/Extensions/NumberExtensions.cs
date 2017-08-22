using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
