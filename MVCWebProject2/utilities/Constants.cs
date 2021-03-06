﻿/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : Constants.cs                      '
'  Description      : Holds the app constant values     ' 
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


namespace MVCWebProject2.utilities
{
    public static class Constants
    {
        public enum BootstrapThemes
        {
            [StringValue("bootstrap.css")]
            Standard = 1,
            [StringValue("bootstrap-cerulean.css")]
            Cerulean = 2,
            [StringValue("bootstrap-cosmo.css")]
            Cosmo = 3,
            [StringValue("bootstrap-cyborg.css")]
            Cyborg = 4,
            [StringValue("bootstrap-darkly.css")]
            Darkly = 5,
            [StringValue("bootstrap-flatly.css")]
            Flatly = 6,
            [StringValue("bootstrap-journal.css")]
            Journal = 7,
            [StringValue("bootstrap-lumen.css")]
            Lumen = 8,
            [StringValue("bootstrap-paper.css")]
            Paper = 9,
            [StringValue("bootstrap-readable.css")]
            Readable = 10,
            [StringValue("bootstrap-sandstone.css")]
            Sandstone = 11,
            [StringValue("bootstrap-simplex.css")]
            Simplex = 12,
            [StringValue("bootstrap-slate.css")]
            Slate = 13,
            [StringValue("bootstrap-solar.css")]
            Solar = 14,
            [StringValue("bootstrap-spacelab.css")]
            Spacelab = 15,
            [StringValue("bootstrap-superhero.css")]
            Superhero = 16,
            [StringValue("bootstrap-united.css")]
            United = 17,
            [StringValue("bootstrap-yeti.css")]
            Yeti = 18
        }

        public enum DropDownListDefaultSelection
        {
            [StringValue("--- Please Select ---")]
            PleaseSelect = 1,
            [StringValue("Choose")]
            Choose = 2,
            [StringValue("")]
            Empty = 3
        }

        public enum VehicleMenuManager
        {
            [StringValue("Fuel Type")]
            FuelType = 5,
            [StringValue("Manufacturers")]
            Manufacturer = 10,
            [StringValue("Models")]
            VehicleModel = 15,
            /*
            [StringValue("Vehicle Status")]
            Status = 20,
            */
            [StringValue("Transmission Type")]
            TransmissionType = 25,
            [StringValue("Vehicles")]
            Vehicle = 1,
            [StringValue("Vehicle Type")]
            VehicleType = 30
        }

        public enum VehicleDataSetManager
        {
            [StringValue("Vehicle")]
            Vehicle = 5,
            [StringValue("Manufacturer")]
            Manufacturer = 10,
            [StringValue("Fuel Type")]
            FuelType = 15,
            [StringValue("Transmission Type")]
            TransmissionType = 20,
            [StringValue("Vehicle Model")]
            VehicleModel = 25,
            [StringValue("Vehicle Status")]
            VehicleStatus = 30,
            [StringValue("Vehicle Group")]
            VehicleGroup = 35
        }

        
    }

    
}