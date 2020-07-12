using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.Models;
using ISMNewsPortal.Mappers;
using NHibernate;
using ISMNewsPortal.BLL.DTO;

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
                using (AdminService adminService = new AdminService())
                {
                    var adminDTO = adminService.FindAdminByLogin(model.Login);
                    var admin = DTOMapper.MapAdmin(adminDTO);
                    if (AdminHelper.CheckPassword(admin, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(admin.Login, true);
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login and/or password!");
                    }
                }  
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
            using (AdminService adminService = new AdminService())
            {
                var adminDTO = adminService.FindAdminByLogin(User.Identity.Name);
                var admin = DTOMapper.MapAdmin(adminDTO);
                string passportSalted = Security.SHA512(model.LastPassword, admin.Salt);
                if (admin.Password != passportSalted)
                {
                    ModelState.AddModelError("", "The old password is incorrect!");
                    return View(model);
                }
                AdminHelper.SetPassword(admin, model.Password);
                adminDTO = DTOMapper.AdminMapper.Map<Admin, AdminDTO>(admin);
                adminService.UpdateAdmin(adminDTO);
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}