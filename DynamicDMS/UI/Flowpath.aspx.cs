using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;


namespace DynamicDMS.UI
{
    public partial class Flowpath : System.Web.UI.Page
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

            Form.DefaultButton = btnDefault.UniqueID;
            if (!IsPostBack)
            {
                LoadDropDownListCategory();
            }
        }

        #region Data Load Related

        protected void LoadDropDownListCategory()
        {
            DataTable dtCategory = PopulateLists.GetDocumentCategories();
            FillList.PopulateDropDownList(dtCategory, ddlCategory, "Select Category");
        }

        private void BindGridViewCategoryFlow()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                    dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LOAD")
                };

                DataSet dsResult = dataManager.GetDataSet("SP_FLOWPATH", parameters);
                FillList.PopulateGridView(dsResult.Tables[0], gvFlowpath);

                if (dsResult.Tables[1].Rows.Count > 0)
                {
                    FillList.PopulateGridView(dsResult.Tables[1], gvMetadata);
                    ViewState["dtMetadata"] = dsResult.Tables[1];
                }
                else
                {
                    DisplayMessage("No metadata found for the selected Category.");
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion

        #region Button Click Related
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridViewCategoryFlow();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                String ProdutListXml = "";
                UpdateMatadataToDataTable();

                if (CheckValidation())
                {
                    DataTable _dtMetadata = (DataTable)ViewState["dtMetadata"];
                    if (_dtMetadata.Rows.Count > 0)
                    {
                        ProdutListXml = XMLGenerator.FromDataTable(_dtMetadata, "Meatadata", "IsEnable");
                    }

                    DataManager dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[14]
                    {
                        dataManager.MakeInParam("@FlowID", SqlDbType.NVarChar, 500, hfFlowId.Value),
                        dataManager.MakeInParam("@CompanyID", SqlDbType.NVarChar, 500, String.Empty),
                        dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                        dataManager.MakeInParam("@FlowName", SqlDbType.NVarChar, 500, txtFlowName.Text),
                        dataManager.MakeInParam("@Description", SqlDbType.NVarChar, 500, txtDiscription.Text),
                        dataManager.MakeInParam("@SerialNo", SqlDbType.NVarChar, 500, txtSerialNo.Text.ToString()),
                        dataManager.MakeInParam("@MetadataList", SqlDbType.Xml, 2000000, ProdutListXml),

                        dataManager.MakeInParam("@CanUpload", SqlDbType.NVarChar, 500, (cbUpload.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@CanDownload", SqlDbType.NVarChar, 500, (cbDownload.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@CanDelete", SqlDbType.NVarChar, 500, (cbDelete.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsApprover", SqlDbType.NVarChar, 500, (cbIsApprover.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@IsActive", SqlDbType.NVarChar, 500, (cbIsActive.Checked ? "1" : "0")),

                        dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, btnSave.Text)
                    };

                    DataTable _dtRerun = _dataManager.GetDataTable("SP_FLOWPATH", parameters);
                    if (_dtRerun.Rows[0]["Type"].ToString() == "1")
                    {
                        DisplayMessage(_dtRerun.Rows[0]["Result"].ToString());
                        ClearAllControls();
                        BindGridViewCategoryFlow();
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
                DisplayMessage("An error has been occured. Please contact with software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void ClearAllControls()
        {
            txtFlowName.Text = String.Empty;
            txtSerialNo.Text = String.Empty;
            txtDiscription.Text = String.Empty;

            cbUpload.Checked = false;
            cbDownload.Checked = false;
            cbDelete.Checked = false;
            cbIsApprover.Checked = false;
            cbIsActive.Checked = true;

            ViewState["dtMetadata"] = null;
            gvMetadata.DataSource = null;
            gvMetadata.DataBind();

            btnSave.Text = "Save";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllControls();
            BindGridViewCategoryFlow();
        }

        #endregion

        #region Check Validation
        protected Boolean CheckValidation()
        {
            bool _result = true;
            if (ddlCategory.SelectedValue == "0")
            {
                _validationMessage = "Please select category name";
                ddlCategory.Focus();
                _result = false;
            }
            else if (String.IsNullOrEmpty(txtFlowName.Text) && txtFlowName.Text.Trim().Length <= 1)
            {
                _validationMessage = "Please select flow name";
                txtFlowName.Focus();
                _result = false;
            }
            else if (gvFlowpath.Rows.Count > 0)
            {
                foreach (GridViewRow row in gvFlowpath.Rows)
                {
                    if (txtFlowName.Text.Trim() == row.Cells[2].Text.ToString().Trim() && btnSave.Text == "Save")
                    {
                        _validationMessage = "Flow Name already existed. Please try with different name.";
                        _result = false;
                    }
                }
            }

            return _result;
        }

        #endregion

        #region GridView Realted
        protected void gvFlowpath_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                hfFlowId.Value = row.Cells[0].Text;
                txtSerialNo.Text = row.Cells[1].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                txtFlowName.Text = row.Cells[2].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                txtDiscription.Text = row.Cells[3].Text.Replace("&nbsp;", "").Replace("&amp;", "&");

                cbUpload.Checked = ((CheckBox)row.FindControl("gcbUpload")).Checked;
                cbDownload.Checked = ((CheckBox)row.FindControl("gcbDownlaod")).Checked;
                cbDelete.Checked = ((CheckBox)row.FindControl("gcbDelete")).Checked;
                cbIsApprover.Checked = ((CheckBox)row.FindControl("gcbIsApprover")).Checked;
                cbIsActive.Checked = ((CheckBox)row.FindControl("gcbIsActive")).Checked;
                btnSave.Text = "Update";

                LoadMetadataForUpdate();
            }

            if (e.CommandName.Equals("DeleteRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                String sQuery = "DELETE FROM Flowpath WHERE FlowID=" + row.Cells[0].Text;

                _dataManager = new DataManager();
                _dataManager.ExecuteNonQuery(sQuery);
                BindGridViewCategoryFlow();
            }
        }

        protected void LoadMetadataForUpdate()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[3]
            {
                dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                dataManager.MakeInParam("@FlowID", SqlDbType.NVarChar, 500, hfFlowId.Value),
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LoadForUpdate")
            };

            DataTable dtResult = dataManager.GetDataTable("SP_FLOWPATH", parameters);
            if (dtResult.Rows.Count > 0)
            {
                FillList.PopulateGridView(dtResult, gvMetadata);
                ViewState["dtMetadata"] = dtResult;
            }
            else
            {
                DisplayMessage("No metadata found for the selected Category.");
            }
        }

        protected void UpdateMatadataToDataTable()
        {
            DataTable _dtTransac = (DataTable)ViewState["dtMetadata"];

            foreach (GridViewRow gridRow in gvMetadata.Rows)
            {
                String sMetadata = gridRow.Cells[0].Text;
                DataRow[] dataRow = _dtTransac.Select("MetadataId = " + sMetadata);

                dataRow[0]["Caption"] = gridRow.Cells[1].Text;
                dataRow[0]["IsEnable"] = ((CheckBox)gridRow.FindControl("gcbIsEnable")).Checked;
                dataRow[0]["IsRequired"] = ((CheckBox)gridRow.FindControl("gcbIsRequired")).Checked;
                _dtTransac.AcceptChanges();

                ViewState["dtMetadata"] = _dtTransac;
            }
        }

        protected void gCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbBox = (CheckBox)sender;
            GridViewRow gridRow = (GridViewRow)cbBox.Parent.Parent;

            Boolean isEnable = ((CheckBox)gridRow.FindControl("gcbIsEnable")).Checked;
            Boolean isRequired = ((CheckBox)gridRow.FindControl("gcbIsRequired")).Checked;

            if ((isEnable == false && isRequired == true))
            {
                ((CheckBox)gridRow.FindControl("gcbIsRequired")).Checked = false;
                DisplayMessage("Please enable the field first, then make it required.");
            }
            else
            {
                DataTable _dtCategory = (DataTable)ViewState["dtMetadata"];
                foreach (DataRow dataRow in _dtCategory.Rows)
                {
                    if (dataRow["MetadataId"].ToString() == gridRow.Cells[0].Text)
                    {
                        dataRow["IsEnable"] = isEnable;
                        dataRow["IsRequired"] = isRequired;
                        _dtCategory.AcceptChanges();

                        ViewState["dtMetadata"] = _dtCategory;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Common Methods
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
        #endregion
    }
}