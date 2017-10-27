using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCWebProject2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_OnError()
        {
            /*
            var error = Server.GetLastError();
            var code = (error is HttpException) ? (error as HttpException).GetHttpCode() : 500;

            if (code != 404)
            {
                // Generate email with error details and send to administrator
                // I'm using RazorMail which can be downloaded from the Nuget Gallery
                // I also have an extension method on type Exception that creates a string representation
                //var email = new RazorMailMessage("Website Error");
                //email.To.Add("errors@wduffy.co.uk");
                //email.Templates.Add(error.GetAsHtml(new HttpRequestWrapper(Request)));
                //Kernel.Get<IRazorMailSender>().Send(email);
            }
            Server.ClearError();
            Response.Redirect("~/error");
            */
        }
    }
}
