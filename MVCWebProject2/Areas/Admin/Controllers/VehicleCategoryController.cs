/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : VehicleCategoryController.cs      '
'  Description      : Manages the vehicle categories    ' 
'  Author           : Brian McAulay                     '
'  Creation Date    : 22 Nov 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using MVCWebProject2.BLL;
using System.Web.Mvc;
using MVCWebProject2.Areas.Admin.Models;
using System;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    [Authorize(Roles = ("Super Admin, Admin"))]
    public class VehicleCategoryController : Controller
    {

        #region Index(LIST)
        // GET: Admin/VehicleCategory
        public ActionResult Index()
        {
            try
            {
                //Return the list of defined vehicle groups from SQL
                return View(model: VehicleCategoriesBLL.GetVehicleCategoryList());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect("~/Admin/Home/Error");
            }
        }
        #endregion

        #region Edit(GET)
        //GET: Admin/VehicleCategory/Edit
        public ActionResult Edit(int id)
        {
            try
            {
                //Build our model from SQL
                var model = VehicleCategoriesBLL.GetVehicleCategoryDataset(id);
                //Create a temp list of our vehicle types to avoid multiple SQL reads when there are errors on the user form
                TempData["SelectList"] = model.VehicleType;
                //Now return the main edit view form
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
        public ActionResult Edit(VehicleCategoryViewModel model)
        {
            //Repopulate the dropdown list of vehicle types without asking SQL again
            TempData.Keep();
            model.VehicleType = (SelectList)TempData["SelectList"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var FullName = Request.Cookies["userInfo"]["FullName"];
                VehicleCategoriesBLL.UpdateVehicleCategory(model, FullName);
                //Take our ID with us to the confirmation form
                ViewBag.Id = model.Id;
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
        //GET: Admin/VehicleCategory/Create
        public ActionResult Create()
        {
            try
            {
                //Generate an empty model with populated dropdown list
                var model = VehicleCategoriesBLL.GetVehicleList();
                
                //Create a temp list of our vehicle types to avoid multiple SQL reads when there are errors on the user form
                TempData["SelectList"] = model.VehicleType;
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
        //POST: Admin/VehicleCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VehicleCategoryViewModel model)
        {
            //Repopulate the dropdown list of vehicle types without asking SQL again
            TempData.Keep();
            model.VehicleType = (SelectList)TempData["SelectList"];

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var FullName = Request.Cookies["userInfo"]["FullName"];
                var result = VehicleCategoriesBLL.AddNewVehicleCategory(model, FullName);
                
                //Take our ID with us to the confirmation form
                ViewBag.Id = result;

                //Clear any temp data
                TempData.Clear();

                //Determine the kind of SQL transaction we have performed
                ViewBag.Message = "added";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Redirect("~/Admin/Home/Error");
            }
            return View("AddUpdateConfirm");
        }
        #endregion

    }
}