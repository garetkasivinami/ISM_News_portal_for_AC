﻿using ISMNewsPortal.Models;
using System.Web.Mvc;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Helpers;
using ISMNewsPortal.Models.Tools;
using System;

namespace ISMNewsPortal.Controllers
{
    [RoleAuthorize]
    [Culture]
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult Index(ToolsModel model)
        {
            NewsPostAdminCollection result = NewsPostHelper.GenerateNewsPostAdminCollection(model);
            return View(result);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult EditNews(int id)
        {
            NewsPostEditModel newsPostAdminView = NewsPostHelper.GetNewsPostEditModel(id);
            return View(newsPostAdminView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult EditNews(NewsPostEditModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.uploadFiles != null && model.uploadFiles[0] != null)
                {
                    FileModelActions.RemoveFile(model.ImageId, Server);
                    model.ImageId = FileModelActions.SaveFile(model.uploadFiles[0], Server);
                }
                if (model.Description == "<br>")
                {
                    ModelState.AddModelError("Description", Language.Language.NoDescription);
                    return View(model);
                }
                NewsPostHelper.UpdateNewsPost(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult DeleteNewsPostRequest(int id)
        {
            NewsPostAdminView newsPostAdminView = NewsPostHelper.GetNewsPostAdminView(id);
            return View(newsPostAdminView);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult DeleteNewsPost(int id)
        {
            NewsPostHelper.DeleteNewsPost(id, Server);
            return RedirectToAction("Index");
        }

        [HttpGet]
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
                var admin = new Admin() { Login = model.Login.Trim(), Email = model.Email };
                AdminHelper.SetPassword(admin, model.Password);
                AdminHelper.CreateAdmin(admin);
                return RedirectToAction("AdminsList");
            }
            return View(model);
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanCreateAdmin)]
        public ActionResult DeleteAdmin(int id)
        {
            var admin = AdminHelper.GetAdmin(id);
            return View(new AdminViewModel(admin));
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanCreateAdmin)]
        public ActionResult DeleteAdminSured(int id)
        {
            var admin = AdminHelper.GetAdmin(id);
            if (admin.Login == User.Identity.Name)
                return RedirectToAction("AdminsList");

            AdminHelper.DeleteAdmin(id);
            return RedirectToAction("AdminsList");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult CreateNews(NewsPostCreateModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Description == "<br>")
                {
                    ModelState.AddModelError("Description", "No description!");
                    return View(model);
                }
                if (model.uploadFiles[0] == null)
                {
                    ModelState.AddModelError("", "No file!");
                    return View(model);
                }
                model.ImageId = FileModelActions.SaveFile(model.uploadFiles[0], Server);
                model.AuthorId = AdminHelper.GetAdmin(User.Identity.Name).Id;
                var newsPost = model.PassToNewsPost();
                NewsPostHelper.CreateNewsPost(newsPost);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Administrator)]
        public ActionResult AdminsList()
        {
            var adminViewModelCollection = AdminHelper.GenerateAdminViewModelCollection();
            return View(adminViewModelCollection);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Moderator, Roles.Administrator, Roles.Creator)]
        public ActionResult Comments(CommentToolsModel commentTools)
        {
            var commentViewModelCollection = CommentHelper.GenerateCommentViewModelCollection(commentTools);
            return View(commentViewModelCollection);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Moderator, Roles.Administrator, Roles.Creator)]
        public ActionResult DeleteComment(int id, int postId)
        {
            CommentHelper.DeleteComment(id, postId);
            return RedirectToAction("Comments", new { id = postId });
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanEditAdmin)]
        public ActionResult EditAdmin(int id)
        {
            var admin = AdminHelper.GetAdmin(id);
            return View(new AdminEditModel(admin));
        }

        [HttpPost]
        [RoleAuthorize(Roles.CanEditAdmin)]
        public ActionResult EditAdmin(AdminEditModel model)
        {
            AdminHelper.UpdateAdminPartial(model.Id, model.Email, string.Join(",", model.Roles), User.IsInRole(Roles.CanSetAdminRole.ToString()));
            return RedirectToAction("AdminsList");
        }
    }
}