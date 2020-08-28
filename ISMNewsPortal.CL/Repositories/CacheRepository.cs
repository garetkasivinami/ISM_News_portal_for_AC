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
        public T GetItem<T>(int id) where T : Model
        {
            MemoryCache cache = MemoryCache.Default;
            Type type = typeof(T);
            return cache.Get(GetNameOfItem(type, id)) as T;
        }

        public bool AddItem<T>(T item) where T : Model
        {
            MemoryCache cache = MemoryCache.Default;
            return cache.Add(GetNameOfItem(item), item, DateTime.Now.AddMinutes(10));
        }

        public void Update<T>(T item) where T : Model
        {
            MemoryCache cache = MemoryCache.Default;
            cache.Set(GetNameOfItem(item), item, DateTime.Now.AddMinutes(10));
        }

        public void Delete<T>(int id) where T : Model
        {
            MemoryCache cache = MemoryCache.Default;
            Type type = typeof(T);
            string key = GetNameOfItem(type, id);
            if (cache.Contains(key))
                cache.Remove(key);
        }

        private string GetNameOfItem(Model item)
        {
            Type type = item.GetType();
            return GetNameOfItem(type, item.Id);
        }
        private string GetNameOfItem(Type type, int id)
        {
            return $"{type.Name}_{id}";
        }
    }
}
