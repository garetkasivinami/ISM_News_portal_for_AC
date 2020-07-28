﻿using ISMNewsPortal.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Repositories
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Admin GetByLogin(string login);
        Admin GetByEmail(string email);
        IEnumerable<Admin> GetByRole(string role);
    }
}