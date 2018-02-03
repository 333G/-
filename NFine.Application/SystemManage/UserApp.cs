using NFine.Code;
using NFine;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Entity;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace NFine.Application.SystemManage
{
    public class UserApp
    {
        private IUserRepository service = new UserRepository();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        public List<UserEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }

        //根据F_ID取得userid，userid是递增编号，f_id是GUID
        public int GetUserId(string F_ID) {
            return 1;
        }
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="keyValue">Id</param>
        public void SubmitForm(Sys_User userEntity, UserInfoEntity userInfoEntity, UserLogOnEntity userLogOnEntity, string keyValue)//keyValue是Id
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
             if (!string.IsNullOrEmpty(keyValue))//keyvalue有数值时,修改时(edit)表单传入的值
            {
                userEntity.Id = keyValue.ToInt();
                userInfoEntity.F_UserId = keyValue.ToInt();
                if (LoginInfo != null)
                    userInfoEntity.F_LastModifyUserId = userEntity.F_LastModifyUserId = LoginInfo.UserId;
                
               userInfoEntity.F_LastModifyTime = userEntity.F_LastModifyTime = DateTime.Now;
            }
            else//新建用户
            {
                if (userEntity.F_IsAdministrator == true)//管理员
                {
                    userEntity.F_Depth = 0;//深度为0
                }
                else//不是管理员
                {
                    List<Sys_User> data = DAL.Sys_UserDAL.Instance.GetDepthList(LoginInfo.UserId);
                    int depth = data[0].F_Depth.ToInt();
                    userInfoEntity.F_State = "暂停";//默认添加非管理员用户状态为暂停，审核之后变为正常
                    userEntity.F_Depth = depth+ 1;//获得当前用户的深度（depth）并+1
                    //给RootID赋值
                    if (depth == 0)//创建者深度为0的时候，直接赋值
                    {
                        //后台赋值,再Sys_User插入值后返回自加的Id赋值给RootId。
                    }
                    else//创建者深度不为0，继承父业务员的RootID
                        userInfoEntity.F_RootId = DAL.OC_UserInfoDAL.Instance.FindEntity(a => a.F_Id == LoginInfo.UserId).F_RootId.ToInt();
                }
                if (LoginInfo != null)
                {
                    userEntity.F_CreatorUserId = LoginInfo.UserId;
                    userInfoEntity.F_CreatorUserId = LoginInfo.UserId;
                    userEntity.F_ParentId = LoginInfo.UserId; ;//给父节点字段赋值.
                }
                userEntity.F_Id = Common.GuId();
                userEntity.F_CreatorTime = DateTime.Now;
                userInfoEntity.F_DeleteMark = false;
            }
            service.SubmitForm(userEntity, userInfoEntity, userLogOnEntity, keyValue);
        }

        public void UpdateForm(UserEntity userEntity)
        {
            service.Update(userEntity);
        }

        //根据F_ID返回model
        public List<Sys_User> GetForm(string F_Id)
        {
            var data = DAL.Sys_UserDAL.Instance.FindList(t => t.F_Id == F_Id);
            return data;
        }

        public UserEntity CheckLogin(string username, string password)
        {
            UserEntity userEntity = service.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    UserLogOnEntity userLogOnEntity = userLogOnApp.GetForm(userEntity.F_Id);
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.F_UserPassword)
                    {
                        DateTime lastVisitTime = DateTime.Now;
                        int LogOnCount = (userLogOnEntity.F_LogOnCount).ToInt() + 1;
                        if (userLogOnEntity.F_LastVisitTime != null)
                        {
                            userLogOnEntity.F_PreviousVisitTime = userLogOnEntity.F_LastVisitTime.ToDate();
                        }
                        userLogOnEntity.F_LastVisitTime = lastVisitTime;
                        userLogOnEntity.F_LogOnCount = LogOnCount;
                        userLogOnApp.UpdateForm(userLogOnEntity);
                        return userEntity;
                    }
                    else
                    {
                        throw new Exception("密码不正确，请重新输入");
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }
    }
}
