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
            HibernateUnitOfWork unit = new HibernateUnitOfWork(NHibernateSession.OpenSession());
            UnitOfWorkManager.SetUnitOfWork(unit);
            GlobalFilters.Filters.Add(new ElmahExceptionLogger());

            string path = "C:\\Test\\";
            using (XMLContex contex = new XMLContex(path))
            {
                contex.CreateRange(new Admin() { Email = "222", Login = "2223", Password = "42342", Salt = "325fds"});

                contex.UpdateRange(new Admin() { Email = "222", Login = "2223", Password = "42342", Salt = "325fdssss" });

                contex.DeleteRange<Admin>(4);
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
