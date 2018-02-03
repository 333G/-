using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace NFine.Code
{
    public class MRequest
    {
        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static string Get(string objName)
        {
            return Utils.ChkSql(HttpContext.Current.Request[objName]);
        }

        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Get(string objName,string defaultValue)
        {
            if (HttpContext.Current.Request[objName] == null)
                return defaultValue;

            return Utils.ChkSql(HttpContext.Current.Request[objName]);
        }
        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static int GetInt(string objName)
        {
            return TypeParse.StrToInt(HttpContext.Current.Request[objName], 0);
        }

        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(string objName, int defaultValue)
        {
            return TypeParse.StrToInt(HttpContext.Current.Request[objName], defaultValue);
        }
        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string objName)
        {
            return TypeParse.StrToDecimal(HttpContext.Current.Request[objName], 0);
        }
        /// <summary>
        /// 取得get值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string objName)
        {
            return TypeParse.StrToDataTime(HttpContext.Current.Request[objName], DateTime.Now);
        }
        /// <summary>
        /// 取得多个值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static string[] GetArray(string objName)
        {
            if (HttpContext.Current.Request[objName] == null) return null;
            if (string.IsNullOrEmpty(Get(objName))) return new string[] { };
            return Get(objName).Split(',');
        }
        /// <summary>
        /// 取得bool值
        /// </summary>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static bool GetBoolean(string objName) {
            if (HttpContext.Current.Request[objName] == null) return false;
            return Get(objName).ToLower() == "true" || Get(objName).ToLower() == "1" ? true : false;
        }
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        public static bool IsPost {
            get {
                return HttpContext.Current.Request.HttpMethod.Equals("POST");
            }
        }
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        public static bool IsGet
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod.Equals("GET");
            }
        }
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIp()
        {
            var result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(result) || !Utils.IsIp(result))
                return "127.0.0.1";

            return result;
        }
        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            var urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// 获得当前页面绝对路径
        /// </summary>
        /// <returns></returns>
        public static string GetAbsolutePath()
        {
            return HttpContext.Current.Request.Url.AbsolutePath;
        }

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }

        /// <summary>
        /// 取得post过来的json对象并转换成字符串
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static string GetJson(Stream inputStream)
        {
            if (inputStream.Length <= 0) return null;
            var streamReader = new StreamReader(inputStream);
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// 保存Request文件
        /// </summary>
        /// <param name="path">保存到文件名</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0) HttpContext.Current.Request.Files[0].SaveAs(path);
        }
    }
}
