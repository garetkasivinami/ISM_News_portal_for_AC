using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace ISMNewsPortal
{
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string cultureName = filterContext.RouteData.Values["lang"] as string;

            // Список культур
            List<string> cultures = new List<string>() { "uk", "en", "ru" };
            if (!cultures.Contains(cultureName))
                cultureName = "en";

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }
    }
}