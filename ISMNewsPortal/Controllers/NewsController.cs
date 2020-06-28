using ISMNewsPortal.Models;
using NHibernate;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            NewsPostViewModel newsPostViewModel;
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                if (newsPost == null || newsPost.ForRegistered && Request.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                Users currentUser = Users.GetUserByLogin(User.Identity.Name, session);
                List<CommentViewModel> comments = new List<CommentViewModel>();
                foreach(Comment comment in newsPost.Comments)
                {
                    comments.Add(new CommentViewModel(comment, comment.User, (currentUser != null)? comment.User.Id == currentUser.Id: false));
                }
                newsPostViewModel = new NewsPostViewModel(newsPost, session.Get<Users>(newsPost.Author.UserId), comments);
            }
            return View(newsPostViewModel);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Details(CommentModel model)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Comment comment = new Comment();
                    comment.Date = DateTime.Now;
                    comment.IsEdited = false;
                    comment.NewsPost = session.Get<NewsPost>(model.PageId);
                    comment.Text = model.Text;
                    comment.User = Users.GetUserByLogin(User.Identity.Name, session);
                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        session.Save(comment); //  Save the book in session
                        transaction.Commit();   //  Commit the changes to the database
                    }
                    return RedirectToAction("Details", "News", new { @id = model.PageId});
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult DeleteComment(int? id, int? pageId)
        {
            if (id != null && pageId != null)
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Comment comment = session.Get<Comment>(id);
                    Users user = Users.GetUserByLogin(User.Identity.Name, session);
                    if (user == null || comment.User.Id != user.Id)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        session.Delete(comment); //  Save the book in session
                        transaction.Commit();   //  Commit the changes to the database
                    }
                    return RedirectToAction("Details", "News", new { @id = pageId });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}