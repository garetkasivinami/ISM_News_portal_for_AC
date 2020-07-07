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
        public ActionResult News(ToolBarModel model)
        {
            NewsPostAdminCollection result = NewsPost.GenerateNewsPostAdminCollection(model);
            return View(result);
        }
        public ActionResult Edit(int id)
        {
            NewsPostAdminView newsPostAdminView = NewsPost.GetNewsPostAdminView(id);
            return View(newsPostAdminView);
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
                string newsPostAuthorName = session.Get<Admin>(newsPost.AuthorId).Login;
                int commentCount = session.Query<Comment>().Where(u => u.NewsPostId == newsPost.Id).Count();
                NewsPostAdminView newsPostAdminView = new NewsPostAdminView(newsPost, newsPostAuthorName, commentCount);
                return View(newsPostAdminView);
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            NewsPost.RemoveNewsPost(id);
            return RedirectToAction("News");
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
                    newsPost.PublicationDate = model.PublicationDate ?? DateTime.Now;
                    newsPost.IsVisible = model.IsVisible;

                    string fileName = System.IO.Path.GetFileName(model.uploadFiles[0].FileName);
                    string path = Server.MapPath("~/Files/" + fileName);
                    model.uploadFiles[0].SaveAs(path);
                    newsPost.ImagePath = path;
                    //newsPost.ImagePath = model.ImagePath;
                    newsPost.Name = model.Name;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(newsPost);
                        transaction.Commit();
                    }
                    return RedirectToAction("News");
                }
            }
            return View(model);
        }
        public ActionResult AdminList()
        {
            AdminViewModelCollection adminViewModelCollection = Admin.GenerateAdminViewModelCollection();
            return View(adminViewModelCollection);
        }
        public ActionResult CreateAdmin()
        {
            return View();
        }
        public ActionResult Comments(ToolBarModel model)
        {
            CommentViewModelCollection commentViewModelCollection = Comment.GenerateCommentViewModelCollection(model);
            return View(commentViewModelCollection);
        }
        public ActionResult DeleteComment(int id)
        {
            Comment.RemoveComment(id);
            return RedirectToAction("Comments");
        }
    }
}