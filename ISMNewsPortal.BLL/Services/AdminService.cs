using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Infrastructure;
using ISMNewsPortal.BLL.Mappers;
using ISMNewsPortal.DAL.Models;
using ISMNewsPortal.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Services
{
    public class AdminService : Service
    {
        public AdminService() : base()
        {

        }

        public AdminService(Service service) : base(service)
        {

        }

        public IEnumerable<AdminDTO> GetAdmins()
        {
            return DTOMapper.AdminMapperToDTO.Map<IEnumerable<Admin>, List<AdminDTO>>(database.Admins.GetAll());
        }

        public AdminDTO GetAdmin(int id)
        {
            var admin = database.Admins.Get(id);
            if (admin == null)
                throw ExceptionGenerator.GenerateException("Admin is null", "AdminService.GetAdmin(int id)", $"id: {id}");
            return DTOMapper.AdminMapperToDTO.Map<Admin, AdminDTO>(admin);
        }

        public AdminDTO GetAdminByLogin(string login)
        {
            var admin = database.Admins.FindSingle(u => u.Login == login);
            if (admin == null)
                throw ExceptionGenerator.GenerateException("Admin is null", "AdminService.GetAdminByLogin(string login)", $"login: {login}");
            return DTOMapper.AdminMapperToDTO.Map<Admin, AdminDTO>(admin);
        }

        public void UpdateAdmin(AdminDTO adminDTO)
        {
            var admin = DTOMapper.AdminMapper.Map<AdminDTO, Admin>(adminDTO);
            database.Admins.Update(admin);
        }

        public void UpdateAdminPartial(int id, string email, string roles = null, bool updateRoles = false)
        {
            var admin = database.Admins.Get(id);
            admin.Email = email;
            if (updateRoles)
                admin.Roles = roles;
            database.Admins.Update(admin);
        }

        public void CreateAdmin(AdminDTO adminDTO)
        {
            var admin = DTOMapper.AdminMapper.Map<AdminDTO, Admin>(adminDTO);
            var createdAdmin = database.Admins.FindSingle(u => u.Login == admin.Login);
            if (createdAdmin != null)
                throw ExceptionGenerator.GenerateException("An administrator with this login already exists", "AdminService.CreateAdmin(AdminDTO adminDTO)",
                    $"adminDTO.Login: {adminDTO.Login}");
            database.Admins.Create(admin);
        }

        public void DeleteAdmin(int id)
        {
            database.Admins.Delete(id);
        }

        public int Count()
        {
            return database.Admins.Count();
        }

        public int Count(Func<Admin, bool> predicate)
        {
            return database.Admins.Count(predicate);
        }
    }
}
