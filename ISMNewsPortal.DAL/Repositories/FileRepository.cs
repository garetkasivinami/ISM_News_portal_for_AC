using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.BusinessModels;
using static ISMNewsPortal.BLL.Mappers.Automapper;

namespace ISMNewsPortal.DAL.Repositories
{
    public class FileRepository : Repository<FileModel>, IFileRepository
    {
        public FileRepository(ISession session) : base(session)
        {
        }

        public FileModel GetByHashCode(string hashCode)
        {
            return _session.Query<FileModel>().SingleOrDefault(u => u.HashCode == hashCode);
        }

        public FileModel GetByName(string name)
        {
            return _session.Query<FileModel>().SingleOrDefault(u => u.Name == name);
        }

        public int GetPostsCount(int fileId)
        {
            return _session.Query<NewsPost>().Count(u => u.ImageId == fileId);
        }
    }
}
