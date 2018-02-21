/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleSearchBLL.cs               '
'  Description      : Manages the business logic on     '
'                     items saved/retrieved to/from SQL '
'  Author           : Brian McAulay                     '
'  Creation Date    : 03 Feb 2018                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using MVCWebProject2.DAL;
using MVCWebProject2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace MVCWebProject2.BLL
{
    public class VehicleSearchBLL
    {

        #region SearchVehicles
        public static List<VehicleSearchResultModel> GetVehicleSearchResults(DateTime StartDate, DateTime EndDate)
        {
            var model = new List<VehicleSearchResultModel>();
            var dt = VehicleSearchDAL.GetVehicleSearchResults(StartDate, EndDate);
            foreach (DataRow dataRow in dt.Rows)
            {
                model.Add(new VehicleSearchResultModel
                {
                    CategoryId = (int)dataRow["CategoryID"],
                    VehicleClassType = dataRow["VehicleClassType"].ToString(),
                    ThumbNail = dataRow["ImageName"].ToString().Replace(Path.GetExtension(dataRow["ImageName"].ToString()).ToLower(), "_thumb") + Path.GetExtension(dataRow["ImageName"].ToString()).ToLower().Trim(),
                    TransmissionType = dataRow["TransmissionType"].ToString(),
                    NumberOfSeats = dataRow["NumberOfseats"].ToString(),
                    NumberOfDoors = dataRow["NumberOfDoors"].ToString(),
                    NumberOfBags = dataRow["LuggageCapacity"].ToString(),
                    Manufacturer = dataRow["Manufacturer"].ToString(),
                    ModelName = dataRow["ModelName"].ToString(),
                    FuelType = dataRow["FuelType"].ToString(),
                    DailyRate = (decimal)dataRow["DailyRate"],
                    WeekendRate = (decimal)dataRow["WeekendRate"],
                    WeeklyRate = (decimal)dataRow["WeeklyRate"],
                    MonthlyRate = (decimal)dataRow["MonthlyRate"],
                    BasicDescription = dataRow["BasicDescription"].ToString(),
                    RentalExtras = ExtrasBLL.GetRentalExtras()
                });
            }
           
            return model;
        }
        #endregion
    }
}