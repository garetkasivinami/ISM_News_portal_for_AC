using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class CommentNullException : Exception
    {
        public CommentNullException(string message = "Comment is null") : base (message)
        {

        }
    }
}
