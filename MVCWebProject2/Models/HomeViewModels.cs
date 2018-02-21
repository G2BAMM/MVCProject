/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : HomeViewModels.cs                 '
'  Description      : Manages the home models           ' 
'  Author           : Brian McAulay                     '
'  Creation Date    : 17 Oct 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using System;
using System.Collections.Generic;

namespace MVCWebProject2.Models
{

    public class Theme
    {
        public string ThemeName { get; set; }
        public string ThemeCSS { get; set; }
    }

    public class VehicleSearchResultModel
    {
        public int CategoryId { get; set; }
        public string VehicleClassType { get; set; }
        public string ThumbNail { get; set; }
        public string TransmissionType { get; set; }
        public string NumberOfSeats { get; set; }
        public string NumberOfDoors { get; set; }
        public string NumberOfBags { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public string FuelType { get; set; }
        public decimal DailyRate { get; set; }
        public decimal WeekendRate { get; set; }
        public decimal WeeklyRate { get; set; }
        public decimal MonthlyRate { get; set; }
        public string BasicDescription { get; set; }
        public List<RentalExtrasListModel> RentalExtras { get; set; }

    }

    public class RentalExtrasListModel
    {
        public int ExtraId { get; set; }
        public string ExtraDescription { get; set; }
        public decimal Price { get; set; }
    }

    public class VehicleSearchModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
    }
}