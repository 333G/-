
using NFine.Data;
using NFine.Domain.Entity.TXLManage;

namespace NFine.Domain.IRepository.TXLManage
{
    public interface IBlacklistRepository : IRepositoryBase<BlacklistEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(BlacklistEntity BlacklistEntity, string keyValue);
    }
}
