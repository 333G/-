
using NFine.Data;
using NFine.Domain.Entity.OCManage;

namespace NFine.Domain.IRepository.OCManage
{
    public interface ISensitiveWordsRepository : IRepositoryBase<SensitiveWordsEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SensitiveWordsEntity SensitiveWordsEntity, string keyValue);
    }
}
