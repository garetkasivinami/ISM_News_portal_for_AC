using System.Web;
using System.Web.Mvc;
using ISMNewsPortal.Models;
using ISMNewsPortal.Helpers;
using ISMNewsPortal.Accounts;
using Microsoft.AspNet.Identity.Owin;
using System.Web.UI;

namespace ISMNewsPortal.Controllers
{
    [Culture]
    public class AccountController : Controller
    {
        public SignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<SignInManager>(); }
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.Client, Duration = 180)]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Admin");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = AdminHelper.GetAdmin(model.Login);
                if (admin != null)
                {
                    var password = Security.SHA512(model.Password, admin.Salt);
                    var result = SignInManager.PasswordSignIn(model.Login, password, true,  false);
                    if (result == SignInStatus.Success)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }
                ModelState.AddModelError("", "Invalid login and/or password!");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logoff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client, Duration = 180)]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            var admin = AdminHelper.GetAdmin(User.Identity.Name);
            if (admin == null)
                return RedirectToAction("Logoff");

            string passportSalted = Security.SHA512(model.LastPassword, admin.Salt);
            if (admin.Password != passportSalted)
            {
                ModelState.AddModelError("", "The old password is incorrect!");
                return View(model);
            }
            AdminHelper.SetPassword(admin, model.Password);
            AdminHelper.UpdateAdmin(admin);
            return RedirectToAction("Index", "Admin");
        }
    }
}