/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/11/7 13:31:49
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.DAL
{
    /// <summary>
    /// DBHelper帮助类
    /// </summary>
    public class DBHelper
    {
        private DBHelper()
        {

        }
        /// <summary>
        /// 写入数据库连接字符串
        /// </summary>
        private static string WriteConnectionString
        {
            get
            {
                string reval = ConfigurationManager.ConnectionStrings["Write_sms"].ToString();
                return reval;
            }
        }
        /// <summary>
        /// 读取数据库连接字符串
        /// </summary>
        private static string ReadConnectionString
        {
            get
            {
                string reval = ConfigurationManager.ConnectionStrings["Read_sms"].ToString();
                return reval;
            }
        }
        /// <summary>
        /// 读取数据的数据库连接
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient GetReadInstance()
        {
            var db = new SqlSugarClient(ReadConnectionString);
            db.IsEnableLogEvent = true;//Enable log events
            db.LogEventStarting = (sql, par) =>
            {
                NFine.Log.HtmlExceptionLogHelper.WriteFile(Log.HtmlExceptionLogHelper.fileTypeStatus.Sql, "执行的sql:" + sql + " 参数：" + par);
                //NFine.Log.LogHelper.GetLogManager.Debug("执行的sql:" + sql + " 参数：" + par);
            };
            return db;
        }
        /// <summary>
        /// 写入数据的数据库连接
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient GetWriteInstance()
        {
            var db = new SqlSugarClient(WriteConnectionString);
            db.IsEnableLogEvent = true;//Enable log events
            db.LogEventStarting = (sql, par) => { Console.WriteLine(sql + " " + par + "\r\n"); };
            return db;
        }
    }
}
