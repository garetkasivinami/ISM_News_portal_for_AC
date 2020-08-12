using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using System.Linq;

namespace ISMNewsPortal.DAL_XML.Repositories
{
    public class FileRepository : Repository<FileModel>, IFileRepository
    {
        public FileRepository(XMLContex contex) : base(contex)
        {

        }

        public FileModel GetByHashCode(string hashCode)
        {
            return GetAll().SingleOrDefault(u => u.HashCode == hashCode);
        }

        public FileModel GetByName(string name)
        {
            return GetAll().SingleOrDefault(u => u.Name == name);
        }

        public int GetPostsCount(int fileId)
        {
            return contex.GetAll<NewsPost>().Where(u => u.ImageId == fileId).Count();
        }
    }
}
