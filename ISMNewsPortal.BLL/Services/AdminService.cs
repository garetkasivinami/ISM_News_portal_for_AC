using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Exceptions;

namespace ISMNewsPortal.BLL.Services
{
    public class AdminService {

        public IEnumerable<Admin> GetAdmins()
        {
            return UnitOfWorkManager.UnitOfWork.Admins.GetAll();
        }

        public Admin GetAdmin(int id)
        {
            var admin = UnitOfWorkManager.UnitOfWork.Admins.Get(id);
            if (admin == null)
                throw new AdminNullException();
            return admin;
        }

        public Admin GetAdminByLogin(string login)
        {
            var admin = UnitOfWorkManager.UnitOfWork.Admins.GetByLogin(login);
            if (admin == null)
                throw new AdminNullException();
            return admin;
        }

        public void UpdateAdmin(Admin adminDTO)
        {
            UnitOfWorkManager.UnitOfWork.Update(adminDTO);
        }

        public void UpdateAdminPartial(int id, string email, string roles = null, bool updateRoles = false)
        {
            var admin = UnitOfWorkManager.UnitOfWork.Admins.Get(id);
            admin.Email = email;
            if (updateRoles)
                admin.Roles = roles;
            UnitOfWorkManager.UnitOfWork.Update(admin);
        }

        public void CreateAdmin(Admin adminDTO)
        {
            var createdAdmin = UnitOfWorkManager.UnitOfWork.Admins.GetByLogin(adminDTO.Login);
            if (createdAdmin != null)
                throw new AdminExistsException("An administrator with this login already exists");
            UnitOfWorkManager.UnitOfWork.Create(adminDTO);
        }

        public void DeleteAdmin(int id)
        {
            UnitOfWorkManager.UnitOfWork.Admins.Delete(id);
        }

        public int Count()
        {
            return UnitOfWorkManager.UnitOfWork.Admins.Count();
        }
    }
}
