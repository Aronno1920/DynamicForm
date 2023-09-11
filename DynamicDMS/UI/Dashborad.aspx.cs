using CoreLibrary;
using DynamicDMS.UI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicDMS
{
    public partial class Dashborad : System.Web.UI.Page
    {
        Cookie _user = new Cookie();
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
                LoadDashboardData();
            }
        }

        protected void LoadDashboardData()
        {
            try
            {
                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[1]
                {
                        _dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString()))
                };
                DataSet dsDashboard = _dataManager.GetDataSet("SP_DASHBOARD", parameters);

                if (dsDashboard.Tables[0].Rows.Count > 0)
                {
                    lbtnOwnDrafted.Text = dsDashboard.Tables[0].Rows[0]["Drafted"].ToString();
                    lbtnOwnSubmitted.Text = dsDashboard.Tables[0].Rows[0]["Submitted"].ToString();
                    lbtnOwnApproved.Text = dsDashboard.Tables[0].Rows[0]["Approved"].ToString();
                    lbtnOwnDeleted.Text = dsDashboard.Tables[0].Rows[0]["Deleted"].ToString();
                }

                if (dsDashboard.Tables[1].Rows.Count > 0)
                {
                    spanTotalDrafted.InnerText = dsDashboard.Tables[1].Rows[0]["Drafted"].ToString();
                    spanTotalSubmitted.InnerText = dsDashboard.Tables[1].Rows[0]["Submitted"].ToString();
                    spanTotalApproved.InnerText = dsDashboard.Tables[1].Rows[0]["Approved"].ToString();
                    spanTotalDeleted.InnerText = dsDashboard.Tables[1].Rows[0]["Deleted"].ToString();
                }

                if(dsDashboard.Tables[2].Rows.Count > 0)
                {
                    FillList.PopulateGridView(dsDashboard.Tables[2], gvOwnDocuments);
                }
                else
                {
                    gvOwnDocuments.DataSource = new DataTable();
                    gvOwnDocuments.DataBind();
                }

                if (dsDashboard.Tables[3].Rows.Count > 0)
                {
                    FillList.PopulateGridView(dsDashboard.Tables[3], gvWorkflowDocument);
                }
                else
                {
                    gvWorkflowDocument.DataSource = new DataTable();
                    gvWorkflowDocument.DataBind();
                }
            }
            catch(Exception ex)
            {
                DisplayMessage("An error has been occured. Please contact with software vendor.\\n \\n" + ex.Message.Replace("'", ""));
            }
        }

        protected void lbtnOwnDrafted_Click(object sender, EventArgs e)
        {
            String sUrl = "~/UI/OwnDocument.aspx?Status=0";
            Response.Redirect(sUrl);
        }

        protected void lbtnOwnSubmitted_Click(object sender, EventArgs e)
        {
            String sUrl = "~/UI/OwnDocument.aspx";
            Response.Redirect(sUrl);
        }

        protected void lbtnOwnApproved_Click(object sender, EventArgs e)
        {
            String sUrl = "~/UI/OwnDocument.aspx?Status=12";
            Response.Redirect(sUrl);
        }

        protected void lbtnOwnDeleted_Click(object sender, EventArgs e)
        {
            String sUrl = "~/UI/OwnDocument.aspx?Status=13";
            Response.Redirect(sUrl);
        }

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }
    }
}