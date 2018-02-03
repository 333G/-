 
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;

namespace NFine.Repository.OCManage
{
    public class SMSInstructionsRepository : RepositoryBase<SMSInstructionsEntity>, ISMSInstructionsRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<SMSInstructionsEntity>(t => t.F_Id == i);
                db.Commit();
            }
            
        }
        public void SubmitForm(SMSInstructionsEntity SMSInstructionsEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(SMSInstructionsEntity);
                }
                else
                {
                    db.Insert(SMSInstructionsEntity);
                }
                db.Commit();
            }
        }
    }
}

