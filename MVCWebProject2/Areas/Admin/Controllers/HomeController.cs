/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : AdminHomeController.cs            '
'  Description      : Manages the admin home page logic '
'  Author           : Brian McAulay                     '
'  Creation Date    : 11 Nov 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    [Authorize (Roles="Super Admin, Admin")]
    public class HomeController : Controller
    {
        // GET: Admin/Home

        public ActionResult Index()
        {
            var message = "Full name is: <strong>" + Request.Cookies["userInfo"]["FullName"] + "</strong>";
            ViewBag.Message = new MvcHtmlString(message);
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}