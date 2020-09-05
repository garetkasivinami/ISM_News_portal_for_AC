using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.CL.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private MemoryCache memoryCache;
        private int minutes;

        public CacheRepository(int minutes)
        {
            memoryCache = MemoryCache.Default;
            this.minutes = minutes;
        }

        public T GetItem<T>(string key) where T : Model
        {
            return memoryCache[key] as T;
        }

        public IEnumerable<T> GetItems<T>(string key) where T : Model
        {
            return memoryCache[key] as IEnumerable<T>;
        }

        public bool Add<T>(T item, string key) where T : Model
        {
            return memoryCache.Add(key, item, DateTime.Now.AddMinutes(minutes));
        }

        public void Update<T>(T item, string key) where T : Model
        {
            memoryCache.Set(key, item, DateTime.Now.AddMinutes(minutes));
        }

        public void Delete(string key)
        {
            memoryCache.Remove(key);
        }

        public bool Add<T>(IEnumerable<T> items, string key) where T : Model
        {
            return memoryCache.Add(key, items, DateTime.Now.AddMinutes(minutes));
        }

        public void Update<T>(IEnumerable<T> items, string key) where T : Model
        {
            memoryCache.Set(key, items, DateTime.Now.AddMinutes(minutes));
        }

        public void DeleteByPartOfTheKey(string partOfKey)
        {
            var keys = memoryCache.Where(u => u.Key.Contains(partOfKey)).Select(u => u.Key);
            foreach (string key in keys)
                Delete(key);
        }
    }
}
