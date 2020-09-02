using ISMNewsPortal.BLL.Repositories;
using NHibernate;
using NHibernate.Context;
using NHibernate.Impl;
using System;

namespace ISMNewsPortal.DAL
{
    public class HibernateUnitOfWork : IUnitOfWork
    {
        private ISessionFactory sessionFactory;

        public HibernateUnitOfWork()
        {
            sessionFactory = NHibernateSession.SessionFactory;
        }

        public void Dispose()
        {
            ISession session = NHibernateSession.Session;
            if (session.Transaction != null)
            {
                session.Transaction.Dispose();
            }

            NHibernateSession.CloseSession();
        }

        public void BeginTransaction()
        {
            NHibernateSession.Session.BeginTransaction();
        }

        public void Save()
        {
            ISession session = NHibernateSession.Session;
            try
            {
                if (session.Transaction.IsActive)
                    session.Transaction.Commit();

            } catch
            {
                session.Transaction.Rollback();
            }
            finally
            {
                Dispose();
            }
        }
    }
}
