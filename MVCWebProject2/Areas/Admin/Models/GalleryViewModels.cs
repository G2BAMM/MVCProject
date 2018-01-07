using System.Collections.Generic;

namespace MVCWebProject2.Areas.Admin.Models
{

    public class GalleryListViewModel
    {
        public int ImageId { get; set; }
        public string MainImage { get; set; }
        public string ThumbnailImage { get; set; }
        public string SmallThumbnail { get; set; }
        public bool Disabled { get; set; }
    }

    public class JSONModel
    {
        public int NumberOfPages { get; set; }
        public List<GalleryListViewModel> Gallery {get; set;}
    }

    
}