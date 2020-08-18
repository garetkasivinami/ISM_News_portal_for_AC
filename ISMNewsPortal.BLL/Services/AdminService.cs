﻿using ISMNewsPortal.BLL.Models;
using System.Collections.Generic;
using ISMNewsPortal.BLL.Exceptions;
using static ISMNewsPortal.BLL.SessionManager;

namespace ISMNewsPortal.BLL.Services
{
    public class AdminService {

        public IEnumerable<Admin> GetAdmins()
        {
            return AdminRepository.GetAll();
        }

        public Admin GetAdmin(int id)
        {
            var admin = AdminRepository.Get(id);
            if (admin == null)
                throw new AdminNullException();
            return admin;
        }
        
        public Admin GetAdminByLogin(string login)
        {
            var admin = AdminRepository.GetByLogin(login);
            return admin;
        }

        public void UpdateAdmin(Admin admin)
        {
            AdminRepository.Update(admin);
            UnitOfWork.Save();
        }

        public void UpdateAdminPartial(int id, string email, string roles = null, bool updateRoles = false)
        {
            var admin = AdminRepository.Get(id);
            admin.Email = email;
            if (updateRoles)
                admin.Roles = roles;
            AdminRepository.Update(admin);
            UnitOfWork.Save();
        }

        public void CreateAdmin(Admin admin)
        {
            var createdAdmin = AdminRepository.GetByLogin(admin.Login);
            if (createdAdmin != null)
                throw new AdminExistsException("An administrator with this login already exists");
            AdminRepository.Create(admin);
            UnitOfWork.Save();
        }

        public void DeleteAdmin(int id)
        {
            AdminRepository.Delete(id);
            UnitOfWork.Save();
        }

        public int Count()
        {
            return AdminRepository.Count();
        }
    }
}
