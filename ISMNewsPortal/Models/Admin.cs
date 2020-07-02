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
    }
    public class AdminViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string AdminAccess { get; set; }
    }
    public class AdminViewModelCollection
    {
        public ICollection<AdminViewModel> AdminViewModels { get; set; }
    }
}