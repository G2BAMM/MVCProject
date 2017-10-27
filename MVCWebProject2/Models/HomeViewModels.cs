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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCWebProject2.Models
{
    public class VehicleViewModel
    {
        public int VehicleID { get; set; }
        [Display(Name = "Manufacturer")]
        public string Make { get; set; }

        [Display(Name = "Model")]
        public string ModelType { get; set; }

        [Display(Name = "Body Type")]
        public int BodyTypeId {get; set; }

        [Display(Name = "No of Doors")]
        public int NumberOfDoors { get; set; }
    }

    public class BodyType
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }


    public class Theme
    {
        public string ThemeName { get; set; }
        public string ThemeCSS { get; set; }
    }
}