using NFine.Data;
using NFine.Domain.Entity.OCManage;


namespace NFine.Domain.IRepository.OCManage
{
    public interface IRechargeRecordRepository : IRepositoryBase<RechargeRecordEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(RechargeRecordEntity RechargeRecordEntity, string keyValue);
    }
}
 



