using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface IFileRepository : IRepository<FileModel>
    {
        FileModel GetByName(string name);
        FileModel GetByHashCode(string hashCode);
        int GetPostsCount(int fileId);
    }
}
