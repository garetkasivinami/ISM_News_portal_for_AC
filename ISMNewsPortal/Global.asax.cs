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

namespace ISMNewsPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            string path = "C:\\Test\\";
            HibernateUnitOfWork unit = new HibernateUnitOfWork(NHibernateSession.OpenSession());
            XMLUnitOfWork xmlUnitOfWork = new XMLUnitOfWork(new XMLContex(path));

            UnitOfWorkManager.SetUnitOfWork(xmlUnitOfWork);
            GlobalFilters.Filters.Add(new ElmahExceptionLogger());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
