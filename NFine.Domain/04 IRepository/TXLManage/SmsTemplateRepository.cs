
using NFine.Data;
using NFine.Domain.Entity.TXLManage;

namespace NFine.Domain.IRepository.TXLManage
{
    public interface ISmsTemplateRepository : IRepositoryBase<SmsTemplateEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SmsTemplateEntity SmsTemplateEntity, string keyValue);
    }
}
