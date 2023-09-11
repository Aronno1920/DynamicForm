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
using System.ComponentModel.Design;
using System.Text;

namespace LogicDMS.UI
{
    public partial class Lookup : System.Web.UI.Page
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
                divIsRequired.Visible = false;

                LoadDropDownListLookupType();
                BindGridViewLookup();
            }
        }

        #region Page Load Related

        protected void LoadDropDownListLookupType()
        {
            try
            {
                DataTable dtCopmany = PopulateLists.GetLookupTypes();
                FillList.PopulateDropDownList(dtCopmany, ddlLookupType, "Select Lookup Type");
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\n\nError:" + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void BindGridViewLookup()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[3]
                {
                    dataManager.MakeInParam("@TypeId", SqlDbType.NVarChar, 500, ddlLookupType.SelectedValue),
                    dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtSearch.Text.Trim()),
                    dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LOAD")
                };

                DataSet _dsRerun = _dataManager.GetDataSet("SP_LOOKUP", parameters);
                FillList.PopulateGridView(_dsRerun.Tables[0], gvLookup);

            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\n\nError:" + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion


        #region Button Related Methods
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckValidation())
                {
                    DataManager dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[10]
                    {
                        dataManager.MakeInParam("@LookupId", SqlDbType.BigInt, 500, hfLookupId.Value),
                        dataManager.MakeInParam("@TypeId", SqlDbType.Int, 500, ddlLookupType.SelectedValue),
                        dataManager.MakeInParam("@Value", SqlDbType.Int, 500, hfLookupValue.Value),
                        dataManager.MakeInParam("@Text", SqlDbType.NVarChar, 500, txtName.Text.Trim()),
                        dataManager.MakeInParam("@Description", SqlDbType.NVarChar, 500, txtDescription.Text.Trim()),
                        dataManager.MakeInParam("@Serial", SqlDbType.Decimal, 18, 2, txtSerial.Text),
                        dataManager.MakeInParam("@IsRequired", SqlDbType.NVarChar, 500, (cbIsRequired.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsActive", SqlDbType.NVarChar, 500, (cbIsActive.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, btnSave.Text)
                    };

                    DataTable _dtRerun = _dataManager.GetDataTable("SP_LOOKUP", parameters);
                    if (_dtRerun.Rows[0]["Type"].ToString() == "1")
                    {
                        DisplayMessage(_dtRerun.Rows[0]["Result"].ToString());
                        ClearAllControls();
                        BindGridViewLookup();
                    }
                    else
                    {
                        DisplayMessage(_dtRerun.Rows[0]["Result"].ToString());
                    }
                }

                else
                {
                    DisplayMessage(_validationMessage);
                }
            }

            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\n\nError:" + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected Boolean CheckValidation()
        {
            bool _result = true;
            if (String.IsNullOrEmpty(ddlLookupType.SelectedValue) || ddlLookupType.SelectedValue == "0")
            {
                _validationMessage = "Please select any lookup type";
                _result = false;
            }
            else if (String.IsNullOrEmpty(txtName.Text))
            {
                _validationMessage = "Please enter lookup text";
                _result = false;
            }

            return _result;
        }

        protected void ClearAllControls()
        {
            cbIsActive.Checked = true;
            txtName.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtSerial.Text = String.Empty;
            btnSave.Text = "Save";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            hfLookupId.Value = "0";
            hfLookupValue.Value = "0";
            ClearAllControls();
        }
        #endregion


        #region Load GridView Data
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridViewLookup();
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Text = String.Empty;
            BindGridViewLookup();
        }

        protected void ddlLookupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLookupType.SelectedValue == "7")
            {
                divIsRequired.Visible = true;
            }
            else
            {
                divIsRequired.Visible = false;
            }

            BindGridViewLookup();
        }
        #endregion


        #region GridView Command

        protected void gvLookup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLookup.PageIndex = e.NewPageIndex;
            BindGridViewLookup();
        }

        protected void gvLookup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                String sLookupId = ((HiddenField)row.FindControl("gHfLookupId")).Value;
                String sLookupTypeId = ((HiddenField)row.FindControl("gHfLookupTypeId")).Value;
                String sLookupValue = ((HiddenField)row.FindControl("gHfLookupValue")).Value;
                String sIsRequired = ((HiddenField)row.FindControl("gHfIsRequired")).Value;

                hfLookupId.Value= sLookupId.Replace("&nbsp;", "");
                ddlLookupType.SelectedIndex = ddlLookupType.Items.IndexOf(ddlLookupType.Items.FindByValue(sLookupTypeId));
                hfLookupValue.Value = sLookupValue.Replace("&nbsp;","");
                cbIsActive.Checked = Convert.ToBoolean(sIsRequired);

                txtSerial.Text = row.Cells[0].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                txtName.Text = row.Cells[1].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                txtDescription.Text = row.Cells[2].Text.Replace("&nbsp;", "").Replace("&amp;", "&");

                bool isActive = true;
                if (row.Cells[3].Text == "Inactive")
                {
                    isActive = false;
                }
                cbIsActive.Checked = isActive;
                btnSave.Text = "Update";
            }
        }

        #endregion


        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }

    }
}