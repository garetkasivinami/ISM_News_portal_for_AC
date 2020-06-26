using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Controllers
{
    public class AccountController : Controller
    {
        public bool IsAuthorized
        {
            get
            {
                return Request.IsAuthenticated;
            }
        }
        // GET: Account
        public ActionResult Register()
        {
            if (IsAuthorized)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult Login()
        {
            if (IsAuthorized)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}