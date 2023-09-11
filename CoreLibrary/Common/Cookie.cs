using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CoreLibrary
{
    public class Cookie
    {
        private String name = "UserInfo";
        private const Int32 cookieExpireTime = 1;

        #region Private Properties
        private HttpContext Context
        {
            get { return HttpContext.Current; }
        }
        public void SetCookie(String name, object value)
        {
            int ExpireTime = cookieExpireTime;
            HttpCookie cookie = Context.Request.Cookies[name];
            if (cookie == null)
            {
                cookie = new HttpCookie(name);
            }
            cookie.Value = Convert.ToString(value);
            cookie.Expires = DateTime.Now.AddDays(ExpireTime);
            Context.Response.Cookies.Add(cookie);
        }
        public String GetCookie(String name)
        {
            HttpCookie cookie = Context.Request.Cookies[name];
            if (cookie == null)
            {
                return "0";
            }
            return cookie.Value;
        }
        public void ClearCookie()
        {
            HttpCookie myCookie = Context.Request.Cookies[name];
            if (myCookie != null)
            {
                myCookie.Expires = DateTime.Now.AddDays(-1);
                Context.Response.Cookies.Add(myCookie);
            }
        }
        public void RemoveCookie()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = Context.Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Context.Request.Cookies[i].Name;
                if (cookieName.ToString().ToLower().Contains(name))
                {
                    aCookie = new HttpCookie(cookieName);
                    aCookie.Expires = DateTime.Now.AddDays(-1);
                    Context.Response.Cookies.Add(aCookie);
                }
            }
        }
        #endregion

        #region Public Properties
        
        public String UserId
        {
            get
            {
                string cookieValue = GetCookie(CookieKey.UserId.ToString());

                if (cookieValue == string.Empty)
                {
                    if (Context.Request["q_app"] == null)
                        return string.Empty;
                    else
                    {
                        SetCookie("application", Convert.ToString(Context.Request["q_app"]));
                        return Convert.ToString(Context.Request["q_app"]);
                    }
                }
                else
                {
                    return cookieValue;
                }
            }
            set
            {
                SetCookie("application", value);
            }
        }

        public String EmployeeId
        {
            get
            {
                string cookieValue = GetCookie(CookieKey.EmployeeId.ToString());

                if (cookieValue == string.Empty)
                {
                    if (Context.Request["q_app"] == null)
                        return string.Empty;
                    else
                    {
                        SetCookie("application", Convert.ToString(Context.Request["q_app"]));
                        return Convert.ToString(Context.Request["q_app"]);
                    }
                }
                else
                {
                    return cookieValue;
                }
            }
            set
            {
                SetCookie("application", value);
            }
        }

        public String GroupId
        {
            get
            {
                string cookieValue = GetCookie(CookieKey.GroupId.ToString());

                if (cookieValue == string.Empty)
                {
                    if (Context.Request["q_app"] == null)
                        return string.Empty;
                    else
                    {
                        SetCookie("application", Convert.ToString(Context.Request["q_app"]));
                        return Convert.ToString(Context.Request["q_app"]);
                    }
                }
                else
                {
                    return cookieValue;
                }
            }
            set
            {
                SetCookie("application", value);
            }
        }

        public String RoleID
        {
            get
            {
                string cookieValue = GetCookie(CookieKey.RoleId.ToString());

                if (cookieValue == string.Empty)
                {
                    if (Context.Request["q_app"] == null)
                        return string.Empty;
                    else
                    {
                        SetCookie("application", Convert.ToString(Context.Request["q_app"]));
                        return Convert.ToString(Context.Request["q_app"]);
                    }
                }
                else
                {
                    return cookieValue;
                }
            }
            set
            {
                SetCookie("application", value);
            }
        }

        public String DepartmentId
        {
            get
            {
                string cookieValue = GetCookie(CookieKey.DepartmentId.ToString());

                if (cookieValue == string.Empty)
                {
                    if (Context.Request["q_app"] == null)
                        return string.Empty;
                    else
                    {
                        SetCookie("application", Convert.ToString(Context.Request["q_app"]));
                        return Convert.ToString(Context.Request["q_app"]);
                    }
                }
                else
                {
                    return cookieValue;
                }
            }
            set
            {
                SetCookie("application", value);
            }
        }

        public String CompanyId
        {
            get
            {
                string cookieValue = GetCookie(CookieKey.CompanyId.ToString());

                if (cookieValue == string.Empty)
                {
                    if (Context.Request["q_app"] == null)
                        return string.Empty;
                    else
                    {
                        SetCookie("application", Convert.ToString(Context.Request["q_app"]));
                        return Convert.ToString(Context.Request["q_app"]);
                    }
                }
                else
                {
                    return cookieValue;
                }
            }
            set
            {
                SetCookie("application", value);
            }
        }

        public String UserName
        {
            get
            {
                string cookieValue = GetCookie(CookieKey.UserName.ToString());

                if (cookieValue == string.Empty)
                {
                    if (Context.Request["q_app"] == null)
                        return string.Empty;
                    else
                    {
                        SetCookie("application", Convert.ToString(Context.Request["q_app"]));
                        return Convert.ToString(Context.Request["q_app"]);
                    }
                }
                else
                {
                    return cookieValue;
                }
            }
            set
            {
                SetCookie("application", value);
            }
        }

        public String RoleName
        {
            get
            {
                string cookieValue = GetCookie(CookieKey.RoleName.ToString());

                if (cookieValue == string.Empty)
                {
                    if (Context.Request["q_app"] == null)
                        return string.Empty;
                    else
                    {
                        SetCookie("application", Convert.ToString(Context.Request["q_app"]));
                        return Convert.ToString(Context.Request["q_app"]);
                    }
                }
                else
                {
                    return cookieValue;
                }
            }
            set
            {
                SetCookie("application", value);
            }
        }
        #endregion
    }

    public enum CookieKey { UserId, EmployeeId, GroupId, RoleId, DepartmentId, CompanyId, UserName, RoleName};
}


