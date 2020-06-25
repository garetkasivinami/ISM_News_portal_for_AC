using ISMNewsPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISMNewsPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<User> users;
            using (NewsModel db = new NewsModel())
            {
                users = db.Users.ToList();
            }
            return View(users);
        }
        public ActionResult AddNewUser()
        {
            using (NewsModel db = new NewsModel())
            {
                db.Users.Add(new Models.User() { About = "", AvatarPath = "", Login = "LOG", Password = "pass", UserName = $"user {db.Users.Count()}", RegistrationDate = DateTime.Now});
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}