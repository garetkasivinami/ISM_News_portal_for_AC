using System.Web.Mvc;

namespace ISMNewsPortal.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "News");
        }
    }
}