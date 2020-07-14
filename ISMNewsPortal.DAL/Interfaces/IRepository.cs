using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.DAL.Models;

namespace ISMNewsPortal.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        int Create(T item);
        void Update(T item);
        void Delete(int id);
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllWithTools(ToolBarModel toolBar);
        IEnumerable<T> Find(Func<T, bool> predicate);
        T FindSingle(Func<T, bool> predicate);
        U Max <U>(Func<T, U> predicate);
        bool Any(Func<T, bool> predicate);
        int Count();
        int Count(Func<T, bool> predicate);
    }
}
