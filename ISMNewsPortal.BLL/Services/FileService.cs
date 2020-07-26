using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Exceptions;

namespace ISMNewsPortal.BLL.Services
{
    public class FileService
    {
        public IEnumerable<FileModel> GetFiles()
        {
            return UnitOfWorkManager.UnitOfWork.Files.GetAll();
        }

        public FileModel GetFile(int id)
        {
            var file = UnitOfWorkManager.UnitOfWork.Files.Get(id);
            if (file == null)
                throw new FileNullException();
            return file;
        }


        public FileModel FindByHashCode(string hashCode)
        {
            var file = UnitOfWorkManager.UnitOfWork.Files.GetByHashCode(hashCode);
            return file;
        }

        public int CreateFile(FileModel fileDTO)
        {
            return UnitOfWorkManager.UnitOfWork.Create(fileDTO);
        }

        public void DeleteFile(int id)
        {
            if (UnitOfWorkManager.UnitOfWork.Files.GetPostsCount(id) > 0)
                return;
            UnitOfWorkManager.UnitOfWork.Files.Delete(id);
        }

        public int Count()
        {
            return UnitOfWorkManager.UnitOfWork.Files.Count();
        }

        public string GetNameById(int id)
        {
            var file = UnitOfWorkManager.UnitOfWork.Files.Get(id);
            if (file == null)
                throw new FileNullException();
            return file.Name;
        }
    }
}
