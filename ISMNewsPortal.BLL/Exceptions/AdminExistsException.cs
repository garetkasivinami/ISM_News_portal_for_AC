using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class AdminExistsException : Exception
    {
        public AdminExistsException (string message = "An administrator already exists") : base(message)
        {

        }
    }
}
