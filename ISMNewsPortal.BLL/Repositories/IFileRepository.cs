using ISMNewsPortal.BLL.Models;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface IFileRepository : IRepository<FileModel>
    {
        FileModel GetByName(string name);
        FileModel GetByHashCode(string hashCode);
        int GetPostsCount(int fileId);
    }
}
