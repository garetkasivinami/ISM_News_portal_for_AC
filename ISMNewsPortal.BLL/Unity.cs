using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISMNewsPortal.BLL.Repositories;

namespace ISMNewsPortal.BLL
{
    public static class Unity
    {
        private static IUnitOfWork unitOfWork;

        public static IUnitOfWork UnitOfWork 
        {
            get
            {
                return unitOfWork;
            }
        }

        public static void SetUnitOfWork(IUnitOfWork targetUnitOfWork)
        {
            unitOfWork = targetUnitOfWork;
        }
    }
}
