using System;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class CommentNullException : Exception
    {
        public CommentNullException(string message = "Comment is null") : base (message)
        {

        }
    }
}
