/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : ManufacturerBLL.cs                '
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
using MVCWebProject2.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWebProject2.BLL
{
    public class ManufacturerBLL
    {

        #region GetManufacturerList
        public static IEnumerable<VehicleManufacturerList> GetManufacturerList()
        {
            var ManufacturerList = new List<VehicleManufacturerList>();
            var dt = ManufacturerDAL.GetManufacturerList();
            foreach (DataRow dataRow in dt.Rows)
            {
                ManufacturerList.Add(new VehicleManufacturerList
                {
                    Id = (int)dataRow["ManufacturerID"],
                    Display = dataRow["Manufacturer"].ToString()
                });
            }
            return ManufacturerList;
        }
        #endregion

        #region GetManufacturer
        public static VehicleManufacturerList GetManufacturer(int ManufacturerID)
        {
            var model = new VehicleManufacturerList();
            var dt = ManufacturerDAL.GetManufacturer(ManufacturerID);
            var dr = dt.Rows[0];
            model.Id = (int)dr["ManufacturerID"];
            model.Display = dr["Manufacturer"].ToString();
            return model;
        }
        #endregion

        #region UpdateManufacturer
        public static void UpdateManufacturer(VehicleManufacturerList model, string UpdatedBy)
        {
            ManufacturerDAL.UpdateManufacturer(model.Id, model.Display, UpdatedBy);
        }
        #endregion

        #region AddManufacturer
        public static void AddManufacturer(VehicleManufacturerList model, string UpdatedBy, out int returnValue)
        {
            ManufacturerDAL.AddManufacturer(model.Display, UpdatedBy, out returnValue);
        }
        #endregion
    }
}