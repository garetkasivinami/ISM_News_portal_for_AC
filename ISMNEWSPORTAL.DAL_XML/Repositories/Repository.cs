using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNEWSPORTAL.DAL_XML.Repositories
{
    public enum ModelState
    {
        Normal,
        Created,
        Updated,
        Deleted
    }
    public class ModelObject<T>
    {
        public T Model;
        public ModelState State;
    }
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
            return contex.Count<T>();
        }

        public int Create(T item)
        {
            item.Id = contex.GetLastId<T>();
            entities.Add(item.Id, new ModelObject<T>() { Model = item, State = ModelState.Created });
            return item.Id;
        }

        public void Delete(int id)
        {
            if (entities.ContainsKey(id))
            {
                entities[id].State = ModelState.Deleted;
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
            return entities.Select(u => u.Value.Model);
        }
        
        public void ResetEntitiesStates()
        {
            foreach(var item in entities)
            {
                item.Value.State = ModelState.Normal;
            }
        }

        public IEnumerable<T> GetWithOptions(Options toolBar)
        {
            return GetAll();
        }

        public void Update(T item)
        {
            if (entities.ContainsKey(item.Id))
            {
                entities[item.Id].Model = item;
                return;
            }
            entities.Add(item.Id, new ModelObject<T>() { Model = item, State = ModelState.Updated } );
        }
    }
}
