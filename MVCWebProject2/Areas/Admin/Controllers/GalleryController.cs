using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

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
                var fileName = Path.GetFileName(fileUpload.FileName);
                var memoryStream = new MemoryStream();
                var fileStream = fileUpload.InputStream;
                fileStream.Position = 0;
                fileStream.CopyTo(memoryStream);

                //First we need to check that it's a valid image we have uploaded
                if (!ValidateFileExtension(fileName))
                {
                    //Invalid file type so refuse the upload
                    ViewBag.ErrorMessage = "Invalid file type, only files with the extension 'JPG' or 'PNG' allowed.";
                    return View("Index");
                }
                //Now we need to make sure we have the minimum image dimensions as we need to make 3 versions
                if (!ValidateImageSize(memoryStream))
                {
                    //Invalid image sizes so refuse the upload
                    ViewBag.ErrorMessage = "Image is too small, the minimum dimension is 200px by 200px.";
                    return View("Index");
                }
                //Remove any spaces in the image filename and replace with underscores
                fileName = fileName.Replace(" ", "_");
                //Generate a unique filename, which will include the original name too
                fileName = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + "_" + fileName;
                //Generate the 'save to' path for the raw uploaded file
                var path = Server.MapPath("~/images/uploads/");
                
                //Generate our thumbnail
                memoryStream = new MemoryStream();
                fileStream.Position = 0;
                fileStream.CopyTo(memoryStream);
                var thumbNail = ResizeImage(memoryStream, "thumb", fileName);
                
                //Generate our small thumbnail
                memoryStream = new MemoryStream();
                fileStream.Position = 0;
                fileStream.CopyTo(memoryStream);
                var smallImage = ResizeImage(memoryStream, "small", fileName);

                try
                {
                    //Save the main image
                    fileUpload.SaveAs(path + fileName);
                    //Save the thumbnail
                    thumbNail.Save(path + thumbNail.FileName);
                    //Save the small thumbnail
                    smallImage.Save(path + smallImage.FileName);
                    //Set our message
                    ViewBag.Message = "File uploaded successfully!";
                }
                catch (Exception ex)
                {
                    var message = ex.Message + "\n\n" + ex.InnerException;
                    ViewBag.ErrorMessage = message;
                }
            }
            return View("Index");
        }
        
        private bool ValidateImageSize(MemoryStream imageFile)
        {
            WebImage img = new WebImage(imageFile);
            if (img.Width < 200 || img.Height < 200)
                return false; 
            else
                return true;
        }

        private WebImage ResizeImage(MemoryStream thumbFile, string imageSize, string fileName)
        {
            WebImage thumbNail = new WebImage(thumbFile);
            
            switch (imageSize.ToLower())
            {
                case "small":
                    thumbNail.Resize(100, 100, true);
                    thumbNail.FileName = fileName.Replace(Path.GetExtension(fileName).ToLower(), "_small") + Path.GetExtension(fileName).ToLower();
                    return thumbNail;
                default:
                    thumbNail.Resize(200, 200, true);
                    thumbNail.FileName = fileName.Replace(Path.GetExtension(fileName).ToLower(), "_thumb") + Path.GetExtension(fileName).ToLower();
                    return thumbNail;       
            }
        }

        private bool ValidateFileExtension(string fileName)
        {
            var isValid = false;

            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".jpg":
                    isValid = true;
                    break;
                case ".jpeg":
                    isValid = true;
                    break;
                case ".png":
                    isValid = true;
                    break;
                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }
    }
}