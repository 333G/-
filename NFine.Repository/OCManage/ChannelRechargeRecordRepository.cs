using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;

namespace NFine.Repository.OCManage
{
    public class ChannelRechargeRecordRepository : RepositoryBase<ChannelRechargeRecordEntity>, IChannelRechargeRecordRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    //db.Delete<BaseChannelEntity>(t => t.Id.ToString() == i);
                    //db.Delete<ChannelRechargeRecordEntity>(t => t.F_Id == i);
                db.Commit();
            }

        }
        public void SubmitForm(ChannelRechargeRecordEntity ChannelRechargeRecordEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(ChannelRechargeRecordEntity);
                }
                else
                {
                    db.Insert(ChannelRechargeRecordEntity);
                }
                db.Commit();
            }
        }
    }
}
