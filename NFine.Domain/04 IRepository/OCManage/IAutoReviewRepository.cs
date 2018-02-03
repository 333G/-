
using NFine.Data;
using NFine.Domain.Entity.OCManage;

namespace NFine.Domain.IRepository.OCManage
{
    public interface IAutoReviewRepository : IRepositoryBase<AutoReviewEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(AutoReviewEntity AutoReviewEntity, string keyValue);
    }
}
