using CoreLibrary;
using DynamicDMS.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDMS
{
    public partial class Default : System.Web.UI.Page
    {
        Cookie userCookie = new Cookie();
        DataManager _dataManager = new DataManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["UserInfo"] = null;

                //String passwordString= UtilityClass.Decrypt("/KHS6ykGutM=", true);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            { 
                String sQuery = @"SELECT UserID as UserID, UserName as UserName, LoginID, ISNULL(CompanyID,1) as CompanyID, DepartmentId, Password
                                  FROM Users  WHERE LoginID='" + txtEmployeeId.Text.Trim() + "'";

                _dataManager = new DataManager();
                DataTable dtUser = _dataManager.GetDataTable(sQuery);

                if (dtUser.Rows.Count > 0)
                {
                    String sEncryptPassword = UtilityClass.Encrypt(txtPassword.Text.Trim(), true);
                    if (sEncryptPassword.Equals(dtUser.Rows[0]["Password"].ToString()))
                    {
                        userCookie = new Cookie();
                        userCookie.SetCookie(CookieKey.UserId.ToString(), dtUser.Rows[0]["UserID"].ToString());
                        userCookie.SetCookie(CookieKey.EmployeeId.ToString(), txtEmployeeId.Text.Trim());
                        userCookie.SetCookie(CookieKey.CompanyId.ToString(), dtUser.Rows[0]["CompanyId"].ToString());
                        userCookie.SetCookie(CookieKey.DepartmentId.ToString(), dtUser.Rows[0]["DepartmentId"].ToString());
                        userCookie.SetCookie(CookieKey.UserName.ToString(), dtUser.Rows[0]["UserName"].ToString());

                        LoadUserWisMenuList();
                        Response.Redirect("~/UI/Dashborad.aspx", false);
                    }
                    else
                    {
                        DisplayMessage("Sorry! Password Invalid! Try Again.");
                    }
                }
                else
                {
                    userCookie.ClearCookie();
                    userCookie.RemoveCookie();
                    DisplayMessage("Sorry! Invalid Login! Try Again.");
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("Login Failed.\\n \\n" + ex.Message.Replace("'", ""));
            }
        }

        protected void LoadUserWisMenuList()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@UserId",  SqlDbType.NVarChar, 500,userCookie.GetCookie(CookieKey.UserId.ToString())),
                    dataManager.MakeInParam("@Action",  SqlDbType.NVarChar, 500,"LoadUserMenu")
                };

                DataTable dtMenuList = dataManager.GetDataTable("SP_USERS", parameters);
                if (dtMenuList.Rows.Count > 0)
                {
                    Session["MenuList"] = dtMenuList;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(txtEmployeeId.Text, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
    }
}