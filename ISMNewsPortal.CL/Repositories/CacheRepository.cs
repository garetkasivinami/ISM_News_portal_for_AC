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
            Type type = typeof(T);
            MemoryCache cache = MemoryCache.Default;
            return cache.Get($"{type.Name}_{id}") as T;
        }

        public bool AddItem<T>(T item) where T : Model
        {
            Type type = typeof(T);
            MemoryCache cache = MemoryCache.Default;
            return cache.Add($"{type.Name}_{item.Id}", item, DateTime.Now.AddMinutes(10));
        }

        public void Update<T>(T item) where T : Model
        {
            Type type = typeof(T);
            MemoryCache cache = MemoryCache.Default;
            cache.Set($"{type.Name}_{item.Id}", item, DateTime.Now.AddMinutes(10));
        }

        public void Delete<T>(int id) where T : Model
        {
            Type type = typeof(T);
            MemoryCache cache = MemoryCache.Default;
            if (cache.Contains(id.ToString()))
            {
                cache.Remove($"{type.Name}_{id}");
            }
        }
    }
}
