using ISMNewsPortal.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Interfaces
{
    public interface IAdminRepository : IRepository<AdminDTO>
    {
        AdminDTO GetByLogin(string login);
        AdminDTO GetByEmail(string email);
        IEnumerable<AdminDTO> GetByRole(string role);
    }
}
