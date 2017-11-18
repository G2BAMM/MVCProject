using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCWebProject2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "signin-microsoft",
                url: "MVCWebProject/signin-microsoft",
                defaults: new { controller = "Account", action = "ExternalLoginCallback" });

            routes.MapRoute(
                name: "signin-google", 
                url: "MVCWebProject/signin-google", 
                defaults: new { controller = "Account", action = "ExternalLoginCallback" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { string.Format("{0}.Controllers", BuildManager.GetGlobalAsaxType().BaseType.Assembly.GetName().Name) } //Programatically allows same controller names across the app e.g. root HomeController and then /Admin/HomeController
            );
        }
    }
}
