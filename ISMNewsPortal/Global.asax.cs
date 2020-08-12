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
        protected void Application_Start()
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", "datebase.xml");

            TypeConnection typeConnection = GetTypeConnection(ConfigurationManager.AppSettings["typeConnection"]);

            switch(typeConnection) {
                case TypeConnection.XML:
                    ConnectionBuilder.BuildXMLSession();
                    break;
                case TypeConnection.Hibernate:
                    ConnectionBuilder.BuildHibernateSession();
                    break;
                default:
                    throw new Exception("Unknown type of connection.");
            }

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
    public static class ConnectionBuilder
    {
        private static bool connectionBuilded;
        public static void BuildHibernateSession()
        {
            ConnectionBuildExceptionCheck();

            var session = NHibernateSession.OpenSession();

            IUnitOfWork unitOfWork = new HibernateUnitOfWork(session);
            var adminRepository = new DAL.Repositories.AdminRepository(session);
            var commentRepository = new DAL.Repositories.CommentRepository(session);
            var newsPostRepository = new DAL.Repositories.NewsPostRepository(session);
            var fileRepository = new DAL.Repositories.FileRepository(session);

            UnitOfWorkManager.SetUnitOfWork(unitOfWork, adminRepository, commentRepository, newsPostRepository, fileRepository);
            connectionBuilded = true;
        }
        public static void BuildXMLSession()
        {
            ConnectionBuildExceptionCheck();

            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", "datebase.xml");
            XMLContex xmlContex = new XMLContex(path);

            var adminRepository = new DAL_XML.Repositories.AdminRepository(xmlContex);
            var commentRepository = new DAL_XML.Repositories.CommentRepository(xmlContex);
            var newsPostRepository = new DAL_XML.Repositories.NewsPostRepository(xmlContex);
            var fileRepository = new DAL_XML.Repositories.FileRepository(xmlContex);

            IUnitOfWork unitOfWork = new XMLUnitOfWork(xmlContex);

            UnitOfWorkManager.SetUnitOfWork(unitOfWork, adminRepository, commentRepository, newsPostRepository, fileRepository);
            connectionBuilded = true;
        }
        private static void ConnectionBuildExceptionCheck()
        {
            if (connectionBuilded)
                throw new Exception("Connection has been builded!");
        }
    }
}
