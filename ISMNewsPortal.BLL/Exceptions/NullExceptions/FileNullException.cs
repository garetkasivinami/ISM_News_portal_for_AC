using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class FileNullException : Exception
    {
        public FileNullException(string message = "File is null") : base(message)
        {

        }
    }
}
