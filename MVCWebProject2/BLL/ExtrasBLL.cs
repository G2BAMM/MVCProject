/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : ExtrasBLL.cs                      '
'  Description      : Manages the business logic on     '
'                     items saved/retrieved to/from SQL '
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
using MVCWebProject2.DAL;
using MVCWebProject2.Models;
using System.Collections.Generic;
using System.Data;

namespace MVCWebProject2.BLL
{
    public class ExtrasBLL
    {
        #region GetRentalExtrasList
        public static List<RentalExtrasListModel> GetRentalExtras()
        {
            var ExtrasList = new List<RentalExtrasListModel>();
            var dt = ExtrasDAL.GetRentalExtras();
            foreach (DataRow dataRow in dt.Rows)
            {
                ExtrasList.Add(new RentalExtrasListModel
                {
                    ExtraId = (int)dataRow["ExtraID"],
                    ExtraDescription = dataRow["ExtraDescription"].ToString(),
                    Price = (decimal)dataRow["ExtraPrice"]
                });
            }
            return ExtrasList;
        }
        #endregion

        #region GetRentalExtra
        public static RentalExtrasListModel GetRentalExtra(int ExtraID)
        {
            var model = new RentalExtrasListModel();
            var dt = ExtrasDAL.GetRentalExtra(ExtraID);
            var dr = dt.Rows[0];
            model.ExtraId = (int)dr["ExtraID"];
            model.ExtraDescription = dr["ExtraDescription"].ToString();
            model.Price = (decimal)dr["ExtraPrice"];
            return model;
        }
        #endregion

        #region UpdateRentalExtra
        public static void UpdateRentalExtra(RentalExtrasListModel model, string UpdatedBy)
        {
           ExtrasDAL.UpdateRentalsExtra(model.ExtraId, model.ExtraDescription, model.Price, UpdatedBy);
        }
        #endregion

        #region AddRentalExtra
        public static void AddRentalExtra(RentalExtrasListModel model, string UpdatedBy, out int returnValue)
        {
            ExtrasDAL.AddRentalExtra(model.ExtraDescription, model.Price, UpdatedBy, out returnValue);
        }
        #endregion
    }
}