using System;

namespace ISMNewsPortal.BLL.Exceptions
{
    public class FileNullException : Exception
    {
        public FileNullException(string message = "File is null") : base(message)
        {

        }
    }
}
