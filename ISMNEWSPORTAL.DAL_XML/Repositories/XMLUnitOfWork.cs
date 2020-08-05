using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using ISMNEWSPORTAL.DAL_XML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNEWSPORTAL.DAL_XML.Repositories
{
    public class XMLUnitOfWork : IUnitOfWork
    {
        private XMLContex contex;
        private AdminRepository adminRepository;
        private CommentRepository commentRepository;
        private NewsPostRepository newsPostRepository;
        private FileRepository fileRepository;

        private Dictionary<Type, object> repositories;
        public IAdminRepository Admins => adminRepository;

        public ICommentRepository Comments => commentRepository;

        public INewsPostRepository NewsPosts => newsPostRepository;

        public IFileRepository Files => fileRepository;

        public XMLUnitOfWork(XMLContex contex)
        {
            this.contex = contex;

            adminRepository = new AdminRepository(contex);
            commentRepository = new CommentRepository(contex);
            newsPostRepository = new NewsPostRepository(contex);
            fileRepository = new FileRepository(contex);

            repositories = new Dictionary<Type, object>();
            repositories.Add(typeof(Admin), adminRepository);
            repositories.Add(typeof(Comment), commentRepository);
            repositories.Add(typeof(NewsPost), newsPostRepository);
            repositories.Add(typeof(FileModel), fileRepository);
        }

        public void Dispose()
        {
            contex.Dispose();
        }

        public void Save()
        {
            AppendChanges(adminRepository);
            AppendChanges(commentRepository);
            AppendChanges(newsPostRepository);
            AppendChanges(fileRepository);
            contex.Save();
        }

        private void AppendChanges<T>(Repository<T> repository) where T : Model
        {
            List<ModelObject<T>> changes = repository.entities.Values.Where(u => u.State == ModelState.Created).ToList();
            T[] changedObjects = ConvertModelObjectToModel<T>(changes);
            contex.CreateRange<T>(changedObjects);

            changes = repository.entities.Values.Where(u => u.State == ModelState.Updated).ToList();
            changedObjects = ConvertModelObjectToModel<T>(changes);
            contex.UpdateRange<T>(changedObjects);

            var deleteObjects = repository.entities.Where(u => u.Value.State == ModelState.Deleted);
            contex.DeleteRange<T>(deleteObjects.Select(u => u.Key).ToArray());

            repository.ResetEntitiesStates();
        }

        private T[] ConvertModelObjectToModel<T>(List<ModelObject<T>> items)
        {
            T[] result = new T[items.Count()];
            for (int i = 0; i < items.Count(); i++)
            {
                result[i] = items[i].Model;
            }
            return result;
        }
    }
}
