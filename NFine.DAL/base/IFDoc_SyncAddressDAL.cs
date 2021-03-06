﻿// <autogenerated>
//   This file was generated by T4 code generator DALScript.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

/*-------------------------------------------------------------------------
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using NFine.Entity;
using SqlSugar;
using NFine.Code;
using System.Linq.Expressions;
using System.Data.SqlClient;

namespace NFine.DAL
{
    /// <summary>
    /// 测试
    /// </summary>
    public partial class IFDoc_SyncAddressDAL
    {
        #region 单例模式
        private static IFDoc_SyncAddressDAL instance;
        private static object _lock = new object();

        private IFDoc_SyncAddressDAL()
        {
        }

        public static IFDoc_SyncAddressDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new IFDoc_SyncAddressDAL();
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
        public object Add(IFDoc_SyncAddress model)
        {
            using (var db=DBHelper.GetWriteInstance())
            {
                return db.Insert(model);
            }
        }

        public List<object> Add(List<IFDoc_SyncAddress> entitys)
        {
            using (var db = DBHelper.GetWriteInstance())
            {
                return db.InsertRange(entitys);
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(IFDoc_SyncAddress model)
        {
            using (var db=DBHelper.GetWriteInstance())
            {
                return db.Update(model);
            }
        }
		/// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<bool> UpdateRange(List<IFDoc_SyncAddress> list)
        {
            using (var db=DBHelper.GetWriteInstance())
            {
                return db.UpdateRange(list);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<IFDoc_SyncAddress, bool>> predicate)
        {
            using (var db = DBHelper.GetWriteInstance())
            {
                return db.Delete(predicate);
            }
        }
        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="predicate">lamda表达式</param>
        /// <returns></returns>
        public IFDoc_SyncAddress FindEntity(Expression<Func<IFDoc_SyncAddress, bool>> predicate)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                return db.Queryable<IFDoc_SyncAddress>().Single(predicate);
            }
        }
        /// <summary>
        /// 直接写sql查询
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public List<IFDoc_SyncAddress> FindList(string strSql)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                return db.SqlQuery<IFDoc_SyncAddress>(strSql);
            }
        }
        /// <summary>
        /// 动态参数拼sql形式查询
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="pars">参数数组</param>
        /// <returns></returns>
        public List<IFDoc_SyncAddress> FindList(string strSql,List<SqlParameter> pars)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                return db.SqlQuery<IFDoc_SyncAddress>(strSql, pars);
            }
        }
        /// <summary>
        /// 根据查询条件，查询列表数据
        /// </summary>
        /// <param name="predicate">lamda表达式</param>
        /// <returns></returns>
        public List<IFDoc_SyncAddress> FindList(Expression<Func<IFDoc_SyncAddress, bool>> predicate)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                return db.Queryable<IFDoc_SyncAddress>().Where(predicate).ToList();
            }
        }
        /// <summary>
        /// 根据查询条件，查询列表数据，并排序
        /// </summary>
        /// <param name="predicate">lamda表达式</param>
        /// <param name="orderbyPredicate">排序lamda表达式</param>
        /// <param name="ordertype">排序方式</param>
        /// <returns></returns>
        public List<IFDoc_SyncAddress> FindList(Expression<Func<IFDoc_SyncAddress, bool>> predicate, Expression<Func<IFDoc_SyncAddress, object>> orderbyPredicate, OrderByType ordertype = OrderByType.Asc)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                return db.Queryable<IFDoc_SyncAddress>().Where(predicate).OrderBy(orderbyPredicate, ordertype).ToList();
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="pagination">分页组件</param>
        /// <returns></returns>
        public List<IFDoc_SyncAddress> FindPageList(Expression<Func<IFDoc_SyncAddress, bool>> predicate, Pagination pagination)
        {
            using (var db=DBHelper.GetReadInstance())
            {
                var tempData = db.Queryable<IFDoc_SyncAddress>().Where(predicate).OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagination">分页组件</param>
        /// <returns></returns>
        public List<IFDoc_SyncAddress> FindPageList(Pagination pagination)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var tempData = db.Queryable<IFDoc_SyncAddress>().OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }
        /// <summary>
        /// 检查是否存在记录，存在返回true,否则返回false
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exists(Expression<Func<IFDoc_SyncAddress, bool>> predicate)
        {
            using (var db=DBHelper.GetReadInstance())
            {
                return db.Queryable<IFDoc_SyncAddress>().Any(predicate);
            }
        }
    }
}
