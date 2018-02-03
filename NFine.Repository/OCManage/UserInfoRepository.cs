
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;

namespace NFine.Repository.OCManage
{
    public class UserInfoRepository : RepositoryBase<UserInfoEntity>, IUserInfoRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<UserInfoEntity>(t => t.F_Id == i);
                db.Commit();
            }
        }
        public void SubmitForm(UserInfoEntity UserInfoEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(UserInfoEntity);
                }
                else
                {
                    db.Insert(UserInfoEntity);
                }
                db.Commit();
            }
        }

    }
}

