using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using System.Data;

namespace NFine.Application.OCManage
{
    public class ChannelRechargeRecordApp
    {
        private IChannelRechargeRecordRepository service = new ChannelRechargeRecordRepository();
        public void SubmitForm(ChannelRechargeRecordEntity ChannelRechargeRecordEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                ChannelRechargeRecordEntity.newCreate(keyValue);
            }
            else
            {
                ChannelRechargeRecordEntity.newModify(keyValue);
            }
            service.SubmitForm(ChannelRechargeRecordEntity, keyValue);
        }
    }
}
