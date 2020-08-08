using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.DAL_XML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.DAL_XML.Repositories
{
    public class XMLUnitOfWork : IUnitOfWork
    {
        private XMLContex contex;
        private AdminRepository adminRepository;
        private CommentRepository commentRepository;
        private NewsPostRepository newsPostRepository;
        private FileRepository fileRepository;

        public XMLUnitOfWork(XMLContex contex, AdminRepository adminRepository, CommentRepository commentRepository,
            NewsPostRepository newsPostRepository, FileRepository fileRepository)
        {
            this.contex = contex;

            this.adminRepository = adminRepository;
            this.commentRepository = commentRepository;
            this.newsPostRepository = newsPostRepository;
            this.fileRepository = fileRepository;
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
            var entitiesValues = repository.entities.Values;

            var changes = entitiesValues.Where(u => u.State == ModelState.Created).ToList();
            var changedObjects = ConvertModelObjectToModel(changes);
            contex.CreateRange(changedObjects);

            changes = entitiesValues.Where(u => u.State == ModelState.Updated).ToList();
            changedObjects = ConvertModelObjectToModel(changes);
            contex.UpdateRange(changedObjects);

            var deleteObjects = entitiesValues.Where(u => u.State == ModelState.Deleted);
            contex.DeleteRange<T>(deleteObjects.Select(u => u.Model.Id).ToArray());

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
