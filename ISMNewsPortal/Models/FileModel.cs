using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISMNewsPortal.Models
{
    public class FileModel
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        //=========================================================
        public FileModel()
        {

        }
        public FileModel(string fileName)
        {
            Name = fileName;
        }
        public static int Save(string fileName)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                FileModel fileModel = new FileModel(fileName);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(fileModel);
                    transaction.Commit();
                }
                return session.Query<FileModel>().Max(u => u.Id);
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
            return $"/Files/?name={FileModel.GetNameById(id)}";
        }
        public static void Delete(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                FileModel fileModel = session.Get<FileModel>(id);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(fileModel);
                    transaction.Commit();
                }
            }
        }
        public static void Delete(string fileName)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                FileModel fileModel = session.Query<FileModel>().SingleOrDefault(u => u.Name == fileName);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(fileModel);
                    transaction.Commit();
                }
            }
        }
    }
}