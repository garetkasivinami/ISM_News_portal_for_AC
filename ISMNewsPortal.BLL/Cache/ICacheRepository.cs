using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface ICacheRepository<T> where T:Model
    {
        T GetItem(int id);
        bool AddItem(T item);
        void Update(T item);
        void Delete(int id);
    }
}
