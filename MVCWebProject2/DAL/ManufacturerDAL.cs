/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : ManufacturerDAL.cs                '
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
    public class ManufacturerDAL
    {
        #region SQLConnectionSetUp
        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        #endregion

        #region GetManufacturerList
        // **************** GET MANUFACTURERS LIST *********************
        public static DataTable GetManufacturerList()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetManufacturers", conn))
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

        #region GetManufacturer
        // **************** GET MANUFACTURER  *********************
        public static DataTable GetManufacturer(int ManufacturerID)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetManufacturer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@ManufacturerID", ManufacturerID);
                    dt = new DataTable();
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion

        #region UpdateManufacturer
        // **************** UPDATE MANUFACTURER  *********************
        public static void UpdateManufacturer(int ManufacturerID, string Manufacturer, string UpdatedBy)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateManufacturer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@ManufacturerID", ManufacturerID);
                    cmd.Parameters.AddWithValue("@Manufacturer", Manufacturer);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

        }
        #endregion

        #region AddManufacturer
        // **************** ADD Manufacturer  *********************
        public static void AddManufacturer(string Manufacturer, string UpdatedBy, out int returnValue)
        {
            returnValue = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("AddManufacturer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@Manufacturer", Manufacturer);
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