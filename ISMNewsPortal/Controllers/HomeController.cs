using ISMNewsPortal.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ICollection<NewsPost> newsPosts;
            using (ISession session = NHibernateSession.OpenSession())
            {
                newsPosts = session.Query<NewsPost>().ToList();
            }
            return View(newsPosts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}