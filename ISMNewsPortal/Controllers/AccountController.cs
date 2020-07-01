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
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Users user = session.Query<Users>().FirstOrDefault(u => u.Login == model.Login || u.UserName == model.UserName);
                    if (user == null)
                    {
                        Users createdUser = new Users();
                        createdUser.UserName = model.UserName;
                        createdUser.Login = model.Login;
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
                            ModelState.AddModelError("", "There already exists a user with this username!");
                        if (user.Login == model.Login)
                            ModelState.AddModelError("", "There already exists a user with this login!");
                    }
                    //using (NewsModel db = new NewsModel())
                    //{
                }
            return View(model);
        }
        public ActionResult Login()
        {
            if (IsAuthorized)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
                using (ISession session = NHibernateSession.OpenSession())
                {
                    Users user = Users.GetUserByLogin(model.Login, session);
                    if (user != null)
                    {
                        if (Users.ComparePasswords(user, model.Password))
                        {
                            // а це точно потрібно?
                            if (IsAuthorized)
                                FormsAuthentication.SignOut();
                            FormsAuthentication.SetAuthCookie(user.Login, true);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    ModelState.AddModelError("", "Invalid login and/or password!");
                }
            return View();
        }
        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session["AdminAccessLevel"] = null;
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            UserSafeModel userModel;
            using (ISession session = NHibernateSession.OpenSession())
            {
                Users user = session.Get<Users>(id);
                if (user == null)
                    return RedirectToAction("Index", "Home");
                userModel = new UserSafeModel(user);
            }
            return View(userModel);
        }
    }
}