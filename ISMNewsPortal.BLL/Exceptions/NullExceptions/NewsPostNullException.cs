using System;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class NewsPostNullException : Exception
    {
        public NewsPostNullException(string message = "News post is null") : base(message)
        {

        }
    }
}
