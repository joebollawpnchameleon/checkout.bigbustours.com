
using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using Common.Model.Interfaces;

namespace Common.Model
{
    public class GenericHttpCacheProvider : ICacheProvider
    {
        public void AddToCache(string key, object value)
        {
            HttpContext.Current.Cache.Add(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        public void AddToCache(string key, object value, DateTime absoluteExpiration)
        {
            HttpContext.Current.Cache.Add(key, value, null, absoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
        }

        public void RemoveFromCache(string key)
        {
            if (HttpContext.Current.Cache[key] != null)
            {
                HttpContext.Current.Cache.Remove(key);
            }
        }

        public void ClearCache()
        {
            var enumerator = HttpContext.Current.Cache.GetEnumerator();

            while (enumerator.MoveNext())
            {
                HttpContext.Current.Cache.Remove(enumerator.Key.ToString());
            }
        }

        public T GetFromCache<T>(string key)
        {
            T data = default(T);
            if (IsExistInCache(key))
            {
                try
                {
                    data = (T)HttpContext.Current.Cache.Get(key);
                }
                catch (Exception ex)
                {
                    return data;
                }
            }
            return data;
        }

        public bool IsExistInCache(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }
    }
}
