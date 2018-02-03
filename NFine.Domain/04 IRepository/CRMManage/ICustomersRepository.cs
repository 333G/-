
using NFine.Data;
using NFine.Domain.Entity.CRMManage;

namespace NFine.Domain.IRepository.CRMManage
{
    public interface ICustomersRepository : IRepositoryBase<CustomersEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(CustomersEntity customersEntity, string keyValue);
    }
}
