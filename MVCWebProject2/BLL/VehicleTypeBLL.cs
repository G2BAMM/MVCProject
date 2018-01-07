using MVCWebProject2.Areas.Admin.Models;
using MVCWebProject2.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MVCWebProject2.BLL
{
    public class VehicleTypeBLL
    {
        public static IEnumerable<VehicleTypeList> GetVehicleTypeList()
        {
            var VehicleTypeList = new List<VehicleTypeList>();
            var dt = VehicleTypeDAL.GetVehicleTypeList();
            foreach (DataRow dataRow in dt.Rows)
            {
                VehicleTypeList.Add(new VehicleTypeList
                {
                    Id = (int)dataRow["VehicleTypeID"],
                    Display = dataRow["VehicleType"].ToString()
                });
            }
            return VehicleTypeList;
        }

        public static VehicleTypeList GetVehicleType(int VehicleTypeID)
        {
            var model = new VehicleTypeList();
            var dt = VehicleTypeDAL.GetVehicleType(VehicleTypeID);
            var dr = dt.Rows[0];
            model.Id = (int)dr["VehicleTypeID"];
            model.Display = dr["VehicleType"].ToString();
            return model;
        }

        public static void UpdateVehicleType(VehicleTypeList model, string UpdatedBy)
        {
            VehicleTypeDAL.UpdateVehicleType(model.Id, model.Display, UpdatedBy);
        }

        public static void AddVehicleType(VehicleTypeList model, string UpdatedBy, out int returnValue)
        {
            VehicleTypeDAL.AddVehicleType(model.Display, UpdatedBy, out returnValue);
        }
    }
}