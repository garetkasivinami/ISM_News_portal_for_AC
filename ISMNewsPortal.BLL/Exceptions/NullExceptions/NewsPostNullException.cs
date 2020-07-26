using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class NewsPostNullException : Exception
    {
        public NewsPostNullException(string message = "News post is null") : base(message)
        {

        }
    }
}
