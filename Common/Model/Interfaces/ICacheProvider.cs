using System;

namespace Common.Model.Interfaces
{
    public interface ICacheProvider
    {
        void AddToCache(string key, object value);

        void AddToCache(string key, object value, DateTime absoluteExpiration);

        void RemoveFromCache(string key);

        void ClearCache();

        T GetFromCache<T>(string key);

        bool IsExistInCache(string key);
    }
}
