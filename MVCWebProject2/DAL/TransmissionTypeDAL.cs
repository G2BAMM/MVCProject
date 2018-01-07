/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : TransmissionTypeDAL.cs            '
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
    public class TransmissionTypeDAL
    {
        #region SQLConnectionSetUp
        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        #endregion

        #region GetTransmissionTypes
        // **************** GET TRANSMISSION TYPE LIST *********************
        public static DataTable GetTransmissionTypeList()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetTransmissionTypes", conn))
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

        #region GetTransmissionType
        // **************** GET TRANSMISSION TYPE  *********************
        public static DataTable GetTransmissionType(int TransmissionID)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetTransmissionType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@TransmissionID", TransmissionID);
                    dt = new DataTable();
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion

        #region UpdateTransmissionType
        // **************** UPDATE TRANSMISSION TYPE  *********************
        public static void UpdateTransmissionType(int TransmissionID, string TransmissionType, string UpdatedBy)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateTransmissionType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@TransmissionID", TransmissionID);
                    cmd.Parameters.AddWithValue("@TransmissionType", TransmissionType);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

        }
        #endregion

        #region AddTransmissionType
        // **************** ADD Manufacturer  *********************
        public static void AddTransmissionType(string TransmissionType, string UpdatedBy, out int returnValue)
        {
            returnValue = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("AddTransmissionType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@TransmissionType", TransmissionType);
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