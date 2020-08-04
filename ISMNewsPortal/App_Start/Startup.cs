using ISMNewsPortal;
using ISMNewsPortal.BLL.Services;
using ISMNewsPortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace ISMNewsPortal
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AdminService admin = new AdminService();
            app.CreatePerOwinContext(() => new UserManager(UserManager.GetUsers));
            app.CreatePerOwinContext<SignInManager>((options, context) => new SignInManager(context.GetUserManager<UserManager>(), context.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider()
            });
        }
    }
}