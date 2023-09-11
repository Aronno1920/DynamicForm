using System;
using System.Data;
using System.Data.SqlClient;


namespace CoreLibrary
{
    public static class PopulateLists
    {
        #region Input Typies
        public static DataTable GetInputTypies()
        {
            DataTable dtFieldType = new DataTable();
            dtFieldType.Columns.Add("ValueField", typeof(String));
            dtFieldType.Columns.Add("DisplayField", typeof(String));

            dtFieldType.Rows.Add("Panel", "Panel");
            dtFieldType.Rows.Add("TextBox", "TextBox");
            dtFieldType.Rows.Add("TextArea", "TextArea");
            dtFieldType.Rows.Add("DatePicker", "DatePicker");
            dtFieldType.Rows.Add("CheckBox", "CheckBox");
            dtFieldType.Rows.Add("RadioButtonList", "RadioButtonList");
            dtFieldType.Rows.Add("DropDownList", "DropDownList");

            return dtFieldType;
        }
        #endregion

        #region Category, Flow
        public static DataTable GetDocumentCategories()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Category")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }

        public static DataTable GetFlowsByCategory(String CategoryId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[2]
            {
                dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, CategoryId),
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "FlowByCategory")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }
        #endregion
        
        #region Document, Expense, Currency and Status Related
        public static DataTable GetAttachmentTypes()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "AttachmentType")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }

        public static DataTable GetExpenseTypes()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "ExpenseType")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }

        public static DataTable GetCurrencys()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Currency")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }

        public static DataTable GetStatuses()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Status")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }
        #endregion

        #region LookupType and Lookup Related
        public static DataTable GetLookupTypes()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "LookupType")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }
        #endregion

        #region Company, Department, Location Related
        public static DataTable GetCompanies()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Company")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }

        public static DataTable GetCompaniesByUser_Category(String sUserId, String sCategoryId)
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[3]
            {
                dataManager.MakeInParam("@UserId", SqlDbType.NVarChar, 500, sUserId),
                dataManager.MakeInParam("@CategoryId", SqlDbType.NVarChar, 500, sCategoryId),
                dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "CompanyByUser_Category")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }

        public static DataTable GetDepartments()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Department")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }

        public static DataTable GetLocations()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Location")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }

        public static DataTable GetDesignations()
        {
            DataManager dataManager = new DataManager();
            SqlParameter[] parameters = new SqlParameter[1]
            {
                     dataManager.MakeInParam("@Action", SqlDbType.NVarChar, 500, "Designation")
            };
            return dataManager.GetDataTable("SP_POPULATE_LIST", parameters);
        }
        #endregion
    }
}