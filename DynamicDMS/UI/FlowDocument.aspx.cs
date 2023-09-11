using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Linq;


namespace DynamicDMS
{
    public partial class FlowDocument : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                LoadDropDownListCompany();
                LoadDropDownListExpense();
                LoadDropDownListCategory();
                LoadDropDownListStatus();

                BindGridViewWorkflowList();
            }
        }

        #region DropDownList Related

        protected void LoadDropDownListCompany()
        {
            DataTable dtComapy = PopulateLists.GetCompanies();
            FillList.PopulateDropDownList(dtComapy, ddlCompany, "Select Company");
        }

        protected void LoadDropDownListCategory()
        {
            DataTable dtServices = PopulateLists.GetDocumentCategories();
            FillList.PopulateDropDownList(dtServices, ddlCategory, "Select Category");
        }

        protected void LoadDropDownListExpense()
        {
            DataTable dtExpense = PopulateLists.GetExpenseTypes();
            FillList.PopulateDropDownList(dtExpense, ddlExpense, "Select Expense Type");
        }

        protected void LoadDropDownListStatus()
        {
            DataTable dtExpense = PopulateLists.GetStatuses();
            FillList.PopulateDropDownList(dtExpense, ddlStatus, "DisplayField", "ValueField", true, "Select Status", "99");
        }

        #endregion

        #region Page Load Related Event

        protected void BindGridViewWorkflowList()
        {
            try
            {
                String sActionType = "WORKFLOW";
                if(cbIsForwarded.Checked== true)
                {
                    sActionType = "FORWARD";
                }

                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[8]
                {
                     _dataManager.MakeInParam("@CompanyId", SqlDbType.NVarChar, 500, ddlCompany.SelectedValue),
                     _dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, ddlCategory.SelectedValue),
                     _dataManager.MakeInParam("@ExpenseId", SqlDbType.NVarChar, 500, ddlExpense.SelectedValue),
                     _dataManager.MakeInParam("@StatusId", SqlDbType.NVarChar, 500, ddlStatus.SelectedValue),
                     _dataManager.MakeInParam("@RefNo", SqlDbType.NVarChar, 500, txtRefNo.Text),
                     _dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtSearchWith.Text),
                     _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                     _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, sActionType)
                };

                DataTable dtDocuments = _dataManager.GetDataTable("SP_DOCUMENT_LIST", parameters);

                lblWorkflowCount.Text = dtDocuments.Rows.Count.ToString();
                FillList.PopulateGridView(dtDocuments, gvWorkflowDocuments);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.\n\nError:" + ex.Message);
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion

        protected void gvWorkflowDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWorkflowDocuments.PageIndex = e.NewPageIndex;
            BindGridViewWorkflowList();
        }

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridViewWorkflowList();
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Dashborad.aspx", false);
        }

        protected void gvWorkflowDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("ViewHistory"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                String sDocId = ((Label)row.FindControl("lblTranID")).Text;

                try
                {
                    DataManager _dataManager = new DataManager();
                    SqlParameter[] parameters = new SqlParameter[1]
                    {
                        _dataManager.MakeInParam("@DocumentID", SqlDbType.NVarChar, 500, sDocId)
                    };

                    DataTable dtHistory = _dataManager.GetDataTable("SP_DOCUMENT_MOVEMENT_HISTORY", parameters);
                    FillList.PopulateGridView(dtHistory, gvHistory);
                }
                catch
                {

                }

                modalExtenderHistory.Show();
            }
        }
    }
}