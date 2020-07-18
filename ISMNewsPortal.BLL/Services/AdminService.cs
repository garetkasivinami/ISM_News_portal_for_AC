using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Services
{
    public class AdminService {

        public IEnumerable<AdminDTO> GetAdmins()
        {
            return Unity.UnitOfWork.Admins.GetAll();
        }

        public AdminDTO GetAdmin(int id)
        {
            var admin = Unity.UnitOfWork.Admins.Get(id);
            if (admin == null)
                throw new Exception("Admin is null");
            return admin;
        }

        public AdminDTO GetAdminByLogin(string login)
        {
            var admin = Unity.UnitOfWork.Admins.GetByLogin(login);
            if (admin == null)
                throw new Exception("Admin is null");
            return admin;
        }

        public void UpdateAdmin(AdminDTO adminDTO)
        {
            Unity.UnitOfWork.Admins.Update(adminDTO);
        }

        public void UpdateAdminPartial(int id, string email, string roles = null, bool updateRoles = false)
        {
            var admin = Unity.UnitOfWork.Admins.Get(id);
            admin.Email = email;
            if (updateRoles)
                admin.Roles = roles;
            Unity.UnitOfWork.Admins.Update(admin);
        }

        public void CreateAdmin(AdminDTO adminDTO)
        {
            var createdAdmin = Unity.UnitOfWork.Admins.GetByLogin(adminDTO.Login);
            if (createdAdmin != null)
                throw new Exception("An administrator with this login already exists");
            Unity.UnitOfWork.Admins.Create(adminDTO);
        }

        public void DeleteAdmin(int id)
        {
            Unity.UnitOfWork.Admins.Delete(id);
        }

        public int Count()
        {
            return Unity.UnitOfWork.Admins.Count();
        }
    }
}
