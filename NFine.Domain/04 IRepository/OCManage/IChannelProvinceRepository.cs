using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Domain.Entity.OCManage;
using NFine.Data;

namespace NFine.Domain.IRepository.OCManage
{
    public interface IChannelProvinceRepository : IRepositoryBase<ChannelProvinceEntity>
    {
        void DeleteForm(string keyValue);

    }
}
  
