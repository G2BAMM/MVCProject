using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    public class VehicleModelController : Controller
    {
        // GET: Admin/VehicleModel
        public ActionResult Index()
        {
            return View();
        }
    }
}