using System.Collections.Generic;
using System;
using System.Runtime.Caching;

namespace SSD.SSO.Caching
{
    public class MemoryCacher
    {
        public object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }

        public bool Add(string key, object value, DateTimeOffset absExpiration)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, absExpiration);
        }

        public void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
    }
    public static class CacheUser
    {
        private static Dictionary<string, object> cacheItems = new Dictionary<string, object>();
        public static IEnumerable<KeyValuePair<string, object>> CacheItems
        {
            get
            { // we are not exposing the raw dictionary now
                foreach (var item in cacheItems) yield return item;
            }
        }
        public static TValue Get<TValue>(string key) where TValue : new()
        {
            return (TValue)cacheItems[key];
        }
        public static bool Contain(string key)
        {
            return cacheItems.ContainsKey(key);
        }
        public static void Set<TValue>(string key, TValue val)
        {
            if (!Contain(key))
                cacheItems.Add(key, val);
            else cacheItems[key] = val;
        }
        public static void Flush()
        {
            cacheItems.Clear();
        }
        public static void Remove(string key)
        {
            if (cacheItems.ContainsKey(key))
                cacheItems.Remove(key);
        }
    }
}