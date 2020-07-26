using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class AdminNullException : Exception
    {
        public AdminNullException(string message = "Admin is null!") : base(message)
        {

        }
    }
}
