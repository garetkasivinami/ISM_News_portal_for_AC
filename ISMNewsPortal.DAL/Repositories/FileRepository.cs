using NHibernate;
using System.Linq;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.BLL.Models;

namespace ISMNewsPortal.DAL.Repositories
{
    public class FileRepository : Repository<FileModel>, IFileRepository
    {
        public FileRepository() : base()
        {
        }

        public FileModel GetByHashCode(string hashCode)
        {
            return hibernateUnitOfWork.Session.Query<FileModel>().SingleOrDefault(u => u.HashCode == hashCode);
        }

        public FileModel GetByName(string name)
        {
            return hibernateUnitOfWork.Session.Query<FileModel>().SingleOrDefault(u => u.Name == name);
        }

        public int GetPostsCount(int fileId)
        {
            return hibernateUnitOfWork.Session.Query<NewsPost>().Count(u => u.ImageId == fileId);
        }
    }
}
