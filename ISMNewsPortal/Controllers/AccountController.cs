﻿using System;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Users user = session.Query<Users>().FirstOrDefault(u => u.Login == model.Email || u.UserName == model.UserName);
                    if (user == null)
                    {
                        Users createdUser = new Users();
                        createdUser.UserName = model.UserName;
                        createdUser.Login = model.Email;
                        Users.ChangePassword(createdUser, model.Password);
                        createdUser.Phone = model.Phone;
                        createdUser.PhoneCountry = 0;
                        createdUser.RegistrationDate = DateTime.Now;
                        createdUser.WarningsCount = 0;
                        createdUser.About = model.About;
                        createdUser.CommentsCount = 0;
                        createdUser.HideCommentsCount = false;
                        createdUser.HideLogin = true;
                        createdUser.HidePhone = true;
                        createdUser.HideRegistrationDate = true;
                        createdUser.IsBanned = false;
                        createdUser.IsActivated = true;
                        createdUser.Comments = new List<Comment>();
                        using (ITransaction transaction = session.BeginTransaction())   //  Begin a transaction
                        {
                            session.Save(createdUser); //  Save the book in session
                            transaction.Commit();   //  Commit the changes to the database
                        }
                        FormsAuthentication.SetAuthCookie(createdUser.Login, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        if (user.UserName == model.UserName)
                        {
                            ModelState.AddModelError("", "Користувач з таким іменем вже існує!");
                        }
                        if (user.Login == model.Email)
                        {
                            ModelState.AddModelError("", "Користувач з таким логіном вже існує!");
                        }
                    }
                    //using (NewsModel db = new NewsModel())
                    //{
                }
                //}
            }

            return View(model);
        }
        public ActionResult Login()
        {
            if (IsAuthorized)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Users user = Users.GetUserByLogin(model.Login, session);
                    if (user != null)
                    {
                        if (Users.ComparePasswords(user, model.Password))
                        {
                            // а це точно потрібно?
                            if (IsAuthorized)
                            {
                                FormsAuthentication.SignOut();
                            }
                            FormsAuthentication.SetAuthCookie(user.Login, true);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    ModelState.AddModelError("", "Користувач з таким логіном або паролем відсутній!");
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}