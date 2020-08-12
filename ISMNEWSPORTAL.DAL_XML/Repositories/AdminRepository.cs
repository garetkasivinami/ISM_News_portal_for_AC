using System;
using System.Collections.Generic;
using System.Linq;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Repositories;

namespace ISMNewsPortal.DAL_XML.Repositories
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        public AdminRepository(XMLContex contex) : base(contex)
        {
            
        }

        public Admin GetByEmail(string email)
        {
            return GetAll().SingleOrDefault(u => u.Email == email);
        }

        public Admin GetByLogin(string login)
        {
            return GetAll().SingleOrDefault(u => u.Login == login);
        }

        public IEnumerable<Admin> GetByRole(string role)
        {
            return GetAll().Where(u => Array.IndexOf(u.Roles.Split(','), role) != -1);
        }
    }
}
