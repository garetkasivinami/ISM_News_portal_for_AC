using ISMNewsPortal.DAL.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Interfaces;
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
            var admin = MapFromAdminDTO<Admin>(item);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(admin);
                transaction.Commit();
                return admin.Id;
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
            var admin = session.Get<Admin>(id);
            return MapToAdminDTO(admin);
        }

        public IEnumerable<Admin> GetAll()
        {
            var admins = session.Query<Admin>();
            return MapToAdminDTOList(admins);
        }

        public IEnumerable<Admin> GetAllWithTools(ToolsDTO toolBar)
        {
            return GetAll();
        }

        public Admin GetByEmail(string email)
        {
            var admin = session.Query<Admin>().Where(u => u.Email == email);
            return MapToAdminDTO(admin);
        }

        public Admin GetByLogin(string login)
        {
            var admin = session.Query<Admin>().SingleOrDefault(u => u.Login == login);
            return MapToAdminDTO(admin);
        }

        public IEnumerable<Admin> GetByRole(string role)
        {
            var admins = session.Query<Admin>().Where(u => Array.IndexOf(u.Roles.Split(','), role) != -1);
            return MapToAdminDTOList(admins);
        }

        public void Update(Admin item)
        {
            var admin = MapFromAdminDTO<Admin>(item);
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
