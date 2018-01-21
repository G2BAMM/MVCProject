/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : VehicleStatusController.cs        '
'  Description      : Manages the vehicle status        ' 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    public class VehicleStatusController : Controller
    {
        // GET: Admin/VehicleStatus
        public ActionResult Index()
        {
            return View();
        }
    }
}