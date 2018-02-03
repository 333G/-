/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/10/24 21:54:36
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Web;

namespace NFine.Log
{
    class LogManager : Ilog
    {
        private log4net.ILog log;

        public LogManager()
        {
            log = log4net.LogManager.GetLogger("WebLogger");
            if (!log.IsWarnEnabled && !log.IsDebugEnabled && !log.IsErrorEnabled && !log.IsInfoEnabled && !log.IsFatalEnabled)
            {
                this.LoadLog4netConfig();
            }
        }

        public static void Loadlog4net()
        {
            log4net.LogManager.GetLogger("WebLogger");
        }

        #region GetLog 提供对ILog本身的访问

        //public log4net.ILog GetLog
        //{
        //    get { return log; }
        //}

        #endregion GetLog 提供对ILog本身的访问

        #region Debug

        public void Debug(string message)
        {
            message = this.GetMessage(message);
            log.Debug(message);
        }

        public void Debug(string message, Exception ex)
        {
            message = this.GetMessage(message, ex);
            log.Debug(message, ex);
        }

        #endregion Debug

        #region Error

        public void Error(string message)
        {
            message = this.GetMessage(message);
            log.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            message = this.GetMessage(message, ex);
            log.Error(message, ex);
        }

        #endregion Error

        #region Fatal

        public void Fatal(string message)
        {
            message = this.GetMessage(message);
            log.Fatal(message);
        }

        public void Fatal(string message, Exception ex)
        {
            message = this.GetMessage(message, ex);
            log.Fatal(message, ex);
        }

        #endregion Fatal

        #region Info

        public void Info(string message)
        {
            message = this.GetMessage(message);
            log.Info(message);
        }

        public void Info(string message, Exception ex)
        {
            message = this.GetMessage(message, ex);
            log.Info(message, ex);
        }

        #endregion Info

        #region Warn

        public void Warn(string message)
        {
            message = this.GetMessage(message);
            log.Warn(message);
        }

        public void Warn(string message, Exception ex)
        {
            message = this.GetMessage(message, ex);
            log.Warn(message, ex);
        }

        #endregion Warn
        
        #region 构造Message

        private string GetMessage(string message)
        {
            return string.Format(" 用户Ip:{0} \r\n 服务器Ip:{1} \r\n 客户端信息:{2} \r\n 浏览器信息:{3}\r\n 错误描述:{4} ",
                PublicMethods.GetUserIp, PublicMethods.GetServerIp, GetUserAgent, PublicMethods.Browser, message);
        }

        private string GetMessage(string message, Exception ex)
        {
            return string.Format(" 用户Ip:{0} \r\n 服务器Ip:{1} \r\n 客户端信息:{2} \r\n 错误页面：{3} \r\n 浏览器信息:{4}\r\n 错误描述:{5} ",
                PublicMethods.GetUserIp, PublicMethods.GetServerIp,
                GetUserAgent, System.Web.HttpContext.Current == null ? "" : System.Web.HttpContext.Current.Request.Url.ToString(),
                PublicMethods.Browser, message); ;
        }

        #endregion 构造Message

        #region 获取用户Agent

        private string GetUserAgent
        {
            get
            {
                string userAgent = "未获取用户Agent";
                try
                {
                    userAgent = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
                }
                catch { }

                return userAgent;
            }
        }

        #endregion 获取用户Agent

        public void LoadLog4netConfig()
        {
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml("<?xml version='1.0' encoding='utf-8'?><configuration><log4net><root><level value='ALL'/><appender-ref ref='SysAppenderDEBUG'/><appender-ref ref='SysAppenderINFO'/><appender-ref ref='SysAppenderERROR'/><appender-ref ref='SysAppenderFATAL'/><appender-ref ref='SysAppenderWARN'/></root><logger name='WebLogger'><level value='DEBUG'/></logger><appender name='SysAppenderDEBUG' type='log4net.Appender.RollingFileAppender' ><param name='File' value='Logs/DEBUG/' /><param name='AppendToFile' value='true' /><param name='RollingStyle' value='Date' /><param name='DatePattern' value='&quot;Logs_&quot;yyyyMMdd&quot;.txt&quot;' /><param name='StaticLogFileName' value='false' /><layout type='log4net.Layout.PatternLayout,log4net'><conversionPattern value='记录时间：%date 线程ID:[%thread]  错误描述：%message%newline'/> </layout><filter type='log4net.Filter.LevelRangeFilter'><param name='LevelMin' value='DEBUG' /><param name='LevelMax' value='DEBUG' /></filter></appender><appender name='SysAppenderINFO' type='log4net.Appender.RollingFileAppender,log4net' ><param name='File' value='Logs/INFO/' /><param name='AppendToFile' value='true' /> <param name='RollingStyle' value='Date' /><param name='DatePattern' value='&quot;Logs_&quot;yyyyMMdd&quot;.txt&quot;' /><param name='StaticLogFileName' value='false' /><layout type='log4net.Layout.PatternLayout,log4net'><conversionPattern value='记录时间：%date 线程ID:[%thread] 日志级别：%-5level  错误描述：%message%newline'/></layout><filter type='log4net.Filter.LevelRangeFilter'><param name='LevelMin' value='INFO' /><param name='LevelMax' value='INFO' /></filter></appender><appender name='SysAppenderERROR' type='log4net.Appender.RollingFileAppender,log4net' ><param name='File' value='Logs/ERROR/' /><param name='AppendToFile' value='true' /><param name='RollingStyle' value='Date' /><param name='DatePattern' value='&quot;Logs_&quot;yyyyMMdd&quot;.txt&quot;' /><param name='StaticLogFileName' value='false' /><layout type='log4net.Layout.PatternLayout,log4net'><conversionPattern value='记录时间：%date 线程ID:[%thread] 日志级别：%-5level  错误描述：%message%newline'/> </layout><filter type='log4net.Filter.LevelRangeFilter'><param name='LevelMin' value='ERROR' /><param name='LevelMax' value='ERROR' /></filter></appender><appender name='SysAppenderFATAL' type='log4net.Appender.RollingFileAppender,log4net' ><param name='File' value='Logs/FATAL/' /><param name='AppendToFile' value='true' /><param name='RollingStyle' value='Date' /><param name='DatePattern' value='&quot;Logs_&quot;yyyyMMdd&quot;.txt&quot;' /><param name='StaticLogFileName' value='false' /><layout type='log4net.Layout.PatternLayout,log4net'><conversionPattern value='记录时间：%date 线程ID:[%thread] 日志级别：%-5level  错误描述：%message%newline'/></layout><filter type='log4net.Filter.LevelRangeFilter'><param name='LevelMin' value='FATAL' /><param name='LevelMax' value='FATAL' /></filter></appender><appender name='SysAppenderWARN' type='log4net.Appender.RollingFileAppender,log4net' ><param name='File' value='Logs/WARN/' /><param name='AppendToFile' value='true' /><param name='RollingStyle' value='Date' /><param name='DatePattern' value='&quot;Logs_&quot;yyyyMMdd&quot;.txt&quot;' /><param name='StaticLogFileName' value='false' /><layout type='log4net.Layout.PatternLayout,log4net'><conversionPattern value='记录时间：%date 线程ID:[%thread] 日志级别：%-5level  错误描述：%message%newline'/></layout><filter type='log4net.Filter.LevelRangeFilter'><param name='LevelMin' value='WARN' /><param name='LevelMax' value='WARN' /></filter></appender></log4net></configuration>");
            //XmlNodeList list = doc.GetElementsByTagName("log4net");
            //log4net.Config.XmlConfigurator.Configure((XmlElement)doc.GetElementsByTagName("log4net")[0]);


            FileInfo configFile = new FileInfo(HttpContext.Current.Server.MapPath("/Configs/log4net.config"));
            log4net.Config.XmlConfigurator.Configure(configFile);
        }
    }
}
