/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleModelBLL.cs                '
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

namespace MVCWebProject2.BLL
{
    public class VehicleModelBLL
    {
        #region GetJSONModelByManufacturer
        public static IEnumerable<VehicleModelList> GetJSONModelByManufacturer(int ManufacturerID)
        {
            var VehicleModelList = new List<VehicleModelList>();
            var dt = VehicleModelDAL.GetModelListByManufacturer(ManufacturerID);
            foreach (DataRow dataRow in dt.Rows)
            {
                VehicleModelList.Add(new VehicleModelList
                {
                    Id = (int)dataRow["ModelID"],
                    Display = dataRow["ModelName"].ToString()
                });
            }
            return VehicleModelList;
        }
        #endregion

        #region GetModelByManufacturer
        public static IEnumerable<VehicleModelList> GetModelByManufacturer(int ManufacturerID)
        {
            var VehicleModelList = new List<VehicleModelList>();
            var dt = VehicleModelDAL.GetModelListByManufacturer(ManufacturerID);
            foreach (DataRow dataRow in dt.Rows)
            {
                VehicleModelList.Add(new VehicleModelList
                {
                    Id = (int)dataRow["ModelID"],
                    Display = dataRow["ModelName"].ToString()
                });
            }
            return VehicleModelList;
        }
        #endregion

        #region GetAccordionList
        public static List<VehicleModelListView> BuildAccordionModel()
        {
            List<VehicleModelListView> model = new List<VehicleModelListView>();
            var manufacturer = ManufacturerDAL.GetManufacturerList();
            foreach (DataRow dataRow in manufacturer.Rows)
            {
                VehicleModelListView vehicleModelView = new VehicleModelListView
                {
                    ManufacturerId = (int)dataRow["ManufacturerID"],
                    Manufacturer = dataRow["Manufacturer"].ToString(),
                    modelList = GetModelByManufacturerList((int)dataRow["ManufacturerID"])
                };
                model.Add(vehicleModelView);
            }
            return model;
        }
        #endregion

        #region GetModelByManufacturer
        private static List<VehicleModelList> GetModelByManufacturerList(int ManufacturerID)
        {
            var VehicleModelList = new List<VehicleModelList>();
            var dt = VehicleModelDAL.GetModelListByManufacturer(ManufacturerID);
            foreach (DataRow dataRow in dt.Rows)
            {
                VehicleModelList.Add(new VehicleModelList
                {
                    Id = (int)dataRow["ModelID"],
                    Display = dataRow["ModelName"].ToString()
                });
            }
            return VehicleModelList;
        }
        #endregion

        #region AddNewModel
        public static void AddNewModel(int ManufacturerID, string ModelName, string UpdatedBy, out int returnValue)
        {
            VehicleModelDAL.AddNewModel(ManufacturerID, ModelName, UpdatedBy, out returnValue);
        }
        #endregion

        #region UpdateModel
        public static void UpdateModel(int ModelID, string ModelName, string UpdatedBy, out int returnValue)
        {
            VehicleModelDAL.UpdateModel(ModelID, ModelName, UpdatedBy, out returnValue);
        }
        #endregion

    }

}