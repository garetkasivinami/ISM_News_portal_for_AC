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
        public ActionResult Index(int? page, string sortType, string filter, string search, string typeSearch)
        {
            NewsPostSimplifiedCollection newsPostSimplifiedCollection = NewsPost.GenerateNewsPostSimplifiedCollection(page ?? 0, sortType, filter, search, typeSearch);
            return View(newsPostSimplifiedCollection);
        }
    }
}