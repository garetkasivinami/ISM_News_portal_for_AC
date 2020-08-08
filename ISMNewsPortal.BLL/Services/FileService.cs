using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.UnitOfWorkManager;

namespace ISMNewsPortal.BLL.Services
{
    public class FileService
    {
        public IEnumerable<FileModel> GetFiles()
        {
            return FileRepository.GetAll();
        }

        public FileModel GetFile(int id)
        {
            var file = FileRepository.Get(id);
            if (file == null)
                throw new FileNullException();
            return file;
        }


        public FileModel FindByHashCode(string hashCode)
        {
            var file = FileRepository.GetByHashCode(hashCode);
            return file;
        }

        public int CreateFile(FileModel fileDTO)
        {
            int id = FileRepository.Create(fileDTO);
            UnitOfWork.Save();
            return id;
        }

        public void DeleteFile(int id)
        {
            if (FileRepository.GetPostsCount(id) > 0)
                return;
            FileRepository.Delete(id);
            UnitOfWork.Save();
        }

        public int Count()
        {
            return FileRepository.Count();
        }

        public string GetNameById(int id)
        {
            var file = FileRepository.Get(id);
            if (file == null)
                throw new FileNullException();
            return file.Name;
        }
    }
}
