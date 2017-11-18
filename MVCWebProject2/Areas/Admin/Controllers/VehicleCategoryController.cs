using MVCWebProject2.BLL;
using System.Web.Mvc;
using MVCWebProject2.Areas.Admin.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    [Authorize(Roles = ("Super Admin, Admin"))]
    public class VehicleCategoryController : Controller
    {
        // GET: Admin/VehicleCategory
        public ActionResult Index()
        {
            //Return the list of defined vehicle groups from SQL
            return View(model: VehicleCategoriesBLL.GetVehicleCategoryList());
        }
        
        //GET: Admin/VehicleCategory/Edit
        public ActionResult Edit(int id)
        {
            //Build our model from SQL
            var model = VehicleCategoriesBLL.GetVehicleCategoryDataset(id);
            //Create a temp list of our vehicle types to avoid multiple SQL reads when there are errors on the user form
            TempData["SelectList"] = model.VehicleType;
            //Now return the main edit view form
            return View(model);
        }

        //POST: Admin/VehicleCategory/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleCategoryViewModel model)
        {

            //Repopulate the dropdown list of vehicle types without asking SQL again
            model.VehicleType = (SelectList)TempData["SelectList"];

            if (!ModelState.IsValid)
            {
                //Reset the temp data so that every time there is an error we rebuild our vehicles list
                TempData["SelectList"] = model.VehicleType;
                return View(model);
            }


            try
            {
                var FullName = Request.Cookies["userInfo"]["FullName"];
                VehicleCategoriesBLL.UpdateVehicleCategory(model, FullName);
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


    }
}