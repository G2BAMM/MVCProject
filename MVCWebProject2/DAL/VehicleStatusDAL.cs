/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleStatusDAL.cs               '
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
    public class VehicleStatusDAL
    {
        #region SQLConnectionSetUp
        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        private static SqlDataReader dr;
        #endregion

        #region GetStatusList

        // **************** GET VEHICLE STATUS LIST *********************

        public static DataTable GetVehicleStatusList()
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

        #region GetStatusIDByTypeName
        // **************** GET VEHICLE STATUS ID BY TYPE NAME *********************
        public static int GetStatusIDByTypeName(string StatusName)
        {
            var StatusID = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleStatusIDByTypeName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StatusName", StatusName);
                    conn.Open();
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    StatusID = (int)dr["StatusID"];
                    conn.Close();
                }
            }
            return StatusID;
        }
        #endregion
    }
}