using CoreLibrary;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Services;

namespace DynamicDMS
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService()]

    public class AutoComplete : System.Web.Services.WebService
    {

        [WebMethod]
        public String[] GetEmployeeInfo(String prefixText)
        {
            List<String> listProducts = new List<String>();
            DataTable _objdt = new DataTable();
            _objdt = GetEmployeeInfoFromDatabase(prefixText);
            if (_objdt.Rows.Count > 0)
            {
                for (int i = 0; i < _objdt.Rows.Count; i++)
                {
                    listProducts.Add(_objdt.Rows[i]["UserName"].ToString());
                }
            }
            return listProducts.ToArray();
        }

        public DataTable GetEmployeeInfoFromDatabase(String prefixText)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.Append("SELECT LoginID+' :: '+ UserName+', '+DG.LookupText+', '+D.LookupText AS UserName  ");
            sQuery.Append("FROM USERS U   ");
            sQuery.Append("LEFT JOIN dbo.SysLookup D ON D.LookupTypeId=2 AND U.DepartmentID=D.LookupValue   ");
            sQuery.Append("LEFT JOIN dbo.SysLookup DG ON DG.LookupTypeId=3 AND U.DesignationId=DG.LookupValue    ");
            sQuery.Append("WHERE U.LoginID LIKE '%" + prefixText + "%'  OR U.UserName LIKE '%" + prefixText + "%'   ");

            DataManager dataManager = new DataManager();
            return dataManager.GetDataTable(sQuery.ToString());
        }
    }
}
