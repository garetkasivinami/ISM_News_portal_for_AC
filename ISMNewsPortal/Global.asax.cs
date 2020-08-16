using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ISMNewsPortal.DAL.Repositories;
using ISMNewsPortal.DAL_XML.Repositories;
using ISMNewsPortal.BLL;
using ISMNewsPortal.BLL.Models;
using System.Configuration;
using ISMNewsPortal.Config;
using ISMNewsPortal.BLL.Repositories;
using System.IO;
using System.Web.Security;

namespace ISMNewsPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected static ConnectionBuilder connectionBuilder;
        protected void Application_Start()
        {
            TypeConnection typeConnection = GetTypeConnection(ConfigurationManager.AppSettings["typeConnection"]);

            switch(typeConnection) {
                case TypeConnection.XML:
                    connectionBuilder = new XMLConnection();
                    break;
                case TypeConnection.Hibernate:
                    connectionBuilder = new HibernateConnection();
                    break;
                default:
                    throw new Exception("Unknown type of connection.");
            }

            connectionBuilder.CreateRepositories();

            IUnitOfWork unitOfWork = connectionBuilder.GetUnitOfWork();
            SessionManager.SetUnitOfWork(unitOfWork);

            GlobalFilters.Filters.Add(new ElmahExceptionLogger());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private TypeConnection GetTypeConnection(string type)
        {
            switch (type)
            {
                case "xml":
                    return TypeConnection.XML;
                default:
                    return TypeConnection.Hibernate;
            }
        }
    }
    public abstract class ConnectionBuilder
    {
        public abstract void CreateRepositories();
        public abstract IUnitOfWork GetUnitOfWork();
    }
    public class HibernateConnection : ConnectionBuilder
    {
        public override void CreateRepositories()
        {
            var adminRepository = new DAL.Repositories.AdminRepository();
            var commentRepository = new DAL.Repositories.CommentRepository();
            var newsPostRepository = new DAL.Repositories.NewsPostRepository();
            var fileRepository = new DAL.Repositories.FileRepository();

            SessionManager.SetRepositories(adminRepository, commentRepository, newsPostRepository, fileRepository);
        }

        public override IUnitOfWork GetUnitOfWork()
        {
            return new HibernateUnitOfWork();
        }
    }

    public class XMLConnection : ConnectionBuilder
    {
        private XMLContex xmlContex;
        public override void CreateRepositories()
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", "datebase.xml");
            xmlContex = new XMLContex(path);

            var adminRepository = new DAL_XML.Repositories.AdminRepository(xmlContex);
            var commentRepository = new DAL_XML.Repositories.CommentRepository(xmlContex);
            var newsPostRepository = new DAL_XML.Repositories.NewsPostRepository(xmlContex);
            var fileRepository = new DAL_XML.Repositories.FileRepository(xmlContex);

            SessionManager.SetRepositories(adminRepository, commentRepository, newsPostRepository, fileRepository);
        }

        public override IUnitOfWork GetUnitOfWork()
        {
            return new XMLUnitOfWork(xmlContex);
        }
    }
}
