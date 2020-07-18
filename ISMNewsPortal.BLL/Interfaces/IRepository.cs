using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.BLL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        int Create(T item);
        void Update(T item);
        void Delete(int id);
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllWithTools(ToolsDTO toolBar);
        int Count();
    }
}
