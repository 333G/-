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
    /// 收件箱管理
    /// </summary>
    public partial class SMC_RceiveSmsDAL
    {

        public List<SMC_ReplyMessage> GetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<SMC_ReplyMessage>();
                var queryParam = queryJson.ToJObject();
                // string F_Mobile, string F_SmsContent, string GroupId, string F_RceiveTime, bool F_TA
                if (!queryParam["F_Mobile"].IsEmpty())
                {
                    string F_Mobile = queryParam["F_Mobile"].ToString();
                    queryable.Where(a => a.mobile == F_Mobile);
                }
                if (!queryParam["F_SmsContent"].IsEmpty())
                {
                    string F_SmsContent = queryParam["F_SmsContent"].ToString();
                    queryable.Where(t => t.receive_content.Contains(F_SmsContent));
                }
                /*组名查询待修改
                 * if (!queryParam["GroupId"].IsEmpty())
                 {
                     string GroupId = queryParam["GroupId"].ToString();
                     queryable.Where(t => t.GroupId == GroupId);
                 } 
                 if (!queryParam["F_TA"].IsEmpty())
                 {
                     string F_TA = queryParam["F_TA"].ToString();
                     queryable.Where(t => t.F_RceiveTime == F_SourceSms);
                 }
                 */

                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }
        /// <summary>
         /// 查询所有可用的收件箱数组
         /// </summary>
         /// <param name="queryJson">查询</param>
         /// <returns></returns>
        public List<SMC_ReplyMessage> GetList(string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<SMC_ReplyMessage>();
                var queryParam = queryJson.ToJObject();
               // string F_Mobile, string F_SmsContent, string GroupId, string F_RceiveTime, bool F_TA
                if (!queryParam["F_Mobile"].IsEmpty())
                {
                    string F_Mobile = queryParam["F_Mobile"].ToString();
                    queryable.Where(a => a.mobile == F_Mobile);
                }
                if (!queryParam["F_SmsContent"].IsEmpty())
                {
                    string F_SmsContent = queryParam["F_SmsContent"].ToString();
                    queryable.Where(t => t.receive_content == F_SmsContent);
                }
               /*组名查询待修改
                * if (!queryParam["GroupId"].IsEmpty())
                {
                    string GroupId = queryParam["GroupId"].ToString();
                    queryable.Where(t => t.GroupId == GroupId);
                } 
                if (!queryParam["F_TA"].IsEmpty())
                {
                    string F_TA = queryParam["F_TA"].ToString();
                    queryable.Where(t => t.F_RceiveTime == F_SourceSms);
                }
                */
                return queryable.ToList();
            }
        }
    }
}
