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
    public partial class CustomersMessageDAL
    {

        #region 单例模式
        private static CustomersMessageDAL instance;
        private static object _lock = new object();

        private CustomersMessageDAL()
        {
        }

        public static CustomersMessageDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new CustomersMessageDAL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        //获取以userId为收件人的短信信息
        public List<Entity.Views.VCustomersMessage> GetList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();


            if (!queryParam["F_StateId"].IsEmpty())
            {
                string F_StateId = queryParam["F_StateId"].ToString(); 
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
              //  expression = expression.And(t => t.F_CustInfo.Contains(keyvalue));
            }

            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select c.F_Id as customerId,c.F_CustInfo as F_CustInfo,c.F_StateId as F_StateId
                                from CRM_Customers c");
                /*
                strSql.Append(@"select u.F_Id as ReceiveId, t.F_Message as ReceiveMessage, u.F_RealName as ReceiveName
                                from CRM_Customers s
                                join CRM_CustomeRecords t
                                    on s.F_MessageID=t.F_Id
                                join Sys_User u on
                                    u.F_Id=s.F_SendID 

                                where s.F_Id=@Id");
                                */
                return db.SqlQuery<Entity.Views.VCustomersMessage>(strSql.ToString());
          //      return db.SqlQuery<Entity.Views.VCustomersMessage>(strSql.ToString(), new { Id = keyValue });

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
