/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : TransmissionTypeController.cs     '
'  Description      : Manages the transmission types    ' 
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
    public class TransmissionTypeController : Controller
    {
        #region Index(LIST)
        // GET: Admin/TransmissionType
        public ActionResult Index()
        {
            SetActiveMenuItem();
            try
            {
                var model = TransmissionTypeBLL.GetTransmissionList();
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
        // GET: /Admin/TransmissionType/Edit
        public ActionResult Edit(int id)
        {
            SetActiveMenuItem();
            try
            {
                var model = TransmissionTypeBLL.GetTransmissionType(id);
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
        // POST: /Admin/TransmissionType/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleTransmissionList model)
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
                TransmissionTypeBLL.UpdateTransmissionType(model, FullName);

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
        // GET: Admin/TransmissionType/Create
        public ActionResult Create()
        {
            SetActiveMenuItem();
            return View();
        }
        #endregion

        #region Create(POST)
        // POST: Admin/TransmissionType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleTransmissionList model)
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
                TransmissionTypeBLL.AddTransmissionType(model, FullName, out returnValue);

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
            ViewBag.SelectedMenuItem = Constants.VehicleMenuManager.TransmissionType.GetStringValue();
        }

        #endregion
    }
}