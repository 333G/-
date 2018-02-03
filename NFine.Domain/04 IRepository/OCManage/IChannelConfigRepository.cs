using NFine.Domain.Entity.OCManage;
using NFine.Data;

namespace NFine.Domain.IRepository.OCManage
{
    public interface IChannelConfigRepository : IRepositoryBase<ChannelConfigEntity>
    {
        void DeleteForm(string keyValue);
    }
}
