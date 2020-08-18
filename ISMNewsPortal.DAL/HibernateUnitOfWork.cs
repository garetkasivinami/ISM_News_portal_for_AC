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
            if(Session.Transaction != null)
            {
                Session.Transaction.Dispose();
            }

            ISession session = CurrentSessionContext.Unbind(sessionFactory);

            session.Close();
            session.Dispose();
        }

        public void BeginTransaction()
        {
            Session.BeginTransaction();
        }

        public void Save()
        {
            try
            {
                if (Session.Transaction != null)
                {
                    Session.Transaction.Commit();
                }
            } catch
            {
                Session.Transaction.Rollback();
            }
            finally
            {
                Dispose();
            }
        }
    }
}
