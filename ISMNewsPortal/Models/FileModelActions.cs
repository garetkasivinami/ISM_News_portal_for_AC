using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ISMNewsPortal.Models
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

            FileModel equalFileModel = FindByHashCode(hashCode);
            if (equalFileModel != null)
                return equalFileModel.Id;

            string fileName = Path.GetFileName(file.FileName);
            fileName = DateTime.Now.Ticks + Path.GetExtension(fileName);
            string path = server.MapPath("~/App_Data/Files/" + fileName);
            file.SaveAs(path);

            file.InputStream.Close();

            return FileModel.Save(fileName, hashCode);
        }
        public static void RemoveFile(int id, HttpServerUtilityBase server)
        {
            string fileName = GetNameById(id);
            string path = server.MapPath("~/App_Data/Files/" + fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                FileModel.Delete(id);
            }
        }
        public static FileModel FindByHashCode(string hashCode)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                return session.Query<FileModel>().SingleOrDefault(u => u.HashCode == hashCode);
            }
        }
        public static string GetNameById(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                return session.Get<FileModel>(id).Name;
            }
        }
        public static string GetNameByIdFormated(int id)
        {
            return $"/Files/?name={GetNameById(id)}";
        }
    }
}