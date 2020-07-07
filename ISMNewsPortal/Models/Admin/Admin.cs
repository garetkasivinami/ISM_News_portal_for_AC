using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class Admin
    {
        public virtual int Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string Salt { get; set; }
        public virtual string Roles { get; set; }
        //==================================================================
        public static void SetPassword(Admin admin, string password)
        {
            string salt;
            admin.Password = Security.SHA512(password, out salt);
            admin.Salt = salt;
        }
        public static int GetAdminIdByLogin(string login)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                return session.Query<Admin>().SingleOrDefault(u => u.Login == login).Id;
            }
        }
        public static Admin GetAdminById(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                return session.Get<Admin>(id);
            }
        }
        public static Admin GetAdminByLogin(string login)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                return session.Query<Admin>().SingleOrDefault(u => u.Login == login);
            }
        }
        public static Admin GetAdminByLoginAndPassword(LoginModel loginModel)
        {
            return GetAdminByLoginAndPassword(loginModel.Login, loginModel.Password);
        }
        public static Admin GetAdminByLoginAndPassword(string login, string password)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                Admin admin = session.Query<Admin>().SingleOrDefault(u => u.Login == login);
                if (admin == null)
                    return null;

                string Sha512password = Security.SHA512(password, admin.Salt);
                if (Sha512password == admin.Password)
                    return admin;
                else
                    return null;
            }
        }
        public static AdminViewModelCollection GenerateAdminViewModelCollection()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                IEnumerable<Admin> admins = session.Query<Admin>();
                ICollection<AdminViewModel> adminViewModels = new List<AdminViewModel>();
                foreach (Admin admin in admins)
                {
                    adminViewModels.Add(new AdminViewModel(admin));
                }
                return new AdminViewModelCollection() { AdminViewModels = adminViewModels };
            }
        }
        public static IEnumerable<string> GetRolesStringsByLogin(string login)
        {
            return RoleCutter(GetAdminByLogin(login).Roles);
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
            return roleString.Split('*');
        }
        public static IEnumerable<Roles> ConvertStringToRoles(string roleString)
        {
            return ConvertStringToRoles(RoleCutter(roleString));
        }
        public static IEnumerable<Roles> ConvertStringToRoles(IEnumerable<string> roleStrings)
        {
            List<Roles> roles = new List<Roles>();
            foreach(string role in roleStrings)
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
            return string.Join("*", temp);
        }
        public static bool AddAdmin(AdminCreateModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                if (session.Query<Admin>().FirstOrDefault(u => u.Login == model.Login) != null)
                    return false;
                Admin admin = new Admin() { Login = model.Login, Email = model.Email};
                Admin.SetPassword(admin, model.Password);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(admin);
                    transaction.Commit();
                }
            }
            return true;
        }
        public static void RemoveAdmin(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                Admin admin = session.Get<Admin>(id);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(admin);
                    transaction.Commit();
                }
            }
        }
        public static void UpdateAdmin(AdminEditModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                Admin admin = session.Get<Admin>(model.Id);
                admin.Email = model.Email;
                admin.Roles = string.Join("*", model.Roles);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(admin);
                    transaction.Commit();
                }
            }
        }
    }
}