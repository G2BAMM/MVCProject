/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleBLL.cs                     '
'  Description      : Manages the business logic on     '
'                     items saved/retrieved to/from SQL '
'  Author           : Brian McAulay                     '
'  Creation Date    : 28 Dec 2017                       '
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
using MVCWebProject2.utilities;

namespace MVCWebProject2.BLL
{
    public class VehicleBLL
    {

        #region GetVehicleList
        public static List<VehicleListViewModel> GetVehicleList()
        {
            List<VehicleListViewModel> myList = new List<VehicleListViewModel>();
            DataTable dt = VehicleDAL.GetVehicleList();
            foreach (DataRow row in dt.Rows)
            {
                VehicleListViewModel myListItems = new VehicleListViewModel
                {
                    VehicleId = (int)row["VehicleID"],
                    Manufacturer = row["Manufacturer"].ToString(),
                    RegistrationNumber = row["RegistrationNumber"].ToString(),
                    VehicleModel = row["ModelName"].ToString(),
                    TransmissionType = row["TransmissionType"].ToString(),
                    VehicleStatus = row["VehicleStatus"].ToString()
                };
                myList.Add(myListItems);
            }
            return myList;
        }

        #endregion

        #region GetVehicleDetails
        public static VehicleViewModel GetVehicleDetails(int VehicleID)
        {
            var ds = VehicleDAL.GetVehicleDetails(VehicleID);
            var model = new VehicleViewModel();
            foreach (DataTable dt in ds.Tables)
            {
                var row = dt.Rows[0];
                if (row["TableName"].ToString() == Constants.VehicleDataSetManager.Vehicle.ToString())
                {
                    model.ManufacturerID = (int)row["ManufacturerID"];
                    model.ModelID = (int)row["ModelID"];
                    model.RegistrationNumber = row["RegistrationNumber"].ToString();
                    model.TransmissionID = (int)row["TransmissionID"];
                    model.VehicleGroupID = (int)row["VehicleGroupID"];
                    model.VehicleID = (int)row["VehicleID"];
                    model.FuelID = (int)row["FuelID"];
                    model.StatusID = (int)row["StatusID"];
                }
                else if (row["TableName"].ToString() == Constants.VehicleDataSetManager.FuelType.ToString())
                {
                    model.VehicleFuelList = PopulateDropDownList(Constants.VehicleDataSetManager.FuelType, dt);
                }
                else if (row["TableName"].ToString() == Constants.VehicleDataSetManager.Manufacturer.ToString())
                {
                    model.VehicleManufacturerList = PopulateDropDownList(Constants.VehicleDataSetManager.Manufacturer, dt);
                }
                else if (row["TableName"].ToString() == Constants.VehicleDataSetManager.TransmissionType.ToString())
                {
                    model.VehicleTransmissionList = PopulateDropDownList(Constants.VehicleDataSetManager.TransmissionType, dt);
                }
                else if (row["TableName"].ToString() == Constants.VehicleDataSetManager.VehicleGroup.ToString())
                {
                    model.VehicleGroupList = PopulateDropDownList(Constants.VehicleDataSetManager.VehicleGroup, dt);
                }
                else if (row["TableName"].ToString() == Constants.VehicleDataSetManager.VehicleModel.ToString())
                {
                    model.VehicleModelList = PopulateDropDownList(Constants.VehicleDataSetManager.VehicleModel, dt);
                }
                else if (row["TableName"].ToString() == Constants.VehicleDataSetManager.VehicleStatus.ToString())
                {
                    model.VehicleStatusList = PopulateDropDownList(Constants.VehicleDataSetManager.VehicleStatus, dt);
                }
            }

            return model;
        }

        #endregion

        #region UpdateVehicle
        public static int UpdateVehicle(int VehicleID,
                                         int ModelID,
                                         string RegistrationNumber,
                                         int StatusID,
                                         int TransmissionID,
                                         int VehicleGroupID,
                                         int FuelID,
                                         string UpdatedBy)
        {
            var result = VehicleDAL.UpdateVehicle(VehicleID, ModelID, RegistrationNumber, StatusID, TransmissionID, VehicleGroupID, FuelID, UpdatedBy);
            return result;
        }
        #endregion

        #region PopulateDropDowns
        private static SelectList PopulateDropDownList(Constants.VehicleDataSetManager TableName, DataTable dt)
        {
            SelectList DropDownList = null;
            switch (TableName)
            {
                case Constants.VehicleDataSetManager.FuelType:
                    var FuelList = new List<VehicleFuelList>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        FuelList.Add(new VehicleFuelList
                        {
                            Id = (int)dataRow["FuelID"],
                            Display = dataRow["FuelType"].ToString()
                        });
                    }
                    DropDownList = new SelectList(FuelList, "id", "Display");
                    break;
                case Constants.VehicleDataSetManager.Manufacturer:
                    var ManufacturerList = new List<VehicleManufacturerList>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        ManufacturerList.Add(new VehicleManufacturerList
                        {
                            Id = (int)dataRow["ManufacturerID"],
                            Display = dataRow["Manufacturer"].ToString()
                        });
                    }
                    DropDownList = new SelectList(ManufacturerList, "id", "Display");
                    break;
                case Constants.VehicleDataSetManager.TransmissionType:
                    var TransmissionList = new List<VehicleTransmissionList>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        TransmissionList.Add(new VehicleTransmissionList
                        {
                            Id = (int)dataRow["TransmissionID"],
                            Display = dataRow["TransmissionType"].ToString()
                        });
                    }
                    DropDownList = new SelectList(TransmissionList, "id", "Display");
                    break;
                case Constants.VehicleDataSetManager.VehicleGroup:
                    var VehicleGroupList = new List<VehicleGroupList>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        VehicleGroupList.Add(new VehicleGroupList
                        {
                            Id = (int)dataRow["CategoryID"],
                            Display = dataRow["VehicleClassType"].ToString()
                        });
                    }
                    DropDownList = new SelectList(VehicleGroupList, "id", "Display");
                    break;
                case Constants.VehicleDataSetManager.VehicleModel:
                    var VehicleModelList = new List<VehicleModelList>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        VehicleModelList.Add(new VehicleModelList
                        {
                            Id = (int)dataRow["ModelID"],
                            Display = dataRow["ModelName"].ToString()
                        });
                    }
                    DropDownList = new SelectList(VehicleModelList, "id", "Display");
                    break;
                case Constants.VehicleDataSetManager.VehicleStatus:
                    var VehicleStatusList = new List<VehicleStatusList>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        VehicleStatusList.Add(new VehicleStatusList
                        {
                            Id = (int)dataRow["StatusID"],
                            Display = dataRow["VehicleStatus"].ToString()
                        });
                    }
                    DropDownList = new SelectList(VehicleStatusList, "id", "Display");
                    break;
                default:
                    break;
            }

            return DropDownList;
        }
        #endregion
    }
}
