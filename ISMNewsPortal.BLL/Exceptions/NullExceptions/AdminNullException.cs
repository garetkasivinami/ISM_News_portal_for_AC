using System;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class AdminNullException : Exception
    {
        public AdminNullException(string message = "Admin is null!") : base(message)
        {

        }
    }
}
