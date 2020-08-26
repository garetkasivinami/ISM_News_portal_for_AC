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
    public class CacheRepository<T> : ICacheRepository<T> where T : Model
    {
        public T GetItem(int id, Type type)
        {
            MemoryCache cache = MemoryCache.Default;
            return cache.Get($"{type.Name}_{id}") as T;
        }

        public bool AddItem(T item)
        {
            Type type = typeof(T);
            MemoryCache cache = MemoryCache.Default;
            return cache.Add($"{type.Name}_{item.Id}", item, DateTime.Now.AddMinutes(10));
        }

        public void Update(T item)
        {
            Type type = typeof(T);
            MemoryCache cache = MemoryCache.Default;
            cache.Set($"{type.Name}_{item.Id}", item, DateTime.Now.AddMinutes(10));
        }

        public void Delete(int id, Type type)
        {
            MemoryCache cache = MemoryCache.Default;
            if (cache.Contains(id.ToString()))
            {
                cache.Remove($"{type.Name}_{id}");
            }
        }
    }
}
