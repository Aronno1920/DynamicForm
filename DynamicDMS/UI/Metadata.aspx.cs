using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace DynamicDMS.UI
{
    public partial class Metadata : System.Web.UI.Page
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
                LoadDropDownListInputType();
            }
        }

        #region Page & Other Load Related
        protected void LoadDropDownListCategory()
        {
            DataTable dtCategory = PopulateLists.GetDocumentCategories();
            FillList.PopulateDropDownList(dtCategory, ddlCategory, "Select Category");
        }

        protected void LoadDropDownListInputType()
        {
            DataTable dtInputType = PopulateLists.GetInputTypies();
            FillList.PopulateDropDownList(dtInputType, ddlFieldType, "Select Input Type");
        }

        private void BindGridViewCategoryMetadata()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                    dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LOAD")
                };

                DataTable dtResult = dataManager.GetDataTable("SP_METADATA", parameters);
                FillList.PopulateGridView(dtResult, gvMetadata);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        #endregion

        #region Button and DropDownList Events Related

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridViewCategoryMetadata();
            LoadDropDownListHeader();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckValidation())
                {
                    DataManager dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[8]
                    {
                        dataManager.MakeInParam("@CategoryID", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                        dataManager.MakeInParam("@FieldName", SqlDbType.NVarChar, 500, txtFieldName.Text.Replace(" ","")),
                        dataManager.MakeInParam("@Caption", SqlDbType.NVarChar, 500, txtFieldName.Text),
                        dataManager.MakeInParam("@InputType", SqlDbType.NVarChar, 500, ddlFieldType.SelectedValue),
                        dataManager.MakeInParam("@PanelId", SqlDbType.NVarChar, 500, ddlPanelNameSave.SelectedValue),
                        dataManager.MakeInParam("@Serial", SqlDbType.NVarChar, 500, txtSerialSave.Text),
                        dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Save")
                    };

                    DataTable _dtRetrun = _dataManager.GetDataTable("SP_METADATA", parameters);
                    if (_dtRetrun.Rows[0]["Type"].ToString() == "1")
                    {
                        txtFieldName.Text = String.Empty;
                        ddlFieldType.SelectedIndex = -1;
                        ddlPanelNameSave.SelectedIndex = -1;
                        txtSerialSave.Text = String.Empty;

                        DisplayMessage(_dtRetrun.Rows[0]["Result"].ToString());

                        BindGridViewCategoryMetadata();
                        LoadDropDownListHeader();
                    }
                    else
                    {
                        DisplayMessage(_dtRetrun.Rows[0]["Result"].ToString());
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                String sDataSourceId = String.Empty;
                String sOrientation = String.Empty;

                if (divPopupLookup.Visible)
                {
                    sDataSourceId = ddlDataSourceName.SelectedValue;
                }

                if (divPopupOrientation.Visible)
                {
                    sOrientation = ddlOrientation.SelectedValue;
                }

                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[13]
                {
                        dataManager.MakeInParam("@MetadataId", SqlDbType.NVarChar, 500, pHfMetadataId.Value),
                        dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                        dataManager.MakeInParam("@MappingField", SqlDbType.NVarChar, 500, txtMappingField.Text),
                        dataManager.MakeInParam("@PanelId", SqlDbType.NVarChar, 500, ddlHeaderName.SelectedValue),
                        dataManager.MakeInParam("@Caption", SqlDbType.NVarChar, 500, txtCaption.Text.ToString()),
                        dataManager.MakeInParam("@Serial", SqlDbType.NVarChar, 500, txtSerial.Text),
                        dataManager.MakeInParam("@RequiredMessage", SqlDbType.NVarChar, 500, txtRequiredMessage.Text),
                        dataManager.MakeInParam("@Placeholder", SqlDbType.NVarChar, 500, txtPlaceholder.Text),

                        dataManager.MakeInParam("@DataSourceId", SqlDbType.NVarChar, 500, sDataSourceId),
                        dataManager.MakeInParam("@Orientation", SqlDbType.NVarChar, 500, sOrientation),

                        dataManager.MakeInParam("@IsActive", SqlDbType.NVarChar, 500, (cbIsActive.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "UPDATE")
                };

                DataTable _dtRerun = _dataManager.GetDataTable("SP_METADATA", parameters);
                if (_dtRerun.Rows[0]["Type"].ToString() == "1")
                {
                    DisplayMessage(_dtRerun.Rows[0]["Result"].ToString());
                    BindGridViewCategoryMetadata();
                }
                else
                {
                    DisplayMessage(_dtRerun.Rows[0]["Result"].ToString());
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
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
            else if (String.IsNullOrEmpty(txtFieldName.Text))
            {
                _validationMessage = "Please enter input caption";
                txtFieldName.Focus();
                _result = false;
            }
            else if (txtFieldName.Text.Trim().Length < 1)
            {
                _validationMessage = "Please enter input caption";
                txtFieldName.Focus();
                _result = false;
            }
            else if (!IsValidInput(txtFieldName.Text))
            {
                _validationMessage = "Invalid input caption. Only characters and spaces are allowed.";
                txtFieldName.Focus();
                _result = false;
            }
            else if (ddlFieldType.SelectedValue == "0")
            {
                _validationMessage = "Please select input type";
                ddlFieldType.Focus();
                _result = false;
            }
            else if (gvMetadata.Rows.Count > 0)
            {
                foreach (GridViewRow row in gvMetadata.Rows)
                {
                    if (txtFieldName.Text.Trim() == row.Cells[5].Text.ToString().Trim() && ddlFieldType.SelectedItem.Text == row.Cells[6].Text.ToString().Trim())
                    {
                        _validationMessage = "Metadata with same name already existed. Please select different name";
                        _result = false;
                    }
                }
            }

            return _result;
        }

        static bool IsValidInput(String input)
        {
            string pattern = @"^[A-Za-z\s]+$";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);
            return match.Success;
        }

        #endregion

        #region GridView Realted
        protected void gvMetadata_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                String sInputType = row.Cells[6].Text.Replace("&nbsp;", "");

                ControlPopupVisibilty(sInputType);
                LoadDropDownListHeader();
                LoadDropDownListLookupType();

                pHfMetadataId.Value = row.Cells[0].Text;
                pHfDataSourceId.Value = row.Cells[2].Text;
                txtSerial.Text = row.Cells[3].Text.Replace("&nbsp;", "");
                txtCaption.Text = row.Cells[5].Text.Replace("&nbsp;", "");
                ptxtFieldType.Text = sInputType;
                txtMappingField.Text = row.Cells[7].Text.Replace("&nbsp;", "");
                txtRequiredMessage.Text = row.Cells[8].Text.Replace("&nbsp;", "");
                txtPlaceholder.Text = row.Cells[9].Text.Replace("&nbsp;", "");
                ddlDataSourceName.SelectedIndex = ddlDataSourceName.Items.IndexOf(ddlDataSourceName.Items.FindByText(row.Cells[10].Text.Replace("&nbsp;", "")));
                ddlOrientation.SelectedIndex = ddlOrientation.Items.IndexOf(ddlOrientation.Items.FindByText(row.Cells[11].Text.Replace("&nbsp;", "")));

                if (row.Cells[3].Text != "---")
                {
                    ddlHeaderName.SelectedIndex = ddlHeaderName.Items.IndexOf(ddlHeaderName.Items.FindByText(row.Cells[4].Text));
                }

                modalExtender.Show();
            }

            if (e.CommandName.Equals("DeleteRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@MetadataId", SqlDbType.NVarChar, 500,  row.Cells[0].Text),
                    dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "DELETE")
                };
                _dataManager.ExecuteProcedure("SP_METADATA", parameters);

                BindGridViewCategoryMetadata();
            }
        }

        protected void LoadDropDownListHeader()
        {
            String sQuery = "SELECT MetadataId as ValueField, Caption as DisplayField FROM Metadata WHERE InputType='Panel' AND CategoryId=" + ddlCategory.SelectedValue + "   ORDER BY Serial ASC;";

            _dataManager = new DataManager();
            DataTable dtHeader = _dataManager.GetDataTable(sQuery);

            FillList.PopulateDropDownList(dtHeader, ddlHeaderName, "Select Any");
            FillList.PopulateDropDownList(dtHeader, ddlPanelNameSave, "Select Panel");
        }

        protected void LoadDropDownListLookupType()
        {
            DataTable dtCategory = PopulateLists.GetLookupTypes();
            FillList.PopulateDropDownList(dtCategory, ddlDataSourceName, "Select Source");
        }

        protected void ControlPopupVisibilty(String sFieldType)
        {
            divPopupHeader.Visible = true;
            divRequiredMessage.Visible = true;
            divMappingField.Visible = false;
            divPopupLookup.Visible = false;
            divPopupOrientation.Visible = false;

            if (ConfigurationManager.AppSettings["ApiIntegration"].ToString() == "1")
            {
                divMappingField.Visible = true;
            }

            switch (sFieldType)
            {
                case "Panel":
                    divPopupHeader.Visible = false;
                    divRequiredMessage.Visible = false;
                    divPopupPlaceholder.Visible = false;
                    break;
                case "DatePicker":
                    divPopupPlaceholder.Visible = false;
                    break;
                case "DropDownList":
                    divPopupLookup.Visible = true;
                    divPopupPlaceholder.Visible = false;
                    break;
                case "RadioButtonList":
                    divPopupLookup.Visible = true;
                    divPopupOrientation.Visible = true;
                    divPopupPlaceholder.Visible = false;
                    break;
                default: break;
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