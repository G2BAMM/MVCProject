/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : HomeController.cs                 '
'  Description      : Manages the home logic            ' 
'  Author           : Brian McAulay                     '
'  Creation Date    : 17 Oct 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using MVCWebProject2.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using MVCWebProject2.utilities;

namespace MVCWebProject2.Controllers
{
    public class HomeController : Controller
    {

        #region GeneralError
        public ActionResult GeneralError()
        {
            //Prevent users from seeing the actual error on any page or section/service
            //and instead redirect them to a general error message view
            //Mail will be sent to site admin highlighting the error and the cause/line no etc
            //See Global.asax for details
            return View();
        }
        #endregion

        #region NotFound Error
        public ActionResult NotFoundError()
        {
            //Possible bad link or user manipulating the url in the browser
            //and instead redirect them to a general 404 error message view
            //Mail will be sent to site admin highlighting the error and the cause/line no etc
            //See Global.asax for details
            return View();
        }
        #endregion

        #region SetTheme
        [Authorize]
        public async Task<ActionResult> ChooseTheme(string id)
        {
            //Move the string value here for better readabilty
            var bootStrapTheme = id;
            //Get our authenitcated user from http (owin) context
            var currentUserId = User.Identity.GetUserId();
            //Set up our user manager
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //Get the details (whole row from SQL table) for our local user from the SQL user manager
            var currentUser = manager.FindById(currentUserId);
            //Set our new theme for the user's SQL record
            currentUser.BootstrapTheme = bootStrapTheme;
            //Update the selected theme to the user's SQL record
            await manager.UpdateAsync(currentUser);

            //Prepare our cookies for the change
            var cookies = new CookieManager();
            //Perform the change now
            cookies.WriteCookie(currentUser);
            //Make sure we keep the user on the same page they set the theme up on
            return Redirect(Request.UrlReferrer.ToString());
        }
        #endregion

        #region Index(GET)
        public ActionResult Index()
        {

            if (Request.IsAuthenticated)
            {
                //Get the user's theme from their login details
                //var currentUserId = User.Identity.GetUserId();
                //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                //var currentUser = manager.FindById(currentUserId);
                
                ViewBag.LoginDetails = User.IsInRole("User") ? "User" : User.IsInRole("Super Admin") ? "Super Admin" : "Unknown";
                ViewBag.Theme = Request.Cookies["userInfo"]["BootstrapTheme"];
                //Cause a deliberate error by division by zero to test the uncaught exception handling by the app
                //var divisor = 0;
                //var result = 10 / divisor;
             }
            
            return View();
            
        }
        #endregion

        #region About(GET)
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            return View();
        }
        #endregion

        #region Contact(GET)
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion


        [Authorize]
        public ActionResult Vehicle()
        {
            ViewBag.Message = "Your vehicle list";
            return View(SeedData());
        }

        private List<VehicleViewModel> SeedData()
        {
            List<VehicleViewModel> list1 = new List<VehicleViewModel>();
            List<BodyType> bodyTypeList = new List<BodyType>();
            BodyType bodyType = new BodyType
            {
                Id = 1,
                Description = "Saloon"
            };
            bodyTypeList.Add(bodyType);

            for (int i = 1; i < 11; i++)
            {
                VehicleViewModel vehicle = new VehicleViewModel
                {
                    VehicleID = i,
                    BodyTypeId = i,
                    Make = "Ford",
                    ModelType = "Fiesta",
                    NumberOfDoors = 4
                };
                list1.Add(vehicle);
            }
            return list1;
        }

       

        
    }
}