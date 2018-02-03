using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Code; 
using SqlSugar;
using NFine.DAL; 


namespace NFine.DAL.Self
{
    public partial class SMailMessageDAL
    {

        #region 单例模式
        private static SMailMessageDAL instance;
        private static object _lock = new object();

        private SMailMessageDAL()
        {
        }

        public static SMailMessageDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMailMessageDAL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        //获取以userId为收件人的短信信息
        public List<NFine.Entity.Views.VSMailMessage> GetReply(string keyValue)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select u.F_Id as ReceiveId, t.F_Message as ReceiveMessage, u.F_RealName as ReceiveName
                                from SMail_Message s
                                join SMail_MessageText t
                                    on s.F_MessageID=t.F_Id
                                join Sys_User u on
                                    u.F_Id=s.F_SendID 

                                where s.F_Id=@Id");
                return db.SqlQuery<Entity.Views.VSMailMessage>(strSql.ToString(), new { Id = keyValue });

            }
        }
        public List<NFine.Entity.Views.VSMailMessage> GetReceiveList(string userId, string queryJson)
        {
            List < NFine.Entity.Views.VSMailMessage > list= new List<NFine.Entity.Views.VSMailMessage>();
            var queryParam = queryJson.ToJObject();
            var expression = ExtLinq.True<NFine.Entity.Views.VSMailMessage>();
            if (!queryParam["sel"].IsEmpty())
            {
                string sel = queryParam["sel"].ToString();
                expression = expression.And(t => t.F_Message.Equals(sel));
            }
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();
                

                    strSql.Append(@"select s.F_Id as F_Id, t.F_Message as F_Message, t.F_CreatorTime as F_CreatorTime, uS.F_RealName as F_SendName,uR.F_RealName as F_ReceiveName,s.F_Statue as F_Statue
                                from SMail_Message s
                                join SMail_MessageText t
                                    on s.F_MessageID=t.F_Id
                                join Sys_User uS on
                                    uS.F_Id=s.F_SendID
                                join Sys_User uR on
                                    uR.F_Id=s.F_RecID
                                where s.F_RecID=@F_RecID");

                    list= db.SqlQuery<Entity.Views.VSMailMessage>(strSql.ToString(), new { F_RecID = userId });
                
                return list;
            }
        }
        //获取以userId为发件人的短信信息
        public List<NFine.Entity.Views.VSMailMessage> GetSendList(string userId )
        {
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();
             
                    strSql.Append(@"select s.F_Id as F_Id, t.F_Message as F_Message, t.F_CreatorTime as F_CreatorTime, uS.F_RealName as F_SendName,uR.F_RealName as F_ReceiveName,s.F_Statue as F_Statue,t.F_DeleteMark as F_DeleteMark
                                from SMail_Message s
                                join SMail_MessageText t
                                    on s.F_MessageID = t.F_Id
                                join Sys_User uR on
                                    uR.F_Id=s.F_RecID
                                join Sys_User uS on
                                    uS.F_Id=s.F_SendID
                                where s.F_SendID=@F_SendID");

                    return db.SqlQuery<Entity.Views.VSMailMessage>(strSql.ToString(), new { F_SendID = userId });
              
            }
        }
        public bool Delete(string keyValue)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                { 
                    strSql.Append(@"delete 
                                from SMail_Message s
                                join SMail_MessageText t
                                    on s.F_MessageID=t.F_Id 
                                where t.F_Id=@F_Id"); 
                     db.SqlQuery<Entity.SMail_Message>(strSql.ToString(), new { F_Id = i }); 
                }
                return true;
            }
            //return false;
        }
    }
}
