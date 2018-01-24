/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : VehicleModelController.cs         '
'  Description      : Manages the vehicle model types   ' 
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
using MVCWebProject2.BLL;
using MVCWebProject2.utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    [Authorize(Roles = ("Super Admin, Admin"))]
    public class VehicleModelController : Controller
    {
        #region Index(GET) Accordion
        // GET: Admin/VehicleModel/Accordion
        public ActionResult Index()
        {
            SetActiveMenuItem();
            try
            {
                var model = VehicleModelBLL.BuildAccordionModel();
                return View(model: model);
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
            ViewBag.SelectedMenuItem = Constants.VehicleMenuManager.VehicleModel.GetStringValue();
        }

        #endregion

        #region UpdateModel
        [HttpPost]
        public string UpdateModel()
        {
            var ReturnValue = 0;

            /*var divisor = 0;
            //Cause a deliberate error to test the AJAX/JSON responses
            var result = 10 / divisor;
            */
            String postedData = new System.IO.StreamReader(System.Web.HttpContext.Current.Request.InputStream).ReadToEnd();
            var myPostedData = JsonConvert.SerializeObject(postedData);
            var data = JObject.Parse(postedData);
            var ManufacturerID = (int)data["ManufacturerID"];
            var ModelID = (int)data["ModelID"];
            var ModelName = data["ModelName"].ToString();
            var FullName = Request.Cookies["userInfo"]["FullName"];

            if (ModelID < 1)
            {
                //This is a new model being added
                VehicleModelBLL.AddNewModel(ManufacturerID, ModelName, FullName, out int returnValue);
                ReturnValue = returnValue;
                if(ReturnValue == -1)
                {
                    return JsonConvert.SerializeObject("-1");
                }
            }
            else
            {
                //This is an update
                VehicleModelBLL.UpdateModel(ModelID, ModelName, FullName, out int returnValue);
                ReturnValue = returnValue;
                if (ReturnValue == -1)
                {
                    return JsonConvert.SerializeObject("-1");
                }
            }
            
            //Retrieve the updated model names and populate the VehicleModelsList model
            var model = VehicleModelBLL.GetJSONModelByManufacturer(ManufacturerID);
            //Send JSON response to calling object
            return JsonConvert.SerializeObject(model);
        }
        #endregion
    }
}