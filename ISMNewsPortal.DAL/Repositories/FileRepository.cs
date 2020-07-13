using ISMNewsPortal.DAL.Interfaces;
using ISMNewsPortal.DAL.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.DAL.Repositories
{
    public class FileRepository : IRepository<FileModel>
    {
        private ISession session;
        public FileRepository(ISession session)
        {
            this.session = session;
        }

        public int Create(FileModel item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(item);
                transaction.Commit();
                return item.Id;
            }
        }

        public void Delete(int id)
        {
            var item = session.Get<FileModel>(id);
            if (item == null)
                return;
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(item);
                transaction.Commit();
            }
        }

        public IEnumerable<FileModel> Find(Func<FileModel, bool> predicate)
        {
            return session.Query<FileModel>().Where(predicate);
        }

        public FileModel FindSingle(Func<FileModel, bool> predicate)
        {
            return session.Query<FileModel>().SingleOrDefault(predicate);
        }

        public FileModel Get(int id)
        {
            return session.Get<FileModel>(id);
        }

        public IEnumerable<FileModel> GetAll()
        {
            return session.Query<FileModel>();
        }

        public int Count()
        {
            return session.Query<FileModel>().Count();
        }

        public int Count(Func<FileModel, bool> predicate)
        {
            return session.Query<FileModel>().Count(predicate);
        }

        public T Max <T>(Func<FileModel, T> predicate)
        {
            return session.Query<FileModel>().Max(predicate);
        }

        public bool Any(Func<FileModel, bool> predicate)
        {
            return session.Query<FileModel>().Any(predicate);
        }

        public void Update(FileModel item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(item);
                transaction.Commit();
            }
        }
    }
}
