
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.SMCManage;
using NFine.Domain.IRepository.SMCManage;
using NFine.Repository.SMCManage;

namespace NFine.Repository.SMCManage
{
    public class SMCRceiveRepository : RepositoryBase<SMCRceiveEntity>, ISMCRceiveRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<SMCRceiveEntity>(t => t.F_Id == i);
                db.Commit();
            }
             
        }
        public void SubmitForm(SMCRceiveEntity SMCRceiveEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(SMCRceiveEntity);
                }
                else
                {
                    db.Insert(SMCRceiveEntity);
                }
                db.Commit();
            }
        }
    }
}
