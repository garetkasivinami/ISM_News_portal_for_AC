using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ISMNewsPortal.DAL.Repositories;
using ISMNewsPortal.BLL;

namespace ISMNewsPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            EFUnitOfWork unit = new EFUnitOfWork(NHibernateSession.OpenSession());
            UnitOfWorkManager.SetUnitOfWork(unit);
            GlobalFilters.Filters.Add(new ElmahExceptionLogger());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
