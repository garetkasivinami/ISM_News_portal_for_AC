using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.Lucene.Repository
{
    public interface ILuceneRepository<T> where T : Model
    {
        string Directory { get; }
        void SaveOrUpdate(IEnumerable<T> items);
        void SaveOrUpdate(T item);
        void Delete(IEnumerable<T> items);
        void Delete(T item);
        bool DeleteAll();
        void Optimize();
    }
}
