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
        protected HibernateUnitOfWork hibernateUnitOfWork;
        public Repository(HibernateUnitOfWork hibernateUnitOfWork) {
            this.hibernateUnitOfWork = hibernateUnitOfWork;
        }
        public int Count()
        {
            return hibernateUnitOfWork.Session.Query<T>().Count();
        }

        public virtual int Create(T item)
        {
            hibernateUnitOfWork.BeginTransaction();
            if (item == null)
                throw new NullReferenceException();

            hibernateUnitOfWork.Session.Save(item);
            return item.Id;
        }

        public virtual void Delete(int id)
        {
            hibernateUnitOfWork.BeginTransaction();
            T item = hibernateUnitOfWork.Session.Get<T>(id);
            if (item == null)
                throw new NullReferenceException();

            hibernateUnitOfWork.Session.Delete(item);
        }

        public T Get(int id)
        {
            return hibernateUnitOfWork.Session.Get<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return hibernateUnitOfWork.Session.Query<T>();
        }

        public virtual IEnumerable<T> GetWithOptions(object toolBar)
        {
            return GetAll();
        }

        public virtual void Update(T item)
        {
            hibernateUnitOfWork.BeginTransaction();
            if (item == null)
                throw new NullReferenceException();
            hibernateUnitOfWork.Session.Update(item);
        }
    }
}
