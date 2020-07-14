using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.Mappers;
using ISMNewsPortal.DAL.Models;
using ISMNewsPortal.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Services
{
    public class FileService : Service
    {
        public FileService() : base()
        {

        }

        public FileService(Service service) : base(service)
        {

        }

        public IEnumerable<FileDTO> GetFiles()
        {
            return DTOMapper.FileMapperToDTO.Map<IEnumerable<FileModel>, List<FileDTO>>(database.Files.GetAll());
        }

        public FileDTO GetFile(int id)
        {
            var file = database.Files.Get(id);
            if (file == null)
                throw ExceptionGenerator.GenerateException("File is null", "FileService.GetFile(int id)", $"id: {id}");
            return DTOMapper.FileMapperToDTO.Map<FileModel, FileDTO>(file);
        }

        public IEnumerable<FileDTO> FindFiles(Func<FileModel, bool> predicate)
        {
            var files = database.Files.Find(predicate);
            return DTOMapper.FileMapperToDTO.Map<IEnumerable<FileModel>, List<FileDTO>>(files);
        }

        public FileDTO FindByHashCode(string hashCode)
        {
            var file = database.Files.FindSingle(u => u.HashCode == hashCode);
            return DTOMapper.FileMapperToDTO.Map<FileModel, FileDTO>(file);
        }

        public void CreateFile(FileDTO fileDTO)
        {
            var file = DTOMapper.FileMapper.Map<FileDTO, FileModel>(fileDTO);
            database.Files.Create(file);
        }

        public void DeleteFile(int id)
        {
            if (database.NewsPosts.Any(u => u.ImageId == id))
                return;
            database.Files.Delete(id);
        }

        public int Count()
        {
            return database.Files.Count();
        }

        public int GetMaxId()
        {
            return database.Files.Max(u => u.Id);
        }

        public string GetNameById(int id)
        {
            var file = database.Files.Get(id);
            if (file == null)
                throw ExceptionGenerator.GenerateException("File is null", "FileService.GetNameById(int id)", $"id: {id}");
            return file.Name;
        }
    }
}
