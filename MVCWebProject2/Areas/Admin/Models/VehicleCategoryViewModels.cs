/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleCategoryViewModels.cs      '
'  Description      : Defines the vehicle group models  '
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin.Models
{

    public class VehicleCategoryListViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Vehicle Description")]
        public string VehicleClassType { get; set; }
        [Display(Name = "Vehicle Name")]
        public string VehicleType { get; set; }
        [Display(Name = "Category Image")]
        public string VehicleImage { get; set; }
        [Display(Name = "Updated By")]
        public string LastUpdatedBy { get; set; }
        [Display(Name = "Updated")]
        public string LastUpdated { get; set; }
    }

    public class VehicleCategoryViewModel
    {
        public int? Id { get; set; }
        [Required]
        [Display(Name = "Vehicle Name")]
        [StringLength(50, ErrorMessage = "Max is 50 chars for vehicle name.", MinimumLength = 1)]
        public string VehicleClassType { get; set; }
        public SelectList VehicleType { get; set; }
        [Required]
        [Display(Name = "Vehicle Type")]
        public int VehicleTypeID { get; set; }
        [Display(Name = "Vehicle Image")]
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true,  DataFormatString = "{0:F2}")]
        [Display(Name = "Daily Rate")]
        public decimal DailyRate { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Weekly Rate")]
        public decimal WeeklyRate { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Weekend Rate")]
        public decimal WeekendRate { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Monthly Rate")]
        public decimal MonthlyRate { get; set; }
        [Required]
        [Display(Name = "No of Seats")]
        [Range(2, 16, ErrorMessage = "Must have at least 2 seats and not more than 16")]
        public int NumberOfSeats { get; set; }
        [Required]
        [Display(Name = "Vehicle Features")]
        [StringLength(250, ErrorMessage = "Max is 250 chars for vehicle description.", MinimumLength = 1)]
        public string BasicDescription { get; set; }
        [Required]
        [Display(Name = "No of Bags/Cases")]
        [Range(1, 4, ErrorMessage = "Min 1 piece of luggage and max 4")]
        public int LuggageCapacity { get; set; }
    }

    public class VehicleTypeList
    {
        public string Id { get; set; }
        public string Display { get; set; }
    }

    public class VehicleCategoryImageGalleryList
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
    }

}