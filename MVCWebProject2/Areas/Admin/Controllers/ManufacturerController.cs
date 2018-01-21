/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : ManufacturerController.cs         '
'  Description      : Manages the manufacturers         ' 
'  Author           : Brian McAulay                     '
'  Creation Date    : 10 Jan 2018                       '
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
using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    public class ManufacturerController : Controller
    {
        #region Index(LIST)
        // GET: Admin/Manufacturer
        public ActionResult Index()
        {
            SetActiveMenuItem();
            try
            {
                var model = ManufacturerBLL.GetManufacturerList();
                return View(model: model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect("~/Admin/Home/Error");
            }
        }
        #endregion

        #region EDIT(GET)
        // GET: /Admin/Manufacturer/Edit
        public ActionResult Edit(int id)
        {
            SetActiveMenuItem();
            try
            {
                var model = ManufacturerBLL.GetManufacturer(id);
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
        #endregion

        #region Create(GET)
        // GET: Admin/Manufacturer/Create
        public ActionResult Create()
        {
            SetActiveMenuItem();
            return View();
        }
        #endregion

        #region Create(POST)
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
        #endregion

        #region SetActiveMenuItem
        private void SetActiveMenuItem()
        {
            ViewBag.SelectedMenuItem = Constants.VehicleMenuManager.Manufacturer.GetStringValue();
        }

        #endregion
    }
}