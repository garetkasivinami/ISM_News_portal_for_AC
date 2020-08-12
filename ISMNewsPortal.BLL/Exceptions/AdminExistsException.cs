using System;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class AdminExistsException : Exception
    {
        public AdminExistsException (string message = "An administrator already exists") : base(message)
        {

        }
    }
}
