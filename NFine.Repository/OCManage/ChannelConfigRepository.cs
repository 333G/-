using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.OCManage
{
    public class ChannelConfigRepository : RepositoryBase<ChannelConfigEntity>, IChannelConfigRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ChannelConfigEntity>(t => t.F_Id == keyValue);

                db.Commit();
            }

        }
    }
}
