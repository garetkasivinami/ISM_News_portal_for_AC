using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.DAL_XML.Models;
using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL;
using System.Xml;

namespace ISMNewsPortal.DAL_XML.Repositories
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
                AdminRepository adminRepository = UnitOfWorkManager.AdminRepository as AdminRepository;
                CommentRepository commentRepository = UnitOfWorkManager.CommentRepository as CommentRepository;
                NewsPostRepository newsPostRepository = UnitOfWorkManager.NewsPostRepository as NewsPostRepository;
                FileRepository fileRepository = UnitOfWorkManager.FileRepository as FileRepository;

                AppendChanges(adminRepository);
                AppendChanges(commentRepository);
                AppendChanges(newsPostRepository);
                AppendChanges(fileRepository);
            }
            catch
            {
                contex.document = rollbackDocument;
            }
            finally
            {
                contex.Save();
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
            T[] result = new T[items.Count()];
            for (int i = 0; i < items.Count(); i++)
            {
                result[i] = items[i].Model;
            }
            return result;
        }
    }
}
