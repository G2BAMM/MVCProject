/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleDAL.cs                     '
'  Description      : Manages the data transport to/from'
'                     data source                       '
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

using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebProject2.DAL
{
    public class VehicleDAL
    {
        #region SQLConnectionSetUp

        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        private static DataSet ds;


        #endregion

        #region GetVehicleList

        // **************** GET VEHICLES LIST *********************

        public static DataTable GetVehicleList()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
            }
            return dt;
        }

        #endregion

        #region GetVehicleStatus
        // **************** GET VEHICLE STATUS LIST *********************
        public static DataTable GetVehicleStatus()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion

        #region GetVehicleGroupList
        // **************** GET VEHICLE CATEGORY LIST *********************
        public static DataTable GetVehicleGroupList(int ManufacturerID)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleCategoryList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion

        #region GetVehicleDetails
        public static DataSet GetVehicleDetails(int VehicleID)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleDetailsDataSet", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    conn.Open();
                    sd.Fill(ds, "VehicleDetails");
                    conn.Close();
                 }
            }
            return ds;
        }
        #endregion

        #region UpdateVehicle
        // **************** UPDATE VEHICLE  *********************
        public static int UpdateVehicle(int VehicleID,
                                        int ModelID,
                                        string RegistrationNumber,
                                        int StatusID,
                                        int TransmissionID,
                                        int VehicleGroupID,
                                        int FuelID,
                                        string UpdatedBy)

        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                var returnValue = 0;
                using (SqlCommand cmd = new SqlCommand("UpdateVehicle", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                    cmd.Parameters.AddWithValue("@VehicleGroupID", VehicleGroupID);
                    cmd.Parameters.AddWithValue("@ModelID", ModelID);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", RegistrationNumber);
                    cmd.Parameters.AddWithValue("@StatusID", StatusID);
                    cmd.Parameters.AddWithValue("@TransmissionID", TransmissionID);
                    cmd.Parameters.AddWithValue("@FuelID", FuelID);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    cmd.Parameters.Add(new SqlParameter("@Return_Value", SqlDbType.Int, 4, ParameterDirection.Output, false, 10, 0, "", DataRowVersion.Proposed, returnValue));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    returnValue = (int)cmd.Parameters["@Return_Value"].Value;
                    conn.Close();
                    return returnValue;
                }
            }
        }
        #endregion

    }

}