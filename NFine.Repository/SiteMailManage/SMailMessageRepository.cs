
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.SiteMailManage;
using NFine.Domain.IRepository.SiteMailManage;
using NFine.Repository.SiteMailManage;

namespace NFine.Repository.SiteMailManage
{
    public class SMailMessageRepository : RepositoryBase<SMailMessageEntity>, ISMailMessageRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<SMailMessageEntity>(t => t.F_Id == i);
                db.Commit();
            }
        }
        public void SubmitForm(SMailMessageEntity SMailMessageEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(SMailMessageEntity);
                }
                else
                {
                    db.Insert(SMailMessageEntity);
                }
                db.Commit();
            }
        }
    }
}
