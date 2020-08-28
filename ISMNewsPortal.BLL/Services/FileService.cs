using ISMNewsPortal.BLL.Models;
using System.Collections.Generic;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.SessionManager;

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
            var file = CacheRepository.GetItem<FileModel>(id);
            if (file != null)
                return file;

            file = FileRepository.Get(id);
            if (file == null)
                throw new FileNullException();
            CacheRepository.AddItem(file);
            return file;
        }


        public FileModel FindByHashCode(string hashCode)
        {
            var file = FileRepository.GetByHashCode(hashCode);
            return file;
        }

        public int CreateFile(FileModel fileModel)
        {
            int id = FileRepository.Create(fileModel);
            CacheRepository.AddItem(fileModel);
            UnitOfWork.Save();
            return id;
        }

        public bool SafeDeleteFile(int id)
        {
            if (FileRepository.GetPostsCount(id) > 0)
                return false;
            DeleteFile(id);
            return true;
        }

        public void DeleteFile(int id)
        {
            FileRepository.Delete(id);
            CacheRepository.Delete<FileModel>(id);
            UnitOfWork.Save();
        }

        public int Count()
        {
            return FileRepository.Count();
        }

        public string GetNameById(int id)
        {
            var file = GetFile(id);
            if (file == null)
                throw new FileNullException();
            return file.Name;
        }
    }
}
