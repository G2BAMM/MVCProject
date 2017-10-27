/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : HomeController.cs                 '
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
using System;
using System.Web;

namespace MVCWebProject2.Controllers
{
    public class HomeController : Controller
    {
        
        [Authorize]
        public async Task<ActionResult> ChooseTheme(string id)
        {
            //Move the string value here for better readabilty
            var bootStrapTheme = id;
            //Find our local user
            var currentUserId = User.Identity.GetUserId();
            //Set up our user manager
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //Get the details for our local user from the user manager
            var currentUser = manager.FindById(currentUserId);
            //Change the user's theme to the one chosen
            currentUser.BootstrapTheme = bootStrapTheme.ToString();
            //Update the DB and login cookie
            var result = await manager.UpdateAsync(currentUser);
            //Update our custom cookie so that we can display the extra field[s] without having to query SQL for it on every page request
            Response.Cookies["userInfo"]["BootstrapTheme"] = currentUser.BootstrapTheme;
            Response.Cookies["userInfo"]["FirstName"] = currentUser.FirstName;
            Response.Cookies["userInfo"]["Surname"] = currentUser.Surname;
            //Make sure we keep the user on the same page they set the theme up on
            return Redirect(Request.UrlReferrer.ToString());
        }

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
             }
                return View();
            
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