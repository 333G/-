using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.DAL.Table
{
    class OC_UserInfoDAL
    {
        public List<OC_UserInfoDAL> GetDepthList(string id)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select  u.F_Id as F_Id, u.Id as Id, u.F_Account as F_Account,u.F_depth as F_Depth,u.F_IsAdministrator as F_IsAdministratorr,r.F_RootId as F_RootId 
                                from Sys_User u
								join OC_UserInfo r
								    on u.F_Id=r.F_UserFid
                                where u.F_IsAdministrator = 0 and u.F_Id=@F_Id");
                return db.SqlQuery<OC_UserInfoDAL>(strSql.ToString(), new { F_Id = id });
            }
        }
    }
}
