using System.Collections.Generic;
using System.Linq;
using System;
using NHibernate;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;

namespace ISMNewsPortal.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : Model
    {
        protected ISession _session;
        public Repository(ISession session) {
            _session = session;
        }
        public int Count()
        {
            return _session.Query<T>().Count();
        }

        public virtual int Create(T item)
        {
            if (item == null)
                throw new NullReferenceException();

            _session.Save(item);
            return item.Id;
        }

        public virtual void Delete(int id)
        {
            T item = _session.Get<T>(id);
            if (item == null)
                throw new NullReferenceException();

            _session.Delete(item);
        }

        public T Get(int id)
        {
            return _session.Get<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _session.Query<T>();
        }

        public virtual IEnumerable<T> GetWithOptions(object toolBar)
        {
            return GetAll();
        }

        public virtual void Update(T item)
        {
            if (item == null)
                throw new NullReferenceException();
            _session.Update(item);
        }
    }
}
