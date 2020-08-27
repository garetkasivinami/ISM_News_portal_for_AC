using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface ICacheRepository
    {
        T GetItem<T>(int id) where T : Model;
        bool AddItem<T>(T item) where T : Model;
        void Update<T>(T item) where T : Model;
        void Delete<T>(int id) where T : Model;
    }
}
