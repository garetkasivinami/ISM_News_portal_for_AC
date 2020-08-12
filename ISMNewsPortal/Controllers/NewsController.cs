using ISMNewsPortal.Models;
using System.Web.Mvc;
using ISMNewsPortal.Helpers;
using ISMNewsPortal.Models.Tools;
using System.Web.UI;

namespace ISMNewsPortal.Controllers
{
    [Culture]
    public class NewsController : Controller
    {
        [HttpGet]
        [OutputCache(Duration = 20, Location = OutputCacheLocation.Client)]
        public ActionResult Index(ToolsModel model)
        {
            var newsPostSimplifiedCollection = NewsPostHelper.GenerateNewsPostSimplifiedCollection(model);
            return View(newsPostSimplifiedCollection);
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 360)]
        public ActionResult Error404()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 5)]
        public ActionResult Details(int id, int? page)
        {
            var newsPostViewModel = NewsPostHelper.GetNewsPostViewModelById(id, page ?? 1, User.IsInRole(Roles.Moderator.ToString()), true);
            return View(newsPostViewModel);

        }

        [HttpGet]
        [Authorize]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult Preview(int id)
        {
            var newsPostViewModel = NewsPostHelper.GetNewsPostViewModelById(id, 1, User.IsInRole(Roles.Moderator.ToString()));
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