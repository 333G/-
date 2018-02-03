
using NFine.Data;
using NFine.Domain.Entity.SiteMailManage;

namespace NFine.Domain.IRepository.SiteMailManage
{
    public interface ISMailMessageTRepository : IRepositoryBase<SMailMessageTEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SMailMessageTEntity SMailMessageTEntity, string keyValue);
    }
}
