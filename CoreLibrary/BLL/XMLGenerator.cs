using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace CoreLibrary
{
    public static class XMLGenerator
    {
        public static string FromDataTable(DataTable dataTable, String sTableName)
        {
            StringBuilder XMLString = new StringBuilder();

            if (string.IsNullOrEmpty(dataTable.TableName) || dataTable.TableName.StartsWith("Table"))
            {
                dataTable.TableName = sTableName;
            }
            XMLString.AppendFormat("<{0}>", dataTable.TableName);

            DataColumnCollection tableColumns = dataTable.Columns;
            foreach (DataRow row in dataTable.Rows)
            {
                XMLString.AppendFormat("<RowData>");
                foreach (DataColumn column in tableColumns)
                {
                    XMLString.AppendFormat("<{1}>{0}</{1}>", row[column].ToString(), column.ColumnName);
                }
                XMLString.AppendFormat("</RowData>");
            }
            XMLString.AppendFormat("</{0}>", dataTable.TableName);
            return XMLString.ToString().Replace("&", "");
        }

        public static string FromDataTable(DataTable dataTable, String sTableName, String StatusColumnName)
        {
            StringBuilder XMLString = new StringBuilder();

            if (string.IsNullOrEmpty(dataTable.TableName) || dataTable.TableName.StartsWith("Table"))
            {
                dataTable.TableName = sTableName;
            }
            XMLString.AppendFormat("<{0}>", dataTable.TableName);

            DataColumnCollection tableColumns = dataTable.Columns;
            foreach (DataRow row in dataTable.Rows)
            {
                if (Convert.ToBoolean(row[StatusColumnName].ToString()) == true)
                {
                    XMLString.AppendFormat("<RowData>");
                    foreach (DataColumn column in tableColumns)
                    {
                        XMLString.AppendFormat("<{1}>{0}</{1}>", row[column].ToString(), column.ColumnName);
                    }
                    XMLString.AppendFormat("</RowData>");
                }
            }
            XMLString.AppendFormat("</{0}>", dataTable.TableName);
            return XMLString.ToString().Replace("&", "");
        }
    }
}