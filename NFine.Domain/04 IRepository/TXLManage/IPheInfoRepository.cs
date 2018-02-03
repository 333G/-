
using NFine.Data;
using NFine.Domain.Entity.TXLManage;

namespace NFine.Domain.IRepository.TXLManage
{
    public interface IPheInfoRepository : IRepositoryBase<PheInfoEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(PheInfoEntity pheInfoEntity, string keyValue);
    }
}
