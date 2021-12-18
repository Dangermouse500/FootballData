using System;
using System.Runtime.Caching;

namespace FootballData.Helpers
{    
    public static class DataInMemoryCache
    {
        private static readonly MemoryCache _cache = MemoryCache.Default;

        public static object GetFromCache(string cacheKey)
        {
            if (_cache.Contains(cacheKey))
            {
                return _cache.Get(cacheKey);
            }

            return null;
        }

        public static void AddToCache(string cacheKey, object objects)
        {
            var cacheItemPolicy = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.Now.AddDays(1)
            };
            _cache.Add(cacheKey, objects, cacheItemPolicy);
        }
    }
}