
using NFine.Data;
using NFine.Domain.Entity.SMCManage;

namespace NFine.Domain.IRepository.SMCManage
{
    public interface ISMCRceiveRepository : IRepositoryBase<SMCRceiveEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SMCRceiveEntity SMCRceiveEntity, string keyValue);
    }
}
