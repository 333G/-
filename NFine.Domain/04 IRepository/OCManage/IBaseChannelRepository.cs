
using NFine.Data;
using NFine.Domain.Entity.OCManage;

namespace NFine.Domain.IRepository.OCManage
{
    public interface IBaseChannelRepository : IRepositoryBase<BaseChannelEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(BaseChannelEntity BaseChannelEntity, string keyValue);
        void newSubmitForm(BaseChannelEntity basechannelEntity, ChannelProvinceEntity channelprovinceEntity, ChannelConfigEntity channelconfigEntity, string keyValue);

    }
}
