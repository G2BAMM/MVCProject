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
        private static DataSet ds;

        #endregion

        #region AddNewGalleryImage

        // ***************** ADD NEW GALLERY IMAGE *********************
        public static void AddNewGalleryImage(string ImageName, string UpdatedBy)

        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("AddNewGalleryImage", conn))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ImageName", ImageName);
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


    }
}