
using NFine.Data;
using NFine.Domain.Entity.OCManage;

namespace NFine.Domain.IRepository.OCManage
{
    public interface IManualReviewRepository : IRepositoryBase<ManualReviewEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(ManualReviewEntity ManualReviewEntity, string keyValue);
    }
}
