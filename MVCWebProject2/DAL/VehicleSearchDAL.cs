/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleSearchDAL.cs               '
'  Description      : Manages the data transport to/from'
'                     data source                       '
'  Author           : Brian McAulay                     '
'  Creation Date    : 03 Feb 2018                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace MVCWebProject2.DAL
{
    public class VehicleSearchDAL
    {
        #region SQLConnectionSetUp
        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        #endregion

        #region GetVehicleSearchResults
        // **************** GET SEARCH RESULTS LIST *********************
        public static DataTable GetVehicleSearchResults(DateTime StartDate, DateTime EndDate)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SearchVehicles", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    dt = new DataTable();
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion
    }
}