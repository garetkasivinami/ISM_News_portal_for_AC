using ISMNewsPortal.DAL.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.BusinessModels;
using static ISMNewsPortal.BLL.Mappers.Automapper;

namespace ISMNewsPortal.DAL.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private ISession session;
        public AdminRepository(ISession session)
        {
            this.session = session;
        }

        public int Count()
        {
            return session.Query<Admin>().Count();
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
            var admin = session.Get<Admin>(id);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(admin);
                transaction.Commit();
            }
        }

        public Admin Get(int id)
        {
            return session.Get<Admin>(id);
        }

        public IEnumerable<Admin> GetAll()
        {
            return session.Query<Admin>();
        }

        public IEnumerable<Admin> GetWithOptions(ToolsDTO toolBar)
        {
            return GetAll();
        }

        public Admin GetByEmail(string email)
        {
            return session.Query<Admin>().FirstOrDefault(u => u.Email == email);
        }

        public Admin GetByLogin(string login)
        {
            return session.Query<Admin>().SingleOrDefault(u => u.Login == login);
        }

        public IEnumerable<Admin> GetByRole(string role)
        {
            return session.Query<Admin>().Where(u => Array.IndexOf(u.Roles.Split(','), role) != -1);
        }

        public void Update(Admin item)
        {
            var admin = item;
            var createdAdmin = session.Get<Admin>(item.Id);
            Map(admin, createdAdmin);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(createdAdmin);
                transaction.Commit();
            }
        }
    }
}
