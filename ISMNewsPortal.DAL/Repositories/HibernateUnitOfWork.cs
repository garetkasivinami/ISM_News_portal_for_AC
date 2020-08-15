using ISMNewsPortal.BLL.Repositories;
using NHibernate;
using NHibernate.Impl;
using System;

namespace ISMNewsPortal.DAL.Repositories
{
    public class HibernateUnitOfWork : IUnitOfWork
    {
        [ThreadStatic]
        private ISession session;
        [ThreadStatic]
        private ITransaction transaction;

        public ISession Session
        {
            get
            {
                if (session != null)
                    return session;
                return session = NHibernateSession.SessionFactory.OpenSession();
            }
        }

        public void Dispose()
        {
            if(transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
            session.Close();
            session = null;
        }

        public void BeginTransaction()
        {
            transaction = Session.BeginTransaction();
        }

        public void Save()
        {
            try
            {
                if (transaction != null)
                {
                    transaction.Commit();
                }
            } catch
            {
                transaction.Rollback();
            }
            finally
            {
                Dispose();
            }
        }
    }
}
