using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Lucene
{
    public interface ILuceneRepositoryFactory
    {
        ILuceneRepository<T> GetRepository<T>() where T : Model;
    }
}
