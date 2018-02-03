
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;

namespace NFine.Repository.OCManage
{
    public class ManualReviewRepository : RepositoryBase<ManualReviewEntity>, IManualReviewRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<ManualReviewEntity>(t => t.F_Id == i);
                db.Commit();
            }
             
        }
        public void SubmitForm(ManualReviewEntity ManualReviewEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(ManualReviewEntity);
                }
                else
                {
                    db.Insert(ManualReviewEntity);
                }
                db.Commit();
            }
        }
    }
}
