using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ISMNewsPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AddComment",
                url: "News/CreateComment",
                defaults: new { controller = "News", action = "CreateComment", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Error404",
                url: "Error404",
                defaults: new { controller = "Home", action = "Error404", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "GetFile",
                url: "Files/{name}",
                defaults: new { controller = "Files", action = "GetFile", name = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Language",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { lang = "en", controller = "News", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = @"ru|en|ua" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { lang = "en", controller = "News", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}