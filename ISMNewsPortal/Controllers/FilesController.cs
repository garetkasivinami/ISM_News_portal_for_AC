using ISMNewsPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Controllers
{
    public class FilesController : Controller
    {
        // GET: Files
        public ActionResult GetFile(string name)
        {
            string file_path = ($"~/App_Data/Files/{name}");
            string file_type = "image/png";
            return new FilePathResult(file_path, file_type);
        }
    }
}