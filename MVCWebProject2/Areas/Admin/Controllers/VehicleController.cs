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
using Newtonsoft.Json;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    [Authorize(Roles = ("Super Admin, Admin"))]
    public class VehicleController : Controller
    {

        #region Index(GET)
        // GET: Admin/VehicleList
        public ActionResult Index()
        {
            SetActiveMenuItem();
            return View(model: VehicleBLL.GetVehicleList());
        }
        #endregion

        #region Edit(GET)
        //GET: Admin/Vehicle/Edit
        public ActionResult Edit(int id)
        {
            SetActiveMenuItem();
            try
            {
                var model = VehicleBLL.GetVehicleDetails(id);
                //Set the temp data so that every time there is an error we rebuild our dropdown lists without making SQL requests
                TempData["VehicleManufacturerList"] = model.VehicleManufacturerList;
                TempData["VehicleModelList"] = model.VehicleModelList;
                TempData["VehicleTransmissionList"] = model.VehicleTransmissionList;
                TempData["VehicleGroupList"] = model.VehicleGroupList;
                TempData["VehicleFuelList"] = model.VehicleFuelList;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect("~/Admin/Home/Error");
            }
        }
        #endregion

        #region Edit(POST)
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
                                            model.CurrentMileage, 
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
        #endregion

        #region Create(GET)
        //GET: Admin/Vehicle/Create
        public ActionResult Create()
        {
            SetActiveMenuItem();
            try
            {
                //Sending zero to this function will still retrieve the correct lists see the sp GetVehicleDetailsDataSet for details
                var model = VehicleBLL.GetVehicleDetails(0);

                

                //Set the temp data so that every time there is an error we rebuild our dropdown lists without making SQL requests
                TempData["VehicleManufacturerList"] = model.VehicleManufacturerList;
                //TempData["VehicleModelList"] = model.VehicleModelList;
                TempData["VehicleTransmissionList"] = model.VehicleTransmissionList;
                TempData["VehicleGroupList"] = model.VehicleGroupList;
                TempData["VehicleFuelList"] = model.VehicleFuelList;

                //Finally we can return the partially completed model
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect("~/Admin/Home/Error");
            }
        }
        #endregion

        #region Create(POST)
        //POST: Admin/Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleViewModel model)
        {

            SetActiveMenuItem();
            TempData.Keep();
            //Repopulate the dropdown lists of vehicle attributes without asking SQL again
            model.VehicleManufacturerList = (SelectList)TempData["VehicleManufacturerList"];
            model.VehicleModelList = (SelectList)TempData["VehicleModelList"];
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
                VehicleBLL.AddNewVehicle(model.ModelID,
                                            model.RegistrationNumber,
                                            model.CurrentMileage,
                                            model.TransmissionID,
                                            model.VehicleGroupID,
                                            model.FuelID,
                                            FullName,
                                            out int returnValue);

                //Take our ID with us to the confirmation form
                ViewBag.Id = returnValue;
                //Clear any temp data
                TempData.Clear();
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
            ViewBag.SelectedMenuItem = Constants.VehicleMenuManager.Vehicle.GetStringValue();
        }

        #endregion

        #region GetJSONVehicleModels
        // GET: Admin/GetJSONVehicleModels/Id
        public string GetJSONVehicleModels(int id)
        {
            //Retrieve model names and populate the VehicleModelsList model
            var model = VehicleModelBLL.GetJSONModelByManufacturer(id);
            //Send JSON response to calling object
            return JsonConvert.SerializeObject(model);
        }
        #endregion
    }
}