/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleViewModels.cs              '
'  Description      : Defines the vehicle attrib models '
'  Author           : Brian McAulay                     '
'  Creation Date    : 21 Dec 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin.Models
{
    public class VehicleListViewModel
    {
        //Shows the list of vehicles on the home page
        public int VehicleId { get; set; }
        [Display(Name = "Make")]
        public string Manufacturer { get; set; }
        [Display(Name = "Model")]
        public string VehicleModel { get; set; }
        [Display(Name = "Reg No")]
        public string RegistrationNumber { get; set; }
        [Display(Name = "Transmission")]
        public string TransmissionType { get; set; }
        [Display(Name = "Group")]
        public string VehicleGroup { get; set; }
    }

    public class VehicleModelListView
    {
        //This is used to build the accordion headers and panes
        public int? ManufacturerId { get; set; }
        public string Manufacturer { get; set; }
        public List<VehicleModelList> modelList { get; set; }
    }

    public class VehicleViewModel
    {
        //The vehicle add/update form
        public int? VehicleID { get; set; }
        public SelectList VehicleManufacturerList { get; set; }
        public SelectList VehicleModelList { get; set; }
        public SelectList VehicleTransmissionList { get; set; }
        public SelectList VehicleGroupList { get; set; }
        public SelectList VehicleFuelList { get; set; }
        public static int MinimumMileage { get; set; }
        [Required]
        [Display(Name = "Make")]
        public int ManufacturerID { get; set; }
        [Required]
        [Display(Name = "Model")]
        public int ModelID { get; set; }
        [Required]
        [StringLength(8, ErrorMessage = "Please enter correct registration number", MinimumLength = 8)]
        [Display(Name = "Reg No")]
        public string RegistrationNumber { get; set; }
        [Required]
        [Display(Name = "Current Mileage")]
        public int CurrentMileage { get; set; }
        [Required]
        [Display(Name = "Transmission")]
        public int TransmissionID { get; set; }
        [Required]
        [Display(Name = "Vehicle Group")]
        public int VehicleGroupID { get; set; }
        [Required]
        [Display(Name = "Fuel Type")]
        public int FuelID { get; set; }
        
    }

    public class VehicleManufacturerList
    {
        //Used to build lists e.g. dropdowns, <ol><ul> or JSON Objects
        public int Id { get; set; }
        [Required]
        [Display(Name = "Manufacturer")]
        [StringLength(50, ErrorMessage = "50 chars max allowed for manufacturer")]
        public string Display { get; set; }
    }

    public class VehicleModelList
    {
        //Used to build lists e.g. dropdowns, <ol><ul> or JSON Objects
        public int Id { get; set; }
        [Required]
        [Display(Name = "Model Name")]
        [StringLength(50, ErrorMessage = "50 chars max allowed for model")]
        public string Display { get; set; }
    }

    public class VehicleTransmissionList
    {
        //Used to build lists e.g. dropdowns, <ol><ul> or JSON Objects
        public int Id { get; set; }
        [Required]
        [Display(Name = "Transmission Type")]
        [StringLength(20, ErrorMessage = "20 chars max allowed for transmission")]
        public string Display { get; set; }
    }

    public class VehicleGroupList
    {
        //Used to build lists e.g. dropdowns, <ol><ul> or JSON Objects
        public int Id { get; set; }
        public string Display { get; set; }
    }

    public class VehicleFuelList
    {
        //Used to build lists e.g. dropdowns, <ol><ul> or JSON Objects & Gallery
        public int Id { get; set; }
        [Required]
        [Display(Name = "Fuel Type")]
        [StringLength(20, ErrorMessage = "20 chars max allowed for fuel type")]
        public string Display { get; set; }
    }

    public class ModelByManufacturerList
    {
        //Used to build paired dropdown lists for JSON Objects on the vehicle form & Gallery
        public SelectList VehicleManufacturerList { get; set; }
        public SelectList VehicleModelList { get; set; }
    }
}