using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Entity;
using NFine.Code;
using SqlSugar;
using System.Linq.Expressions;

namespace NFine.BLL
{
     public partial class OC_UserInfoManager
    {
        #region 单例模式

        private static OC_UserInfoManager instance;
        private static object _lock = new object();

        private OC_UserInfoManager()
        {
        }

        public static OC_UserInfoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OC_UserInfoManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        //根据F_Id查询单个实体
        public OC_UserInfo GetModel(string CreatorUserId)
        {
            return DAL.OC_UserInfoDAL.Instance.FindEntity(a => a.F_Id == CreatorUserId);
        }





        /// <summary>
        /// 分页查询,多表联查，Views中包含多表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="pagination">分页组件</param>
        /// <returns></returns>
        public List<Entity.Views.VOC_UserInfo> GetPageList(Expression<Func<OC_UserInfo ,bool>> expression_User,Expression<Func<OC_GroupChannel,bool>> expression_Channel, Expression<Func<Sys_User,bool>>expression_SysUser,Pagination pagination)
        {
            //using (var db=DAL.DBHelper.GetReadInstance())
            //{
            //    var tempData = db.Queryable<Entity.Views.VOC_UserInfo>().Where(expression).OrderBy(pagination.sidx);
            //    pagination.records = tempData.Count();
            //    return tempData.ToPageList(pagination.page, pagination.rows);
            //}
           
            //获取视图,返回查询PageList
            using (var db = DAL.DBHelper.GetReadInstance())
            {
                //表OC_UserInfo和OC_GroupChannel的F_UserId字段关联
                List<Entity.Views.VOC_UserInfo> list = db.Queryable<OC_UserInfo>()
                    .JoinTable<OC_GroupChannel>((s1, s2) => s1.F_UserId == s2.F_UserId)//left join
                    .JoinTable<Sys_User>((s1, s3) => (s1.F_UserId == s3.Id))
                     .Where<Sys_User>((s1, s3) => s3.F_IsAdministrator == false)
                    .OrderBy<OC_UserInfo,OC_GroupChannel>((s1,s2)=>s1.F_UserId)
                    .Select< OC_GroupChannel, Sys_User,Entity.Views.VOC_UserInfo>
                    ((s1, s2, s3) => new Entity.Views.VOC_UserInfo { F_Id = s1.F_Id, F_UserFid = s1.F_UserFid, F_UserId = s1.F_UserId, F_RootId = s1.F_RootId, F_ManagerId = s1.F_ManagerId, F_Account = s1.F_Account, F_SendedNum = s1.F_SendedNum, F_Balance = s1.F_Balance, F_Reviewed = s1.F_Reviewed, F_State = s1.F_State, F_DeleteMark = s1.F_DeleteMark, F_EnabledMark = s1.F_EnabledMark, F_Description = s1.F_Description, F_MobileChannel = s2.F_MobileChannel, F_UnicomChannel = s2.F_UnicomChannel, F_TelecomChannel = s2.F_TelecomChannel, F_ChannelName = s2.F_ChannelName, F_ChannelType = s2.F_ChannelType, Signature = s3.F_Signature })
               .ToPageList(pagination.page, pagination.rows);
                return list;
            }
        }
    }
}
