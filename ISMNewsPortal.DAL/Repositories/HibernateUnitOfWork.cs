using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;
using NHibernate;

namespace ISMNewsPortal.DAL.Repositories
{
    public class HibernateUnitOfWork : IUnitOfWork
    {
        private bool disposed;

        private ISession session;
        private ITransaction transaction;

        public HibernateUnitOfWork(ISession session)
        {
            this.session = session;
            //this.transaction = session.BeginTransaction();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                session.Close();
                disposed = true;
            }
        }

        public void Save()
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                transaction.Commit();
            }
        }
    }
}
