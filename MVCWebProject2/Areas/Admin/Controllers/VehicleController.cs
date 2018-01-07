/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : VehicleController.cs              '
'  Description      : Manages the vehicle attributes    ' 
'  Author           : Brian McAulay                     '
'  Creation Date    : 21 Dec 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using System.Web.Mvc;
using MVCWebProject2.utilities;
using MVCWebProject2.BLL;
using MVCWebProject2.Areas.Admin.Models;
using System;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    [Authorize(Roles = ("Super Admin, Admin"))]
    public class VehicleController : Controller
    {
        
        // GET: Admin/VehicleList
        public ActionResult Index()
        {
            SetActiveMenuItem();
            return View(model: VehicleBLL.GetVehicleList());
        }

        //GET: Admin/Vehicle/Edit
        public ActionResult Edit(int id)
        {
            SetActiveMenuItem();
            var model = VehicleBLL.GetVehicleDetails(id);
            //Set the temp data so that every time there is an error we rebuild our dropdown lists without making SQL requests
            TempData["VehicleManufacturerList"] = model.VehicleManufacturerList;
            TempData["VehicleModelList"] = model.VehicleModelList;
            TempData["VehicleStatusList"] = model.VehicleStatusList;
            TempData["VehicleTransmissionList"] = model.VehicleTransmissionList;
            TempData["VehicleGroupList"] = model.VehicleGroupList;
            TempData["VehicleFuelList"] = model.VehicleFuelList;
            return View(model);
        }

        //POST: Admin/VehicleCategory/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleViewModel model)
        {
            SetActiveMenuItem();
            TempData.Keep();
            //Repopulate the dropdown lists of vehicle attributes without asking SQL again
            model.VehicleManufacturerList = (SelectList)TempData["VehicleManufacturerList"];
            model.VehicleModelList = (SelectList)TempData["VehicleModelList"];
            model.VehicleStatusList = (SelectList)TempData["VehicleStatusList"];
            model.VehicleTransmissionList = (SelectList)TempData["VehicleTransmissionList"];
            model.VehicleGroupList = (SelectList)TempData["VehicleGroupList"];
            model.VehicleFuelList = (SelectList)TempData["VehicleFuelList"];
            

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {

                var FullName = Request.Cookies["userInfo"]["FullName"];
                //Now update the record
                var result = VehicleBLL.UpdateVehicle((int)model.VehicleID, 
                                            model.ModelID, 
                                            model.RegistrationNumber, 
                                            model.StatusID, 
                                            model.TransmissionID, 
                                            model.VehicleGroupID, 
                                            model.FuelID, 
                                            FullName);

                //Take our ID with us to the confirmation form
                ViewBag.Id = model.VehicleID;
                //Clear any temp data
                TempData.Clear();
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

        #region SetActiveMenuItem
        private void SetActiveMenuItem()
        {
            ViewBag.SelectedMenuItem = Constants.VehicleMenuManager.Vehicle.GetStringValue();
        }

        #endregion
    }
}