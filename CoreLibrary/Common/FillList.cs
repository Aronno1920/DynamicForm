using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CoreLibrary
{
    public static class FillList
    {
        public static void PopulateDropDownList(DataTable DropDownListData, DropDownList DropDownListId, String DisplayField, String ValueField)
        {
            DataTable dtDropDown = new DataTable();
            dtDropDown.Columns.Add(new DataColumn("ItemValue", typeof(string)));
            dtDropDown.Columns.Add(new DataColumn("ItemText", typeof(string)));

            foreach (DataRow myRow in DropDownListData.Rows)
            {
                DataRow dRow = dtDropDown.NewRow();
                dRow["ItemValue"] = myRow[ValueField];
                dRow["ItemText"] = myRow[DisplayField];
                dtDropDown.Rows.Add(dRow);
            }
            DropDownListId.DataSource = dtDropDown;
            DropDownListId.DataTextField = "itemText";
            DropDownListId.DataValueField = "itemValue";
            DropDownListId.DataBind();
        }

        public static void PopulateDropDownList(DataTable DropDownListData, DropDownList DropDownListId, String DisplayField, String ValueField, Boolean IsExtraTop, String TopText, String TopValue)
        {
            DataTable dtDropDown = new DataTable();
            dtDropDown.Columns.Add(new DataColumn("ItemValue", typeof(string)));
            dtDropDown.Columns.Add(new DataColumn("ItemText", typeof(string)));

            if (IsExtraTop == true)
            {
                DataRow dRow = dtDropDown.NewRow();
                try
                {
                    dRow["ItemValue"] = TopValue;
                }
                catch (Exception)
                {
                    dRow["ItemValue"] = "-1";
                }

                try
                {
                    dRow["itemText"] = TopText;
                }
                catch (Exception)
                {
                    dRow["itemText"] = "";
                }
                dtDropDown.Rows.Add(dRow);
            }

            // add list contents from the Dataset
            foreach (DataRow myRow in DropDownListData.Rows)
            {
                DataRow dRow = dtDropDown.NewRow();
                dRow["ItemValue"] = myRow[ValueField];
                dRow["ItemText"] = myRow[DisplayField];
                dtDropDown.Rows.Add(dRow);
            }
            DropDownListId.DataSource = dtDropDown;
            DropDownListId.DataTextField = "itemText";
            DropDownListId.DataValueField = "itemValue";
            DropDownListId.DataBind();
        }

        public static void PopulateDropDownList(DataTable DropDownListData, DropDownList DropDownListId, String TopText)
        {
            DataTable dtDropDown = new DataTable();
            dtDropDown.Columns.Add(new DataColumn("ItemValue", typeof(string)));
            dtDropDown.Columns.Add(new DataColumn("ItemText", typeof(string)));

            DataRow dRow = dtDropDown.NewRow();
            dRow["ItemValue"] = "0";
            dRow["itemText"] = TopText;
            dtDropDown.Rows.Add(dRow);

            foreach (DataRow myRow in DropDownListData.Rows)
            {
                dRow = dtDropDown.NewRow();
                dRow["ItemValue"] = myRow["ValueField"];
                dRow["ItemText"] = myRow["DisplayField"];
                dtDropDown.Rows.Add(dRow);
            }
            DropDownListId.DataSource = dtDropDown;
            DropDownListId.DataTextField = "itemText";
            DropDownListId.DataValueField = "itemValue";
            DropDownListId.DataBind();
        }

        public static void PopulateRadioButtonList(DataTable DropDownListData, RadioButtonList RadioButtonListId)
        {
            DataTable dtDropDown = new DataTable();
            dtDropDown.Columns.Add(new DataColumn("ItemValue", typeof(string)));
            dtDropDown.Columns.Add(new DataColumn("ItemText", typeof(string)));

            foreach (DataRow myRow in DropDownListData.Rows)
            {
                DataRow dRow = dtDropDown.NewRow();
                dRow["ItemValue"] = myRow["ValueField"];
                dRow["ItemText"] = myRow["DisplayField"];
                dtDropDown.Rows.Add(dRow);
            }
            RadioButtonListId.DataSource = dtDropDown;
            RadioButtonListId.DataTextField = "itemText";
            RadioButtonListId.DataValueField = "itemValue";
            RadioButtonListId.DataBind();
        }


        public static void PopulateGridView(DataTable GridViewData, GridView GridViewId)
        {
            if (GridViewData.Rows.Count == 0)
            {
                GridViewData = new DataTable();
            }

            GridViewId.DataSource = GridViewData;
            GridViewId.DataBind();
        }
    }
}