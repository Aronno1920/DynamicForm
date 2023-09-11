using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text;

namespace DynamicDMS.UI
{
    public partial class User : System.Web.UI.Page
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

            try
            {
                DataTable dtMenu = (DataTable)Session["MenuList"];
                String sPageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                bool isPermitted = dtMenu.AsEnumerable().Any(row => row["menu_url"].ToString().Contains(sPageName));
                if (!isPermitted)
                {
                    Response.Redirect(String.Format("~/UI/UnauthorizedPage.aspx"), false);
                    return;
                }
            }
            catch
            {
                Response.Redirect(String.Format("~/Default.aspx"), false);
            }
            #endregion

            Form.DefaultButton = btnSearch.UniqueID;
            if (!Page.IsPostBack)
            {
                LoadDropDownListCompany();
                LoadDropDownListLocation();
                LoadDropDownListDepartment();
                LoadDropDownListDesignation();
                BindGridViewUserList();
            }
        }

        #region Page Load Related

        protected void LoadDropDownListCompany()
        {
            try
            {
                DataTable dtCopmany = PopulateLists.GetCompanies();
                FillList.PopulateDropDownList(dtCopmany, ddlCompany, "Select Company");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void LoadDropDownListDepartment()
        {
            try
            {
                DataTable dtDeaprtment = PopulateLists.GetDepartments();
                FillList.PopulateDropDownList(dtDeaprtment, ddlDepartment, "Select Department");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        
        protected void LoadDropDownListLocation()
        {
            try
            {
                DataTable dtLocation = PopulateLists.GetLocations();
                FillList.PopulateDropDownList(dtLocation, ddlLocation, "Select Location");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void LoadDropDownListDesignation()
        {
            try
            {
                DataTable dtDesignation = PopulateLists.GetDesignations();
                FillList.PopulateDropDownList(dtDesignation, ddlDesignation, "Select Designation");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void BindGridViewUserList()
        {
            try
            {
                StringBuilder sSerchQuery = new StringBuilder();
                if (!String.IsNullOrEmpty(txtSearch.Text))
                {
                    sSerchQuery.Append("WHERE U.LoginID LIKE '%" + txtSearch.Text + "%' OR U.UserName LIKE '%" + txtSearch.Text + "%' OR U.ContactNo LIKE '%" + txtSearch.Text + "%'    ");
                    sSerchQuery.Append("OR U.Email LIKE '%"+ txtSearch.Text + "%' OR C.LookupText  LIKE '%" + txtSearch.Text + "%' OR D.LookupText  LIKE '%" + txtSearch.Text +"%'      ");
                    sSerchQuery.Append("OR DG.LookupText  LIKE '%" + txtSearch.Text + "%'    ");
                }

                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT U.UserID,U.LoginID,U.UserName,C.LookupText AS CompanyName,D.LookupText AS DepartmentName,DG.LookupText AS DesignationName, L.LookupText AS LocationName, U.ContactNo, U.Email, CASE WHEN U.IsAdmin=1 THEN 'Yes' ELSE 'No' END AS UserType,CASE WHEN U.IsActive=1 THEN 'Active' ELSE 'Inactive' END AS Status, u.IsSendNotification   ");
                sQuery.Append("FROM Users U    ");
                sQuery.Append("LEFT JOIN SysLookup C ON C.LookupTypeId=1 AND U.CompanyID=C.LookupValue    ");
                sQuery.Append("LEFT JOIN SysLookup D ON D.LookupTypeId=2 AND U.DepartmentID=D.LookupValue    ");
                sQuery.Append("LEFT JOIN SysLookup DG ON DG.LookupTypeId=3 AND U.DesignationId=DG.LookupValue    ");
                sQuery.Append("LEFT JOIN SysLookup L ON L.LookupTypeId=4 AND U.LocationID=L.LookupValue    ");
                sQuery.Append(sSerchQuery.ToString());
                sQuery.Append("Order by u.IsActive desc");

                _dataManager = new DataManager();
                DataTable dtUserList = _dataManager.GetDataTable(sQuery.ToString());
                FillList.PopulateGridView(dtUserList, gvUserList);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        #endregion


        #region Click Event Related
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            String sUserExist = IsUserAlreadyExist();
            if (String.IsNullOrEmpty(sUserExist))
            {
                sUserExist = "No user found with the Employee Id";
            }

            DisplayMessage(sUserExist);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (CheckValidation())
                {
                    String sEncryptPassword = UtilityClass.Encrypt(txtPassword.Text.Trim(), true);

                    DataManager dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[15]
                    {
                        dataManager.MakeInParam("@UserId",  SqlDbType.NVarChar, 500,hfUserId.Value),
                         dataManager.MakeInParam("@LoginId",  SqlDbType.NVarChar, 500,txtEmployeeId.Text.Trim()),
                         dataManager.MakeInParam("@Name",  SqlDbType.NVarChar, 500,txtUserName.Text),
                         dataManager.MakeInParam("@Password",  SqlDbType.NVarChar, 500,sEncryptPassword),
                         dataManager.MakeInParam("@CompanyId",  SqlDbType.NVarChar, 500,ddlCompany.SelectedValue),

                         dataManager.MakeInParam("@DesignationId",  SqlDbType.NVarChar, 500,ddlDesignation.SelectedValue),
                         dataManager.MakeInParam("@DepartmentId",  SqlDbType.NVarChar, 500,ddlDepartment.SelectedValue),
                         dataManager.MakeInParam("@LocationId",  SqlDbType.NVarChar, 500,ddlLocation.SelectedValue),
                         dataManager.MakeInParam("@Contact",  SqlDbType.NVarChar, 500,txtContactNo.Text),
                         dataManager.MakeInParam("@Email",  SqlDbType.NVarChar, 500,txtEmailAddress.Text),

                         dataManager.MakeInParam("@IsNotify",  SqlDbType.NVarChar, 500,cbIsNotified.Checked),
                         dataManager.MakeInParam("@IsAdmin",  SqlDbType.NVarChar, 500,cbIsAdmin.Checked),
                         dataManager.MakeInParam("@IsActive",  SqlDbType.NVarChar, 500,cbIsActive.Checked),
                         dataManager.MakeInParam("@EntryBy",  SqlDbType.NVarChar, 500,_user.GetCookie(CookieKey.UserId.ToString())),
                         dataManager.MakeInParam("@Action",  SqlDbType.NVarChar, 500,(btnSave.Text == "Save"?"Insert":"Update"))
                    };
                    DataTable dtResult = dataManager.GetDataTable("SP_USERS", parameters);

                    if (dtResult.Rows[0]["Result"].ToString().Length >= 3)
                    {
                        DisplayMessage(dtResult.Rows[0]["Result"].ToString());
                        ClearAllControls();
                        BindGridViewUserList();
                    }
                    else
                    {
                        DisplayMessage("User create failed. " + dtResult.Rows[0]["Result"].ToString());
                    }
                }
                else
                {
                    DisplayMessage(_validationMessage);
                }
            }

            catch (Exception ex)
            {
                DisplayMessage("User create failed. " + ex.Message);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllControls();
        }


        protected void gvUserList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByText(row.Cells[4].Text));
                LoadDropDownListLocation();
                LoadDropDownListDepartment();

                hfUserId.Value = row.Cells[1].Text.Replace("&nbsp;", "");
                txtEmployeeId.Text = row.Cells[2].Text.Replace("&nbsp;", "").Replace("&amp;", "&"); ;
                txtUserName.Text = row.Cells[3].Text.Replace("&nbsp;", "").Replace("&amp;", "&"); ;
                ddlDepartment.SelectedIndex = ddlDepartment.Items.IndexOf(ddlDepartment.Items.FindByText(row.Cells[5].Text));
                ddlDesignation.SelectedIndex = ddlDesignation.Items.IndexOf(ddlDesignation.Items.FindByText(row.Cells[6].Text));
                ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByText(row.Cells[7].Text));
                txtContactNo.Text = row.Cells[8].Text.Replace("&nbsp;", "").Replace("&amp;", "&"); ;
                txtEmailAddress.Text = row.Cells[9].Text.Replace("&nbsp;", "").Replace("&amp;", "&"); ;

                bool isActive = true;
                isActive = (row.Cells[10].Text == "No" ? false : true);
                cbIsAdmin.Checked = isActive;

                isActive = (row.Cells[11].Text == "Inactive" ? false : true);
                cbIsActive.Checked = isActive;

                isActive = (row.Cells[11].Text == "0" ? false : true);
                cbIsNotified.Checked = isActive;

                btnSave.Text = "Update";
            }
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Text = String.Empty;
            BindGridViewUserList();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridViewUserList();
        }

        protected void gvUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUserList.PageIndex = e.NewPageIndex;
            BindGridViewUserList();
        }

        #endregion

        #region Other Related
        protected Boolean CheckValidation()
        {
            bool _result = true;
            if (String.IsNullOrEmpty(txtEmployeeId.Text))
            {
                _validationMessage = "Please enter employee Id";
                _result = false;
            }
            else if (String.IsNullOrEmpty(txtUserName.Text))
            {
                _validationMessage = "Please enter employee name";
                _result = false;
            }
            else if (btnSave.Text == "Save" && String.IsNullOrEmpty(txtPassword.Text))
            {
                _validationMessage = "Please enter password";
                _result = false;
            }
            else if (String.IsNullOrEmpty(ddlDesignation.SelectedValue) || ddlDesignation.SelectedValue == "0")
            {
                _validationMessage = "Please select any designation";
                _result = false;
            }
            else if (btnSave.Text == "Save" && !String.IsNullOrEmpty(IsUserAlreadyExist()))
            {
                _result = false;
            }

            return _result;
        }

        protected String IsUserAlreadyExist()
        {

            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                   dataManager.MakeInParam("@LoginId",  SqlDbType.NVarChar, 500,txtEmployeeId.Text.Trim()),
                   dataManager.MakeInParam("@Action",  SqlDbType.NVarChar, 500,"Check")
            };
            DataSet dsResult = dataManager.GetDataSet("SP_USERS", parameters);

            if (dsResult.Tables[0].Rows.Count > 0)
            {
                _validationMessage = "User already exist with the employee ID";
            }

            return _validationMessage;
        }

        protected void ClearAllControls()
        {
            txtEmployeeId.Text = String.Empty;
            txtUserName.Text = String.Empty;
            txtPassword.Text = String.Empty;
            txtPassword.Enabled = true;
            ddlCompany.SelectedIndex = -1;

            ddlDesignation.SelectedIndex = -1;
            ddlDepartment.SelectedIndex = -1;
            ddlLocation.SelectedIndex = -1;
            txtContactNo.Text = String.Empty;
            txtEmailAddress.Text = String.Empty;

            cbIsNotified.Checked = true;
            cbIsAdmin.Checked = false;
            cbIsActive.Checked = true;
            btnSave.Text = "Save";
        }

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
        #endregion

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(hfUserId.Value))
                {
                    DisplayMessage("Please select any user first.");
                    return;
                }
                else if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    DisplayMessage("Please enter new password for the user.");
                    txtPassword.Focus();
                    return;
                }
                else
                {
                    String sEncryptPassword = UtilityClass.Encrypt(txtPassword.Text.Trim(), true);

                    DataManager dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[3]
                    {
                         dataManager.MakeInParam("@UserId",  SqlDbType.NVarChar, 500,hfUserId.Value),
                         dataManager.MakeInParam("@Password",  SqlDbType.NVarChar, 500,sEncryptPassword),
                         dataManager.MakeInParam("@Action",  SqlDbType.NVarChar, 500,"ResetPassword")
                    };
                    DataTable dtResult = dataManager.GetDataTable("SP_USERS", parameters);

                    if (dtResult.Rows[0]["Result"].ToString().Length >= 3)
                    {
                        DisplayMessage(dtResult.Rows[0]["Result"].ToString());
                        ClearAllControls();
                        BindGridViewUserList();
                    }
                }
            }

            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}
