using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.DAL_XML.Models;
using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL;
using System.Xml;
using ISMNewsPortal.DAL_XML.Repositories;

namespace ISMNewsPortal.DAL_XML
{
    public class XMLUnitOfWork : IUnitOfWork
    {
        private XMLContex contex;
        private XmlDocument rollbackDocument;

        public XMLUnitOfWork(XMLContex contex)
        {
            this.contex = contex;
        }

        public void Dispose()
        {
            contex.Dispose();
        }

        public void Save()
        {
            rollbackDocument = contex.document.Clone() as XmlDocument;

            try
            {
                AdminRepository adminRepository = SessionManager.AdminRepository as AdminRepository;
                CommentRepository commentRepository = SessionManager.CommentRepository as CommentRepository;
                NewsPostRepository newsPostRepository = SessionManager.NewsPostRepository as NewsPostRepository;
                FileRepository fileRepository = SessionManager.FileRepository as FileRepository;

                AppendChanges(adminRepository);
                AppendChanges(commentRepository);
                AppendChanges(newsPostRepository);
                AppendChanges(fileRepository);

                contex.Save();
            }
            catch
            {
                contex.document = rollbackDocument;
            }
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
            int itemsCount = items.Count();
            T[] result = new T[itemsCount];
            for (int i = 0; i < itemsCount; i++)
            {
                result[i] = items[i].Model;
            }
            return result;
        }
    }
}
