/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/10/24 21:55:39
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using System;
using System.Web;

namespace NFine.Log
{
    class PublicMethods
    {
        #region 获取IP

        /// <summary>
        /// 获取服务端IP
        /// </summary>
        public static string GetServerIp
        {
            get
            {
                string serverIP = "未获取服务器IP";
                try
                {
                    serverIP = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
                }
                catch { }

                return serverIP;
            }
        }

        #region 获取客户端IP

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        public static string GetUserIp
        {
            get
            {
                string userIP = "未获取用户IP";

                try
                {
                    if (HttpContext.Current == null
                || HttpContext.Current.Request == null
                || HttpContext.Current.Request.ServerVariables == null)
                        return "";

                    string CustomerIP = "";

                    //CDN加速后取到的IP simone 090805
                    CustomerIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                    if (!string.IsNullOrEmpty(CustomerIP))
                    {
                        return CustomerIP;
                    }

                    CustomerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (!String.IsNullOrEmpty(CustomerIP))
                        return CustomerIP;

                    if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    {
                        CustomerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (CustomerIP == null)
                            CustomerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                    else
                    {
                        CustomerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }

                    if (string.Compare(CustomerIP, "unknown", true) == 0)
                        return HttpContext.Current.Request.UserHostAddress;
                    return CustomerIP;
                }
                catch { }

                return userIP;
            }
        }

        #endregion 获取客户端IP

        #endregion 获取IP

        #region 获取用户Agent

        public static string GetUserAgent
        {
            get
            {
                string userAgent = "未获取用户Agent";
                try
                {
                    userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
                }
                catch { }

                return userAgent;
            }
        }

        #endregion 获取用户Agent

        #region 获取浏览器信息
        /// <summary>
        /// 获取浏览器信息
        /// </summary>
        public static string Browser
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;
                var browser = HttpContext.Current.Request.Browser;
                return string.Format("{0} {1}", browser.Browser, browser.Version);
            }
        }
        #endregion
    }
}
