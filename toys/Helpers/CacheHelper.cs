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
        /// <param name="expire">expiration time, in minute</param>
        /// <returns></returns>
        T GetOrSet<T>(string key, Func<T> factory, int expire);

        /// <summary>
        /// remote cache
        /// </summary>
        /// <param name="key">string of key to remove</param>        
        void Remove(string key);
    }

    public class MemCache : ICache
    {
        private static MemoryCache cache = MemoryCache.Default;

        public T GetOrSet<T>(string key, Func<T> factory, int expire)
        {
            var newValue = new Lazy<T>(factory);
            var oldValue = cache.AddOrGetExisting(key, newValue, policy: new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(expire) }) as Lazy<T>;

            try
            {
                return (oldValue ?? newValue).Value;
            }
            catch (Exception) // Exception thrown each time if something going wrong during value reading
            {
                cache.Remove(key);
                return default(T);
            }
        }

        public void Remove(string key)
        {
            if (cache.Contains(key))
                cache.Remove(key);
        }
    }
}
