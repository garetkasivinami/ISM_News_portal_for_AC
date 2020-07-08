using ISMNewsPortal.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(ToolBarModel model)
        {
            NewsPostSimplifiedCollection newsPostSimplifiedCollection = NewsPostHelperActions.GenerateNewsPostSimplifiedCollection(model);
            return View(newsPostSimplifiedCollection);
        }

        [HttpGet]
        public ActionResult Error404()
        {
            return View();
        }
    }
}