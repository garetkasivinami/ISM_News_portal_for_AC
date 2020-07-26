using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface IRepository<T> where T : Model
    {
        int Create(T item);
        void Update(T item);
        void Delete(int id);
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetWithOptions(Options toolBar);
        int Count();
    }
}
