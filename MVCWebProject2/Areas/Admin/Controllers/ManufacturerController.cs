using MVCWebProject2.Areas.Admin.Models;
using MVCWebProject2.BLL;
using MVCWebProject2.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    public class ManufacturerController : Controller
    {
        // GET: Admin/Manufacturer
        public ActionResult Index()
        {
            SetActiveMenuItem();
            var model = ManufacturerBLL.GetManufacturerList();
            return View(model: model);
        }

        // GET: /Admin/Manufacturer/Edit
        public ActionResult Edit(int id)
        {
            SetActiveMenuItem();
            var model = ManufacturerBLL.GetManufacturer(id);
            return View(model: model);
        }

        // POST: /Admin/Manufacturer/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleManufacturerList model)
        {
            SetActiveMenuItem();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                //Set our updater name
                var FullName = Request.Cookies["userInfo"]["FullName"];

                //Now update the record
                ManufacturerBLL.UpdateManufacturer(model, FullName);

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

        // GET: Admin/Manufacturer/Create
        public ActionResult Create()
        {
            SetActiveMenuItem();
            return View();
        }

        // POST: Admin/Manufacturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleManufacturerList model)
        {
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
                ManufacturerBLL.AddManufacturer(model, FullName, out returnValue);

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
            ViewBag.SelectedMenuItem = Constants.VehicleMenuManager.Manufacturer.GetStringValue();
        }

        #endregion
    }
}