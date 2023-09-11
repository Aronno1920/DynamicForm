using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace DynamicDMS.UI
{
    public partial class Profile : System.Web.UI.Page
    {

        Cookie _user = new Cookie();
        String _validationMessage = String.Empty;
        DataManager _dataManager = new DataManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Check User Login Status
            if (String.IsNullOrEmpty(_user.GetCookie(CookieKey.UserId.ToString())) || _user.GetCookie(CookieKey.UserId.ToString()) == "0")
            {
                Response.Redirect(String.Format("~/Default.aspx", false));
            }
            #endregion


            if (!IsPostBack)
            {
                LoadUserProfile();
            }
        }

        protected void LoadUserProfile()
        {
            try
            {
                hfUserID.Value = _user.GetCookie(CookieKey.UserId.ToString());

                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT U.LoginID,U.UserName,U.Password,C.LookupText AS CompanyName,Dp.LookupText AS DepartmentName,L.LookupText AS LocationName,Dg.LookupText AS DesignationName,U.ProfilePhoto, U.ContactNo,U.Email   ");
                sQuery.Append("FROM Users U   ");
                sQuery.Append("LEFT JOIN SysLookup C ON C.LookupTypeId=1 AND U.CompanyID=C.LookupValue   ");
                sQuery.Append("LEFT JOIN SysLookup Dp ON Dp.LookupTypeId=2 AND U.DepartmentID=DP.LookupValue   ");
                sQuery.Append("LEFT JOIN SysLookup L ON L.LookupTypeId=4 AND U.LocationID=L.LookupValue   ");
                sQuery.Append("LEFT JOIN SysLookup Dg ON Dg.LookupTypeId=3 AND U.DesignationId=Dg.LookupValue   ");
                sQuery.Append("WHERE U.UserID="+hfUserID.Value);

                _dataManager = new DataManager();
                DataTable dtUser=_dataManager.GetDataTable(sQuery.ToString());

                if(dtUser != null)
                {
                    hfPassword.Value = dtUser.Rows[0]["Password"].ToString();
                    txtLoginID.Text = dtUser.Rows[0]["LoginID"].ToString();
                    txtName.Text = dtUser.Rows[0]["UserName"].ToString();
                    txtDesignation.Text = dtUser.Rows[0]["DesignationName"].ToString();
                    txtDepartment.Text = dtUser.Rows[0]["DepartmentName"].ToString();
                    txtCompany.Text = dtUser.Rows[0]["CompanyName"].ToString();
                    txtLocation.Text = dtUser.Rows[0]["LocationName"].ToString();
                    txtContactNo.Text = dtUser.Rows[0]["ContactNo"].ToString();
                    txtEmailAddress.Text = dtUser.Rows[0]["Email"].ToString();
                    imgPhoto.ImageUrl = dtUser.Rows[0]["ProfilePhoto"].ToString();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string fullPath = Server.MapPath("~/Images/EmployeePhoto/") + Convert.ToInt32(Request.Cookies["LoginID"].Value).ToString() + System.IO.Path.GetExtension(Session["filePath"].ToString());

                byte[] buffer = (byte[])Session["ImageBytes"];
                string path = Path.GetRandomFileName();
                path = Path.ChangeExtension(fullPath, ".jpg");
                File.WriteAllBytes(path, buffer);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnChangeEmail_Click(object sender, EventArgs e)
        {
            try
            {
                String sQueryPart = String.Empty;

                if (String.IsNullOrEmpty(txtNewContact.Text) && String.IsNullOrEmpty(txtNewEmail.Text))
                {
                    DisplayMessage("Please enter new contact or email for update");
                    return;
                }
                else if (!String.IsNullOrEmpty(txtNewContact.Text))
                {
                    sQueryPart = "ContactNo = '" + txtNewContact.Text + "'";
                }
                else if (!String.IsNullOrEmpty(txtNewEmail.Text))
                {
                    sQueryPart = (String.IsNullOrEmpty(sQueryPart) ? "" : ", ") + "Email='" + txtNewEmail.Text + "'";
                }
                
                if (sQueryPart != String.Empty)
                {
                    String sQuery = "UPDATE dbo.Users SET " + sQueryPart + "  WHERE UserID=" + hfUserID.Value;

                    _dataManager = new DataManager();
                    _dataManager.ExecuteNonQuery(sQuery);
                    DisplayMessage("Notification setting has been updated.");

                    LoadUserProfile();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnPassword_Click(object sender, EventArgs e)
        {
            try
            {
                String sConfirmPassword = UtilityClass.Encrypt(txtConfirmNewPassword.Text, true);

                if (CheckPasswordValidation())
                {
                    String sQuery = "UPDATE dbo.Users SET Password='" + sConfirmPassword + "' WHERE UserID=" + hfUserID.Value;

                    _dataManager = new DataManager();
                    _dataManager.ExecuteNonQuery(sQuery);
                    DisplayMessage("Password has been changed successfully.");

                    LoadUserProfile();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected Boolean CheckPasswordValidation()
        {
            Boolean isSuccess = true;

            String sOldPassword = UtilityClass.Encrypt(txtCurrentPassword.Text, true);
            String sNewPassword = UtilityClass.Encrypt(txtNewPassword.Text, true);
            String sConfirmPassword = UtilityClass.Encrypt(txtConfirmNewPassword.Text, true);

            if (String.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                isSuccess = false;
                DisplayMessage("Please enter your current password.");
                txtCurrentPassword.Focus();
            }
            else if (String.IsNullOrEmpty(txtNewPassword.Text))
            {
                isSuccess = false;
                DisplayMessage("Please enter your new password.");
                txtNewPassword.Focus();
            }
            else if(String.IsNullOrEmpty(txtConfirmNewPassword.Text))
            {
                isSuccess = false;
                DisplayMessage("Please enter your new password again.");
                txtConfirmNewPassword.Focus();
            }
            else if (!hfPassword.Value.Equals(sOldPassword))
            {
                isSuccess = false;
                DisplayMessage("Current password not match. Please enter again");
                txtCurrentPassword.Focus();
            }
            else if (!sNewPassword.Equals(sConfirmPassword))
            {
                isSuccess = false;
                DisplayMessage("New password and Confirm password not match. Please enter again");
                txtCurrentPassword.Focus();
            }

            return isSuccess;
        }

        #region Others
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
        #endregion
    }
}