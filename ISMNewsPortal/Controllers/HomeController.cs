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
        public ActionResult Index(int? page, string sortType, string filter)
        {
            int numberPage = page ?? 0;
            int pages;
            List<NewsPost> newsPosts;
            Func<NewsPost, bool> filterFunc;
            Func<NewsPost, object> sortFunc;
            switch (filter)
            {
                case "today":
                    filterFunc = NewsPostHelperActions.FilterToday;
                    break;
                case "yesterday":
                    filterFunc = NewsPostHelperActions.FilterYesterday;
                    break;
                case "week":
                    filterFunc = NewsPostHelperActions.FilterWeek;
                    break;
                default:
                    filter = null;
                    filterFunc = NewsPostHelperActions.FilterAll;
                    break;
            }
            switch (sortType)
            {
                case "name":
                    sortFunc = u => u.Name;
                    break;
                case "description":
                    sortFunc = u => u.Description;
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

                int newsCount = selectedNewsPost.Count();
                pages = newsCount / NewsPost.NewsInOnePage;
                if (newsCount % NewsPost.NewsInOnePage != 0)
                    pages++;
                selectedNewsPost = selectedNewsPost.Skip(NewsPost.NewsInOnePage * numberPage).Take(NewsPost.NewsInOnePage);
                newsPosts = selectedNewsPost.ToList();
                ICollection<NewsPostSimplifiedView> newsPostSimplifyViews = new List<NewsPostSimplifiedView>();
                foreach (NewsPost newsPost in newsPosts)
                {
                    int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                    newsPostSimplifyViews.Add(new NewsPostSimplifiedView(newsPost, commentCount));
                }
                //using (ITransaction transaction = session.BeginTransaction())
                //{
                //    Admin admin = new Admin();
                //    admin.Login = "Big man";
                //    Admin.SetPassword(admin, "12345678");
                //    session.Save(admin);
                //    transaction.Commit();
                //}
                return View(new NewsPostSimplifiedCollection()
                {
                    NewsPostSimpliedViews = newsPostSimplifyViews,
                    Pages = pages,
                    Page = numberPage,
                    Filter = filter,
                    SortType = sortType
                });
            }

        }
    }
}