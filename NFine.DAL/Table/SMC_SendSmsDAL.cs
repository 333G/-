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
    /// 发送短信管理
    /// </summary>
    public partial class SMC_SendSmsDAL
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<SMC_SendSms> GetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var LoginInfo = OperatorProvider.Provider.GetCurrent();
                var queryable = db.Queryable<SMC_SendSms>().Where(t => t.F_CreatorUserId == LoginInfo.UserId);/*.Where(t => t.F_DeleteMark == false && t.F_EnabledMark == true);*/
                var queryParam = queryJson.ToJObject();
                if (!queryParam["F_MobileList"].IsEmpty())
                {
                    string F_MobileList = queryParam["F_MobileList"].ToString();
                    queryable.Where(a => a.F_MobileList.Contains(F_MobileList));
                }
                if (!queryParam["F_SmsContent"].IsEmpty())
                {
                    string F_SmsContent = queryParam["F_SmsContent"].ToString();
                    queryable.Where(a => a.F_SmsContent.Contains(F_SmsContent));
                }
                if (!queryParam["F_SendTime"].IsEmpty())//日期查询待完善！！
                {
                    DateTime? F_SendTime = Convert.ToDateTime(queryParam["F_SendTime"]);
                    queryable.Where(a => a.F_SendTime.Value.Day == F_SendTime.Value.Day);
                }
                if (!queryParam["F_SendSign"].IsEmpty())
                {
                    string F_SendSign = queryParam["F_SendSign"].ToString();
                    queryable.Where(a => a.F_SendSign == F_SendSign);
                }
                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }

        //按条件操作的时候，按照搜索条件取得List
        public List<SMC_SendSms> GetList(string queryJson)
        {
            var expression = ExtLinq.True<SMC_SendSms>();
            var queryParam = queryJson.ToJObject();

            //查询条件 
            if (!queryParam["F_UserId"].IsEmpty())
            {
                int UserId = queryParam["F_UserId"].ToInt();
                string F_ID = DAL.Sys_UserDAL.Instance.FindEntity(t => t.Id == UserId).F_Id;
                expression = expression.And(t => t.F_CreatorUserId == F_ID);
            }
            if (!queryParam["F_RootId"].IsEmpty())
            {
                int F_RootId = queryParam["F_RootId"].ToInt();
                expression = expression.And(t => t.F_RootId == F_RootId);
            }
            if (!queryParam["F_SmsContent"].IsEmpty())
            {
                string keyvalue = queryParam["F_SmsContent"].ToString();
                expression = expression.And(t => t.F_SmsContent.Contains(keyvalue));
            }
            if (!queryParam["F_SendSign"].IsEmpty())
            {
                string F_SendSign = queryParam["F_SendSign"].ToString();
                expression = expression.And(t => t.F_SendSign == F_SendSign);
            }
            if (!queryParam["F_SendState"].IsEmpty())
            {
                int F_SendState = queryParam["F_SendState"].ToInt();
                expression = expression.And(t => t.F_SendState == F_SendState);
            }
            if (!queryParam["F_DealState"].IsEmpty())
            {
                int F_DealState = queryParam["F_DealState"].ToInt();
                expression = expression.And(t => t.F_DealState == F_DealState);
            }
            if (!queryParam["F_OperateState"].IsEmpty())
            {
                int F_OperateState = queryParam["F_OperateState"].ToInt();
                expression = expression.And(t => t.F_OperateState == F_OperateState);
            }
            if (!queryParam["Operator"].IsEmpty())
            {
                string Operator = queryParam["Operator"].ToString();
                DateTime now = System.DateTime.Now;
                DateTime a = now.AddDays(-7);//前7天
                DateTime b = now.AddDays(7);//后7天
                if (Operator == "0")
                {//+-7天内记录记录
                    expression = expression.And(t => t.F_SendTime >= a);
                    expression = expression.And(t => t.F_SendTime <= b);
                }
                else
                {//7天外记录 
                    expression = expression.And(t => t.F_SendTime < a);
                    expression = expression.Or(t => t.F_SendTime > b);
                }
            }
            return DAL.SMC_SendSmsDAL.Instance.FindList(expression);
        }
        //获取当前登陆者的发送短信记录信息
        public List<SMC_SendSms> newGetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var LoginInfo = OperatorProvider.Provider.GetCurrent();
                var queryable = db.Queryable<SMC_SendSms>().Where(t => t.F_CreatorUserId == LoginInfo.UserId);
                var queryParam = queryJson.ToJObject();
                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }


    }
}
