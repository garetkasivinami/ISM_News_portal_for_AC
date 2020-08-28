using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ISMNewsPortal.BLL;
using System.Configuration;
using ISMNewsPortal.Config;
using ISMNewsPortal.BLL.Repositories;
using ISMNewsPortal.ConnectionBuilders;
using ISMNewsPortal.Lucene;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.CL.Repositories;
using ISMNewsPortal.BLL.Models;

namespace ISMNewsPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var typeConnectionString = ConfigurationManager.AppSettings["typeConnection"];
            var typeConnection = GetTypeConnection(typeConnectionString);

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

            string luceneFolderRelativePath = ConfigurationManager.ConnectionStrings["lucene"].ConnectionString;
            string lucenePath = $"{luceneFolderRelativePath}/{typeConnectionString}";
            var luceneRepositoryFactory = new LuceneRepositoryFactory(lucenePath);
            SessionManager.SetLuceneRepositoryFactory(luceneRepositoryFactory);

            CacheRepository cacheRepository = new CacheRepository();
            SessionManager.SetCacheRepository(cacheRepository);

            connectionBuilder.CreateRepositories();

            var updateLuceneValue = ConfigurationManager.AppSettings["updateLucene"];
            if (bool.Parse(updateLuceneValue))
                new NewsPostService().UpdateLucene();

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
