/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : GalleryBLL.cs                     '
'  Description      : Manages the business logic on     '
'                     items saved/retrieved to/from SQL '
'  Author           : Brian McAulay                     '
'  Creation Date    : 27 Nov 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using MVCWebProject2.Areas.Admin.Models;
using MVCWebProject2.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MVCWebProject2.BLL
{
    public class GalleryBLL
    {

    #region ImageUploadProcesses

        #region AddNewGalleryImage
        public static void AddNewGalleryImage(HttpPostedFileBase fileUpload, string updatedBy)
        {
            try
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                //Make sure we don't attempt to upload a very large file
                if (fileUpload.ContentLength > 1048576) //1MB limit
                {
                    throw new InvalidCastException("File size too big, please make sure your file size is less than 1MB.");
                }

                //We need to check that it's a valid image we have uploaded
                if (!ValidateFileExtension(fileName))
                {
                    //Invalid file type so refuse the upload
                    throw new InvalidCastException("Invalid file type, only files with the extension 'JPG' or 'PNG' allowed.");
                }
                

                //Now we can try to create our images
                WebImage uploadedImage = new WebImage(fileUpload.InputStream);
                
                //Now we need to make sure we have the minimum image dimensions as we need to make 3 versions
                if (!ValidateImageSize(uploadedImage))
                {
                    //Invalid image sizes so refuse the upload
                    throw new InvalidCastException("Image is too small, the minimum dimension is Width 200px by Height 150px.");
                }

                //Remove any spaces in the image filename and replace with underscores
                fileName = fileName.Replace(" ", "_");
                
                //Generate a unique filename, which will include the original name too
                fileName = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + "_" + fileName;
                
                //Generate the 'save to' path for the raw uploaded file
                var path = HttpContext.Current.Server.MapPath("~/images/uploads/");
                
                //Now save the original file to disk
                uploadedImage.Save(path + fileName);

                //Generate the thumbnails and save them
                var thumbnailImage = ResizeImage(uploadedImage, "thumb", fileName);
                thumbnailImage.Save(path + thumbnailImage.FileName);
                var smallThumbnailImage = ResizeImage(uploadedImage, "small", fileName);
                smallThumbnailImage.Save(path + smallThumbnailImage.FileName);
                
                //Finally update the DB now
                GalleryDAL.AddNewGalleryImage(fileName, updatedBy);
            }
            finally
            {

            }
        }

        #endregion

        #region ValidateImageSize
        private static bool ValidateImageSize(WebImage img)
        {
            if (img.Width < 200 || img.Height < 150)
                return false;
            else
                return true;
        }
        #endregion

        #region ResizeImage
        private static WebImage ResizeImage(WebImage thumbNail, string imageSize, string fileName)
        {

            switch (imageSize.ToLower())
            {
                case "small":
                    thumbNail.Resize(100, 75, false);
                    thumbNail.FileName = fileName.Replace(Path.GetExtension(fileName).ToLower(), "_small") + Path.GetExtension(fileName).ToLower();
                    return thumbNail;
                default:
                    thumbNail.Resize(200, 150, false);
                    thumbNail.FileName = fileName.Replace(Path.GetExtension(fileName).ToLower(), "_thumb") + Path.GetExtension(fileName).ToLower();
                    return thumbNail;
            }
        }
        #endregion

        #region ValidateFileExtension
        private static bool ValidateFileExtension(string fileName)
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
    #endregion

    #endregion


}