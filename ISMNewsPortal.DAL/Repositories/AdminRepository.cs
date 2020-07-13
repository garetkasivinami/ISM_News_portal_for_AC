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
    public class AdminRepository : IRepository<Admin>
    {
        private ISession session;
        public AdminRepository(ISession session)
        {
            this.session = session;
        }

        public int Create(Admin item)
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
            var item = session.Get<Admin>(id);
            if (item == null)
                return;
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(item);
                transaction.Commit();
            }
        }

        public IEnumerable<Admin> Find(Func<Admin, bool> predicate)
        {
            return session.Query<Admin>().Where(predicate);
        }

        public Admin FindSingle(Func<Admin, bool> predicate)
        {
            return session.Query<Admin>().SingleOrDefault(predicate);
        }

        public Admin Get(int id)
        {
            return session.Get<Admin>(id);
        }

        public IEnumerable<Admin> GetAll()
        {
            return session.Query<Admin>();
        }

        public int Count()
        {
            return session.Query<Admin>().Count();
        }

        public int Count(Func<Admin, bool> predicate)
        {
            return session.Query<Admin>().Count(predicate);
        }

        public void Update(Admin item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(item);
                transaction.Commit();
            }
        }
    }
}
