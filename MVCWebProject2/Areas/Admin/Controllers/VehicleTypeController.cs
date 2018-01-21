/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : VehicleTypeController.cs          '
'  Description      : Manages the vehicle types         ' 
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
    public class VehicleTypeController : Controller
    {
        #region Index(LIST)
        // GET: Admin/VehicleType
        public ActionResult Index()
        {
            SetActiveMenuItem();
            try
            {
                var model = VehicleTypeBLL.GetVehicleTypeList();
                return View(model: model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect("~/Admin/Home/Error");
            }
        }
        #endregion

        #region Edit(GET)
        // GET: /Admin/VehicleType/Edit
        public ActionResult Edit(int id)
        {
            SetActiveMenuItem();
            try
            {
                var model = VehicleTypeBLL.GetVehicleType(id);
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
        // POST: /Admin/VehicleType/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleTypeList model)
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
                VehicleTypeBLL.UpdateVehicleType(model, FullName);

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
        // GET: Admin/VehicleType/Create
        public ActionResult Create()
        {
            SetActiveMenuItem();
            return View();
        }
        #endregion

        #region Create(POST)
        // POST: Admin/VehicleType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleTypeList model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                //Set our initial return value
                var returnValue = 0;
                //Set our updater name
                var FullName = Request.Cookies["userInfo"]["FullName"];
                //Attempt to add our record
                VehicleTypeBLL.AddVehicleType(model, FullName, out returnValue);

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
            ViewBag.SelectedMenuItem = Constants.VehicleMenuManager.VehicleType.GetStringValue();
        }

        #endregion
    }
}