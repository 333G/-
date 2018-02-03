using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Data;
using NFine.Domain.Entity.OCManage;

namespace NFine.Domain.IRepository.OCManage
{
    public interface IChannelRechargeRecordRepository : IRepositoryBase<ChannelRechargeRecordEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(ChannelRechargeRecordEntity ChannelRechargeRecordEntity, string keyValue);
    }
}




