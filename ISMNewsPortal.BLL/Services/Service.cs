using ISMNewsPortal.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Services
{
    public abstract class Service : IDisposable
    {
        protected EFUnitOfWork database;

        public Service()
        {
            database = new EFUnitOfWork();
            database.SignedObjects.Add(this);
        }

        public Service(Service service)
        {
            database = service.database;
            database.SignedObjects.Add(this);
        }

        public void Dispose()
        {
            database.SignedObjects.Remove(this);
            database.Dispose();
        }
    }
}
