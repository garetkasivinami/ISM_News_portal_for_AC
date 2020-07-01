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
                    return RedirectToAction("Index", "Home");
                Admin admin = session.Query<Admin>().FirstOrDefault(u => u.UserId == currentUser.Id);
                if (admin == null)
                    return RedirectToAction("Index", "Home");
                Session["AdminAccessLevel"] = admin.AccessLevel;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            if (GetAdminAccessLevel() == -1)
                return RedirectToAction("LoginAdmin");
            return View();
        }
        public ActionResult News()
        {
            int adminAccessLevel = GetAdminAccessLevel();
            if (adminAccessLevel == -1)
                return RedirectToAction("LoginAdmin");
            ICollection<NewsPostAdminView> newsPostsAdminView = new List<NewsPostAdminView>();
            using (ISession session = NHibernateSession.OpenSession())
            {
                IQueryable<NewsPost> newsPosts = session.Query<NewsPost>();
                foreach (NewsPost newsPost in newsPosts)
                {
                    newsPostsAdminView.Add(new NewsPostAdminView(newsPost));
                }
            }
            return View(new NewsPostAdminCollection() { NewsPostAdminViews = newsPostsAdminView, ViewActionLinks = adminAccessLevel >= 2 });
        }
        public ActionResult Edit(int? id)
        {
            int adminAccessLevel = GetAdminAccessLevel();
            if (adminAccessLevel == -1)
                return RedirectToAction("LoginAdmin");
            if (adminAccessLevel < 2)
                return RedirectToAction("News");
            NewsPostAdminView newsPostAdminView;
            if (id != null)
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    NewsPost newsPost = session.Get<NewsPost>(id);
                    if (newsPost == null)
                        return RedirectToAction("Index");
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
            int adminAccessLevel = GetAdminAccessLevel();
            if (adminAccessLevel == -1)
                return RedirectToAction("News");
            if (adminAccessLevel < 2)
                return RedirectToAction("News");
            if (ModelState.IsValid)
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
            return RedirectToAction("News");
        }
        [HttpGet]
        [Authorize]
        public ActionResult DeleteRequest(int? id)
        {
            int adminAccessLevel = GetAdminAccessLevel();
            if (adminAccessLevel == -1)
                return RedirectToAction("News");
            if (adminAccessLevel < 2)
                return RedirectToAction("News");
            NewsPostAdminView newsPostAdminView;
            if (id == null)
                return RedirectToAction("Index");
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                if (newsPost == null)
                    return RedirectToAction("Index");
                newsPostAdminView = new NewsPostAdminView(newsPost);
            }
            return View(newsPostAdminView);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (GetAdminAccessLevel() == -1)
                return RedirectToAction("News");
            if (id == null)
                return RedirectToAction("Index");
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
            int adminAccessLevel = GetAdminAccessLevel();
            if (adminAccessLevel == -1)
                return RedirectToAction("News");
            if (adminAccessLevel < 2)
                return RedirectToAction("News");
            return View();
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateNews(NewsPostModelCreate model)
        {
            if (GetAdminAccessLevel() == -1)
                return RedirectToAction("LoginAdmin");
            if (ModelState.IsValid)
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
            return View(model);
        }
        public ActionResult AllUsers()
        {
            int adminAccessLevel = GetAdminAccessLevel();
            if (adminAccessLevel == -1)
                return RedirectToAction("LoginAdmin");
            if (adminAccessLevel < 2)
                return RedirectToAction("News");
            ICollection<UserSafeModel> userSafeModels = new List<UserSafeModel>();
            using (ISession session = NHibernateSession.OpenSession())
            {
                IQueryable<Users> users = session.Query<Users>();
                foreach (Users user in users)
                {
                    userSafeModels.Add(new UserSafeModel(user));
                }
            }
            return View(userSafeModels);
        }
        public int GetAdminAccessLevel()
        {
            object accessLevel = Session["AdminAccessLevel"];
            if (accessLevel == null)
                return -1;
            return Convert.ToInt32(accessLevel);
        }
        public int GetAdminAccessLevel(out int result)
        {
            result = GetAdminAccessLevel();
            return result;
        }

    }
}