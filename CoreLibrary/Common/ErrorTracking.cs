using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreLibrary
{
    public static class ErrorTracking
    {
        public static void SaveError(String sUserID, String sPageName, String sMethodName, Exception ex)
        {
            //DataManager _dbManager = new DataManager();
            //String sQuery = "INSERT INTO ErrorLoggerSystem(UserID,ComputerName,ComputerIPAddress,PageName,MethodName,ErrorSource,ErrorMessage,ErrorDate)" +
            //" VALUES('" + sUserID + "','" + UtilityClass.GetComputerName() + "','" + UtilityClass.GetIPAddress() + "','" + sPageName + "','" + sMethodName + "','" + ex.Source.Replace("'", "\"") + "','" + ex.Message.Replace("'", "\"") + "',GETDATE())";

            //_dbManager.ExecuteNonQuery(sQuery);
        }
    }
}
