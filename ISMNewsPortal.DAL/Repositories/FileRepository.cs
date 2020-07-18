using ISMNewsPortal.DAL.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Interfaces;
using ISMNewsPortal.BLL.DTO;
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

        public int Create(FileDTO item)
        {
            var fileModel = MapFromFileDTO<FileModel>(item);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(fileModel);
                transaction.Commit();
                return fileModel.Id;
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

        public FileDTO Get(int id)
        {
            var fileModel = session.Get<FileModel>(id);
            return MapToFileDTO(fileModel);
        }

        public IEnumerable<FileDTO> GetAll()
        {
            var fileModels = session.Query<FileModel>();
            return MapToFileDTOList(fileModels);
        }

        public IEnumerable<FileDTO> GetAllWithTools(ToolsDTO toolBar)
        {
            throw new NotImplementedException();
        }

        public FileDTO GetByHashCode(string hashCode)
        {
            var fileModel = session.Query<FileModel>().SingleOrDefault(u => u.HashCode == hashCode);
            return MapToFileDTO(fileModel);
        }

        public FileDTO GetByName(string name)
        {
            var fileModel = session.Query<FileModel>().SingleOrDefault(u => u.Name == name);
            return MapToFileDTO(fileModel);
        }

        public int GetPostsCount(int fileId)
        {
            return session.Query<NewsPost>().Count(u => u.ImageId == fileId);
        }

        public void Update(FileDTO item)
        {
            var fileModel = MapFromFileDTO<FileModel>(item);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(fileModel);
                transaction.Commit();
            }
        }
    }
}
