using ISMNewsPortal.BLL.BusinessModels;
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
    public class Repository<T> : IRepository<T> where T : Model
    {
        protected XMLContex contex;
        internal Dictionary<int, ModelObject<T>> entities;
        private bool gotAll;

        public Repository(XMLContex contex)
        {
            this.contex = contex;

            entities = new Dictionary<int, ModelObject<T>>();
        }
        public int Count()
        {
            return GetAll().Count<T>();
        }

        public virtual int Create(T item)
        {
            contex.SetNewItemId<T>(item);
            entities.Add(item.Id, new ModelObject<T>() { Model = item, State = ModelState.Created });
            return item.Id;
        }

        public virtual void Delete(int id)
        {
            if (entities.ContainsKey(id))
            {
                entities[id].State = ModelState.Deleted;
                return;
            }
            entities.Add(id, new ModelObject<T>() { Model = Get(id), State = ModelState.Deleted });
        }

        public T Get(int id)
        {
            if (entities.ContainsKey(id))
            {
                ModelObject<T> modelObject = entities[id];
                if (modelObject.State == ModelState.Deleted)
                    return null;

                return modelObject.Model;
            }
            T entity = contex.Get<T>(id);
            entities.Add(id, new ModelObject<T>() { Model = entity, State = ModelState.Normal});
            return entity;
        }

        public IEnumerable<T> GetAll()
        {
            if (!gotAll)
            {
                IEnumerable<T> xmlEntities = contex.GetAll<T>();
                foreach(T entity in xmlEntities)
                {
                    if (!entities.ContainsKey(entity.Id))
                    {
                        entities.Add(entity.Id, new ModelObject<T>() { Model = entity, State = ModelState.Normal });
                    }
                }
                gotAll = true;
            }
            return entities.Where(u => u.Value.State != ModelState.Deleted).Select(u => u.Value.Model);
        }
        
        public void ResetEntitiesStates()
        {
            var deleteList = entities.Where(u => u.Value.State == ModelState.Deleted).ToList();
            foreach(var item in deleteList)
            {
                entities.Remove(item.Key);
            }

            foreach(var item in entities)
            {
                item.Value.State = ModelState.Normal;
            }
        }

        public virtual IEnumerable<T> GetWithOptions(object toolBar)
        {
            return GetAll();
        }

        public virtual void Update(T item)
        {
            if (entities.ContainsKey(item.Id))
            {
                var entity = entities[item.Id];
                entity.Model = item;
                if (entity.State == ModelState.Normal)
                    entity.State = ModelState.Updated;
                return;
            }
            entities.Add(item.Id, new ModelObject<T>() { Model = item, State = ModelState.Updated } );
        }
    }
}
