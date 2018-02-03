

using NFine.Data;
using NFine.Domain.Entity.OCManage;

namespace NFine.Domain.IRepository.OCManage
{
    public interface ISMSInstructionsRepository : IRepositoryBase<SMSInstructionsEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SMSInstructionsEntity SMSInstructionsEntity, string keyValue);
    }
}

