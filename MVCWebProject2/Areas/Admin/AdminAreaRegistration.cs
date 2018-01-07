using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute(
               "Admin_Gallery",
               "Admin/Gallery/{action}/{PageNumber}/{NumberOfItems}",
               defaults: new { controller = "Gallery", action = "Index", PageNumber = UrlParameter.Optional, NumberOfItems = UrlParameter.Optional }
           );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
        }
    }
}