using System.Collections.Generic;
using System.Linq;
using System;
using NHibernate;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.BLL;

namespace ISMNewsPortal.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : Model
    {
        protected HibernateUnitOfWork hibernateUnitOfWork
        {
            get
            {
                return SessionManager.UnitOfWork as HibernateUnitOfWork;
            }
        }
        public Repository() {

        }
        public int Count()
        {
            return NHibernateSession.Session.Query<T>().Count();
        }

        public virtual int Create(T item)
        {
            hibernateUnitOfWork.BeginTransaction();
            if (item == null)
                throw new NullReferenceException();

            NHibernateSession.Session.Save(item);
            return item.Id;
        }

        public virtual void Delete(int id)
        {
            hibernateUnitOfWork.BeginTransaction();
            ISession session = NHibernateSession.Session;
            T item = session.Get<T>(id);
            if (item == null)
                throw new NullReferenceException();

            session.Delete(item);
        }

        public T Get(int id)
        {
            return NHibernateSession.Session.Get<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return NHibernateSession.Session.Query<T>();
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
            NHibernateSession.Session.Update(item);
        }
    }
}
