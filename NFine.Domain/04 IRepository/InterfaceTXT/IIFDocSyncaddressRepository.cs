
using NFine.Data;
using NFine.Domain.Entity.InterfaceTXT;

namespace NFine.Domain.IRepository.InterfaceTXT
{
    public interface IIFDocSyncaddressRepository : IRepositoryBase<IFDocSyncaddressEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(IFDocSyncaddressEntity IFDocSyncaddressEntity, string keyValue);
    }
}
