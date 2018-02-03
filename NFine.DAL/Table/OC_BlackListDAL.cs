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
    /// 黑白名单管理
    /// </summary>
    public partial class OC_BlackListDAL
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<OC_BlackList> GetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<OC_BlackList>().Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true);
                var queryParam = queryJson.ToJObject();
                if (!queryParam["F_Mobile"].IsEmpty())
                {
                    string F_Mobile = queryParam["F_Mobile"].ToString();
                    queryable.Where(a => a.F_Mobile.Contains(F_Mobile));
                }
                if (!queryParam["F_Sign"].IsEmpty())
                {
                    bool? F_Sign = queryParam["F_Sign"].ToBool();
                    queryable.Where(t => t.F_Sign == F_Sign);
                }
                if (!queryParam["F_Level"].IsEmpty())
                {
                    int F_Level = queryParam["F_Level"].ToInt();
                    queryable.Where(t => t.F_Level == F_Level);
                }

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
        public List<OC_BlackList> GetList(string F_Mobile,string F_Sign,string F_Level)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<OC_BlackList>().Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true);
                if (!F_Mobile.IsEmpty())
                {
                    queryable.Where(a => a.F_Mobile.Contains(F_Mobile));
                }
                if (!F_Sign.IsEmpty())
                {
                    bool sign = F_Sign.ToBool();
                    queryable.Where(t => t.F_Sign == sign);
                }
                if (!F_Level.IsEmpty())
                {
                    int level = F_Level.ToInt();
                    queryable.Where(t => t.F_Level == level);
                }
                return queryable.ToList();
            }
        }
    }
}
