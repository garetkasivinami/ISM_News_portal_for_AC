using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Services;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ISMNewsPortal.Helpers
{
    public static class FileModelActions
    {
        public static int SaveFile(HttpPostedFileBase file, HttpServerUtilityBase server)
        {
            byte[] hashBytes;
            using (MD5 md5 = MD5.Create())
            {
                Stream stream = file.InputStream;
                hashBytes = md5.ComputeHash(stream);
                stream.Seek(0, SeekOrigin.Begin);
            }
            string hashCode = System.Text.Encoding.Unicode.GetString(hashBytes);
            FileService fileService = new FileService();
            var equalFileModelDTO = fileService.FindByHashCode(hashCode);
            if (equalFileModelDTO != null)
                return equalFileModelDTO.Id;

            string fileName = Path.GetFileName(file.FileName);
            string path = server.MapPath("~/App_Data/Files/" + fileName);
            if (File.Exists(path))
            {
                fileName = $"{DateTime.Now.Ticks}_{fileName}";
                path = server.MapPath("~/App_Data/Files/" + fileName);
            }
            file.SaveAs(path);

            file.InputStream.Close();
            var fileModelDTO = new FileModel() { HashCode = hashCode, Name = fileName };
            return fileService.CreateFile(fileModelDTO);
        }

        public static void RemoveFile(int id, HttpServerUtilityBase server)
        {
            FileService fileService = new FileService();
            string fileName = fileService.GetNameById(id);
            string path = server.MapPath("~/App_Data/Files/" + fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                fileService.SafeDeleteFile(id);
            }
        }

        public static string GetNameByIdFormated(int id)
        {
            FileService fileService = new FileService();
            return $"/Files/?name={fileService.GetNameById(id)}";
        }
    }
}