using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.OCManage
{
    public class ChannelProvinceRepository : RepositoryBase<ChannelProvinceEntity>, IChannelProvinceRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                
                db.Delete<ChannelProvinceEntity>(t => t.F_Id == keyValue);

                db.Commit();
            }

        }
    }
}
