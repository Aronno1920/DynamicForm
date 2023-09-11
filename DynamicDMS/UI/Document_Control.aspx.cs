using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreLibrary;


namespace DynamicDMS.UI
{
    public partial class Document_Control : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
        String _successMessage = String.Empty;
        String _errorMessage = String.Empty;

        DataManager _dataManager = new DataManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Check User Login Status
            if (String.IsNullOrEmpty(_user.GetCookie(CookieKey.UserId.ToString())) || _user.GetCookie(CookieKey.UserId.ToString()) == "0")
            {
                Response.Redirect(String.Format("~/Default.aspx", false));
            }

            if (Request.QueryString["mode"] == null)
            {
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
            }

            #endregion

            if (!Page.IsPostBack)
            {
                this.Form.DefaultButton = btnDefault.UniqueID;
                txtEntryDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                divButtonApprove.Visible = false;

                if (!String.IsNullOrEmpty(Request.QueryString["categoryId"]))
                {
                    hfCategoryId.Value = Request.QueryString["categoryId"];

                    String sMode = Request.QueryString["mode"].ToString();
                    if (sMode == "wf")
                    {
                        ControlApprovalMode();
                    }
                }
                else
                {
                    Response.Redirect(String.Format("~/UI/UnauthorizedPage.aspx"), false);
                    return;
                }

                LoadDropDownListCompany();
                LoadDropDownListExpense();
                LoadDropDownListAttachmentType();
                BindGridViewOwnDocument();

                if (!String.IsNullOrEmpty(Request.QueryString["documentID"]))
                {
                    hfDocumentId.Value = Request.QueryString["documentID"];
                    LoadDocumentForUpdate();
                    LoadDropDownListRevertTo();
                }
            }

            LoadMetadataWithUploader();
        }

        #region Dynamic Control Related Methods
        private void LoadMetadataWithUploader()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, hfCategoryId.Value),
                    dataManager.MakeInParam("@DocumentId", SqlDbType.NVarChar, 500, hfDocumentId.Value)
                };

                DataSet dsMetaControl = dataManager.GetDataSet("SP_SYS_METADATA_CONTROLS", parameters);
                if (dsMetaControl.Tables[0].Rows.Count > 0)
                {
                    ViewState["Metadata"] = dsMetaControl.Tables[0];
                    GenerateDynamicLayout();
                    GenerateDynamicGridView();
                }

                if (dsMetaControl.Tables[1].Rows.Count > 0)
                {
                    ControlVisibility(dsMetaControl.Tables[1]);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
            }
        }

        private void GenerateDynamicLayout()
        {
            DataTable dtControls = (DataTable)ViewState["Metadata"];
            DataRow[] panelInfo = dtControls.Select("InputType='Panel'");

            // Define the number of panels
            int numPanels = panelInfo.Length - 1;

            for (int panelNum = 0; panelNum <= numPanels; panelNum++)
            {
                // Create the outer div element with class "panel panel-info"
                HtmlGenericControl outerDivPanel = new HtmlGenericControl("div");
                outerDivPanel.Attributes["class"] = "panel panel-info";
                outerDivPanel.Attributes["style"] = "margin-top: 15px";

                // Create the panel header div with class "panel-header"
                HtmlGenericControl panelHeaderDiv = new HtmlGenericControl("div");
                panelHeaderDiv.Attributes["class"] = "panel-header";
                panelHeaderDiv.InnerText = panelInfo[panelNum]["Caption"].ToString();

                // Create the panel body div with class "panel-body"
                HtmlGenericControl panelBodyDiv = new HtmlGenericControl("div");
                panelBodyDiv.Attributes["class"] = "panel-body";

                // Define the number of rows dynamically for each panel
                DataRow[] insideControls = dtControls.Select("PanelId=" + panelInfo[panelNum]["MetadataId"].ToString());

                Int32 iTotalColumn = insideControls.Length;
                Int32 numRowsPerPanel = iTotalColumn / 4;
                if (iTotalColumn % 4 > 0)
                {
                    numRowsPerPanel = numRowsPerPanel + 1;
                }

                Int32 dataIndex = 1;
                for (int row = 1; row <= numRowsPerPanel; row++)
                {
                    // Create the outer div element with class "row"
                    HtmlGenericControl outerDivRow = new HtmlGenericControl("div");
                    outerDivRow.Attributes["class"] = "row";

                    for (int cell = 1; cell <= 4; cell++)
                    {
                        if (iTotalColumn < dataIndex)
                            break;

                        DataRow data = insideControls.FirstOrDefault(r => r["RowID"].ToString() == dataIndex.ToString());
                        DynamicControls.GenerateControl(outerDivRow, data);

                        dataIndex = dataIndex + 1;
                    }

                    panelBodyDiv.Controls.Add(outerDivRow);
                }
                outerDivPanel.Controls.Add(panelHeaderDiv);
                outerDivPanel.Controls.Add(panelBodyDiv);

                placeholder.Controls.Add(outerDivPanel);
            }
        }

        private void GenerateDynamicGridView()
        {

        }

        protected DataTable ReturnUserInputs()
        {
            DataTable dtUserInput = (DataTable)ViewState["Metadata"];
            if (dtUserInput.Rows.Count > 0)
            {
                for (Int32 i = 0; i < dtUserInput.Rows.Count; i++)
                {
                    String FieldName = Convert.ToString(dtUserInput.Rows[i]["FieldName"]);
                    String FieldType = Convert.ToString(dtUserInput.Rows[i]["InputType"]);

                    switch (FieldType.ToLower().Trim())
                    {
                        case "textbox":
                            TextBox txtbBox = (TextBox)placeholder.FindControl("txt" + FieldName);
                            if (txtbBox != null)
                                dtUserInput.Rows[i]["InputValue"] = txtbBox.Text;
                            break;

                        case "textarea":
                            TextBox textArea = (TextBox)placeholder.FindControl("txt" + FieldName);
                            if (textArea != null)
                                dtUserInput.Rows[i]["InputValue"] = textArea.Text;
                            break;

                        case "datepicker":
                            TextBox txtDatePicker = (TextBox)placeholder.FindControl("txt" + FieldName);
                            if (txtDatePicker != null)
                                dtUserInput.Rows[i]["InputValue"] = txtDatePicker.Text;
                            break;

                        case "checkbox":
                            CheckBox checkBox = (CheckBox)placeholder.FindControl("chk" + FieldName);
                            if (checkBox != null)
                                dtUserInput.Rows[i]["InputValue"] = checkBox.Checked;
                            break;

                        case "radioButtonList":
                            RadioButtonList radioButtonList = (RadioButtonList)placeholder.FindControl("rbtn" + FieldName);
                            if (radioButtonList != null)
                                dtUserInput.Rows[i]["InputValue"] = radioButtonList.SelectedValue;
                            break;

                        case "dropDownList":
                            DropDownList dropDownList = (DropDownList)placeholder.FindControl("ddl" + FieldName);
                            if (dropDownList != null)
                                dtUserInput.Rows[i]["InputValue"] = dropDownList.SelectedValue;
                            break;

                        default: break;
                    }
                }
            }

            return dtUserInput;
        }
        
        #endregion

        #region Set active/inactive status of Controls

        protected void ControlVisibility(DataTable dtControls)
        {
            divPageHeader.InnerText = dtControls.Rows[0]["CategoryName"].ToString();
            divUploader.Visible = Convert.ToBoolean(dtControls.Rows[0]["IsCanUpload"].ToString());

            if (Convert.ToBoolean(dtControls.Rows[0]["IsApprover"].ToString()) == true)
            {
                btnWorkflowForward.Text = "Approved";
                hfIsApproval.Value = "1";
                btnWorkflowDecline.Visible = true;
            }
            else
            {
                hfIsApproval.Value = "0";
                btnWorkflowDecline.Visible = false;
            }
        }

        protected void ControlApprovalMode()
        {
            ddlCompany.Enabled = false;
            ddlExpenseType.Enabled = false;
            txtRemarks.Enabled = false;
            btnLoadRefNo.Enabled = false;
            divButtonPrepare.Visible = false;
            divButtonApprove.Visible = true;
        }

        #endregion

        #region Control Bind With Data from Database

        protected void LoadDropDownListCompany()
        {
            String sUserId = _user.GetCookie(CookieKey.UserId.ToString());
            String sServiceId = hfCategoryId.Value;

            DataTable dtComapy = PopulateLists.GetCompaniesByUser_Category(sUserId, sServiceId);
            FillList.PopulateDropDownList(dtComapy, ddlCompany, "Select Company");
        }

        protected void LoadDropDownListExpense()
        {
            DataTable dtExpense = PopulateLists.GetExpenseTypes();
            FillList.PopulateDropDownList(dtExpense, ddlExpenseType, "Select Item Category");
        }

        protected void LoadDropDownListAttachmentType()
        {
            DataTable dtDocumentType = PopulateLists.GetAttachmentTypes();
            FillList.PopulateDropDownList(dtDocumentType, ddlDocumentType, "Select Document Type");
        }

        private DataSet LoadDocumentInformation()
        {
            DataSet _dsResult = new DataSet();
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[1]
                {
                     _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value)
                };

                _dsResult = _dataManager.GetDataSet("SP_SELECT_DOCUMENT", parameters);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return _dsResult;
        }

        protected void LoadDocumentForUpdate()
        {
            try
            {
                DataSet ds = LoadDocumentInformation();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(ds.Tables[0].Rows[0]["CompanyId"].ToString()));
                    ddlExpenseType.SelectedIndex = ddlExpenseType.Items.IndexOf(ddlExpenseType.Items.FindByValue(ds.Tables[0].Rows[0]["ExpenseTypeID"].ToString()));
                    txtEntryDate.Text = ds.Tables[0].Rows[0]["EntryDate"].ToString();

                    txtBillRefNo.Text = ds.Tables[0].Rows[0]["BillREfNo"].ToString();
                    hfRefDocumentID.Value = ds.Tables[0].Rows[0]["RefDocumentId"].ToString();
                    txtRefTracking.Text = ds.Tables[0].Rows[0]["RefDocumentNo"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                }

                FillList.PopulateGridView(ds.Tables[1], gvComment); //DocumentComments
                FillList.PopulateGridView(ds.Tables[2], gvAttachment); //Document
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion

        #region Attachment GridView Related

        protected void gBtnPreview_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imDownload = (ImageButton)sender;
            GridViewRow row = ((GridViewRow)imDownload.Parent.Parent);
            int transID = Convert.ToInt32(row.Cells[0].Text);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('Viewer.aspx?id=" + transID.ToString() + "');", true);
        }

        protected void gBtnDownload_Click(object sender, ImageClickEventArgs e)
        {
            //SectionVisibility activity = new SectionVisibility(hfCategoryId.Value, hfDocumentId.Value);
            //if (activity.IsEnableDownload)
            //{
                string sPathToSaveFileTo = Server.MapPath("~/" + ConfigurationManager.AppSettings["DraftFolder"].ToString() + "/");  // on this path i will create selected PDF File Data    open pdf for checking

                ImageButton imDownload = (ImageButton)sender;
                GridViewRow row = ((GridViewRow)imDownload.Parent.Parent);

                String sFileName = row.Cells[1].Text;
                String sFileTitle = row.Cells[4].Text;
                String sContentType = row.Cells[2].Text;

                FileInfo file = new FileInfo(sPathToSaveFileTo + sFileName);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + sFileTitle);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = sContentType;
                    Response.Flush();
                    Response.TransmitFile(sPathToSaveFileTo + sFileName);
                    Response.End();
                }
                else
                {
                    DisplayMessage("Requested file is not available for download");
                }
            //}
            //else
            //{
            //    DisplayMessage("Sorry! You don't have permission to download attachment.");
            //}
        }

        protected void gBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            //SectionVisibility activity = new SectionVisibility(hfCategoryId.Value, hfDocumentId.Value);
            //if (activity.IsEnableDelete)
            //{
                ImageButton imDownload = (ImageButton)sender;
                GridViewRow row = ((GridViewRow)imDownload.Parent.Parent);
                int transID = Convert.ToInt32(row.Cells[0].Text);

                String sQuery = "SELECT EntryBy FROM dbo.DocumentData WHERE TransID =" + transID;
                _dataManager = new DataManager();

                DataTable dtUser = _dataManager.GetDataTable(sQuery);
                if (dtUser.Rows.Count > 0 && dtUser.Rows[0]["EntryBy"].ToString() == _user.GetCookie(CookieKey.UserId.ToString()))
                {
                    String sUpdateQuery = "UPDATE dbo.DocumentData SET IsActive=0 WHERE TransId=" + transID;
                    DataManager dataManager = new DataManager();

                    dataManager.ExecuteNonQuery(sUpdateQuery);
                    DisplayMessage("Attachment file has been deleted.");
                    BindGridViewDocumentData();
                }
                else
                {
                    DisplayMessage("Only document owner can delete his attachment file.");
                }
            //}
            //else
            //{
            //    DisplayMessage("Sorry! You don't have permission to delete uploaded attachment.");
            //}
        }

        protected void BindGridViewDocumentData()
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[1]
                {
                  _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value)
                };

                DataTable dtAttachemnt = _dataManager.GetDataTable("SP_SELECT_DOCUMENT_DATA", parameters);
                FillList.PopulateGridView(dtAttachemnt, gvAttachment); //Document
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion

        #region Validation Check Related Methods
        protected Boolean CheckSaveValidation()
        {
            Boolean _result = true;

            if (ddlCompany.SelectedValue == "0")
            {
                _errorMessage = "Please select company";
                ddlCompany.Focus();
                return false;
            }
            else if (ddlExpenseType.SelectedValue == "0")
            {
                _errorMessage = "Please select item category";
                ddlExpenseType.Focus();
                return false;
            }
            //else if (txtPINumber.Enabled == true && String.IsNullOrEmpty(txtPINumber.Text))
            //{
            //    _errorMessage = "Please enter PI number";
            //    txtPINumber.Focus();
            //    return false;
            //}
            //else if (txtPONo.Enabled == true && String.IsNullOrEmpty(txtPONo.Text))
            //{
            //    _errorMessage = "Please enter PO number";
            //    txtPONo.Focus();
            //    return false;
            //}
            //else if (txtChallanNo.Enabled == true && String.IsNullOrEmpty(txtChallanNo.Text))
            //{
            //    _errorMessage = "Please enter challan number";
            //    txtChallanNo.Focus();
            //    return false;
            //}
            //else if (txtMRRNo.Enabled == true && String.IsNullOrEmpty(txtMRRNo.Text))
            //{
            //    _errorMessage = "Please enter MMR number";
            //    txtMRRNo.Focus();
            //    return false;
            //}
            //else if (txtBillNo.Enabled == true && String.IsNullOrEmpty(txtBillNo.Text))
            //{
            //    _errorMessage = "Please enter bill number";
            //    txtBillNo.Focus();
            //    return false;
            //}
            //else if (txtBillAmount.Enabled == true && String.IsNullOrEmpty(txtBillAmount.Text))
            //{
            //    _errorMessage = "Please enter bill amount";
            //    txtBillAmount.Focus();
            //    return false;
            //}
            //else if (txtDiscountAmount.Enabled == true && String.IsNullOrEmpty(txtDiscountAmount.Text))
            //{
            //    _errorMessage = "Please enter discount amount";
            //    txtDiscountAmount.Focus();
            //    return false;
            //}
            //else if (CheckMRRNumberValidation() == false)
            //{
            //    txtMRRNo.Focus();
            //    return false;
            //}

            return _result;
        }

        protected Boolean CheckforRemarks()
        {
            Boolean _result = true;
            if (String.IsNullOrEmpty(txtRemarksBoss.Text))
            {
                _errorMessage = "Please write remarks and try again";
                txtRemarksBoss.Focus();
                _result = false;
            }
            return _result;
        }

        protected Boolean CheckValidationForRevert()
        {
            Boolean _result = true;

            if (ddlRevertTo.SelectedValue == "0")
            {
                _errorMessage = "Please select any role to reject";
                ddlCompany.Focus();
                return false;
            }
            else if (String.IsNullOrEmpty(txtRemarksBoss.Text))
            {
                _errorMessage = "Please write remarks and try again";
                txtRemarksBoss.Focus();
                _result = false;
            }

            return _result;
        }

        protected Boolean CheckUploadValidation()
        {
            Boolean _result = true;

            if (ddlDocumentType.SelectedValue == "0")
            {
                _errorMessage = "Please select document type";
                ddlDocumentType.Focus();
                return false;
            }
            else if (!FileUpload1.HasFile)
            {
                _errorMessage = "Please select any file first.";
                return false;
            }

            return _result;
        }

        //protected Boolean CheckMRRNumberValidation()
        //{
        //    Boolean _result = true;

        //    try
        //    {
        //        String sQueryPart = String.Empty;
        //        if (!String.IsNullOrEmpty(Request.QueryString["DocumentID"]))
        //        {
        //            sQueryPart = " AND DocumentID<>" + Request.QueryString["DocumentID"].ToString();
        //        }

        //        String sQuery = "SELECT MRRNo FROM dbo.DocumentInfo WHERE MRRNo<>'' AND MRRNo='" + txtMRRNo.Text + "'" + sQueryPart;
        //        _dataManager = new DataManager();
        //        DataTable dtMRR = _dataManager.GetDataTable(sQuery);

        //        if (dtMRR.Rows.Count > 0)
        //        {
        //            _result = false;
        //            _errorMessage = "Document has been processed with the MRR number. Please enter different MRR number";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
        //        ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        //    }
        //    return _result;
        //}
        #endregion

        #region Document Save and Upload Related Methods
        protected String SaveDocumentRelatedInformation(Int32 Status, String sAction)
        {
            String MetadataXml = "";

            try
            {
                DataTable dtUserInput = ReturnUserInputs();
                if (dtUserInput.Rows.Count > 0)
                {
                    MetadataXml = XMLGenerator.FromDataTable(dtUserInput, "Meatadata");
                }

                if (CheckSaveValidation())
                {
                    _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[10]
                    {
                        _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                        _dataManager.MakeInParam("@RefDocumentID", SqlDbType.NVarChar, 500, hfRefDocumentID.Value),

                        _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                        _dataManager.MakeInParam("@ExpenseId", SqlDbType.NVarChar, 500, ddlExpenseType.SelectedValue),
                        _dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, hfCategoryId.Value),
                        _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarks.Text),
                        _dataManager.MakeInParam("@MetadataList", SqlDbType.Xml, 2000000, MetadataXml),

                        _dataManager.MakeInParam("@Status", SqlDbType.NVarChar, 500, Status.ToString()),
                        _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, sAction)
                    };
                    DataTable _dtReturn = _dataManager.GetDataTable("SP_DOCUMENT_INITIAL", parameters);

                    if (_dtReturn.Rows.Count > 0)
                    {
                        hfDocumentId.Value = _dtReturn.Rows[0]["DocumentID"].ToString();
                        txtBillRefNo.Text = _dtReturn.Rows[0]["BillRefNo"].ToString();

                        _successMessage = "Document has been saved Successfully! Your Tracking No.: " + txtBillRefNo.Text.Trim();
                    }
                    else
                    {
                        hfDocumentId.Value = String.Empty;
                        _errorMessage = "Document save failed. Please try again";
                    }
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return (String.IsNullOrEmpty(_errorMessage) ? _successMessage : _errorMessage);
        }

        protected String UploadDocumentFile()
        {
            try
            {
                if (CheckUploadValidation())
                {
                    String sExtension = Path.GetExtension(FileUpload1.FileName);
                    String sContentType = FileUpload1.PostedFile.ContentType;
                    String sTitle = Path.GetFileName(FileUpload1.FileName);
                    String sDraftPath = ConfigurationManager.AppSettings["DraftLocation"].ToString();
                    String sApprovePath = ConfigurationManager.AppSettings["ApproveLocation"].ToString();

                    _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[11]
                    {
                        _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value.ToString()),
                        _dataManager.MakeInParam("@Extension", SqlDbType.NVarChar, 500, sExtension),
                        _dataManager.MakeInParam("@Title", SqlDbType.NVarChar, 500, sTitle),
                        _dataManager.MakeInParam("@TotalPage", SqlDbType.NVarChar, 500, String.Empty),
                        _dataManager.MakeInParam("@TypeID", SqlDbType.NVarChar, 500, ddlDocumentType.SelectedValue.ToString()),
                        _dataManager.MakeInParam("@ContentType", SqlDbType.NVarChar, 500, sContentType),
                        _dataManager.MakeInParam("@DraftPath", SqlDbType.NVarChar, 500, sDraftPath),
                        _dataManager.MakeInParam("@ApprovePath", SqlDbType.NVarChar, 500, sApprovePath),
                        _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtFileRemarks.Text),
                        _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "UPLOAD")
                    };

                    DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_FILE_UPLOADER", parameters);
                    if (_dtReturn.Rows.Count > 0 && _dtReturn.Rows[0]["Result"].ToString().Length > 10)
                    {
                        _successMessage = "File has been uploaded Successfully!";

                        String sDraftFolder = ConfigurationManager.AppSettings["DraftFolder"].ToString();
                        String sFilePath = Server.MapPath("~/" + sDraftFolder + "/" + _dtReturn.Rows[0]["Result"].ToString());
                        FileUpload1.SaveAs(sFilePath);

                        String sApproveFolder = ConfigurationManager.AppSettings["ApproveFolder"].ToString();
                        String sApproveFilePath = Server.MapPath("~/" + sApproveFolder + "/" + _dtReturn.Rows[0]["Result"].ToString());
                        FileUpload1.SaveAs(sApproveFilePath);

                        String sFileId = _dtReturn.Rows[0]["FileId"].ToString();

                        if (Convert.ToBoolean(_dtReturn.Rows[0]["SignatureRequired"].ToString()))
                        {
                            GenerateElectronicSign(sFileId);
                        }
                    }
                    else
                    {
                        _errorMessage = "File upload failed. Please try again";
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return (String.IsNullOrEmpty(_errorMessage) ? _successMessage : _errorMessage);
        }

        protected void GenerateElectronicSign(String sFileData)
        {
            GenerateWatermark generate = new GenerateWatermark();
            generate.AsDraft(sFileData);
        }

        protected void ClearAllControls()
        {
            hfDocumentId.Value = String.Empty;
            ddlCompany.SelectedIndex = -1;
            ddlExpenseType.SelectedIndex = -1;
            txtRemarks.Text = String.Empty;
        }

        protected void LoadDocumentAfterUpload()
        {
            DataSet dsDocument = LoadDocumentInformation();

            if (dsDocument.Tables[1].Rows.Count > 0)
            {
                gvComment.DataSource = dsDocument.Tables[1];
                gvComment.DataBind();
            }
            else
            {
                gvComment.DataSource = null;
            }

            if (dsDocument.Tables[2].Rows.Count > 0)
            {
                gvAttachment.DataSource = dsDocument.Tables[2];
                gvAttachment.DataBind();
            }
            else
            {
                gvAttachment.DataSource = null;
                gvAttachment.DataBind();
            }
        }

        #endregion

        #region Button Click Related Methods
        protected void btnUploadDocument_Click(object sender, EventArgs e)
        {
            _successMessage = String.Empty;
            _errorMessage = String.Empty;

            if (CheckUploadValidation())
            {
                if (hfDocumentId.Value == "0")
                {
                    String message = SaveDocumentRelatedInformation(0, "INSERT");
                    DisplayMessage(message);
                }
                if (String.IsNullOrEmpty(_errorMessage))
                {
                    String message = UploadDocumentFile();
                    LoadDocumentAfterUpload();
                    DisplayMessage(message);
                }
            }
            else
            {
                DisplayMessage(_errorMessage);
            }
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            _successMessage = String.Empty;
            _errorMessage = String.Empty;

            if (hfDocumentId.Value == "0")
            {
                String message = SaveDocumentRelatedInformation(0, "INSERT");
                DisplayMessage(message);
            }
            else
            {
                String message = SaveDocumentRelatedInformation(0, "UPDATE");
                DisplayMessage(message);
            }
            if (FileUpload1.HasFile && !String.IsNullOrEmpty(_errorMessage))
            {
                String message = UploadDocumentFile();
                LoadDocumentAfterUpload();
                DisplayMessage(message);
            }
        }

        protected void btnSendToNext_Click(object sender, EventArgs e)
        {
            _successMessage = String.Empty;
            _errorMessage = String.Empty;

            if (hfDocumentId.Value == "0")
            {
                String message = SaveDocumentRelatedInformation(1, "INSERT");
                DisplayMessage((String.IsNullOrEmpty(_errorMessage)) ? "Document has been submitted" : _errorMessage);
            }
            else
            {
                String message = SaveDocumentRelatedInformation(1, "UPDATE");
                DisplayMessage((String.IsNullOrEmpty(_errorMessage)) ? "Document has been submitted" : _errorMessage);
            }

            if (FileUpload1.HasFile && !String.IsNullOrEmpty(_errorMessage))
            {
                String message = UploadDocumentFile();
                DisplayMessage(message);
            }

            if (String.IsNullOrEmpty(_errorMessage))
            {
                Response.Redirect("~/UI/OwnDocument.aspx", false);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[4]
                {
                         _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                         _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, "Document has been decline."),
                         _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                         _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "D")
                };
                DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_WORKFLOW_ACTION", parameters);

                if (_dtReturn.Rows.Count > 0)
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString().Trim());
                    //hfControlStatus.Value = "false";
                    //ManageControlsStatus();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/DocumentSCM.aspx?ServiceID=" + hfCategoryId.Value, false);
        }

        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/OwnDocument.aspx", false);
        }

        #endregion

        #region Document Workflow Action Related Methos

        protected void LoadDropDownListRevertTo()
        {
            try
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT ServiceFlowID AS ValueField,FlowName AS DisplayField FROM dbo.ServiceFlow    ");
                sQuery.Append("WHERE ServiceID=" + hfCategoryId.Value + " AND SerialNo < (   ");
                sQuery.Append("SELECT MAX(F.SerialNo) FROM dbo.DocumentInfo D    ");
                sQuery.Append("INNER JOIN dbo.ServiceFlow F ON D.ServiceID=F.ServiceID AND D.FlowId=F.ServiceFlowID     ");
                sQuery.Append("WHERE DocumentID=" + hfDocumentId.Value + ") ORDER BY SerialNo DESC   ");

                _dataManager = new DataManager();
                DataTable dtRoleList = _dataManager.GetDataTable(sQuery.ToString());
                FillList.PopulateDropDownList(dtRoleList, ddlRevertTo, "Select Role to Revert");
            }
            catch
            {

            }
        }

        protected void UpdateDocumentInformation(String sAction)
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[9]
                {
                        _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                        _dataManager.MakeInParam("@RefDocumentID", SqlDbType.NVarChar, 500, hfRefDocumentID.Value),

                        _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                        _dataManager.MakeInParam("@ExpenseId", SqlDbType.NVarChar, 500, ddlExpenseType.SelectedValue),
                        _dataManager.MakeInParam("@ServiceID", SqlDbType.NVarChar, 500, hfCategoryId.Value),
                        _dataManager.MakeInParam("@TotalPage", SqlDbType.NVarChar, 500, 0),
                        _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarksBoss.Text),

                        _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, sAction)
                };

                DataTable _dtReturn = _dataManager.GetDataTable("SP_DOCUMENT_FLOW", parameters);
                if (_dtReturn.Rows.Count > 0)
                {
                    DisplayMessage(_dtReturn.Rows[0]["Result"].ToString().Trim());
                    BindGridViewDocumentData();

                    Response.Redirect("~/UI/Dashborad.aspx", false);
                }
                else
                {
                    hfDocumentId.Value = String.Empty;
                    DisplayMessage("Document save failed. Please try again");
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), "UpdateDocumentInformation", ex);
            }
        }

        protected void SendEmailToDocumentOwner()
        {
            try
            {
                String sQuery = "SELECT U.Email FROM dbo.Users U  INNER JOIN dbo.DocumentInfo D ON U.UserID=D.EntryBy  WHERE D.DocumentID=" + Request.QueryString["DocumentID"];

                _dataManager = new DataManager();
                DataTable dtUser = _dataManager.GetDataTable(sQuery);

                if (!String.IsNullOrEmpty(dtUser.Rows[0]["Email"].ToString()))
                {
                    EmailSender.Send("saaronno@gmail.com", "Document has been rejected");
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnWorkflowDraft_Click(object sender, EventArgs e)
        {
            UpdateDocumentInformation("DRAFT");
        }

        protected void btnWorkflowForward_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckSaveValidation() && CheckforRemarks())
                {
                    UpdateDocumentInformation("F");

                    if (hfIsApproval.Value == "1")
                    {
                        GenerateWatermark generate = new GenerateWatermark();
                        generate.AsApprove(hfDocumentId.Value);
                    }
                }
                else
                {
                    DisplayMessage(_errorMessage);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnWorkflowReject_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckValidationForRevert())
                {
                    _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[5]
                    {
                     _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                     _dataManager.MakeInParam("@RoleID", SqlDbType.NVarChar, 500, ddlRevertTo.SelectedValue),
                     _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarksBoss.Text.ToString()),
                     _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                     _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "R")
                    };
                    DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_WORKFLOW_ACTION", parameters);

                    if (_dtReturn.Rows.Count > 0)
                    {
                        DisplayMessage(_dtReturn.Rows[0]["Result"].ToString().Trim());
                        BindGridViewDocumentData();

                        SendEmailToDocumentOwner();

                        Response.Redirect("Dashborad.aspx", false);
                    }
                }
                else
                {
                    DisplayMessage(_errorMessage);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnWorkflowDecline_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckforRemarks())
                {
                    _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[4]
                    {
                     _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value),
                     _dataManager.MakeInParam("@Remarks", SqlDbType.NVarChar, 500, txtRemarksBoss.Text.ToString()),
                     _dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                     _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "D")
                    };
                    DataTable _dtReturn = _dataManager.GetDataTable("SP_SYS_WORKFLOW_ACTION", parameters);

                    if (_dtReturn.Rows.Count > 0)
                    {
                        DisplayMessage(_dtReturn.Rows[0]["Result"].ToString().Trim());
                        BindGridViewDocumentData();

                        Response.Redirect("~/UI/Dashborad.aspx", false);
                    }
                }
                else
                {
                    DisplayMessage(_errorMessage);
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void btnWorkflowBackToList_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region ModalPopupExtender Related

        protected void BindGridViewOwnDocument()
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[3]
                {
                        _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, txtSearchRefNo.Text),
                        _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "OWN")
                };
                DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_LIST", parameters);
                FillList.PopulateGridView(dtDocuments, gvDraftDocuments);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected void gvDraftDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ModalPopupExtender1.Show();
            gvDraftDocuments.PageIndex = e.NewPageIndex;
            BindGridViewOwnDocument();
            ModalPopupExtender1.Show();
        }

        protected void btnSearchRefNo_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            if (String.IsNullOrEmpty(txtSearchRefNo.Text))
            {
                txtSearchRefNo.Focus();
                DisplayMessage("Please enter document reference no.");
            }
            else
            {
                BindGridViewOwnDocument();
            }
        }

        protected void btnSearchClear_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            txtSearchRefNo.Text = String.Empty;
            BindGridViewOwnDocument();
            ModalPopupExtender1.Show();
        }

        protected void btnSelectAsset_Click(object sender, EventArgs e)
        {
            string sRefNumber = ((Button)sender).CommandArgument;
            string sDocumnetId = ((Button)sender).CommandName;
            txtRefTracking.Text = sRefNumber;
            hfRefDocumentID.Value = sDocumnetId;
        }
        #endregion

        #region Others
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
        #endregion

        #region API Integration
        protected async void btnSearch_Click(object sender, EventArgs e)
        {
            String apiEndpoint = "http://103.234.27.131:807/api/DocSoGenaration/DealerInfo?EmpID=32712";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    Uri apiUri = new Uri(apiEndpoint);
                    HttpResponseMessage response = await httpClient.GetAsync(apiUri);
                    // response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        String jsonString = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            //List<DealerInfo> data = JsonConvert.DeserializeObject<List<DealerInfo>>(jsonString);
                        }
                        else
                        {
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}