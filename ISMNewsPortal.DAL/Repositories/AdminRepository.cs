using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.BLL.Models;

namespace ISMNewsPortal.DAL.Repositories
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository() : base()
        {

        }

        public Admin GetByEmail(string email)
        {
            return NHibernateSession.Session.Query<Admin>().FirstOrDefault(u => u.Email == email);
        }

        public Admin GetByLogin(string login)
        {
            return NHibernateSession.Session.Query<Admin>().SingleOrDefault(u => u.Login == login);
        }
    }
}
