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
        public AdminRepository(HibernateUnitOfWork hibernateUnitOfWork) : base(hibernateUnitOfWork)
        {

        }

        public Admin GetByEmail(string email)
        {
            return hibernateUnitOfWork.Session.Query<Admin>().FirstOrDefault(u => u.Email == email);
        }

        public Admin GetByLogin(string login)
        {
            return hibernateUnitOfWork.Session.Query<Admin>().SingleOrDefault(u => u.Login == login);
        }

        public IEnumerable<Admin> GetByRole(string role)
        {
            return hibernateUnitOfWork.Session.Query<Admin>().Where(u => Array.IndexOf(u.Roles.Split(','), role) != -1);
        }
    }
}
