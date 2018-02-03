using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using NFine.Domain.Entity.OCManage;


namespace NFine.Application.OCManage
{
    public class ChannelProvinceApp
    {
        private IChannelProvinceRepository service = new ChannelProvinceRepository();
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void UpdataForm(ChannelProvinceEntity channelProvinceentity)
        {
            service.Update(channelProvinceentity);
        }
    }
}
