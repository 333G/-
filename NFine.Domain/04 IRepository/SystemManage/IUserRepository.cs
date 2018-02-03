using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Entity;

namespace NFine.Domain.IRepository.SystemManage
{
    public interface IUserRepository : IRepositoryBase<UserEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(Sys_User userEntity, UserInfoEntity userInfoEntity, UserLogOnEntity userLogOnEntity, string keyValue);

        int GetUserID(string F_ID);
    }
}
