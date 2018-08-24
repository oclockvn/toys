using System;
using System.Collections.Generic;
using System.Linq;

namespace toys.Extensions
{
    public static class QueryExtensions
    {
        /// <summary>
        /// Distincts the ex.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="this">The this.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctEx<T, TKey>(this IEnumerable<T> @this, Func<T, TKey> keySelector)
        {
            return @this.GroupBy(keySelector).Select(grps => grps).Select(e => e.First());
        }
    }
}
