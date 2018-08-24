using System;
using System.Runtime.Caching;

namespace toys.Helpers
{
    public interface ICache
    {
        /// <summary>
        /// get data from cache. If not exist, fetch from data source
        /// </summary>
        /// <typeparam name="T">return data</typeparam>
        /// <param name="key">string of key to get or set</param>
        /// <param name="factory">callback used to get data if not exist</param>
        /// <param name="expireMinutes">expiration time, in minute</param>
        /// <returns></returns>
        T GetOrSet<T>(string key, Func<T> factory, int expireMinutes);

        /// <summary>
        /// remote cache by key
        /// </summary>
        /// <param name="key">string of key to remove</param>        
        void Remove(string key);
    }

    public class MemCache : ICache
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;

        /// <inheritdoc />
        /// <summary>
        /// get data from cache. If not exist, fetch from data source
        /// </summary>
        /// <typeparam name="T">return data</typeparam>
        /// <param name="key">string of key to get or set</param>
        /// <param name="factory">callback used to get data if not exist</param>
        /// <param name="expireMinutes">expiration time, in minute</param>
        /// <returns></returns>
        public T GetOrSet<T>(string key, Func<T> factory, int expireMinutes)
        {
            var newValue = new Lazy<T>(factory);
            var oldValue = Cache.AddOrGetExisting(key, newValue, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(expireMinutes) }) as Lazy<T>;

            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch (Exception) // Exception thrown each time if something going wrong during value reading
            {
                Cache.Remove(key);
                return default(T);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// remote cache by key
        /// </summary>
        /// <param name="key">string of key to remove</param>
        public void Remove(string key)
        {
            if (Cache.Contains(key))
                Cache.Remove(key);
        }
    }
}
