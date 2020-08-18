using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ISMNewsPortal.BLL;
using System.Configuration;
using ISMNewsPortal.Config;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.ConnectionBuilders;

namespace ISMNewsPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            TypeConnection typeConnection = GetTypeConnection(ConfigurationManager.AppSettings["typeConnection"]);

            ConnectionBuilder connectionBuilder;
            switch (typeConnection) {
                case TypeConnection.XML:
                    connectionBuilder = new XMLConnection();
                    break;
                case TypeConnection.Hibernate:
                    connectionBuilder = new HibernateConnection();
                    break;
                default:
                    throw new Exception("Unknown type of connection.");
            }

            IUnitOfWork unitOfWork = connectionBuilder.GetUnitOfWork();
            SessionManager.SetUnitOfWork(unitOfWork);

            connectionBuilder.CreateRepositories();

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
}
