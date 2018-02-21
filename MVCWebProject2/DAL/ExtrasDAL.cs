/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : ExtrasDAL.cs                      '
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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MVCWebProject2.DAL
{
    public class ExtrasDAL
    {
        #region SQLConnectionSetUp
        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        #endregion

        #region GetExtras(List)
        // **************** GET RENTAL EXTRAS LIST *********************
        public static DataTable GetRentalExtras()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetRentalExtras", conn))
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

        #region GetRentalsExtra
        // **************** GET RENTALS EXTRA *********************
        public static DataTable GetRentalExtra(int ExtraID)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetRentalExtra", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@ExtraID", ExtraID);
                    dt = new DataTable();
                    conn.Open();
                    sd.Fill(dt);
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion

        #region UpdateRentalsExtras
        // **************** UPDATE RENTALS EXTRA  *********************
        public static void UpdateRentalsExtra(int ExtraID, string ExtraDescription, decimal ExtraPrice, string UpdatedBy)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateRentalExtra", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@ExtraID", ExtraID);
                    cmd.Parameters.AddWithValue("@ExtraDescription", ExtraDescription);
                    cmd.Parameters.AddWithValue("@ExtraPrice", ExtraPrice);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

        }
        #endregion

        #region AddExtrasType
        // **************** ADD RENTALS EXTRAS   *********************
        public static void AddRentalExtra(string ExtraDescription, decimal ExtraPrice, string UpdatedBy, out int returnValue)
        {
            returnValue = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("AddRentalExtra", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@ExtraDescription", ExtraDescription);
                    cmd.Parameters.AddWithValue("@ExtraPrice", ExtraPrice);
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