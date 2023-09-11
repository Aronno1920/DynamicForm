using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreLibrary;


namespace DynamicDMS.UI
{
    public partial class PrintPreview : System.Web.UI.Page
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

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["DocumentID"]))
                {
                    hfDocumentId.Value = Request.QueryString["DocumentID"];
                    LoadDocumentInformation();
                }
            }
        }

        protected void LoadDocumentInformation()
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[1]
                {
                  _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, hfDocumentId.Value)
                };

                DataSet ds = _dataManager.GetDataSet("SP_SELECT_DOCUMENT", parameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    hfServiceId.Value = ds.Tables[0].Rows[0]["ServiceID"].ToString();
                    txtCompany.Text = ds.Tables[0].Rows[0]["ComapnyName"].ToString();
                    txtItemType.Text = ds.Tables[0].Rows[0]["ExpenseType"].ToString();
                    txtEntryDate.Text = ds.Tables[0].Rows[0]["EntryDate"].ToString();
                    txtBillRefNo.Text = ds.Tables[0].Rows[0]["BillREfNo"].ToString();
                    txtRefTracking.Text = ds.Tables[0].Rows[0]["RefDocumentNo"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();


                    txtPONo.Text = ds.Tables[0].Rows[0]["PONO"].ToString();
                    txtPINumber.Text = ds.Tables[0].Rows[0]["PINo"].ToString();
                    txtMRRNo.Text = ds.Tables[0].Rows[0]["MRRNo"].ToString();
                    txtChallanNo.Text = ds.Tables[0].Rows[0]["ChallanNo"].ToString();

                    txtBillNo.Text = ds.Tables[0].Rows[0]["PartyBillNo"].ToString();
                    txtBillAmount.Text = ds.Tables[0].Rows[0]["BillAmount"].ToString();
                    txtAuditAmount.Text = ds.Tables[0].Rows[0]["DiscountAmount"].ToString();
                    txtFinalAmount.Text = ds.Tables[0].Rows[0]["FinalAmount"].ToString();
                }

                if (ds.Tables[1].Rows.Count > 0)    //DocumentComments
                {
                    gvComment.DataSource = ds.Tables[1];
                    gvComment.DataBind();
                }
                else
                {
                    gvComment.DataSource = null;
                }

                if (ds.Tables[2].Rows.Count > 0)    //Document
                {
                    gvAttachment.DataSource = ds.Tables[2];
                    gvAttachment.DataBind();
                }
                else
                {
                    gvAttachment.DataSource = null;
                    gvAttachment.DataBind();
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #region Others
        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
        #endregion

        protected void gBtnPreview_Click(object sender, ImageClickEventArgs e)
        {
            //SectionVisibility activity = new SectionVisibility(hfServiceId.Value, hfDocumentId.Value);
            //if (activity.IsEnableDownload)
            //{
            //    ImageButton imDownload = (ImageButton)sender;
            //    GridViewRow row = ((GridViewRow)imDownload.Parent.Parent);
            //    int transID = Convert.ToInt32(row.Cells[0].Text);
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('Viewer.aspx?id=" + transID.ToString() + "');", true);
            //}
            //else
            //{
            //    DisplayMessage("Sorry! You don't have permission to download attachment.");
            //}
        }
    }
}