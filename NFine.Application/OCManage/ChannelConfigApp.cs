using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;

namespace NFine.Application.OCManage
{
    public class ChannelConfigApp
    {
        private IChannelConfigRepository service = new ChannelConfigRepository();
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
    }
}
