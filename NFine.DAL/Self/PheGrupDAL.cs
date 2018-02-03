using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace NFine.DAL.Self
{
    public partial class PheGrupDAL
    {

        #region 单例模式
        private static PheGrupDAL instance;
        private static object _lock = new object();

        private PheGrupDAL()
        {
        }

        public static PheGrupDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new PheGrupDAL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        //获取群组管理页面显示所需数据
        public List<NFine.Entity.Views.VPheGrup> GetList(string keyword)
        {
            List<Entity.Views.VPheGrup> list = new List<Entity.Views.VPheGrup>();
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();
                if (!string.IsNullOrEmpty(keyword))
                {
                    strSql.Append(@" select distinct g.F_Id as F_Id, g.GroupName as GroupName, (select count(1) from TXL_PhoneInfo  where GroupId  =g.F_Id ) as F_MemberNumber, g.IsDefault as IsDefault,g.F_Description as Description
                                from TXL_PhoneGroup g  
                                where  g.GroupName =@groupName
                                 "); 
                 list = db.SqlQuery<Entity.Views.VPheGrup>(strSql.ToString(), new { groupName = keyword }); 
                }

                else
                {
                    strSql.Append(@" select distinct g.F_Id as F_Id, g.GroupName as GroupName, (select count(1) from TXL_PhoneInfo  where GroupId  =g.F_Id ) as F_MemberNumber, g.IsDefault as IsDefault,g.F_Description as Description
                                from TXL_PhoneGroup g  
                                 ");
                    list=db.SqlQuery<Entity.Views.VPheGrup>(strSql.ToString());
                }
                return list;
            }
        }

        //群组之下是否有用户
        public bool IsHaveUser(string keyvalue)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                return db.Queryable<Entity.TXL_PhoneInfo>().Any(t=>t.GroupId==keyvalue);
            }
        } 
    }
}
