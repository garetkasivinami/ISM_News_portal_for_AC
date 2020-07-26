using ISMNewsPortal.DAL.Models;
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
    public class FileRepository : IFileRepository
    {
        private ISession session;
        public FileRepository(ISession session)
        {
            this.session = session;
        }

        public int Count()
        {
            return session.Query<FileModel>().Count();
        }

        public int Create(FileModel item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(item);
                transaction.Commit();
                return item.Id;
            }
        }

        public void Delete(int id)
        {
            var fileModel = session.Get<FileModel>(id);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(fileModel);
                transaction.Commit();
            }
        }

        public FileModel Get(int id)
        {
            return session.Get<FileModel>(id);
        }

        public IEnumerable<FileModel> GetAll()
        {
            return session.Query<FileModel>();
        }

        public IEnumerable<FileModel> GetWithOptions(ToolsDTO toolBar)
        {
            return GetAll();
        }

        public FileModel GetByHashCode(string hashCode)
        {
            return session.Query<FileModel>().SingleOrDefault(u => u.HashCode == hashCode);
        }

        public FileModel GetByName(string name)
        {
            return session.Query<FileModel>().SingleOrDefault(u => u.Name == name);
        }

        public int GetPostsCount(int fileId)
        {
            return session.Query<NewsPost>().Count(u => u.ImageId == fileId);
        }

        public void Update(FileModel item)
        {
            var fileModel = item;
            var createdFileModel = session.Get<FileModel>(item.Id);
            Map(fileModel, createdFileModel);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(createdFileModel);
                transaction.Commit();
            }
        }
    }
}
