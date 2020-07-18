using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.Models;
using ISMNewsPortal.Mappers;
using Microsoft.Ajax.Utilities;
using NHibernate;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ISMNewsPortal.BLL.Infrastructure;

namespace ISMNewsPortal.Controllers
{
    public class NewsController : Controller
    {
        [HttpGet]
        public ActionResult Index(ToolBarModel model)
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
        public ActionResult Details(int id, int? page)
        {
            try
            {
                NewsPostViewModel newsPostViewModel = NewsPostHelper.GetNewsPostViewModelById(id, page ?? 0, User.IsInRole(Roles.Moderator.ToString()), true);
                return View(newsPostViewModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.Message + ex.StackTrace);
                return new HttpNotFoundResult();
            }
        }

        [HttpGet]
        [Authorize]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult Preview(int id)
        {
            try
            {
                NewsPostViewModel newsPostViewModel = NewsPostHelper.GetNewsPostViewModelById(id, 0, User.IsInRole(Roles.Moderator.ToString()));
                return View("Details", newsPostViewModel);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex.Message);
                return new HttpNotFoundResult();
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public PartialViewResult CreateComment(CommentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment(model);
                comment.Id = CommentHelper.CreateComment(comment);
                return PartialView("_Comment", new CommentViewModel(comment));
            }
            return null;
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