/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleCategoriesBLL.cs           '
'  Description      : Manages the business logic on     '
'                     items saved/retrieved to/from SQL '
'  Author           : Brian McAulay                     '
'  Creation Date    : 08 Nov 2017                       '
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
using System.IO;
using System.Web.Mvc;

namespace MVCWebProject2.BLL
{
    public class VehicleCategoriesBLL 
    {
        #region GetList
        //Get the main list of vehicle types
        public static List<VehicleCategoryListViewModel> GetVehicleCategoryList()
        {
            List<VehicleCategoryListViewModel> myList = new List<VehicleCategoryListViewModel>();
            DataTable dt = VehicleCategoriesDAL.GetVehicleCategoryList();
            foreach(DataRow row in dt.Rows)
            {
                VehicleCategoryListViewModel myListItems = new VehicleCategoryListViewModel
                {
                    Id = (int)row["CategoryID"],
                    VehicleImage = row["ImageName"].ToString(),
                    VehicleClassType = row["VehicleClassType"].ToString().Length < 20 ? row["VehicleClassType"].ToString() : row["VehicleClassType"].ToString().Substring(0, 20) + "...",
                    VehicleType = row["VehicleType"].ToString(),
                    LastUpdatedBy = row["LastUpdatedBy"].ToString(),
                    LastUpdated = Convert.ToDateTime(row["LastUpdated"]).ToString("dd MMM yyyy")
                };
                myList.Add(myListItems);
            }
            return myList;
        }

        #endregion

        #region GetVehicleCatagoryDataSet
        //Get the individual vehicle category record
        public static VehicleCategoryViewModel GetVehicleCategoryDataset(int CategoryID)
        {
            //Generate a new empty model
            var model = new VehicleCategoryViewModel();

            //Get the dataset from SQL
            DataSet ds = VehicleCategoriesDAL.GetVehicleCategory(CategoryID);
            //Move dataset to the first table
            DataTable dt = ds.Tables[0];
            //Get the first row from the table and populate the model propertites
            DataRow row = dt.Rows[0];
            model.Id = CategoryID;
            model.BasicDescription = row["BasicDescription"].ToString().Trim();
            model.DailyRate = (decimal)row["DailyRate"];
            model.LuggageCapacity = (int)row["LuggageCapacity"];
            model.MonthlyRate = (decimal)row["MonthlyRate"];
            model.NumberOfSeats = (int)row["NumberOfSeats"];
            model.VehicleClassType = row["VehicleClassType"].ToString();
            model.ImageId = (int)row["ImageId"];
            model.VehicleTypeID = (int)row["VehicleTypeID"];
            model.WeekendRate = (decimal)row["WeekendRate"];
            model.WeeklyRate = (decimal)row["WeeklyRate"];
            model.ImageName = row["ImageName"].ToString().Replace(Path.GetExtension(row["ImageName"].ToString()).ToLower(), "_thumb") + Path.GetExtension(row["ImageName"].ToString()).ToLower().Trim();

            //Get the second table in order to populate the related dropdown list
            dt = ds.Tables[1];
            var list = new List<VehicleTypeList>();
            foreach (DataRow dataRow in dt.Rows)
            {
                list.Add(new VehicleTypeList
                {
                    Id = (int)dataRow["VehicleTypeID"],
                    Display = dataRow["VehicleType"].ToString()
                });
            }
            model.VehicleType = new SelectList(list, "id", "Display");
            return model;
        }

        #endregion

        #region UpdateRecord
        //Update the vehicle category record
        public static void UpdateVehicleCategory(VehicleCategoryViewModel model, string UpdatedBy)
        {
            VehicleCategoriesDAL.UpdateVehicleCategory((int)model.Id,
                                                              model.VehicleClassType,
                                                              model.VehicleTypeID,
                                                              model.ImageId,
                                                              model.DailyRate,
                                                              model.WeeklyRate,
                                                              model.WeekendRate,
                                                              model.MonthlyRate,
                                                              model.NumberOfSeats,
                                                              model.BasicDescription,
                                                              model.LuggageCapacity,
                                                              UpdatedBy);
                 
            
            
            
        }

        #endregion

        #region AddNewRecord

        public static int AddNewVehicleCategory(VehicleCategoryViewModel model, string UpdatedBy)
        {
            var result = VehicleCategoriesDAL.AddNewVehicleCategory(model.VehicleClassType,
                                                              model.VehicleTypeID,
                                                              model.ImageId,
                                                              model.DailyRate,
                                                              model.WeeklyRate,
                                                              model.WeekendRate,
                                                              model.MonthlyRate,
                                                              model.NumberOfSeats,
                                                              model.BasicDescription,
                                                              model.LuggageCapacity,
                                                              UpdatedBy);



            return result;
        }

        #endregion
        
        #region GetVehicleList
        public static VehicleCategoryViewModel GetVehicleList()
        {
            DataTable dt = VehicleCategoriesDAL.GetVehicleTypes();
            var model = new VehicleCategoryViewModel();
            var list = new List<VehicleTypeList>();
            foreach (DataRow dataRow in dt.Rows)
            {
                list.Add(new VehicleTypeList
                {
                    Id = (int)dataRow["VehicleTypeID"],
                    Display = dataRow["VehicleType"].ToString()
                });
            }
            model.VehicleType = new SelectList(list, "id", "Display");
            return model;
        }
        #endregion

    }
}