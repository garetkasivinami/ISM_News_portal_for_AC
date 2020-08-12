using System.Web.Mvc;

namespace ISMNewsPortal.Controllers
{
    public class FilesController : Controller
    {
        // GET: Files
        [HttpGet]
        [OutputCache(Duration = 180, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult GetFile(string name)
        {
            string file_path = ($"~/App_Data/Files/{name}");
            string file_type = "image/png";
            return new FilePathResult(file_path, file_type);
        }
    }
}