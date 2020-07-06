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
        public virtual string AdminAccess { get; set; }
        //==================================================================
        public static void SetPassword(Admin admin, string password)
        {
            string salt;
            admin.Password = Security.SHA512(password, out salt);
            admin.Salt = salt;
        }
        public static Admin GetAdminByLogin(string login)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                return session.Query<Admin>().SingleOrDefault(u => u.Login == login);
            }
        }
        public static int GetAdminIdByLogin(string login)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                return session.Query<Admin>().SingleOrDefault(u => u.Login == login).Id;
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
                    adminViewModels.Add(new AdminViewModel()
                    {
                        Id = admin.Id,
                        Login = admin.Login,
                        AdminAccess = admin.AdminAccess
                    });
                }
                return new AdminViewModelCollection() { AdminViewModels = adminViewModels };
            }
        }
    }
}