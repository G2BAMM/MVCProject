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


namespace MVCWebProject2.BLL
{
    public class GalleryBLL
    {
        #region GetImageGallery
        public static List<GalleryListViewModel> GetImageGallery(int pageNumber, int numberOfItems, out int numberOfPages)
        {
            List<GalleryListViewModel> myList = new List<GalleryListViewModel>();
            DataTable dt = GalleryDAL.GetImageGalleryList(pageNumber, numberOfItems, out numberOfPages);
            foreach (DataRow row in dt.Rows)
            {
                GalleryListViewModel myListItems = new GalleryListViewModel
                {
                    ImageId = (int)row["ImageID"],
                    MainImage = row["ImageName"].ToString().Trim(),
                    ThumbnailImage = row["ImageName"].ToString().Replace(Path.GetExtension(row["ImageName"].ToString()).ToLower(), "_thumb") + Path.GetExtension(row["ImageName"].ToString()).ToLower().Trim(),
                    SmallThumbnail = row["ImageName"].ToString().Replace(Path.GetExtension(row["ImageName"].ToString()).ToLower(), "_small") + Path.GetExtension(row["ImageName"].ToString()).ToLower().Trim(),
                    Disabled = (bool)row["Disabled"]
                };
                myList.Add(myListItems);
            }
            return myList;
        }

        public static DataTable GetJsonImageGallery(int pageNumber, int numberOfItems, out int numberOfPages)
        {
            return GalleryDAL.GetImageGalleryList(pageNumber, numberOfItems, out numberOfPages);
        }
        #endregion

        #region DeleteImage
        public static void DeleteImage(int ImageID)
        {
            //First we delete from SQL, this will return the main image name to allow us to delete the files from the server
            var imageName = GalleryDAL.DeleteGalleryImage(ImageID);
            //Check that we got a value back, if this is an empty string then there was no image to delete 
            //and attempting to remove them from the drive will cause an error 
            if (imageName != "")
            {
                var path = HttpContext.Current.Server.MapPath("~/images/uploads/");
                var ThumbnailImage = imageName.Replace(Path.GetExtension(imageName.ToString()).ToLower(), "_thumb") + Path.GetExtension(imageName.ToString()).ToLower().Trim();
                var SmallThumbnail = imageName.Replace(Path.GetExtension(imageName.ToString()).ToLower(), "_small") + Path.GetExtension(imageName.ToString()).ToLower().Trim();
                //Now delete the 3 files
                File.Delete(path + imageName);
                File.Delete(path + ThumbnailImage);
                File.Delete(path + SmallThumbnail);
            }
            else
            {
                throw new InvalidCastException("No image selected for deletion, please use the 'Delete' button to remove the image you wish to delete.");
            }
        }
        #endregion

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