using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Entity;
using NFine.Repository.SystemManage;
using NFine.DAL;
namespace NFine.Repository.SystemManage
{
    public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<UserEntity>(t => t.F_Id == keyValue);
                db.Delete<UserLogOnEntity>(t => t.F_Id == keyValue);
                db.Delete<UserInfoEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(Sys_User userEntity, UserInfoEntity userInfoEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))//updata
                {
                    Sys_User OldUserModel=BLL.Sys_UserManager.Instance.GetModel(userEntity.Id);//取旧model，更新内容后提交
                    //OC_UserInfo OldUserInfoModel = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == userEntity.Id);似乎用不着更新UserInfo表字段
                    OldUserModel.F_Account = userInfoEntity.F_Account = userEntity.F_Account;
                    OldUserModel.F_Birthday = userEntity.F_Birthday;
                    OldUserModel.F_DepartmentId = userEntity.F_DepartmentId;
                    OldUserModel.F_Description = userEntity.F_Description;
                    OldUserModel.F_DutyId = userEntity.F_DutyId;
                    OldUserModel.F_Email = userEntity.F_Email;
                    OldUserModel.F_MobilePhone = userEntity.F_MobilePhone;
                    OldUserModel.F_ManagerId = userEntity.F_ManagerId;
                    OldUserModel.F_RealName = userEntity.F_RealName;
                    OldUserModel.F_RoleId = userEntity.F_RoleId;
                    OldUserModel.F_Birthday = userEntity.F_Birthday;
                    OldUserModel.F_LastModifyTime = userEntity.F_LastModifyTime;
                    OldUserModel.F_LastModifyUserId = userEntity.F_LastModifyUserId;
                    OldUserModel.F_Signature = userEntity.F_Signature;
                    OldUserModel.F_WeChat = userEntity.F_WeChat;
                    OldUserModel.F_EnabledMark = userEntity.F_EnabledMark;
                    Sys_UserDAL.Instance.Update(OldUserModel);
                }
                else//insert
                {
                    userInfoEntity.F_Id = userEntity.F_Id;
                    userInfoEntity.F_UserFid = userEntity.F_Id;
                    userInfoEntity.F_CreatorTime = userEntity.F_CreatorTime;
                    userInfoEntity.F_Reviewed = 1;//默认条条需审核
                    userInfoEntity.F_MessageNum = 0;//每手机24小时内最多接收短信
                    userInfoEntity.F_OneCode = 0;//1小时验证码限制
                    userInfoEntity.F_TwentyFourCode = 0;//24小时验证码限制
                    userLogOnEntity.F_Id = userEntity.F_Id;
                    userLogOnEntity.F_UserSecretkey = Md5.md5(Common.CreateNo(), 16).ToLower();
                    userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5(userLogOnEntity.F_UserPassword, 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    //db.Insert(userEntity);//弃用的旧方法
                    int UserID =Sys_UserDAL.Instance.Add(userEntity);//往Sys_User表插入同时获取自增ID的返回值
                    userLogOnEntity.F_UserId = UserID.ToString();
                    userInfoEntity.F_UserId = UserID;
                    if (userEntity.F_IsAdministrator==false)//非管理员
                    {
                        if (userEntity.F_Depth == 1)//用户深度为1.作为最初用户，RootId为本身Id
                        {
                            userInfoEntity.F_RootId = UserID;
                        }
                        db.Insert(userInfoEntity);//往OC_UserInfo表中插入数据   
                    }
                    db.Insert(userLogOnEntity);                    
                }
                db.Commit();
            }
        }

        public int GetUserID(string F_ID) {
            return 1;
        }
    }
}
