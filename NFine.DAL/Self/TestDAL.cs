/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/11/10 9:41:37
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFine.Entity;
using SqlSugar;
using NFine.Code;
using System.Linq.Expressions;

namespace NFine.DAL.Self
{
    public class TestDAL
    {
        #region 单例模式
        private static TestDAL instance;
        private static object _lock = new object();

        private TestDAL()
        {
        }

        public static TestDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new TestDAL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        /// <summary>
        /// 多表联查的情况
        /// </summary>
        /// <returns></returns>
        public List<Entity.Views.VTest> MultipleTable()
        {
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT SMS_SubTest.*,SMS_Test.TestName,SMS_Test.TestDescription FROM SMS_SubTest JOIN SMS_Test ");
                strSql.Append("ON SMS_Test.ID = SMS_SubTest.Test_Id");
                return db.SqlQuery<Entity.Views.VTest>(strSql.ToString());
            }
        }
        /// <summary>
        /// 根据查询条件，查询列表数据，并排序
        /// </summary>
        /// <param name="predicate">lamda表达式</param>
        /// <returns></returns>
        public List<Entity.Views.VTest> FindList(Expression<Func<Entity.Views.VTest, bool>> predicate, Expression<Func<Entity.Views.VTest, object>> orderbyPredicate, OrderByType ordertype = OrderByType.Asc)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                return db.Queryable<Entity.Views.VTest>().Where(predicate).OrderBy(orderbyPredicate, ordertype).ToList();
            }
        }
        public List<Entity.Views.VTest> MultipleTableAndOrderBy()
        {
            using (var db = DBHelper.GetReadInstance())
            {
                return db.Queryable<Entity.Views.VTest>().Where(a => a.TestName == "aaaa").OrderBy(a => a.ID, OrderByType.Asc).ToList();
            }
        }
    }
}
