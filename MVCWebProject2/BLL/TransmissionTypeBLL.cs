/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : TransmissionTypeBLL.cs            '
'  Description      : Manages the business logic on     '
'                     items saved/retrieved to/from SQL '
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
using MVCWebProject2.Areas.Admin.Models;
using MVCWebProject2.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MVCWebProject2.BLL
{
    public class TransmissionTypeBLL
    {
        public static IEnumerable<VehicleTransmissionList> GetTransmissionList()
        {
            var TransmissionList = new List<VehicleTransmissionList>();
            var dt = TransmissionTypeDAL.GetTransmissionTypeList();
            foreach (DataRow dataRow in dt.Rows)
            {
                TransmissionList.Add(new VehicleTransmissionList
                {
                    Id = (int)dataRow["TransmissionID"],
                    Display = dataRow["TransmissionType"].ToString()
                });
            }
            return TransmissionList;
        }

        public static VehicleTransmissionList GetTransmissionType(int TransmissionID)
        {
            var model = new VehicleTransmissionList();
            var dt = TransmissionTypeDAL.GetTransmissionType(TransmissionID);
            var dr = dt.Rows[0];
            model.Id = (int)dr["TransmissionID"];
            model.Display = dr["TransmissionType"].ToString();
            return model;
        }

        public static void UpdateTransmissionType(VehicleTransmissionList model, string UpdatedBy)
        {
            TransmissionTypeDAL.UpdateTransmissionType(model.Id, model.Display, UpdatedBy);
        }

        public static void AddTransmissionType(VehicleTransmissionList model, string UpdatedBy, out int returnValue)
        {
            TransmissionTypeDAL.AddTransmissionType(model.Display, UpdatedBy, out returnValue);
        }
    }
}