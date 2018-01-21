/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : VehicleStatusBLL.cs               '
'  Description      : Manages the business logic on     '
'                     items saved/retrieved to/from SQL '
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
using System.Data;
using System.Linq;
using System.Web;
using MVCWebProject2.Areas.Admin.Models;
using MVCWebProject2.DAL;
using MVCWebProject2.utilities;

namespace MVCWebProject2.BLL
{
    public class VehicleStatusBLL
    {
        #region GetVehicleStatusList
        public static List<VehicleStatusList> GetVehicleStatusList()
        {
            List<VehicleStatusList> myList = new List<VehicleStatusList>();
            DataTable dt = VehicleStatusDAL.GetVehicleStatusList();
            foreach (DataRow row in dt.Rows)
            {
                VehicleStatusList myListItems = new VehicleStatusList
                {
                    Id = (int)row["StatusID"],
                    Display = row["VehicleStatus"].ToString()
                };
                myList.Add(myListItems);
            }
            return myList;
        }

        #endregion

        #region GetVehicleStatusIDByStatusType
        public static int GetVehicleStatusIDByStatusType(string StatusTypeName)
        {
            //Returns the status id for any guven status name
            var result = VehicleStatusDAL.GetStatusIDByTypeName(StatusTypeName);
            return result;
        }

        #endregion
    }
}