/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleModelDAL.cs                '
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
    public class VehicleModelDAL
    {
        #region SQLConnectionSetUp
        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        private static DataSet ds;
        #endregion

        #region GetModelListByManufacturer
        // **************** GET MODEL(MANUFACTURER) LIST *********************
        public static DataTable GetModelListByManufacturer(int ManufacturerID)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleModelsByManufacturer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ManufacturerID", ManufacturerID);
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
    }
}