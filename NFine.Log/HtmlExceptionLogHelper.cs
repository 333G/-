/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/10/22 23:34:27
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Text;
using System.Web;

namespace NFine.Log
{
    /// <summary>
    /// 网页异常拦截
    /// </summary>
    public class HtmlExceptionLogHelper
    {
        public enum fileTypeStatus
        {
            Error = 1,  //1错误日志
            Debug = 2,  //2工作日志
            Info = 3, //3为临时日志
            Money = 4,    //金钱日志
            Sql=5,  //sql日志
        }

        #region 写文本文件

        /// <summary>
        /// 写文本文件
        /// </summary>
        /// <param name="fileType">文件类型（1Error，2Debug，3Info）</param>
        /// <param name="content">文件内容</param>
        public static void WriteFile(fileTypeStatus fileType, string content)
        {
            string exceLogs = "{0}{1}.txt";
            string taskLogs = "{0}{1}.txt";
            string TempLogs = "{0}{1}.txt";
            string path;
            string fileName;

            switch (fileType)
            {
                case fileTypeStatus.Error:
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory + @"\Logs\ERROR\";
                        fileName = string.Format(exceLogs, path, DateTime.Now.ToString("yyMMdd"));
                    }
                    break;

                case fileTypeStatus.Debug:
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory + @"\Logs\DEBUG\";
                        fileName = string.Format(taskLogs, path, DateTime.Now.ToString("yyMMdd"));
                    }
                    break;

                case fileTypeStatus.Info:
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory + @"\Logs\INFO\";
                        fileName = string.Format(TempLogs, path, DateTime.Now.ToString("yyMMdd"));
                    }
                    break;
                case fileTypeStatus.Sql:
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory + @"Logs\Sql\";
                        fileName = string.Format(TempLogs,path,DateTime.Now.ToString("yyMMdd"));
                    }
                    break;
                case fileTypeStatus.Money:
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory + @"\Logs\Money\";
                        fileName = string.Format(TempLogs, path, DateTime.Now.ToString("yyMMdd"));
                    }
                    break;

                default:
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory + @"\Logs\DEBUG\";
                        fileName = string.Format(TempLogs, path, DateTime.Now.ToString("yyMMdd"));
                    }
                    break;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString());

            sb.Append(string.Format("\r\n用户Agent信息：{0}", (PublicMethods.GetUserAgent == null ? "" : PublicMethods.GetUserAgent.ToString())));
            sb.Append(string.Format("\r\n客户端IP：{0}", (PublicMethods.GetUserIp == null ? "" : PublicMethods.GetUserIp.ToString())));
            sb.Append(string.Format("\r\n服务端IP：{0}", (PublicMethods.GetServerIp == null ? "" : PublicMethods.GetServerIp.ToString())));
            sb.Append(string.Format("\r\n详细信息：{0}\r\n", content));

            if (!Directory.Exists(path)) // 目录不存在则建立
                Directory.CreateDirectory(path);

            File.AppendAllText(fileName, sb.ToString());
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="objErr">错误类型</param>
        public static void SetExceptionLog(Exception objErr)
        {
            LogHelper.GetLogManager.Error(logContent(objErr, "SetExceptionLog Mothed"));
        }

        #endregion 写文本文件

        #region 用于错误日志记录

        /// <summary>
        /// 用于错误日志记录
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string logContent(Exception ex, string errorId)
        {
            StringBuilder sb = new StringBuilder();
            if (HttpContext.Current != null)
                sb.AppendFormat("\r\n错误入口:{0}", HttpContext.Current.Request.Url.ToString());
            if (!string.IsNullOrEmpty(errorId))
                sb.AppendFormat("\r\n错误Id:{0}", errorId);
            sb.AppendFormat("\r\n错误信息:{0}", ex.Message.ToString());
            sb.AppendFormat("\r\n堆栈追踪:{0}", ex.StackTrace.ToString());
            return sb.ToString();
        }

        #endregion 用于错误日志记录
    }
}
