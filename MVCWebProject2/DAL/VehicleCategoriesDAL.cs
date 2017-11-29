/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehiclesDAL.cs                    '
'  Description      : Manages the data transport to/from'
'                     data source                       '
'  Author           : Brian McAulay                     '
'  Creation Date    : 08 Nov 2017                       '
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

namespace MVCWebProject2.DAL
{
    public class VehicleCategoriesDAL
    {
        #region SQLConnectionSetUp

        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        private static DataSet ds;

        
        #endregion

        #region GetVehicleList

        // **************** GET VEHICLE CATEGORY LIST *********************

        public static DataTable GetVehicleCategoryList()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleCategoryList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    try
                    {
                        conn.Open();
                        sd.Fill(dt);
                        conn.Close();
                    }
                    catch (SqlException ex)
                    {
                        var error = ex.InnerException;
                    }
                }
            }
            return dt;
        }

        #endregion

        #region AddNewVehicleCategory
        /*
        // **************** ADD NEW VEHICLE CATEGORY *********************
        public bool AddVehicleCategory(VehicleCategoryListViewModel smodel)
        {
            var returnValue = false;
            CreateConnection();
            SqlCommand cmd = new SqlCommand("AddNewStudent", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", smodel.Name);
            cmd.Parameters.AddWithValue("@City", smodel.City);
            cmd.Parameters.AddWithValue("@Address", smodel.Address);
            try
            { 
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch 
            {
                throw new InvalidOperationException("A database problem was encountered!");
            }
            finally
            {
                DisposeConnection();
            }

            return returnValue;
            
        }
        */
        #endregion

        #region GetVehicleCategoryDetails
        
        // ********** VIEW VEHICLE CATEGORY DETAILS ********************
        public static DataSet GetVehicleCategory(int CategoryId)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetVehicleCategoryDataSet", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", CategoryId);
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    try
                    {
                        conn.Open();
                        sd.Fill(ds, "VehicleCategory");
                        conn.Close();
                    }
                    catch (SqlException ex)
                    {
                        var error = ex.InnerException;
                    }
                }
            }
            return ds;
        }
        

        #endregion

        #region UpdateVehicleCategory
        
        // ***************** UPDATE VEHICLE CATEGORY DETAILS *********************
        public static void UpdateVehicleCategory(int Id,
                                          string VehicleClassType,
                                          int VehicleTypeId,
                                          int ImageId,
                                          decimal DailyRate,
                                          decimal WeeklyRate,
                                          decimal WeekendRate,
                                          decimal MonthlyRate,
                                          int NumberOfSeats,
                                          string BasicDescription,
                                          int LuggageCapacity,
                                          string UpdatedBy)
    
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                var returnValue = 0;
                using (SqlCommand cmd = new SqlCommand("UpdateVehicleCategory", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@VehicleClassType", VehicleClassType);
                    cmd.Parameters.AddWithValue("@VehicleTypeId", VehicleTypeId);
                    cmd.Parameters.AddWithValue("@ImageId", ImageId);
                    cmd.Parameters.AddWithValue("@DailyRate", DailyRate);
                    cmd.Parameters.AddWithValue("@WeeklyRate", WeeklyRate);
                    cmd.Parameters.AddWithValue("@WeekendRate", WeekendRate);
                    cmd.Parameters.AddWithValue("@MonthlyRate", MonthlyRate);
                    cmd.Parameters.AddWithValue("@NumberOfSeats", NumberOfSeats);
                    cmd.Parameters.AddWithValue("@BasicDescription", BasicDescription);
                    cmd.Parameters.AddWithValue("@LuggageCapacity", LuggageCapacity);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    cmd.Parameters.Add(new SqlParameter("@Return_Value", SqlDbType.Int, 4, ParameterDirection.Output, false, 10, 0, "", DataRowVersion.Proposed, returnValue));
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        //if((int)cmd.Parameters["@Return_Value"].Value == 1)
                        //    returnValue = true;
                    }

                    catch (SqlException ex)
                    {
                        var error = ex.InnerException;
                    }
                }
            }
        }
        
        #endregion

        #region DeleteVehicleCategory
        /*
        // ********************** DELETE VEHICLE CATEGORY *******************
        public bool DeleteVehicleCategory(int id)
        {
            var returnValue = false;
            CreateConnection();
            SqlCommand cmd = new SqlCommand("DeleteStudent", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StdId", id);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                returnValue = true;
            }
            catch
            {
                throw new InvalidOperationException("A database problem was encountered!");
            }
            finally
            {
                DisposeConnection();
            }

            return returnValue;
        }
        */

        #endregion


    }
}