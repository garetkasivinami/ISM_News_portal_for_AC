using ISMNewsPortal.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;

namespace ISMNewsPortal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult LoginAdmin()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                Users currentUser = Users.GetUserByLogin(User.Identity.Name, session);
                if (currentUser == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                Admin admin = session.Query<Admin>().FirstOrDefault(u => u.UserId == currentUser.Id);
                if (admin == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                Session["AdminAccessLevel"] = admin.AccessLevel;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            if (Session["AdminAccessLevel"] == null)
            {
                return RedirectToAction("LoginAdmin");
            }
            return View();
        }
        public ActionResult News()
        {
            ICollection<NewsPostAdminView> newsPostsAdminView = new List<NewsPostAdminView>();
            using (ISession session = NHibernateSession.OpenSession())
            {
                IQueryable<NewsPost> newsPosts = session.Query<NewsPost>();
                foreach (NewsPost newsPost in newsPosts)
                {
                    newsPostsAdminView.Add(new NewsPostAdminView(newsPost));
                }
            }
            return View(newsPostsAdminView);
        }
        public ActionResult Edit(int? id)
        {
            NewsPostAdminView newsPostAdminView;
            if (id != null)
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    NewsPost newsPost = session.Get<NewsPost>(id);
                    if (newsPost == null)
                    {
                        return RedirectToAction("Index");
                    }
                    newsPostAdminView = new NewsPostAdminView(newsPost);

                }
                return View(newsPostAdminView);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsPostAdminView newsPostAdminView)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    NewsPost newsPost = new NewsPost();
                    newsPost.Id = newsPostAdminView.Id;
                    newsPost.ImagePath = newsPostAdminView.ImagePath;
                    newsPost.LikesCount = newsPostAdminView.LikesCount;
                    newsPost.CommentsCount = newsPostAdminView.CommentsCount;
                    newsPost.CreatedDate = newsPostAdminView.CreatedDate;
                    newsPost.Author = session.Get<Users>(newsPostAdminView.AuthorId);
                    newsPost.Name = newsPostAdminView.Name;
                    newsPost.ForRegistered = newsPostAdminView.ForRegistered;
                    newsPost.Descrition = newsPostAdminView.Descrition;
                    newsPost.EditDate = DateTime.Now;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(newsPost);
                        transaction.Commit();
                    }
                }
            }
            return RedirectToAction("News");
        }
        [HttpGet]
        [Authorize]
        public ActionResult DeleteRequest(int? id)
        {
            NewsPostAdminView newsPostAdminView;
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                if (newsPost == null)
                {
                    return RedirectToAction("Index");
                }
                newsPostAdminView = new NewsPostAdminView(newsPost);
            }
            return View(newsPostAdminView);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                if (newsPost == null)
                {
                    return RedirectToAction("Index");
                }
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(newsPost);
                    transaction.Commit();
                }
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult CreateNews()
        {
            return View();
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateNews(NewsPostModelCreate model)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Users currentUser = Users.GetUserByLogin(User.Identity.Name);
                    if (currentUser == null)
                    {
                        return RedirectToAction("Logoff", "Account");
                    }
                    NewsPost newsPost = new NewsPost();
                    newsPost.Author = currentUser;
                    newsPost.CommentsCount = 0;
                    newsPost.CreatedDate = DateTime.Now;
                    newsPost.Descrition = model.Desc;
                    newsPost.EditDate = null;
                    newsPost.ForRegistered = model.ForRegistered;
                    newsPost.ImagePath = model.ImagePath;
                    newsPost.LikesCount = 0;
                    newsPost.Name = model.Name;
                    newsPost.Likes = new List<UserLike>();
                    newsPost.Comments = new List<Comment>();
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(newsPost);
                        transaction.Commit();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
    }
}