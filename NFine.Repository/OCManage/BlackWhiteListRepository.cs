
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;

namespace NFine.Repository.OCManage
{
    public class BlackWhiteListRepository : RepositoryBase<BlackWhiteListEntity>, IBlackWhiteListRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<BlackWhiteListEntity>(t => t.F_Id == i);
                db.Commit();
            }
             
        }
        public void SubmitForm(BlackWhiteListEntity blackWhiteListEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(blackWhiteListEntity);
                }
                else
                {
                    db.Insert(blackWhiteListEntity);
                }
                db.Commit();
            }
        }
    }
}
