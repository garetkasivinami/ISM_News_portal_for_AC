using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ISMNewsPortal.BLL.Infrastructure
{
    public static class ExceptionGenerator
    {
        public static Exception GenerateException (string message, string methodInfo, params string[] parameters)
        {
            StringBuilder exceptionMessageBuilder = new StringBuilder();
            exceptionMessageBuilder.AppendLine(message);
            exceptionMessageBuilder.AppendLine(methodInfo);
            foreach(string parameter in parameters)
            {
                exceptionMessageBuilder.AppendLine(parameter);
            }
            return new Exception(exceptionMessageBuilder.ToString());
        }
    }
}
