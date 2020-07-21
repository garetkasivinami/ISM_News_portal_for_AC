using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.Models;
using ISMNewsPortal.BLL.Infrastructure;
using NHibernate;
using ISMNewsPortal.BLL.DTO;
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
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var admin = AdminHelper.GetAdmin(model.Login);
                    if (AdminHelper.CheckPassword(admin, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(admin.Login, true);
                        return RedirectToAction("Index", "Admin");
                    }
                } catch (Exception ex)
                {
                    ErrorLogger.LogError(ex.Message);
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
            try
            {
                var admin = AdminHelper.GetAdmin(User.Identity.Name);
                string passportSalted = Security.SHA512(model.LastPassword, admin.Salt);
                if (admin.Password != passportSalted)
                {
                    ModelState.AddModelError("", "The old password is incorrect!");
                    return View(model);
                }
                AdminHelper.SetPassword(admin, model.Password);
                AdminHelper.UpdateAdmin(admin);
            } catch(Exception ex)
            {
                ErrorLogger.LogError(ex.Message);
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}