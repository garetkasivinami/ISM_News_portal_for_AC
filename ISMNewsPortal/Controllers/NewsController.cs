using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.BLL.Services;
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
using ISMNewsPortal.BLL.BusinessModels;
using ISMNewsPortal.Helpers;

namespace ISMNewsPortal.Controllers
{
    [Culture]
    public class NewsController : Controller
    {
        [HttpGet]
        public ActionResult Index(Options model)
        {
            NewsPostSimplifiedCollection newsPostSimplifiedCollection = NewsPostHelper.GenerateNewsPostSimplifiedCollection(model);
            return View(newsPostSimplifiedCollection);
        }

        [HttpGet]
        public ActionResult Error404()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CatError()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Details(int id, int? page)
        {
            NewsPostViewModel newsPostViewModel = NewsPostHelper.GetNewsPostViewModelById(id, page ?? 1, User.IsInRole(Roles.Moderator.ToString()), true);
            return View(newsPostViewModel);

        }

        [HttpGet]
        [Authorize]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult Preview(int id)
        {
            NewsPostViewModel newsPostViewModel = NewsPostHelper.GetNewsPostViewModelById(id, 1, User.IsInRole(Roles.Moderator.ToString()));
            return View("Details", newsPostViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateComment(CommentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = model.ConvertToComment();
                comment.Id = CommentHelper.CreateComment(comment);
                return PartialView("_Comment", new CommentViewModel(comment));
            }
            return Json(new { error = CommentHelper.BuildErrorMessage(ModelState) });
        }

        [HttpGet]
        [Authorize]
        [RoleAuthorize(Roles.Moderator)]
        public ActionResult DeleteComment(int id, int newsPostId, bool detailsMode = true)
        {
            CommentHelper.DeleteComment(id);
            string action = detailsMode ? "Details" : "Preview";
            return RedirectToAction(action, new { id = newsPostId });
        }
    }
}