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
    public class CacheNewsPostRepository : ICacheRepository<NewsPost>
    {
        public NewsPost GetItem(int id)
        {
            MemoryCache cache = MemoryCache.Default;
            return cache.Get(id.ToString()) as NewsPost;
        }

        public bool AddItem(NewsPost item)
        {
            MemoryCache cache = MemoryCache.Default;
            return cache.Add(item.Id.ToString(), item, DateTime.Now.AddMinutes(10));
        }

        public void Update(NewsPost item)
        {
            MemoryCache cache = MemoryCache.Default;
            cache.Set(item.Id.ToString(), item, DateTime.Now.AddMinutes(10));
        }

        public void Delete(int id)
        {
            MemoryCache cache = MemoryCache.Default;
            if (cache.Contains(id.ToString()))
            {
                cache.Remove(id.ToString());
            }
        }
    }
}
