using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using iTextSharp.text;

namespace DynamicDMS.UI
{
    public partial class MenuRole : System.Web.UI.Page
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

            if (!this.IsPostBack)
            {
                LoadDropDownListService();
            }
        }

        #region Page Load Related

        protected void LoadDropDownListService()
        {
            DataTable dtService = PopulateLists.GetDocumentCategories();
            FillList.PopulateDropDownList(dtService, ddlService, "Select Category");
        }

        protected void BindGridViewMenuList()
        {
            try
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT P.PermissionID,A.MenuId, CASE WHEN A.Parent IS NULL THEN A.Title ELSE (A.Parent +' >> '+A.Title) END MenuTitle, A.Description, A.Url,CAST((CASE WHEN P.PermissionID IS NULL THEN 'FALSE' ELSE 'TRUE' END) AS BIT) Assigned   ");
                sQuery.Append("FROM (SELECT S.ID AS MenuId, Title, Description, Url,(SELECT Title FROM dbo.Menu WHERE ID=S.ParentId) AS Parent,S.ParentId, S.SLNo   ");
                sQuery.Append("FROM dbo.Menu S  WHERE S.IsActive=1 AND S.CategoryId IS NULL AND S.ID NOT IN (SELECT ID FROM dbo.Menu WHERE ParentId=0 AND IsParentOnly=0)) A    ");
                sQuery.Append("LEFT JOIN dbo.MenuPermission P ON A.MenuId=P.MenuID AND P.FlowId=" + ddlRoleName.SelectedValue+"   ");
                sQuery.Append("ORDER BY ParentId, SLNo   ");

                _dataManager = new DataManager();
                DataTable dtMenus = _dataManager.GetDataTable(sQuery.ToString());
                FillList.PopulateGridView(dtMenus, gvMenuList);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with Software vendor.\\n \\n" + ex.Message.Replace("'", ""));
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.","").Replace("_","."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        #endregion

        #region Click Event Related

        protected void ddlService_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtRole = PopulateLists.GetFlowsByCategory(ddlService.SelectedValue);
            FillList.PopulateDropDownList(dtRole, ddlRoleName, "Select Flowpath");
        }

        protected void ddlRoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridViewMenuList();
        }

        protected void cbSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbMenuItem = (CheckBox)sender;
            GridViewRow grdrDropDownRow = (GridViewRow)cbMenuItem.Parent.Parent;

            if (cbMenuItem.Checked)
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("INSERT INTO dbo.MenuPermission(FlowId, MenuID, EntryBy, EntryDate)   ");
                sQuery.Append("VALUES ("+ ddlRoleName.SelectedValue+", "+grdrDropDownRow.Cells[1].Text+", "+1+", GETDATE())");

                _dataManager = new DataManager();
                _dataManager.ExecuteNonQuery(sQuery.ToString());
            }
            else
            {
                String sQuery = "DELETE FROM dbo.MenuPermission  WHERE PermissionID="+grdrDropDownRow.Cells[0].Text;

                _dataManager = new DataManager();
                _dataManager.ExecuteNonQuery(sQuery);
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