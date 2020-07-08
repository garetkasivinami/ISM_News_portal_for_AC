using ISMNewsPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static int SaveFile(HttpPostedFileBase file, HttpServerUtilityBase server)
        {
            string fileName = Path.GetFileName(file.FileName);
            fileName = DateTime.Now.Ticks + Path.GetExtension(fileName);
            string path = server.MapPath("~/App_Data/Files/" + fileName);
            file.SaveAs(path);

            return FileModel.Save(fileName);
        }
        public static void RemoveFile(int id, HttpServerUtilityBase server)
        {
            string fileName = FileModel.GetNameById(id);
            string path = server.MapPath("~/App_Data/Files/" + fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                FileModel.Delete(id);
            }
        }
    }
}