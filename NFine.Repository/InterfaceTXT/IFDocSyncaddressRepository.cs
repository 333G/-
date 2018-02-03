
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.InterfaceTXT;
using NFine.Domain.IRepository.InterfaceTXT;
using NFine.Repository.InterfaceTXT;

namespace NFine.Repository.InterfaceTXT
{
    public class IFDocSyncaddressRepository : RepositoryBase<IFDocSyncaddressEntity>, IIFDocSyncaddressRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<IFDocSyncaddressEntity>(t => t.F_Id == i);
                db.Commit();
            }

            
        }
        public void SubmitForm(IFDocSyncaddressEntity IFDocSyncaddressEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(IFDocSyncaddressEntity);
                }
                else
                {
                    db.Insert(IFDocSyncaddressEntity);
                }
                db.Commit();
            }
        }
    }
}
