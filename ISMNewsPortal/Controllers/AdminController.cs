using ISMNewsPortal.Models;
using System.Web.Mvc;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.BLL.DTO;
using ISMNewsPortal.Mappers;

namespace ISMNewsPortal.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult Index(ToolBarModel model)
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
            if (model.uploadFiles[0] != null)
            {
                FileModelActions.RemoveFile(model.ImageId, Server);
                model.ImageId = FileModelActions.SaveFile(model.uploadFiles[0], Server);
            }
            if (ModelState.IsValid)
            {
                using (NewsPostService newsPostService = new NewsPostService())
                {
                    var newsPost = new NewsPost(model);
                    var newsPostDTO = DTOMapper.MapNewsPostDTO(newsPost);
                    newsPostService.UpdateNewsPost(newsPostDTO);
                }
            }
            return RedirectToAction("News");
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
            using (NewsPostService newsPostService = new NewsPostService())
            {
                newsPostService.DeleteNewsPost(id);
            }
            return RedirectToAction("News");
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
                using (AdminService adminService = new AdminService())
                {
                    try
                    {
                        var admin = new Admin() { Login = model.Login, Email = model.Email };
                        AdminHelper.SetPassword(admin, model.Password);
                        var adminDTO = DTOMapper.MapAdminDTO(admin);
                        adminService.CreateAdmin(adminDTO);
                    }
                    catch
                    {
                        ModelState.AddModelError("", "There already exists a user with this login!");
                        return View(model);
                    }
                }
            }
            return RedirectToAction("AdminList");
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanCreateAdmin)]
        public ActionResult DeleteAdmin(int id)
        {
            using (AdminService adminService = new AdminService())
            {
                AdminDTO adminDTO = adminService.GetAdmin(id);
                Admin admin = DTOMapper.MapAdmin(adminDTO);
                return View(new AdminViewModel(admin));
            }
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanCreateAdmin)]
        public ActionResult DeleteAdminSured(int id)
        {
            using (AdminService adminService = new AdminService())
            {
                AdminDTO adminDTO = adminService.GetAdmin(id);
                if (adminDTO.Login == User.Identity.Name)
                {
                    return RedirectToAction("AdminList");
                }
                adminService.DeleteAdmin(id);
                return RedirectToAction("AdminList");
            }
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
                using (NewsPostService newsPostService = new NewsPostService())
                {
                    model.ImageId = FileModelActions.SaveFile(model.uploadFiles[0], Server);
                    using (AdminService adminService = new AdminService(newsPostService))
                    {
                        model.AuthorId = adminService.FindAdminByLogin(User.Identity.Name).Id;
                    }
                    var newsPost = new NewsPost(model);
                    var newsPostDTO = DTOMapper.MapNewsPostDTO(newsPost);
                    newsPostService.CreateNewsPost(newsPostDTO);
                    return RedirectToAction("News");
                }
            }
            return View(model);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Administrator)]
        public ActionResult AdminsList()
        {
            AdminViewModelCollection adminViewModelCollection = AdminHelper.GenerateAdminViewModelCollection();
            return View(adminViewModelCollection);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Moderator, Roles.Administrator, Roles.Creator)]
        public ActionResult Comments(int postId)
        {
            CommentViewModelCollection commentViewModelCollection = CommentHelper.GenerateCommentViewModelCollection(postId);
            return View(commentViewModelCollection);
        }

        [HttpGet]
        [RoleAuthorize(Roles.Moderator, Roles.Administrator, Roles.Creator)]
        public ActionResult DeleteComment(int id, int postId)
        {
            using (CommentService commentService = new CommentService())
            {
                commentService.DeleteComment(id);
                return RedirectToAction("Comments", new { postId });
            }
        }

        [HttpGet]
        [RoleAuthorize(Roles.CanEditAdmin)]
        public ActionResult EditAdmin(int id)
        {
            using (AdminService adminService = new AdminService())
            {
                AdminDTO adminDTO = adminService.GetAdmin(id);
                Admin admin = DTOMapper.MapAdmin(adminDTO);
                return View(new AdminEditModel(admin));
            }
        }

        [HttpPost]
        [RoleAuthorize(Roles.CanEditAdmin)]
        public ActionResult EditAdmin(AdminEditModel model)
        {
            using (AdminService adminService = new AdminService())
            {
                adminService.UpdateAdminPartial(model.Id, model.Email, string.Join("*", model.Roles), User.IsInRole(Roles.CanSetAdminRole.ToString()));
                return RedirectToAction("AdminList");
            }
        }
    }
}