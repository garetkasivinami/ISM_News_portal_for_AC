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

namespace ISMNewsPortal.Controllers
{
    public class NewsController : Controller
    {
        [HttpGet]
        public ActionResult Details(int id, int? page)
        {
            NewsPostViewModel newsPostViewModel = NewsPostHelper.GetNewsPostViewModelById(id, page ?? 0, true);
            return View(newsPostViewModel);
        }

        [Authorize]
        [RoleAuthorize(Roles.Creator)]
        public ActionResult Preview(int id)
        {
            NewsPostViewModel newsPostViewModel = NewsPostHelper.GetNewsPostViewModelById(id, 0);
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
                using (CommentService commentService = new CommentService())
                {
                    Comment comment = new Comment(model);
                    CommentDTO commentDTO = DTOMapper.CommentMapperToDTO.Map<Comment, CommentDTO>(comment);
                    commentService.CreateComment(commentDTO);
                    return RedirectToAction("Details", "News", new { @id = model.PageId });
                }
            }
            return View(model);
        }
    }
}