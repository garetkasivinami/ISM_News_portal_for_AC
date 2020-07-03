using ISMNewsPortal.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Serialization;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls.WebParts;

namespace ISMNewsPortal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult News()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                IQueryable<NewsPost> newsPosts = session.Query<NewsPost>();
                ICollection<NewsPostAdminView> newsPostsAdminView = new List<NewsPostAdminView>();
                foreach (NewsPost newsPost in newsPosts)
                {
                    string newsPostAuthorName = session.Get<Admin>(newsPost.AuthorId).Login;
                    int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                    newsPostsAdminView.Add(new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount));
                }
                return View(new NewsPostAdminCollection() { NewsPostAdminViews = newsPostsAdminView, ViewActionLinks = true });
            }
        }
        public ActionResult Edit(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                if (newsPost == null)
                    return RedirectToAction("Index");
                string newsPostAuthorName = session.Get<Admin>(newsPost.AuthorId).Login;
                int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                NewsPostAdminView newsPostAdminView = new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount);
                return View(newsPostAdminView);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsPostAdminView newsPostAdminView)
        {
            if (ModelState.IsValid)
                using (ISession session = NHibernateSession.OpenSession())
                {
                    NewsPost newsPost = new NewsPost();
                    newsPost.Id = newsPostAdminView.Id;
                    newsPost.ImagePath = newsPostAdminView.ImagePath;
                    newsPost.CreatedDate = newsPostAdminView.CreatedDate;
                    newsPost.AuthorId = newsPostAdminView.AuthorId;
                    newsPost.Name = newsPostAdminView.Name;
                    newsPost.Description = newsPostAdminView.Description;
                    newsPost.EditDate = DateTime.Now;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(newsPost);
                        transaction.Commit();
                    }
                }
            return RedirectToAction("News");
        }
        [HttpGet]
        public ActionResult DeleteRequest(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                if (newsPost == null)
                    return RedirectToAction("Index");
                string newsPostAuthorName = session.Get<Admin>(newsPost.AuthorId).Login;
                int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                NewsPostAdminView newsPostAdminView = new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount);
                return View(newsPostAdminView);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                if (newsPost == null)
                    return RedirectToAction("Index");
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateNews(NewsPostModelCreate model)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    NewsPost newsPost = new NewsPost();
                    newsPost.AuthorId = Admin.GetAdminIdByLogin(User.Identity.Name);
                    newsPost.CreatedDate = DateTime.Now;
                    newsPost.Description = model.Description;
                    newsPost.EditDate = null;
                    newsPost.ImagePath = model.ImagePath;
                    newsPost.Name = model.Name;

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
        public ActionResult AdminList()
        {
            ICollection<AdminViewModel> adminViewModels = new List<AdminViewModel>();
            using (ISession session = NHibernateSession.OpenSession())
            {
                IEnumerable<Admin> admins = session.Query<Admin>();
                foreach (Admin admin in admins)
                {
                    adminViewModels.Add(new AdminViewModel()
                    {
                        Id = admin.Id,
                        Login = admin.Login,
                        AdminAccess = admin.AdminAccess
                    });
                }
            }
            return View(new AdminViewModelCollection() { AdminViewModels = adminViewModels});
        }
        public ActionResult CreateAdmin()
        {
            return View();
        }
        public ActionResult Comments()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                List<CommentViewModel> commentViewModels = new List<CommentViewModel>();
                IQueryable<Comment> comments = session.Query<Comment>();
                foreach(Comment comment in comments)
                {
                    commentViewModels.Add(new CommentViewModel(comment));
                }
                return View(new CommentViewModelCollection() { CommentViewModels = commentViewModels});
            }
        }
    }
}