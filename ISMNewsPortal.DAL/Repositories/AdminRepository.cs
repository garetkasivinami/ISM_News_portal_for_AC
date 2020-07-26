using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.BusinessModels;

namespace ISMNewsPortal.DAL.Repositories
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository(ISession session) : base(session)
        {

        }

        public Admin GetByEmail(string email)
        {
            return _session.Query<Admin>().FirstOrDefault(u => u.Email == email);
        }

        public Admin GetByLogin(string login)
        {
            return _session.Query<Admin>().SingleOrDefault(u => u.Login == login);
        }

        public IEnumerable<Admin> GetByRole(string role)
        {
            return _session.Query<Admin>().Where(u => Array.IndexOf(u.Roles.Split(','), role) != -1);
        }
    }
}
