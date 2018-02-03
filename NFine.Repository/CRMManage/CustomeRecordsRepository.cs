
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.CRMManage;
using NFine.Domain.IRepository.CRMManage;
using NFine.Repository.CRMManage;

namespace NFine.Repository.CRMManage
{
    public class CustomeRecordsRepository : RepositoryBase<CustomeRecordsEntity>, ICustomeRecordsRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<CustomeRecordsEntity>(t => t.F_Id == i); 
                db.Commit();
            }
        }
        public void SubmitForm(CustomeRecordsEntity CustomeRecordsEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(CustomeRecordsEntity);
                }
                else
                {
                    db.Insert(CustomeRecordsEntity);
                }
                db.Commit();
            }
        }
    }
}
