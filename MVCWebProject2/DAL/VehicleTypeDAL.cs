/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleTypeDAL.cs                 '
'  Description      : Manages the data transport to/from'
'                     data source                       '
'  Author           : Brian McAulay                     '
'  Creation Date    : 10 Jan 2018                       '
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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCWebProject2.DAL
{
    public class VehicleTypeDAL
    {
        #region SQLConnectionSetUp
        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        #endregion

        #region GetVehicleTypes
        // **************** GET VEHICLE TYPE LIST *********************
        public static DataTable GetVehicleTypeList()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleTypes", conn))
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

        #region GetVehicleType
        // **************** GET VEHICLE TYPE  *********************
        public static DataTable GetVehicleType(int VehicleTypeID)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@VehicleTypeID", VehicleTypeID);
                    dt = new DataTable();
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion

        #region UpdateVehicleType
        // **************** UPDATE VEHICLE TYPE  *********************
        public static void UpdateVehicleType(int VehicleTypeID, string VehicleType, string UpdatedBy)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateVehicleType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@VehicleTypeID", VehicleTypeID);
                    cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

        }
        #endregion

        #region AddVehicleType
        // **************** ADD VEHICLE TYPE  *********************
        public static void AddVehicleType(string VehicleType, string UpdatedBy, out int returnValue)
        {
            returnValue = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("AddVehicleType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    cmd.Parameters.Add(new SqlParameter("@Return_Value", SqlDbType.Int, 4, ParameterDirection.Output, false, 10, 0, "", DataRowVersion.Proposed, returnValue));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    returnValue = (int)cmd.Parameters["@Return_Value"].Value;
                    conn.Close();
                }
            }

        }
        #endregion
    }
}