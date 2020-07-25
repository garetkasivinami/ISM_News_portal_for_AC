using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Services
{
    public class FileService
    {
        public IEnumerable<FileModel> GetFiles()
        {
            return Unity.UnitOfWork.Files.GetAll();
        }

        public FileModel GetFile(int id)
        {
            var file = Unity.UnitOfWork.Files.Get(id);
            if (file == null)
                throw new Exception("File is null");
            return file;
        }


        public FileModel FindByHashCode(string hashCode)
        {
            var file = Unity.UnitOfWork.Files.GetByHashCode(hashCode);
            return file;
        }

        public int CreateFile(FileModel fileDTO)
        {
            return Unity.UnitOfWork.Files.Create(fileDTO);
        }

        public void DeleteFile(int id)
        {
            if (Unity.UnitOfWork.Files.GetPostsCount(id) > 0)
                return;
            Unity.UnitOfWork.Files.Delete(id);
        }

        public int Count()
        {
            return Unity.UnitOfWork.Files.Count();
        }

        public string GetNameById(int id)
        {
            var file = Unity.UnitOfWork.Files.Get(id);
            if (file == null)
                throw new Exception("File is null");
            return file.Name;
        }
    }
}
