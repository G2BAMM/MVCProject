using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCWebProject2.BLL;
using MVCWebProject2.utilities;
using MVCWebProject2.Areas.Admin.Models;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    public class FuelTypeController : Controller
    {
        // GET: Admin/FuelType
        public ActionResult Index()
        {
            SetActiveMenuItem();
            var model = FuelTypeBLL.GetFuelTypes();
            return View(model: model);
        }

        // GET: /Admin/FuelType
        public ActionResult Edit(int id)
        {
            SetActiveMenuItem();
            var model = FuelTypeBLL.GetFuelType(id);
            return View(model: model);
        }

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

        // GET: Admin/FuelType/Create
        public ActionResult Create()
        {
            SetActiveMenuItem();
            return View();
        }

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

        #region SetActiveMenuItem
        private void SetActiveMenuItem()
        {
            ViewBag.SelectedMenuItem = Constants.VehicleMenuManager.FuelType.GetStringValue();
        }

        #endregion
    }
}