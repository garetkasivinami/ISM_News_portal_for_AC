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

        public CacheRepository()
        {
            memoryCache = MemoryCache.Default;
        }

        public T GetItem<T>(int id) where T : Model
        {
            Type type = typeof(T);
            return memoryCache.Get(GetNameOfItem(type, id)) as T;
        }

        public bool AddItem<T>(T item) where T : Model
        {
            return memoryCache.Add(GetNameOfItem(item), item, DateTime.Now.AddMinutes(10));
        }

        public void Update<T>(T item) where T : Model
        {
            memoryCache.Set(GetNameOfItem(item), item, DateTime.Now.AddMinutes(10));
        }

        public void Delete<T>(int id) where T : Model
        {
            Type type = typeof(T);
            string key = GetNameOfItem(type, id);
            if (memoryCache.Contains(key))
                memoryCache.Remove(key);
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
