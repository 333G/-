using System;
using System.Collections.Generic;
using System.Linq;
using NFine.Entity;
using SqlSugar;
using NFine.Code;
using System.Linq.Expressions;
using System.Data.SqlClient;
using NFine.Entity.Models;

namespace NFine.DAL
{
    /// <summary>
    /// 测试
    /// </summary>
    public partial class Sys_PhoneNumAreaInfoDAL
    {
        #region 单例模式
        private static Sys_PhoneNumAreaInfoDAL instance;
        private static object _lock = new object();

        private Sys_PhoneNumAreaInfoDAL()
        {
        }

        public static Sys_PhoneNumAreaInfoDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Sys_PhoneNumAreaInfoDAL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public object Add(Sys_PhoneNumAreaInfo model)
        {
            using (var db = DBHelper.GetWriteInstance())
            {
                return db.Insert(model);
            }
        }
        public List<object> Add(List<Sys_PhoneNumAreaInfo> entitys)
        {
            using (var db = DBHelper.GetWriteInstance())
            {
                return db.InsertRange(entitys);
            }
        }
        /// <summary>
        /// 直接写sql查询
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public List<Sys_PhoneNumAreaInfo> FindList(string strSql)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                return db.SqlQuery<Sys_PhoneNumAreaInfo>(strSql);
            }
        }
    }
}
