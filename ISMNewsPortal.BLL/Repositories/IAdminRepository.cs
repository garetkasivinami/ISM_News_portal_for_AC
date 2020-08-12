using ISMNewsPortal.BLL.Models;
using System.Collections.Generic;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Admin GetByLogin(string login);
        Admin GetByEmail(string email);
        IEnumerable<Admin> GetByRole(string role);
    }
}
