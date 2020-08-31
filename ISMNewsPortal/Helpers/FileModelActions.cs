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
        public static FileService FileService { get; private set; }

        static FileModelActions()
        {
            FileService = new FileService();
        }

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
            var equalFileModelDTO = FileService.FindByHashCode(hashCode);
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
            return FileService.CreateFile(fileModelDTO);
        }

        public static void RemoveFile(int id, HttpServerUtilityBase server)
        {
            string fileName = FileService.GetNameById(id);
            string path = server.MapPath("~/App_Data/Files/" + fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                FileService.SafeDeleteFile(id);
            }
        }

        public static string GetNameByIdFormated(int id)
        {
            return $"/Files/?name={FileService.GetNameById(id)}";
        }
    }
}