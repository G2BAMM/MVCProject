/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : FuelTypeDAL.cs                    '
'  Description      : Manages the data transport to/from'
'                     data source                       '
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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCWebProject2.DAL
{
    public class FuelTypeDAL
    {
        #region SQLConnectionSetUp
        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        #endregion

        #region GetFuelTypes
        // **************** GET FUEL TYPE LIST *********************
        public static DataTable GetFuelTypes()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetFuelTypes", conn))
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

        #region GetFuelType
        // **************** GET FUEL TYPE  *********************
        public static DataTable GetFuelType(int FuelID)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetFuelType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@FuelID", FuelID);
                    dt = new DataTable();
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion

        #region UpdateFuelType
        // **************** UPDATE FUEL TYPE  *********************
        public static void UpdateFuelType(int FuelID, string FuelType, string UpdatedBy)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateFuelType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@FuelID", FuelID);
                    cmd.Parameters.AddWithValue("@FuelType", FuelType);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            
        }
        #endregion

        #region AddFuelType
        // **************** ADD FUEL TYPE  *********************
        public static void AddFuelType(string FuelType, string UpdatedBy, out int returnValue)
        {
            returnValue = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("AddFuelType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@FuelType", FuelType);
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