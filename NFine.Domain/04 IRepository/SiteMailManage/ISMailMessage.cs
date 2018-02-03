
using NFine.Data;
using NFine.Domain.Entity.SiteMailManage;

namespace NFine.Domain.IRepository.SiteMailManage
{
    public interface ISMailMessageRepository : IRepositoryBase<SMailMessageEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SMailMessageEntity SMailMessageEntity, string keyValue);
    }
}
