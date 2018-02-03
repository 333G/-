
using NFine.Code;
using NFine.DAL;
using NFine.Domain.Entity.SMCManage;
using NFine.Domain.IRepository.SMCManage;
using NFine.Entity;
using NFine.Repository.SMCManage;
using SqlSugar;
using System;
using System.Collections.Generic;
using NFine.Application.SystemSecurity;


namespace NFine.Application.SMCManage
{
    public class SMCSendApp
    {
        private ISMCSendRepository service = new SMCSendRepository();
        public List<SMCSendEntity> GetList(Pagination pagination, string queryJson)
        {

            var expression = ExtLinq.True<SMCSendEntity>();
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
            return service.FindList(expression, pagination);
        }


        public List<SMC_SendSms> GetListSugar(Pagination pagination, string queryJson)
        {

            List<SMC_SendSms> list = new List<SMC_SendSms>();
            var queryParam = queryJson.ToJObject();
            using (var db = DBHelper.GetReadInstance())
            {
                //string sql = "select SUBSTRING(F_MobileList,0,100) as F_MobileList,F_CreatorUserId,F_RootId,F_SmsContent,F_SendSign,F_SendState,F_DealState,F_OperateState,F_SendTime from SMC_SendSms";
                var queryable = db.Queryable<SMC_SendSms>();
                //var queryable_1 = db.SqlQuery<SMC_SendSms>(sql);
                // queryable.Select("LEFT('F_MobileList',100),F_CreatorUserId,F_RootId,F_SmsContent,F_SendSign,F_SendState,F_DealState,F_OperateState,F_SendTime");//截取号码的100位//,F_UserId,F_RootId,F_SmsContent,F_SendSign,F_SendState,F_DealState,F_OperateState,F_SendTime
                //查询条件 //,t.F_CreatorUserId,t.F_RootId,t.F_SmsContent,t.F_SendSign,t.F_SendState,t.F_DealState,t.F_OperateState,t.F_SendTime
                //queryable.Select("SUBSTRING(F_MobileList,0,100),F_CreatorUserId,F_RootId,F_SmsContent,F_SendSign,F_SendState,F_DealState,F_OperateState,F_SendTime");
                queryable.Select("SUBSTRING(F_MobileList,0,100) as F_MobileList,F_Id,F_GroupChannelId,F_IsTimer,F_CreatorUserId,F_RootId,F_SmsContent,F_SendSign,F_SendState,F_DealState,F_OperateState,F_SendTime,F_Priority,F_Price");//sql把手机号码截取100位，不然太慢了
                if (!queryParam["F_UserId"].IsEmpty())
                {
                    int UserId = queryParam["F_UserId"].ToInt();
                    string F_ID = DAL.Sys_UserDAL.Instance.FindEntity(t => t.Id == UserId).F_Id;
                    queryable.Where(t => t.F_CreatorUserId == F_ID);
                }
                if (!queryParam["F_RootId"].IsEmpty())
                {
                    int F_RootId = queryParam["F_RootId"].ToInt();
                    queryable.Where(t => t.F_RootId == F_RootId);
                }
                if (!queryParam["F_SmsContent"].IsEmpty())
                {
                    string keyvalue = queryParam["F_SmsContent"].ToString();
                    queryable.Where(t => t.F_SmsContent.Contains(keyvalue));
                }
                if (!queryParam["F_SendSign"].IsEmpty())
                {
                    string F_SendSign = queryParam["F_SendSign"].ToString();
                    queryable.Where(t => t.F_SendSign == F_SendSign);
                }
                if (!queryParam["F_SendState"].IsEmpty())
                {
                    int F_SendState = queryParam["F_SendState"].ToInt();
                    queryable.Where(t => t.F_SendState == F_SendState);
                }
                if (!queryParam["F_DealState"].IsEmpty())
                {
                    int F_DealState = queryParam["F_DealState"].ToInt();
                    queryable.Where(t => t.F_DealState == F_DealState);
                }
                if (!queryParam["F_OperateState"].IsEmpty())
                {
                    int F_OperateState = queryParam["F_OperateState"].ToInt();
                    queryable.Where(t => t.F_OperateState == F_OperateState);
                }
                if (!queryParam["Operator"].IsEmpty())
                {
                    string Operator = queryParam["Operator"].ToString();
                    DateTime now = System.DateTime.Now;
                    DateTime a = now.AddDays(-7);//前7天
                    DateTime b = now.AddDays(7);//后7天
                    if (Operator == "0")
                    {//+-7天内记录记录
                        queryable.Where(t => t.F_SendTime >= a);
                        queryable.Where(t => t.F_SendTime <= b);
                    }
                    else
                    {//7天外记录
                        queryable.Where(t => (t.F_SendTime < a || t.F_SendTime > b));
                    }
                }
                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }

        public SMCSendEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(SMCSendEntity smcSendEntity, string keyValue)
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            smcSendEntity.F_Signature = BLL.Sys_UserManager.Instance.GetModel(LoginInfo.Id).F_Signature;//获取签名
            string sql = "select F_Id from OC_UserInfo ";
            List<OC_UserInfo> resultList = DAL.OC_UserInfoDAL.Instance.FindList(sql);
            int length = resultList.Count;
            Dictionary<string, string> map = new Dictionary<string, string>();
            for (int i = 0; i < length; i++)
            {
                OC_UserInfo oc_UserInfo = resultList[i];
                map.Add(oc_UserInfo.F_Id, oc_UserInfo.F_Id);
            }
            if (map.ContainsKey(LoginInfo.UserId)) //判断是否为普通用户
            {
                var oc_UserInfo = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_Id == OperatorProvider.Provider.GetCurrent().UserId);//获取当前登陆者实体信息，以便取得ROOTID
                smcSendEntity.F_RootId = oc_UserInfo.F_RootId;
            }
            else if (!map.ContainsKey(LoginInfo.UserId) & LoginInfo.UserCode != "admin")//当前登陆者为业务员
            {
                var sys_User = DAL.Sys_UserDAL.Instance.FindEntity(t => t.F_Id == LoginInfo.UserId);
                smcSendEntity.F_RootId = sys_User.Id;
            }
            else//管理员登录
            {
                var sys_User = DAL.Sys_UserDAL.Instance.FindEntity(t => t.F_Id == LoginInfo.UserId);
                smcSendEntity.F_RootId = null;
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                smcSendEntity.Modify(keyValue);
            }
            else
            {
                smcSendEntity.F_Priority = DAL.OC_GroupChannelDAL.Instance.FindEntity(t => t.F_ID == smcSendEntity.F_GroupChannelId).F_Priority.ToInt();//获取通道优先级
                if (smcSendEntity.F_MobileCount.ToInt() / 1000 > 0)
                    smcSendEntity.F_Priority = smcSendEntity.F_Priority - 5;//如果单次插入号码>1000，优先级降低五个
                smcSendEntity.F_SendState = 0;//发送状态，默认待发送，标记为0.
                smcSendEntity.F_DealState = 0;//审核状态，默认未审核，标记为0.
                smcSendEntity.F_OperateState = 0;//处理状态，默认待处理，标记为0.
                smcSendEntity.Create();
            }
            NFine.Domain.Entity.SystemSecurity.LogEntity logEntity = new Domain.Entity.SystemSecurity.LogEntity();
            logEntity.F_ModuleName = "提交短信";
            logEntity.F_Type = DbLogType.Submit.ToString();
            logEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            logEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            try
            {
                service.SubmitForm(smcSendEntity, keyValue);
                logEntity.F_Result = false;
                logEntity.F_Description = "提交短信成功,短信数量为:" + smcSendEntity.F_MobileCount + "条";
                new LogApp().WriteDbLog(logEntity);
            }
            catch (Exception ex)
            {
                logEntity.F_Result = false;
                logEntity.F_Description = "提交短信失败，" + ex.Message;
                new LogApp().WriteDbLog(logEntity);
            }

        }
        public void UpdateForm(SMCSendEntity SMCSendEntity)
        {
            service.Update(SMCSendEntity);
        }
    }
}
