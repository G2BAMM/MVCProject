/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : GalleryDAL.cs                     '
'  Description      : Manages the data transport to/from'
'                     data source                       '
'  Author           : Brian McAulay                     '
'  Creation Date    : 27 Nov 2017                       '
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
    public class GalleryDAL
    {
        #region SQLConnectionSetUp

        private static string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        private static DataTable dt;
        
        #endregion

        #region AddNewGalleryImage
        // ***************** ADD NEW GALLERY IMAGE *********************
        public static void AddNewGalleryImage(string ImageName, int ModelID, string UpdatedBy)

        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("AddNewGalleryImage", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ImageName", ImageName);
                    cmd.Parameters.AddWithValue("@ModelID", ModelID);
                    cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    //if((int)cmd.Parameters["@Return_Value"].Value == 1)
                    //    returnValue = true;
                }
            }
        }
        #endregion

        #region DeleteGalleryImage
        // ***************** Delete GALLERY IMAGE *********************
        public static string DeleteGalleryImage(int ImageID)
        {
            var imageName = "";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteGalleryImage", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ImageID", ImageID);
                    cmd.Parameters.AddWithValue("@ImageName", "").Direction = ParameterDirection.Output;
                    cmd.Parameters["@ImageName"].Size = 255;
                    cmd.Parameters["@ImageName"].DbType = DbType.String;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    imageName = cmd.Parameters["@ImageName"].Value.ToString();
                    conn.Close();
                }
            }
            //Now we need to return the image name that was deleted to remove the image from the drive
            return imageName;
        }
        #endregion

        #region GetGallery
        // **************** GET IMAGE GALLERY *********************
        public static DataTable GetImageGalleryList(int pageNumber, int numberOfItems, out int numberOfPages)
        {
            numberOfPages = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetImageGalleryList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter sd = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                    cmd.Parameters.AddWithValue("@NumberOfItems", numberOfItems);
                    cmd.Parameters.AddWithValue("@NumberOfPages", "").Direction = ParameterDirection.Output;
                    cmd.Parameters["@NumberOfPages"].Size = 4;
                    cmd.Parameters["@NumberOfPages"].DbType = DbType.Int32;
                    dt = new DataTable("GalleryList");
                    conn.Open();
                    sd.Fill(dt);
                    numberOfPages = (int)cmd.Parameters["@NumberOfPages"].Value;
                    conn.Close();
                }
            }
            return dt;
        }
        #endregion
    }
}