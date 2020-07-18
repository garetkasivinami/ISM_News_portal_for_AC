using ISMNewsPortal.DAL.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Interfaces;
using ISMNewsPortal.BLL.DTO;
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

        public int Create(AdminDTO item)
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

        public AdminDTO Get(int id)
        {
            var admin = session.Get<Admin>(id);
            return MapToAdminDTO(admin);
        }

        public IEnumerable<AdminDTO> GetAll()
        {
            var admins = session.Query<Admin>();
            return MapToAdminDTOList(admins);
        }

        public IEnumerable<AdminDTO> GetAllWithTools(ToolsDTO toolBar)
        {
            return GetAll();
        }

        public AdminDTO GetByEmail(string email)
        {
            var admin = session.Query<Admin>().Where(u => u.Email == email);
            return MapToAdminDTO(admin);
        }

        public AdminDTO GetByLogin(string login)
        {
            var admin = session.Query<Admin>().Where(u => u.Email == login);
            return MapToAdminDTO(admin);
        }

        public IEnumerable<AdminDTO> GetByRole(string role)
        {
            var admins = session.Query<Admin>().Where(u => Array.IndexOf(u.Roles.Split(','), role) != -1);
            return MapToAdminDTOList(admins);
        }

        public void Update(AdminDTO item)
        {
            var admin = MapFromAdminDTO<Admin>(item);
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(admin);
                transaction.Commit();
            }
        }
    }
}
