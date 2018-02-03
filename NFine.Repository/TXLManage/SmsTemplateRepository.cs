
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.TXLManage;
using NFine.Domain.IRepository.TXLManage;
using NFine.Repository.TXLManage;

namespace NFine.Repository.TXLManage
{
    public class SmsTemplateRepository : RepositoryBase<SmsTemplateEntity>, ISmsTemplateRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<SmsTemplateEntity>(t => t.F_Id == i);
                db.Commit();
            }
           
        }
        public void SubmitForm(SmsTemplateEntity SmsTemplateEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(SmsTemplateEntity);
                }
                else
                {
                    db.Insert(SmsTemplateEntity);
                }
                db.Commit();
            }
        }
    }
}
