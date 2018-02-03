using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;

namespace NFine.DAL
{
    /// <summary>
    /// 敏感词管理
    /// </summary>
    public partial class SMS_SensitiveWordsDAL
    {

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<SMS_SensitiveWords> GetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<SMS_SensitiveWords>();
                var queryParam = queryJson.ToJObject();
                if (!queryParam["F_SensitiveWords"].IsEmpty())
                {
                    string F_SensitiveWords = queryParam["F_SensitiveWords"].ToString();
                    queryable.Where(a => a.F_SensitiveWords.Contains(F_SensitiveWords));
                }
                //if (!queryParam["F_CreatorUserId"].IsEmpty())
                //{
                //    string F_CreatorUserId = queryParam["F_CreatorUserId"].ToString();
                //    queryable.Where(t => t.F_CreatorUserId == F_CreatorUserId);
                //}
                //if (!queryParam["F_Level"].IsEmpty())
                //{
                //    int F_Level = queryParam["F_Level"].ToInt();
                //    queryable.Where(t => t.F_Level == F_Level);
                //}

                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }
        /// <summary>
        /// 查询所有可用的黑白名单数组
        /// </summary>
        /// <param name="queryJson">查询</param>
        /// <returns></returns>
        public List<SMS_SensitiveWords> GetList(string F_SensitiveWords, string F_CreatorUserId)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<SMS_SensitiveWords>();
                if (!F_SensitiveWords.IsEmpty())
                {
                    queryable.Where(a => a.F_SensitiveWords == F_SensitiveWords);
                }
                if (!F_CreatorUserId.IsEmpty())
                {
                    
                    queryable.Where(t => t.F_CreatorUserId == F_CreatorUserId);
                }
                //if (!F_Level.IsEmpty())
                //{
                //    int level = F_Level.ToInt();
                //    queryable.Where(t => t.F_Level == level);
                //}
                return queryable.ToList();
            }
        }
    }
}
