using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static ISMNewsPortal.BLL.Mappers.Automapper;

namespace ISMNewsPortal.Helpers
{
    public static class AdminHelper
    {
        public static void CreateAdmin(Admin admin)
        {
            AdminService adminService = new AdminService();
            var adminDTO = MapToAdminDTO(admin);
            adminService.CreateAdmin(adminDTO);
        }

        public static void UpdateAdmin(Admin admin)
        {
            AdminService adminService = new AdminService();
            var adminDTO = MapToAdminDTO(admin);
            adminService.UpdateAdmin(adminDTO);
        }

        public static void UpdateAdminPartial(int id, string email, string roles = null, bool updateRoles = false)
        {
            AdminService adminService = new AdminService();
            adminService.UpdateAdminPartial(id, email, roles, updateRoles);
        }

        public static void DeleteAdmin(int id)
        {
            AdminService adminService = new AdminService();
            adminService.DeleteAdmin(id);
        }

        public static void SetPassword(Admin admin, string password)
        {
            admin.Password = Security.SHA512(password, out string salt);
            admin.Salt = salt;
        }

        public static bool CheckPassword(Admin admin, string password)
        {
            string sha512password = Security.SHA512(password, admin.Salt);
            if (sha512password == admin.Password)
                return true;
            else
                return false;
        }

        public static Admin GetAdmin(int id)
        {
            AdminService adminService = new AdminService();
            var adminDTO = adminService.GetAdmin(id);
            return MapFromAdminDTO<Admin>(adminDTO);
        }

        public static Admin GetAdmin(string login)
        {
            AdminService adminService = new AdminService();
            var adminDTO = adminService.GetAdminByLogin(login);
            return MapFromAdminDTO<Admin>(adminDTO);
        }

        public static Admin GetAdminByLoginAndPassword(string login, string password)
        {
            Admin admin = GetAdmin(login);
            if (CheckPassword(admin, password))
                return admin;
            else
                return null;
        }

        public static AdminViewModelCollection GenerateAdminViewModelCollection()
        {
            AdminService adminService = new AdminService();
            var adminDTOs = adminService.GetAdmins();
            var admins = MapFromAdminDTOList<Admin>(adminDTOs);
            var adminViewModels = new List<AdminViewModel>();
            foreach (Admin admin in admins)
            {
                adminViewModels.Add(new AdminViewModel(admin));
            }
            return new AdminViewModelCollection() { AdminViewModels = adminViewModels };
        }

        public static IEnumerable<string> GetRolesStringsByLogin(string login)
        {
            return RoleCutter(GetAdmin(login).Roles);
        }

        public static IEnumerable<Roles> GetRoles(Admin admin)
        {
            return ConvertStringToRoles(admin.Roles);
        }

        public static void SetRoles(Admin admin, IEnumerable<Roles> roles)
        {
            admin.Roles = ConvertRolesToString(roles.ToArray());
        }

        public static IEnumerable<string> RoleCutter(string roleString)
        {
            if (roleString == null)
            {
                return new string[0];
            }
            return roleString.Split(',');
        }

        public static IEnumerable<Roles> ConvertStringToRoles(string roleString)
        {
            return ConvertStringToRoles(RoleCutter(roleString));
        }

        public static IEnumerable<Roles> ConvertStringToRoles(IEnumerable<string> roleStrings)
        {
            List<Roles> roles = new List<Roles>();
            foreach (string role in roleStrings)
            {
                object parcedRole = Enum.Parse(typeof(Roles), role);
                if (parcedRole != null)
                    roles.Add((Roles)parcedRole);
            }
            return roles;
        }

        public static string ConvertRolesToString(params Roles[] roles)
        {
            IEnumerable<string> temp = roles.Select(r => Enum.GetName(r.GetType(), r));
            return string.Join(",", temp);
        }
    }
}