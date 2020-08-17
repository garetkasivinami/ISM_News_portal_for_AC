using ISMNewsPortal.BLL.Repositories;
using NHibernate;
using NHibernate.Context;
using NHibernate.Impl;
using System;

namespace ISMNewsPortal.DAL.Repositories
{
    public class HibernateUnitOfWork : IUnitOfWork
    {
        [ThreadStatic]
        private ITransaction transaction;
        private ISessionFactory sessionFactory;

        public HibernateUnitOfWork()
        {
            sessionFactory = NHibernateSession.SessionFactory;
        }

        public ISession Session
        {
            get
            {
                if (!CurrentSessionContext.HasBind(sessionFactory))
                    CurrentSessionContext.Bind(sessionFactory.OpenSession());

                return NHibernateSession.SessionFactory.GetCurrentSession();
            }
        }

        public void Dispose()
        {
            if(transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }

            ISession session = CurrentSessionContext.Unbind(sessionFactory);

            session.Close();
            session.Dispose();
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
