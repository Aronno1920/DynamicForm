using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CoreLibrary
{
    public class SectionVisibility
    {
        public String ServiceName { get; private set; }
        public Int32 RoleId { get; private set; }

        public Boolean IsEnablePI { get; private set; }
        public Boolean IsEnablePO { get; private set; }
        public Boolean IsEnableChallan { get; private set; }
        public Boolean IsEnableMRR { get; private set; }
        public Boolean IsEnableBill { get; private set; }
        public Boolean IsEnableAmount { get; private set; }
        public Boolean IsEnableDiscount { get; private set; }

        public Boolean IsVisibleUploader { get; private set; }
        public Boolean IsEnableDownload { get; private set; }
        public Boolean IsEnableDelete { get; private set; }

        public Boolean IsLastDestination { get; private set; }


        public SectionVisibility(String ServiceId, String DocumentId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                 dataManager.MakeInParam("@ServiceId", SqlDbType.NVarChar, 500, ServiceId),
                 dataManager.MakeInParam("@DocumentId", SqlDbType.NVarChar, 500, DocumentId)
            };

            DataTable dtPermission = dataManager.GetDataTable("SP_SYS_CONTROL_STATUS", parameters);
            if (dtPermission.Rows.Count > 0)
            {
                ServiceName = dtPermission.Rows[0]["ServiceName"].ToString();
                RoleId = Convert.ToInt32(dtPermission.Rows[0]["RoleID"].ToString());

                //IsEnablePI = Convert.ToBoolean(dtPermission.Rows[0]["IsPI"].ToString());
                //IsEnablePO = Convert.ToBoolean(dtPermission.Rows[0]["IsPO"].ToString());
                //IsEnableChallan = Convert.ToBoolean(dtPermission.Rows[0]["IsChallan"].ToString());
                //IsEnableMRR = Convert.ToBoolean(dtPermission.Rows[0]["IsMRR"].ToString());
                
                //IsEnableBill = Convert.ToBoolean(dtPermission.Rows[0]["IsBill"].ToString());
                //IsEnableAmount = Convert.ToBoolean(dtPermission.Rows[0]["IsAmount"].ToString());
                //IsEnableDiscount = Convert.ToBoolean(dtPermission.Rows[0]["IsDiscount"].ToString());

                IsVisibleUploader = Convert.ToBoolean(dtPermission.Rows[0]["IsCanUpload"].ToString());
                IsEnableDownload = Convert.ToBoolean(dtPermission.Rows[0]["IsCanDownload"].ToString());
                IsEnableDelete = Convert.ToBoolean(dtPermission.Rows[0]["IsCanDelete"].ToString());

                IsLastDestination = Convert.ToBoolean(dtPermission.Rows[0]["IsApprover"].ToString());
            }
        }
    }
}
