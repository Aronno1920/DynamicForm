using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Org.BouncyCastle.Asn1.Ocsp;

namespace CoreLibrary
{
    public static class MenuDAO
    {
        public static String GetTopLevelMenuHTML(DataTable dtTopMenu)
        {
            StringBuilder sbHtml = new StringBuilder("<ul class=\"navbar-nav\">");

            foreach (DataRow topRow in dtTopMenu.Select("IsParentOnly=true"))
            {
                sbHtml.Append("<li class=\"nav-item\">");
                sbHtml.Append("<a class=\"nav-link active\" aria-current=\"page\" href=\"" + GetFullRootUrl(topRow["menu_url"].ToString()) + "\">" + topRow["menu_name"].ToString() + "</a>");
                sbHtml.Append("</li>");
            }

            foreach (DataRow topRow in dtTopMenu.Select("ParentId=0 and IsParentOnly=false"))
            {
                String sSubMenuName = "subMenu" + topRow["menu_id"].ToString();

                sbHtml.Append("<li class=\"nav-item dropdown\">");
                sbHtml.Append("<a id=\"" + sSubMenuName + "\" href=\"#\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\" class=\"nav-link dropdown-toggle\">" + topRow["menu_name"].ToString() + "</a>");
                sbHtml.Append(GetChildMenus(topRow["menu_id"].ToString(), sSubMenuName, dtTopMenu));
                sbHtml.Append("</li>");
            }

            sbHtml.Append("</ul>");
            return sbHtml.ToString();
        }

        private static String GetChildMenus(String parentID, String sSubMenuName, DataTable dtMenu)
        {
            DataRow[] childRows = dtMenu.Select("ParentId=" + parentID);
            if (childRows.Length == 0)
                return "";

            StringBuilder bldr = new StringBuilder("<ul aria-labelledby=\"" + sSubMenuName + "\" class=\"dropdown-menu border-0 shadow\">");
            foreach (DataRow crow in childRows)
            {
                bldr.Append("<li><a href=\"" + GetFullRootUrl(crow["menu_url"].ToString()) + "\" class=\"dropdown-item\">" + crow["menu_name"].ToString() + "</a></li>");
            }
            bldr.Append("</ul>");
            return bldr.ToString();
        }

        public static String GetFullRootUrl(String pagePath)
        {
            HttpRequest httpRequest = HttpContext.Current.Request;
            UriBuilder uriBuilder = new UriBuilder(httpRequest.UrlReferrer);
            uriBuilder.Query = String.Empty;

            String newRequest = uriBuilder.Uri.ToString();
            if (pagePath != "#")
                return newRequest.Replace(newRequest, "/" + pagePath);
            else
                return newRequest.Replace(newRequest, newRequest);
        }
    }
}