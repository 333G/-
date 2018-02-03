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
    public partial class Sys_UserDAL
    {
        //获取用户角色为        
        public List<Sys_User> GetList()
        {
            var userId = OperatorProvider.Provider.GetCurrent().UserId;//当前登录用户ID
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();

                //此处u.F_Id as F_Id 必须指定，否则页面上收件人对应的F_Id不一致
                strSql.Append(@"select u.F_Id as F_Id, u.F_RealName as F_RealName,u.F_Account as F_Account
                                from Sys_User u
                                join Sys_Role r
                                    on u.F_RoleID=r.F_Id 
                                where  u.F_Id!=@F_Id and r.F_Type = 1 or r.F_Type = 2 "); 
                return db.SqlQuery<Sys_User>(strSql.ToString(), new { F_Id = userId });
            }
        }

        //取当前用户深度的记录&&//取指定类别的记录,F_Account，在子账户管理中使用（禁用和启用按钮）       
        public List<Sys_User> GetDepthList(string id)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select F_Id, Id,F_Account,F_Depth
                                from Sys_User 
                                where F_Id=@F_Id");
               return db.SqlQuery<Sys_User>(strSql.ToString(), new { F_Id = id });
            }
        }
            
        //通过ID取记录   
        public List<Sys_User> GetListByid(string id)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();

                //此处u.F_Id as F_Id 必须指定，否则页面上收件人对应的F_Id不一致
                strSql.Append(@"select u.F_Id as F_Id, u.F_RealName as F_RealName
                                from Sys_User u
                                join Sys_Role r
                                    on u.F_RoleID=r.F_Id 
                                where  u.F_Id!=@F_Id and r.F_Type = 1 or r.F_Type = 2 ");
                return db.SqlQuery<Sys_User>(strSql.ToString(), new { F_Id = id });
            }
        }
        
       
        public List<Sys_User> GetList(string keyword)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<Sys_User>().Where(t => t.F_Account != "admin");
                if (!string.IsNullOrEmpty(keyword))
                    queryable.Where(t => t.F_IsAdministrator.Equals(keyword));

                var tempData = queryable.OrderBy(t => t.F_Account);
                return tempData.ToList();
            }
        }

        public List<Sys_User> GetList(Pagination pagination, string keyword)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<Sys_User>().Where(t => t.F_Account != "admin");
                if (!string.IsNullOrEmpty(keyword))
                    queryable.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword));

                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="userId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Sys_User> GetList(Pagination pagination, string userId, string keyword)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<Sys_User>().Where(t => t.F_ParentId == userId && t.F_Account != "admin");

                if (!string.IsNullOrEmpty(keyword))
                    queryable.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword));

                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }


    }
}
