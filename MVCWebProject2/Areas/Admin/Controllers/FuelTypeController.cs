/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : FuelTypeController.cs             '
'  Description      : Manages the fuel types            ' 
'  Author           : Brian McAulay                     '
'  Creation Date    : 05 Jan 2018                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using System;
using System.Web.Mvc;
using MVCWebProject2.BLL;
using MVCWebProject2.utilities;
using MVCWebProject2.Areas.Admin.Models;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    public class FuelTypeController : Controller
    {
        #region Index(FUEL TYPE LIST)
        // GET: Admin/FuelType
        public ActionResult Index()
        {
            SetActiveMenuItem();
            var model = FuelTypeBLL.GetFuelTypes();
            return View(model: model);
        }
        #endregion

        #region Edit (GET)
        // GET: /Admin/FuelType
        public ActionResult Edit(int id)
        {
            SetActiveMenuItem();
            try
            {
                var model = FuelTypeBLL.GetFuelType(id);
                return View(model: model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect("~/Admin/Home/Error");
            }
        }
        #endregion

        #region Edit(POST)
        // POST: /Admin/FuelType/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleFuelList model)
        {
            SetActiveMenuItem();
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                //Set our updater name
                var FullName = Request.Cookies["userInfo"]["FullName"];
                
                //Now update the record
                FuelTypeBLL.UpdateFuelType(model, FullName);

                //Take our ID with us to the confirmation form
                ViewBag.Id = model.Id;

                //Determine the kind of SQL transaction we have performed
                ViewBag.Message = "updated";

                //We can now safely go to the confirmation view
                return View("AddUpdateConfirm");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect("~/Admin/Home/Error");
            }
            
        }
        #endregion

        #region Create(GET)
        // GET: Admin/FuelType/Create
        public ActionResult Create()
        {
            SetActiveMenuItem();
            return View();
        }
        #endregion

        #region Create(POST)
        // POST: Admin/FuelType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleFuelList model)
        {
            SetActiveMenuItem();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                //Set our iniotal return value
                var returnValue = 0;
                //Set our updater name
                var FullName = Request.Cookies["userInfo"]["FullName"];
                //Attempt to add our record
                FuelTypeBLL.AddFuelType(model, FullName, out returnValue);
                
                //Take our ID with us to the confirmation form
                ViewBag.Id = returnValue;

                //Determine the kind of SQL transaction we have performed
                ViewBag.Message = "added";

                //We can now safely go to the confirmation view
                return View("AddUpdateConfirm");
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect("~/Admin/Home/Error");
            }
            
        }
        #endregion

        #region SetActiveMenuItem
        private void SetActiveMenuItem()
        {
            ViewBag.SelectedMenuItem = Constants.VehicleMenuManager.FuelType.GetStringValue();
        }

        #endregion
    }
}