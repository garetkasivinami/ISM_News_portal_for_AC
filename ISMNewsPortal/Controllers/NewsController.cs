using ISMNewsPortal.Models;
using Microsoft.Ajax.Utilities;
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
                return RedirectToAction("Index", "Home");
            NewsPostViewModel newsPostViewModel;
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                if (newsPost == null || newsPost.ForRegistered && !Request.IsAuthenticated)
                    return RedirectToAction("Index", "Home");
                Users currentUser = Users.GetUserByLogin(User.Identity.Name, session);
                Admin admin = null;
                if (currentUser != null)
                    admin = session.Query<Admin>().FirstOrDefault(u => u.UserId == currentUser.Id);
                bool adminRequest = admin != null && admin.AccessLevel > 0;
                List<CommentViewModel> comments = new List<CommentViewModel>();
                foreach (Comment comment in newsPost.Comments)
                {
                    Users user = comment.User;
                    comments.Add(new CommentViewModel(comment, new AuthorInfo() { UserId = user.Id, UserName = user.UserName }, adminRequest || (currentUser != null ? user.Id == currentUser.Id : false)));
                }
                UserLike userLike = currentUser?.Likes.FirstOrDefault(u => u.NewsPost.Id == id);
                Users authorOfPost = newsPost.Author;
                newsPostViewModel = new NewsPostViewModel(newsPost, new AuthorInfo() { UserId = authorOfPost.Id, UserName = authorOfPost.UserName }, comments, userLike != null);
            }
            return View(newsPostViewModel);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Details(CommentModel model)
        {
            if (ModelState.IsValid)
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Comment comment = new Comment();
                    comment.Date = DateTime.Now;
                    comment.IsEdited = false;
                    comment.NewsPost = session.Get<NewsPost>(model.PageId);
                    comment.Text = model.Text;
                    comment.User = Users.GetUserByLogin(User.Identity.Name, session);
                    comment.NewsPost.CommentsCount++;
                    comment.User.CommentsCount++;
                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        session.Save(comment); //  Save the book in session
                        session.Update(comment.NewsPost);
                        session.Update(comment.User);
                        transaction.Commit();   //  Commit the changes to the database
                    }
                    return RedirectToAction("Details", "News", new { @id = model.PageId });
                }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult DeleteComment(int? id, int? postId)
        {
            if (id != null && postId != null)
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Comment comment = session.Get<Comment>(id);
                    Users user = Users.GetUserByLogin(User.Identity.Name, session);
                    if (user == null || comment.User.Id != user.Id)
                        return RedirectToAction("Index", "Home");
                    NewsPost newsPost = comment.NewsPost;
                    newsPost.CommentsCount--;
                    user.CommentsCount--;
                    using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                    {
                        session.Delete(comment); //  Save the book in session
                        session.Update(newsPost);
                        session.Update(user);
                        transaction.Commit();   //  Commit the changes to the database
                    }
                    return RedirectToAction("Details", "News", new { @id = postId });
                }
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult EditComment(int id)
        {
            Comment comment;
            using (ISession session = NHibernateSession.OpenSession())
            {
                comment = session.Get<Comment>(id);
                if (comment == null)
                    return RedirectToAction("Index", "Home");
                Users user = Users.GetUserByLogin(User.Identity.Name);
                if (user.Id != comment.User.Id)
                    return RedirectToAction("Index", "Home");
            }
            return View(new CommentEditModel() { Id = id, Text = comment.Text });
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditComment(CommentEditModel model)
        {
            Comment comment;
            if (ModelState.IsValid)
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Users user = Users.GetUserByLogin(User.Identity.Name, session);
                    comment = session.Get<Comment>(model.Id);
                    if (user == null || user.Id != comment.User.Id)
                        return RedirectToAction("Index", "Home");
                    comment.Text = model.Text;
                    comment.IsEdited = true;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(comment);
                        transaction.Commit();
                    }
                }
            else
                return View(model);
            return RedirectToAction("Details", "News", new { @id = comment.NewsPost.Id });
        }
        [Authorize]
        public ActionResult LikePost(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            using (ISession session = NHibernateSession.OpenSession())
            {
                NewsPost newsPost = session.Get<NewsPost>(id);
                Users user = Users.GetUserByLogin(User.Identity.Name, session);
                if (user == null || newsPost == null)
                    return RedirectToAction("Index", "Home");
                UserLike userLike = null;
                if (user.LikesCount > 0)
                    userLike = user.Likes.FirstOrDefault(u => u.NewsPost.Id == id);
                if (userLike == null)
                {
                    userLike = new UserLike();
                    userLike.NewsPost = newsPost;
                    userLike.User = user;

                    newsPost.LikesCount++;
                    user.LikesCount++;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(userLike);
                        session.Update(user);
                        session.Update(newsPost);
                        transaction.Commit();
                    }
                }
                else
                {
                    newsPost.LikesCount--;
                    user.LikesCount--;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(userLike);
                        session.Update(user);
                        session.Update(newsPost);
                        transaction.Commit();
                    }
                }
            }
            return RedirectToAction("Details", "News", new { @id = id });
        }
    }
}