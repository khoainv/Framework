using System.Collections.Generic;
using System;
using System.Runtime.Caching;
using SSD.Framework;
using System.Collections.Specialized;

namespace SSD.Web.Caching
{
    public class MemoryCacher:Singleton<MemoryCacher>
    {
        public object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }
        //2000000 objects => OutOfMemory
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

        public MemoryCache Cache
        {
            get
            {
                return MemoryCache.Default;
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
            {
                try
                {
                    cacheItems.Add(key, val);
                }
                //5999500 object => OutOfMemory
                catch (OutOfMemoryException ex)
                {
                    cacheItems.Clear();
                    cacheItems.Add(key, val);
                }
            }
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
    //public class MemoryCacher
    //{
    //    private Object Statlock = new object();
    //    private long size;
    //    private MemoryCache MemCache;
    //    private CacheItemPolicy CIPOL = new CacheItemPolicy();

    //    public MemoryCacher(double CacheSize)
    //    {
    //        NameValueCollection CacheSettings = new NameValueCollection(3);
    //        CacheSettings.Add("cacheMemoryLimitMegabytes", Convert.ToString(CacheSize));
    //        CacheSettings.Add("pollingInterval", Convert.ToString("00:00:01"));
    //        MemCache = new MemoryCache("TestCache", CacheSettings);
    //    }

    //    public void AddItem(string Name, string Value)
    //    {
    //        CacheItem CI = new CacheItem(Name, Value);
    //        MemCache.Add(CI, CIPOL);
    //    }
    //    public long Count
    //    {
    //        get { return MemCache.GetCount(); }
    //    }
    //}
}