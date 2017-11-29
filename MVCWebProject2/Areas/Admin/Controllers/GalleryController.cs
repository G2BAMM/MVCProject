/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : GalleryController.cs              '
'  Description      : Manages the vehicle images        ' 
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
using System;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MVCWebProject2.BLL;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    [Authorize(Roles = ("Super Admin, Admin"))]
    public class GalleryController : Controller
    {
        // GET: Admin/Gallery
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/AddNewImage
        public ActionResult AddNewImage()
        {
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewImage(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null && fileUpload.ContentLength > 0)
            {
                try
                {
                    //Save the new image
                    var updatedBy = Request.Cookies["userInfo"]["FullName"];
                    GalleryBLL.AddNewGalleryImage(fileUpload, updatedBy);
                    ViewBag.Message = "File uploaded successfully!";
                }
                catch (Exception ex)
                {
                    var message = ex.Message + Environment.NewLine + ex.InnerException;
                    if(message.Contains("An image could not be constructed from the content provided."))
                    {
                        message = "File was not an image, please only upload image files where the type is either PNG or JPG.";
                    }
                    ViewBag.ErrorMessage = message;
                }
            }
            return View("Index");
        }
        
       
    }
}