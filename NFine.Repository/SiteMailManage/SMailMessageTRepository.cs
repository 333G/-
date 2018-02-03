
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.SiteMailManage;
using NFine.Domain.IRepository.SiteMailManage;
using NFine.Repository.SiteMailManage;

namespace NFine.Repository.SiteMailManage
{
    public class SMailMessageTRepository : RepositoryBase<SMailMessageTEntity>, ISMailMessageTRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<SMailMessageTEntity>(t => t.F_Id == i);
                db.Commit();
            }
        }
        public void SubmitForm(SMailMessageTEntity SMailMessageTEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(SMailMessageTEntity);
                }
                else
                {
                    db.Insert(SMailMessageTEntity);
                }
                db.Commit();
            }
        }
    }
}
