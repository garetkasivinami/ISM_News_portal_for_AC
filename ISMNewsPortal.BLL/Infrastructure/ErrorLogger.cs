using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ISMNewsPortal.BLL.Infrastructure
{
    public static class ErrorLogger
    {
        static string path = HttpContext.Current.Server.MapPath("~/App_Data/Files/log.txt");
        public static void LogError(string message)
        {
            string delim = "\n============================================================================================";
            File.AppendAllText(path, $"\n{DateTime.Now} - Error!\n" + message + delim);
        }
    }
}