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
        T GetItem<T>(string key) where T : Model;
        IEnumerable<T> GetItems<T>(string key) where T : Model;
        bool Add<T>(T item, string key) where T : Model;
        void Update<T>(T item, string key) where T : Model;
        void Delete(string key);
        void DeleteByPartOfTheKey(string key);
        bool Add<T>(IEnumerable<T> items, string key) where T : Model;
        void Update<T>(IEnumerable<T> items, string key) where T : Model;
    }
}
