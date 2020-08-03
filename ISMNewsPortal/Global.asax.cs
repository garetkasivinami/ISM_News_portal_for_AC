using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ISMNewsPortal.DAL.Repositories;
using ISMNEWSPORTAL.DAL_XML.Repositories;
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

            IUnitOfWork unitOfWork;
            switch(typeConnection) {
                case TypeConnection.XML:
                    unitOfWork = new XMLUnitOfWork(new XMLContex(path));
                    break;
                case TypeConnection.Hibernate:
                    unitOfWork = new HibernateUnitOfWork(NHibernateSession.OpenSession());
                    break;
                default:
                    unitOfWork = null;
                    break;
            }

            UnitOfWorkManager.SetUnitOfWork(unitOfWork);
            GlobalFilters.Filters.Add(new ElmahExceptionLogger());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e)
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", "datebase.xml");
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
}
