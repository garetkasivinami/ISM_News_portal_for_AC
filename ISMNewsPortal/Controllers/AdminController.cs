using ISMNewsPortal.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Serialization;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
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
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult News(ToolBarModel model)
        {
            NewsPostAdminCollection result = NewsPost.GenerateNewsPostAdminCollection(model);
            return View(result);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult EditNews(int id)
        {
            NewsPostEditModel newsPostAdminView = NewsPost.GetNewsPostEditModel(id);
            return View(newsPostAdminView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult EditNews(NewsPostEditModel model)
        {
            if (model.uploadFiles[0] != null)
            {
                FilesController.RemoveFile(model.ImageId, Server);
                model.ImageId = FilesController.SaveFile(model.uploadFiles[0], Server);
            }
            if (ModelState.IsValid)
                NewsPost.Update(model);
            return RedirectToAction("News");
        }

        [HttpGet]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult DeleteNewsPostRequest(int id)
        {
            NewsPostAdminView newsPostAdminView = NewsPost.GetNewsPostAdminView(id);
            return View(newsPostAdminView);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult DeleteNewsPost(int id)
        {
            NewsPost.RemoveNewsPost(id);
            return RedirectToAction("News");
        }

        [HttpGet]
        [Authorize]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult CreateNews()
        {
            return View();
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanCreateAdmin)]
        public ActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [RoleAuthorize(Roles.CanCreateAdmin)]
        public ActionResult CreateAdmin(AdminCreateModel model)
        {
            if (ModelState.IsValid)
            {
                if (Admin.AddAdmin(model))
                    return RedirectToAction("AdminList");
                else
                    ModelState.AddModelError("", "There already exists a user with this login!");
            }
            return View(model);
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanCreateAdmin)]
        public ActionResult DeleteAdmin(int id)
        {
            Admin admin = Admin.GetAdminById(id);
            return View(new AdminViewModel(admin));
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanCreateAdmin)]
        public ActionResult DeleteAdminSured(int id)
        {
            Admin admin = Admin.GetAdminById(id);
            if (admin.Login == User.Identity.Name)
            {
                return RedirectToAction("AdminList");
            }
            Admin.RemoveAdmin(id);
            return RedirectToAction("AdminList");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult CreateNews(NewsPostCreateModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.uploadFiles[0] == null)
                {
                    ModelState.AddModelError("", "No file!");
                    return View(model);
                }
                model.ImageId = FilesController.SaveFile(model.uploadFiles[0], Server);
                model.AuthorId = Admin.GetAdminIdByLogin(User.Identity.Name);
                NewsPost.AddNewsPost(model);
                return RedirectToAction("News");
            }
            return View(model);
        }
        
        [HttpGet]
        [RoleAuthorize(Roles.Administrator)]
        public ActionResult AdminList()
        {
            AdminViewModelCollection adminViewModelCollection = Admin.GenerateAdminViewModelCollection();
            return View(adminViewModelCollection);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Moderator,Roles.Administrator, Roles.Creator)]
        public ActionResult Comments(ToolBarModel model)
        {
            CommentViewModelCollection commentViewModelCollection = Comment.GenerateCommentViewModelCollection(model);
            return View(commentViewModelCollection);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Moderator, Roles.Administrator, Roles.Creator)]
        public ActionResult DeleteComment(int id)
        {
            Comment.RemoveComment(id);
            return RedirectToAction("Comments");
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanEditAdmin)]
        public ActionResult EditAdmin(int id)
        {
            Admin admin = Admin.GetAdminById(id);
            return View(new AdminEditModel(admin));
        }

        [HttpPost]
        [RoleAuthorize(Roles.CanEditAdmin)]
        public ActionResult EditAdmin(AdminEditModel model)
        {
            Admin.UpdateAdmin(model, User.IsInRole(Roles.CanSetAdminRole.ToString()));
            return RedirectToAction("AdminList");
        }
    }
}