
using NFine.Data;
using NFine.Domain.Entity.CRMManage;

namespace NFine.Domain.IRepository.CRMManage
{
    public interface ICustomeRecordsRepository : IRepositoryBase<CustomeRecordsEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(CustomeRecordsEntity CustomeRecordsEntity, string keyValue);
    }
}
