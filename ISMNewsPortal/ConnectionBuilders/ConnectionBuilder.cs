using ISMNewsPortal.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.ConnectionBuilders
{
    public abstract class ConnectionBuilder
    {
        public abstract void CreateRepositories();
        public abstract IUnitOfWork GetUnitOfWork();
    }
}