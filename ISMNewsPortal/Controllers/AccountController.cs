using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.Models;
using NHibernate;
using ISMNewsPortal.BLL.Models;
using ISMNewsPortal.Helpers;

namespace ISMNewsPortal.Controllers
{
    [Culture]
    public class AccountController : Controller
    {
        [HttpGet]
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
                if (admin != null && AdminHelper.CheckPassword(admin, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(admin.Login, true);
                    var cookies = new HttpCookie("Request_p", admin.Salt);
                    cookies.Expires = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
                    HttpContext.Response.Cookies.Add(cookies);
                    return RedirectToAction("Index", "Admin");
                }
                ModelState.AddModelError("", "Invalid login and/or password!");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Request.Cookies.Remove("password");
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
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