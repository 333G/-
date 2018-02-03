using NFine.Code;
using NFine.Entity;
using SqlSugar;
using System.Collections.Generic;
using System.Linq;
using System;
using NFine.Entity.Views;
using System.Linq.Expressions;

namespace NFine.DAL.Self
{
    public partial class SMCSendSmsDAL
    {
        #region 单例模式
        private static SMCSendSmsDAL instance;
        private static object _lock = new object();

        private SMCSendSmsDAL()
        {
        }

        public static SMCSendSmsDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMCSendSmsDAL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        //这里是一个多表联查，使用的sqlsuger，by lx
        public List<NFine.Entity.Views.VSMCSendSms> GetReceiveList(Pagination pagination, string queryJson)
        {
            List<NFine.Entity.Views.VSMCSendSms> list = new List<NFine.Entity.Views.VSMCSendSms>();
            var queryParam = queryJson.ToJObject();
            var expression = ExtLinq.True<SMC_SendSms>();       
            using (var db = DBHelper.GetReadInstance())
            {
                //表SMC_SendSms中的F_CreatorUserId字段与表OC_UserInfo中的F_UserFid字段关联
                list = db.Queryable<SMC_SendSms>()
               .JoinTable<Entity.OC_UserInfo>((s1, s2) => s1.F_CreatorUserId == s2.F_UserFid)
               .JoinTable<Sys_User>((s1, s3) => s1.F_CreatorUserId == s3.F_Id)
               .Select<SMC_SendSms, Entity.OC_UserInfo, Sys_User, NFine.Entity.Views.VSMCSendSms>
               ((s1, s2, s3) => new NFine.Entity.Views.VSMCSendSms { F_Id = s1.F_Id, F_UserId = s3.Id, F_SmsContent = s1.F_SmsContent, F_RootId = s2.F_RootId, F_SendTime = s1.F_SendTime, F_SendSign = s1.F_SendSign, F_SendState = s1.F_SendState, F_DealState = s1.F_DealState, F_OperateState = s1.F_OperateState, F_MobileList = s1.F_MobileList, F_Price = s1.F_Price })
               .ToList();              
                return list;
            }
        }
    }
}
