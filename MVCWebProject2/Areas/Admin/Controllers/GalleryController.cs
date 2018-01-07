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
using System.Web;
using System.Web.Mvc;
using MVCWebProject2.BLL;
using Newtonsoft.Json;
using MVCWebProject2.Areas.Admin.Models;

namespace MVCWebProject2.Areas.Admin.Controllers
{
    [Authorize(Roles = ("Super Admin, Admin"))]
    public class GalleryController : Controller
    {
        // GET: Admin/Gallery
        public ActionResult Index(int? PageNumber, int? NumberOfItems)
        {
            var _pageNumber = 0; 
            var _numberOfItems = 0;
            var result = 0;
            var _numberOfPages = 0;
            if (int.TryParse(Convert.ToString(PageNumber), out result))
                //Pagenumber was present so set it
                _pageNumber = (int)PageNumber;
            else
                //No page number was set so default to page 1
                _pageNumber = 1;
            if (int.TryParse(Convert.ToString(NumberOfItems), out result))
                //Number of items was present so set it
                _numberOfItems = (int)NumberOfItems;
            else
            {
                //Number of items was not set, so default to page 1 with 8 items
                _numberOfItems = 8;
                _pageNumber = 1;
            }
            
            //Retrieve gallery images
            var model = GalleryBLL.GetImageGallery(_pageNumber, _numberOfItems, out _numberOfPages);
            
            //Set these items to manage the pagers in the view
            ViewBag.NumberOfPages = _numberOfPages;
            ViewBag.NumberOfItems = _numberOfItems;
            ViewBag.CurrentPage = _pageNumber;
            return View(model);
        }

        // GET: Admin/AddNewImage
        public ActionResult AddNewImage()
        {
            return RedirectToAction("Index/1/8");
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
                    TempData["Message"] = "File uploaded successfully!";
                   
                    return RedirectToAction("Index/1/8");
                }
                catch (Exception ex)
                {
                    var message = ex.Message + Environment.NewLine + ex.InnerException;
                    if(message.Contains("An image could not be constructed from the content provided."))
                    {
                        message = "File was not an image, please only upload image files where the type is either 'PNG' or 'JPG'.";
                    }
                    TempData["ErrorMessage"] = message;
                    return RedirectToAction("Index/1/8");
                }
            } 
            return View("Index/1/8");
        }
        
       // GET: Admin/Delete/Id
       public ActionResult Delete(int id)
        {
            try
            {
                GalleryBLL.DeleteImage(id);
                TempData["Message"] = "Image deleted successfully";
                return RedirectToAction("Index/1/8");
            }
            catch (Exception ex)
            {
                var message = ex.Message + Environment.NewLine + ex.InnerException;
                TempData["ErrorMessage"] = message;
                return RedirectToAction("Index/1/8");
            }
            
        }

        // GET: Admin/JSONGallery
        [AllowAnonymous]
        public string GetJsonGallery(int? PageNumber, int? NumberOfItems)
        {
            //Set the page number and number of items from the request
            var _pageNumber = PageNumber;
            var _numberOfItems = NumberOfItems;
            var _numberOfPages = 0;

            //Retrieve gallery images and populate the gallery model
            var model = GalleryBLL.GetImageGallery((int)_pageNumber, (int)_numberOfItems, out _numberOfPages);
            var JsonString = new JSONModel();
            JsonString.NumberOfPages = _numberOfPages;
            JsonString.Gallery = model;
            
            //Send JSON response to calling object
            return JsonConvert.SerializeObject(JsonString);
        }
    }
}