using CoreLibrary;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;

namespace LogicDMS.UI
{
    public partial class Category : System.Web.UI.Page
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
                BindGridViewServices();
            }
        }

        #region Page Load Related

        protected void BindGridViewServices()
        {
            try
            {
                DataManager dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                        dataManager.MakeInParam("@SearchBy", SqlDbType.NVarChar, 500, txtSearch.Text.Trim()),
                        dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LOAD")
                };

                DataTable _dtRetrun = _dataManager.GetDataTable("SP_CATEGORY", parameters);
                FillList.PopulateGridView(_dtRetrun, gvCategorys);
            }
            catch (Exception ex)
            {
                DisplayMessage("An error has been occurred. Please contact the software vendor.");
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
                    SqlParameter[] parameters = new SqlParameter[6]
                    {
                        dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, hfCategoryId.Value),
                        dataManager.MakeInParam("@CategoryName", SqlDbType.NVarChar, 500, txtCategoryName.Text.Trim()),
                        dataManager.MakeInParam("@Description", SqlDbType.NVarChar, 500, txtDescription.Text.Trim()),
                        dataManager.MakeInParam("@IsActive", SqlDbType.NVarChar, 500, (cbIsActive.Checked ? "1" : "0")),
                        dataManager.MakeInParam("@EntryBy", SqlDbType.NVarChar, 500, _user.GetCookie(CookieKey.UserId.ToString())),
                        dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, btnSave.Text)
                    };

                    DataTable _dtRerun = _dataManager.GetDataTable("SP_CATEGORY", parameters);
                    if (_dtRerun.Rows[0]["Type"].ToString() == "1")
                    {
                        DisplayMessage(_dtRerun.Rows[0]["Result"].ToString());
                        ClearAllControls();
                        BindGridViewServices();
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
                DisplayMessage("An error has been occurred. Please contact the software vendor.");
                ErrorTracking.SaveError(_user.EmployeeId, this.GetType().FullName.Replace("ASP.", "").Replace("_", "."), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        protected Boolean CheckValidation()
        {
            bool _result = true;
            if (String.IsNullOrEmpty(txtCategoryName.Text))
            {
                _validationMessage = "Please enter category name";
                _result = false;
            }           
            return _result;
        }

        protected void ClearAllControls()
        {
            txtCategoryName.Text = String.Empty;
            txtDescription.Text = String.Empty;
            cbIsActive.Checked = true;
            btnSave.Text = "Save";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllControls();
        }
        #endregion

        protected void DisplayMessage(String sMessage)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "showalert", "alert('" + sMessage + "');", true);
            return;
        }

        protected void gvCategorys_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategorys.PageIndex = e.NewPageIndex;
            BindGridViewServices();
        }

        protected void gvCategorys_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                hfCategoryId.Value = row.Cells[1].Text.Replace("&nbsp;", "");
                txtCategoryName.Text = row.Cells[2].Text.Replace("&nbsp;", "").Replace("&amp;", "&");
                txtDescription.Text = row.Cells[3].Text.Replace("&nbsp;", "").Replace("&amp;", "&");

                bool isActive = true;
                if (row.Cells[4].Text == "Inactive")
                {
                    isActive = false;
                }
                cbIsActive.Checked = isActive;
                btnSave.Text = "Update";
            }
            else if (e.CommandName.Equals("ViewRow"))
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                lblCategoryName.Text = " - "+ row.Cells[2].Text.Replace("&nbsp;", "").Replace("&amp;", "&");

                _dataManager = new DataManager();
                SqlParameter[] parameters = new SqlParameter[2]
                {
                    _dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, row.Cells[1].Text.Replace("&nbsp;", "")),
                    _dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "ServiceManPower")
                };

                DataTable _dtReturn = _dataManager.GetDataTable("SP_CATEGORY", parameters);
                if (_dtReturn.Rows.Count > 0)
                {
                    gvCategoryUser.DataSource = _dtReturn;
                    gvCategoryUser.DataBind();
                    MergeRows(gvCategoryUser, 2);
                }
                else
                {
                    gvCategoryUser.DataSource = null;
                    gvCategoryUser.DataBind();
                }
            }
        }

        public static void MergeRows(GridView gridView, int ColNumber)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                for (int i = 0; i < ColNumber; i++)
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 : previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridViewServices();
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            txtSearch.Text = String.Empty;
            BindGridViewServices();
        }
    }
}