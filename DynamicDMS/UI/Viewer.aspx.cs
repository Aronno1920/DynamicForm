using CoreLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DynamicDMS.UI;
using System.Configuration;
using System.Text;

namespace DynamicDMS.UI
{
    public partial class Viewer : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["FileTitle"] != null)
            {
                this.Title = Session["FileTitle"].ToString();
                Page.Title = Session["FileTitle"].ToString();
            }
            
            if (!IsPostBack)
            {
                LoadFileInformation();
            }
        }

        private void LoadFileInformation()
        {
            try
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.Append("SELECT DD.Name, DD.ContentType,DI.IsApproved FROM DocumentData DD   ");
                sQuery.Append("INNER JOIN dbo.DocumentInfo DI ON DD.DocumentID=DI.DocumentID   ");
                sQuery.Append("WHERE DD.TransID=" + Convert.ToInt32(Request.QueryString["id"]));


                DataManager _dataManager = new DataManager();
                DataTable dtFile = _dataManager.GetDataTable(sQuery.ToString());
                if (dtFile.Rows.Count > 0)
                {
                    String sFolderName = String.Empty;
                    if (dtFile.Rows[0]["IsApproved"].ToString() == "1" || dtFile.Rows[0]["IsApproved"].ToString() =="True")
                    {
                        sFolderName = ConfigurationManager.AppSettings["ApproveFolder"].ToString();
                    }
                    else
                    {
                        sFolderName = ConfigurationManager.AppSettings["DraftFolder"].ToString();
                    }

                    String filePath = Server.MapPath("~/"+ sFolderName + "/") + dtFile.Rows[0]["Name"].ToString();
                    if (File.Exists(filePath))
                    {
                        this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        this.Response.ContentType = dtFile.Rows[0]["ContentType"].ToString();
                        this.Response.AppendHeader("Content-Disposition;", "attachment;filename=" + filePath);
                        this.Response.WriteFile(filePath);
                        this.Response.Flush();
                        this.Response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message.ToString();
            }
        }
    }
}