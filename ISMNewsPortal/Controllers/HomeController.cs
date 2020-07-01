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
            List<NewsPost> newsPosts;
            using (ISession session = NHibernateSession.OpenSession())
            {
                newsPosts = session.Query<NewsPost>().ToList();
                if (!User.Identity.IsAuthenticated)
                    newsPosts = newsPosts.Where(u => u.ForRegistered == false).ToList();
            }
            ICollection<NewsPostSimplifyView> newsPostSimplifyViews = new List<NewsPostSimplifyView>();
            for (int i = newsPosts.Count - 1; i >= 0; i--)
            {
                newsPostSimplifyViews.Add(new NewsPostSimplifyView(newsPosts[i]));
            }
            return View(newsPostSimplifyViews);
        }

        public ActionResult About()
        {
            ViewBag.Message = "About ACME COSMETICS";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}