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
        public const int NewsInOnePage = 10;
        public ActionResult Index(int? page, string sortType, string filter)
        {
            int numberPage = page ?? 0;
            int pages;
            List<NewsPost> newsPosts;
            Func<NewsPost, bool> filterFunc;
            Func<NewsPost, object> sortFunc;
            switch(filter)
            {
                case "today":
                    filterFunc = FilterToday;
                    break;
                case "yesterday":
                    filterFunc = FilterYesterday;
                    break;
                case "week":
                    filterFunc = FilterWeek;
                    break;
                default:
                    filter = null;
                    filterFunc = FilterAll;
                    break;
            }
            switch(sortType)
            {
                case "name":
                    sortFunc = u => u.Name;
                    break;
                case "description":
                    sortFunc = u => u.Descrition;
                    break;
                default:
                    sortType = null;
                    sortFunc = u => long.MaxValue - u.CreatedDate.Ticks;
                    break;
            }
            using (ISession session = NHibernateSession.OpenSession())
            {
                IEnumerable<NewsPost> selectedNewsPost = session.Query<NewsPost>().Where(filterFunc).
                    OrderBy(sortFunc);

                if (!User.Identity.IsAuthenticated)
                    selectedNewsPost = selectedNewsPost.Where(u => u.ForRegistered == false);
                int newsCount = selectedNewsPost.Count();
                pages = newsCount / NewsInOnePage;
                if (newsCount % NewsInOnePage != 0)
                {
                    pages++;
                }
                selectedNewsPost = selectedNewsPost.Skip(NewsInOnePage * numberPage).Take(NewsInOnePage);
                newsPosts = selectedNewsPost.ToList();
            }
            ICollection<NewsPostSimplifyView> newsPostSimplifyViews = new List<NewsPostSimplifyView>();
            foreach (NewsPost newsPost in newsPosts)
            {
                newsPostSimplifyViews.Add(new NewsPostSimplifyView(newsPost));
            }
            return View(new NewsPostSimplifyCollection()
            {
                NewsPostSimplifyViews = newsPostSimplifyViews,
                pages = pages,
                currentPage = numberPage,
                sortType = sortType,
                filter = filter
            });
        }
        private bool FilterToday(NewsPost newsPost)
        {
            return newsPost.CreatedDate.Day == DateTime.Now.Day;
        }
        private bool FilterYesterday(NewsPost newsPost)
        {
            return newsPost.CreatedDate.Day == DateTime.Now.AddDays(-1).Day;
        }
        private bool FilterWeek(NewsPost newsPost)
        {
            return (DateTime.Now - newsPost.CreatedDate).Days < 7;
        }
        private bool FilterAll(NewsPost newsPost)
        {
            return true;
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