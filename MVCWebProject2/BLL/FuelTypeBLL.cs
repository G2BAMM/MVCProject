/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : FuelTypeBLL.cs                    '
'  Description      : Manages the business logic on     '
'                     items saved/retrieved to/from SQL '
'  Author           : Brian McAulay                     '
'  Creation Date    : 05 Jan 2018                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using MVCWebProject2.Areas.Admin.Models;
using System.Collections.Generic;
using MVCWebProject2.DAL;
using System.Data;

namespace MVCWebProject2.BLL
{
    public class FuelTypeBLL
    {
        public static IEnumerable<VehicleFuelList> GetFuelTypes()
        {
            var FuelList = new List<VehicleFuelList>();
            var dt = FuelTypeDAL.GetFuelTypes();
            foreach (DataRow dataRow in dt.Rows)
            {
                FuelList.Add(new VehicleFuelList
                {
                    Id = (int)dataRow["FuelID"],
                    Display = dataRow["FuelType"].ToString()
                });
            }
            return FuelList;
        }

        public static VehicleFuelList GetFuelType(int FuelID)
        {
            var model = new VehicleFuelList();
            var dt = FuelTypeDAL.GetFuelType(FuelID);
            var dr = dt.Rows[0];
            model.Id = (int)dr["FuelID"];
            model.Display = dr["FuelType"].ToString();
            return model;
        }

        public static void UpdateFuelType(VehicleFuelList model, string UpdatedBy)
        {
            FuelTypeDAL.UpdateFuelType(model.Id, model.Display, UpdatedBy);
        }

        public static void AddFuelType(VehicleFuelList model, string UpdatedBy, out int returnValue)
        {
             FuelTypeDAL.AddFuelType(model.Display, UpdatedBy, out returnValue);
        }
    }
}