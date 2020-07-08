using ISMNewsPortal.Models;
using Microsoft.Ajax.Utilities;
using NHibernate;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Controllers
{
    public class NewsController : Controller
    {
        [HttpGet]
        public ActionResult Details(int id, int? page)
        {
            NewsPostViewModel newsPostViewModel = NewsPostHelperActions.GetNewsPostViewModelById(id, page ?? 0, true);
            return View(newsPostViewModel);
        }
        [Authorize]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult Preview(int id)
        {
            NewsPostViewModel newsPostViewModel = NewsPostHelperActions.GetNewsPostViewModelById(id, 0);
            return View("Details", newsPostViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateComment(CommentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                if (HelperActions.XSSAtackCheker(model.Text))
                {
                    ModelState.AddModelError("", HelperActions.XssIndectDetectedError);
                    return View(model);
                }
                Comment.AddNewComment(model);
                return RedirectToAction("Details", "News", new { @id = model.PageId });
            }
            return View(model);
        }
    }
}