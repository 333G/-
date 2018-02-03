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
    /// 自动免审模版
    /// </summary>
    public partial class OC_AutoReviewTempleteDAL
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<OC_AutoReviewTemplete> GetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            { 
                var queryable = db.Queryable<OC_AutoReviewTemplete>().Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true);
                var queryParam = queryJson.ToJObject();
                if (!queryParam["F_UserID"].IsEmpty())
                {
                    int F_UserID = queryParam["F_UserID"].ToInt();
                    queryable.Where(a => a.F_UserID == F_UserID);
                }
                if (!queryParam["F_RootID"].IsEmpty())
                {
                    int F_RootID = queryParam["F_RootID"].ToInt();
                    queryable.Where(t => t.F_RootID == F_RootID);
                }
                if (!queryParam["F_ParentID"].IsEmpty())
                {
                    string F_ParentID = queryParam["F_ParentID"].ToString();
                    queryable.Where(t => t.F_ParentID == F_ParentID);
                }
                if (!queryParam["F_SourceSms"].IsEmpty())
                {
                    string F_SourceSms = queryParam["F_SourceSms"].ToString();
                    queryable.Where(t => t.F_SourceSms == F_SourceSms);
                }

                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }
        /// <summary>
        /// 查询所有可用的自动免审模板数组
        /// </summary>
        /// <param name="queryJson">查询</param>
        /// <returns></returns>
        public List<OC_AutoReviewTemplete> GetList(string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<OC_AutoReviewTemplete>();//.Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true);
                 var queryParam =queryJson.ToJObject();
                 
                if (!queryParam["F_UserID"].IsEmpty())
                {
                    int F_UserID = queryParam["F_UserID"].ToInt();
                    queryable.Where(a => a.F_UserID == F_UserID);
                }
                if (!queryParam["F_RootID"].IsEmpty())
                {
                    int F_RootID = queryParam["F_RootID"].ToInt();
                    queryable.Where(t => t.F_RootID == F_RootID);
                }
                if (!queryParam["F_ParentID"].IsEmpty())
                {
                    string F_ParentID = queryParam["F_ParentID"].ToString();
                    queryable.Where(t => t.F_ParentID == F_ParentID);
                }
                if (!queryParam["F_SourceSms"].IsEmpty())
                {
                    string F_SourceSms = queryParam["F_SourceSms"].ToString();
                    queryable.Where(t => t.F_SourceSms == F_SourceSms);
                }
                return queryable.ToList();
            }
        }
    }
}
