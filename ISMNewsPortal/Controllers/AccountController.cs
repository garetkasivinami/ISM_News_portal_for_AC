using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ISMNewsPortal.Models;
using NHibernate;

namespace ISMNewsPortal.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Admin admin = Admin.GetAdminByLoginAndPassword(model);
                if (admin != null)
                {
                    FormsAuthentication.SetAuthCookie(admin.Login, true);
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
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult LogoffAction()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}