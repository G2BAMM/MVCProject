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
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCWebProject2.Areas.Admin.Models
{
    public class VehicleListViewModel
    {
        public int VehicleId { get; set; }
        [Display(Name = "Make")]
        public string Manufacturer { get; set; }
        [Display(Name = "Model")]
        public string VehicleModel { get; set; }
        [Display(Name = "Reg No")]
        public string RegistrationNumber { get; set; }
        [Display(Name = "Transmission")]
        public string TransmissionType { get; set; }
        [Display(Name = "Current Status")]
        public string VehicleStatus { get; set; }
    }

    public class VehicleViewModel
    {
        public int? VehicleID { get; set; }
        public SelectList VehicleManufacturerList { get; set; }
        public SelectList VehicleModelList { get; set; }
        public SelectList VehicleStatusList { get; set; }
        public SelectList VehicleTransmissionList { get; set; }
        public SelectList VehicleGroupList { get; set; }
        public SelectList VehicleFuelList { get; set; }
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
        [Display(Name = "Status")]
        public int StatusID { get; set; }
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
        public int Id { get; set; }
        [Required]
        [Display(Name = "Manufacturer")]
        [StringLength(50, ErrorMessage = "50 chars max allowed for manufacturer")]
        public string Display { get; set; }
    }

    public class VehicleModelList
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Model Name")]
        [StringLength(50, ErrorMessage = "50 chars max allowed for model")]
        public string Display { get; set; }
    }

    public class VehicleStatusList
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Hire Status")]
        [StringLength(20, ErrorMessage = "20 chars max allowed for hire status")]
        public string Display { get; set; }
    }

    public class VehicleTransmissionList
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Transmission Type")]
        [StringLength(20, ErrorMessage = "20 chars max allowed for transmission")]
        public string Display { get; set; }
    }

    public class VehicleGroupList
    {
        public int Id { get; set; }
        public string Display { get; set; }
    }

    public class VehicleFuelList
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Fuel Type")]
        [StringLength(20, ErrorMessage = "20 chars max allowed for fuel type")]
        public string Display { get; set; }
    }

    public class ModelByManufacturerList
    {
        public SelectList VehicleManufacturerList { get; set; }
        public SelectList VehicleModelList { get; set; }
    }
}