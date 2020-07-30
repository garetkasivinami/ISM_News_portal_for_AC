using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.UnitOfWorkManager;

namespace ISMNewsPortal.BLL.Services
{
    public class AdminService {

        public IEnumerable<Admin> GetAdmins()
        {
            return UnitOfWork.Admins.GetAll();
        }

        public Admin GetAdmin(int id)
        {
            var admin = UnitOfWork.Admins.Get(id);
            if (admin == null)
                throw new AdminNullException();
            return admin;
        }

        public Admin GetAdminByLogin(string login)
        {
            var admin = UnitOfWork.Admins.GetByLogin(login);
            if (admin == null)
                throw new AdminNullException();
            return admin;
        }

        public void UpdateAdmin(Admin adminDTO)
        {
            UnitOfWork.Update(adminDTO);
            UnitOfWork.Save();
        }

        public void UpdateAdminPartial(int id, string email, string roles = null, bool updateRoles = false)
        {
            var admin = UnitOfWork.Admins.Get(id);
            admin.Email = email;
            if (updateRoles)
                admin.Roles = roles;
            UnitOfWork.Update(admin);
            UnitOfWork.Save();
        }

        public void CreateAdmin(Admin adminDTO)
        {
            var createdAdmin = UnitOfWork.Admins.GetByLogin(adminDTO.Login);
            if (createdAdmin != null)
                throw new AdminExistsException("An administrator with this login already exists");
            UnitOfWork.Create(adminDTO);
            UnitOfWork.Save();
        }

        public void DeleteAdmin(int id)
        {
            UnitOfWork.Admins.Delete(id);
            UnitOfWork.Save();
        }

        public int Count()
        {
            return UnitOfWork.Admins.Count();
        }
    }
}
