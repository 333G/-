
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.CRMManage;
using NFine.Domain.IRepository.CRMManage;
using NFine.Repository.CRMManage;

namespace NFine.Repository.CRMManage
{
    public class CustomersRepository : RepositoryBase<CustomersEntity>, ICustomersRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<CustomersEntity>(t => t.F_Id == i); 
                db.Commit();
            }
        }
        public void SubmitForm(CustomersEntity CustomersEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(CustomersEntity);
                }
                else
                {
                    db.Insert(CustomersEntity);
                }
                db.Commit();
            }
        }
    }
}
