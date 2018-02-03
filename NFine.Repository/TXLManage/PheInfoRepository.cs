
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.TXLManage;
using NFine.Domain.IRepository.TXLManage;
using NFine.Repository.TXLManage;

namespace NFine.Repository.TXLManage
{
    public class PheInfoRepository : RepositoryBase<PheInfoEntity>, IPheInfoRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<PheInfoEntity>(t => t.F_Id == i);
                db.Commit();
            }
           
        }
        public void SubmitForm(PheInfoEntity PheInfoEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(PheInfoEntity);
                }
                else
                {
                    db.Insert(PheInfoEntity);
                }
                db.Commit();
            }
        }
    }
}
