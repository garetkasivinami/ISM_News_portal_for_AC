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
        public ActionResult Details(int id, int? page)
        {
            NewsPostViewModel newsPostViewModel = NewsPostHelperActions.GetNewsPostViewModelById(id, page ?? 0);
            return View(newsPostViewModel);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Details(CommentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                if (HelperActions.XSSAtackCheker(model.Text))
                {
                    ModelState.AddModelError("", HelperActions.XssIndectDetectedError);
                    return View(model);
                }
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Comment comment = new Comment();
                    comment.Date = DateTime.Now;
                    comment.NewsPostId = model.PageId;
                    comment.Text = model.Text;
                    comment.UserName = model.UserName;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(comment);
                        transaction.Commit();
                    }
                    return RedirectToAction("Details", "News", new { @id = model.PageId });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}