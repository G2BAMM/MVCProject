using System.Collections.Generic;

namespace MVCWebProject2.Areas.Admin.Models
{

    public class GalleryListViewModel
    {
        //This populates the galleries both modal and the manager page on CMS
        public int ImageId { get; set; }
        public string MainImage { get; set; }
        public string ThumbnailImage { get; set; }
        public string SmallThumbnail { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public bool Disabled { get; set; }
    }
    
    public class JSONModel
    {
        //This produces the JSON list of images for the modal gallery
        public int NumberOfPages { get; set; }
        public List<GalleryListViewModel> Gallery {get; set;}
    }

    
}