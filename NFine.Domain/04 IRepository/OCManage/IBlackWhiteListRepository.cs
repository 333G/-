
using NFine.Data;
using NFine.Domain.Entity.OCManage;

namespace NFine.Domain.IRepository.OCManage
{
    public interface IBlackWhiteListRepository : IRepositoryBase<BlackWhiteListEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(BlackWhiteListEntity BlackWhiteListEntity, string keyValue);
    }
}
