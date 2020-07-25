using ISMNewsPortal.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Interfaces
{
    public interface IFileRepository : IRepository<FileModel>
    {
        FileModel GetByName(string name);
        FileModel GetByHashCode(string hashCode);
        int GetPostsCount(int fileId);
    }
}
