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
        //=========================================
        public static bool AddAdmin(AdminCreateModel model)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                if (session.Query<Admin>().FirstOrDefault(u => u.Login == model.Login) != null)
                    return false;
                Admin admin = new Admin() { Login = model.Login, Email = model.Email };
                AdminHelperActions.SetPassword(admin, model.Password);
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
                if (session.Query<Admin>().Count() == 1)
                    return;
                Admin admin = session.Get<Admin>(id);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(admin);
                    transaction.Commit();
                }
            }
        }
        public static void UpdateAdmin(AdminEditModel model, bool updateRoles)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                Admin admin = session.Get<Admin>(model.Id);
                admin.Email = model.Email;
                if (updateRoles)
                    admin.Roles = string.Join("*", model.Roles ?? new string[0]);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(admin);
                    transaction.Commit();
                }
            }
        }
        public static void UpdateAdmin(Admin admin)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(admin);
                    transaction.Commit();
                }
            }
        }
    }
}