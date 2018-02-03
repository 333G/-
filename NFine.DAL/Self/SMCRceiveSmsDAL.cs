using NFine.Code;
using NFine.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.DAL.Self
{
    public partial class SMCRceiveSmsDAL
    {
        #region 单例模式
        private static SMCRceiveSmsDAL instance;
        private static object _lock = new object();

        private SMCRceiveSmsDAL()
        {
        }

        public static SMCRceiveSmsDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMCRceiveSmsDAL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        //这里是一个多表联查，使用的sqlsuger，by lx
        public List<NFine.Entity.Views.VSMCRceiveSms> GetReceiveList(Pagination pagination, string queryJson)
        {
            List<NFine.Entity.Views.VSMCRceiveSms> list = new List<NFine.Entity.Views.VSMCRceiveSms>();
            var queryParam = queryJson.ToJObject();
            var expression = ExtLinq.True<SMC_ReplyMessage>();
            using (var db = DBHelper.GetReadInstance())
            {
                
                list = db.Queryable<SMC_ReplyMessage>()
               .JoinTable<SMC_SendSms>((s1, s2) => s1.F_CreatorUserId == s2.F_CreatorUserId)
               .JoinTable<Sys_User>((s1, s3) => s1.F_CreatorUserId == s3.F_Id)
               .Select<SMC_ReplyMessage, SMC_SendSms, Sys_User, NFine.Entity.Views.VSMCRceiveSms>
               ((s1, s2, s3) => new NFine.Entity.Views.VSMCRceiveSms { F_Id = s1.F_Id, F_UserId = s3.Id, receive_time = s1.receive_time, mobile = s1.mobile, F_SmsContent = s1.receive_content })
               .ToList();
                return list;
            }
        }
    }
}
