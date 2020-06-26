﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ISMNewsPortal.Models;

namespace ISMNewsPortal.Controllers
{
    public class AccountController : Controller
    {
        Random random = new Random();
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
                using (NewsModel db = new NewsModel())
                {
                    User user = db.Users.FirstOrDefault(u => u.Login == model.Email || u.UserName == model.UserName);
                    if (user == null)
                    {
                        User createdUser = new User();
                        createdUser.UserName = model.UserName;
                        createdUser.Login = model.Email;
                        createdUser.Password = SHA512(model.Password, "TEST SALT");
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
                        createdUser.IsAdmin = false;
                        createdUser.IsActivated = true;

                        db.Users.Add(createdUser);
                        db.SaveChanges();

                        FormsAuthentication.SetAuthCookie(createdUser.Login, true);
                        return RedirectToAction("Index", "Home");
                    } else
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
                }
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

        public string SHA512(string input, string salt)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            bytes = bytes.Concat(saltBytes).ToArray();
            Array.Resize(ref bytes, 64);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}