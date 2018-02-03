
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;

namespace NFine.Repository.OCManage
{
    public class BaseChannelRepository : RepositoryBase<BaseChannelEntity>, IBaseChannelRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                //string[] keys = keyValue.Split(',');
                //foreach (string i in keys)
               
                db.Delete<BaseChannelEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
            
        }
        public void SubmitForm(BaseChannelEntity BaseChannelEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(BaseChannelEntity);
                }
                else
                {
                    db.Insert(BaseChannelEntity);
                }
                db.Commit();
            }
        }


        public void newSubmitForm(BaseChannelEntity basechannelEntity, ChannelProvinceEntity channelprovinceEntity, ChannelConfigEntity channelconfigEntity, string keyValue)
        {

            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(basechannelEntity);
                    db.Update(channelconfigEntity);
                }
                else
                {
                    channelconfigEntity.F_Id = basechannelEntity.F_Id;
                    //db.Insert(basechannelEntity);
                    db.Insert(channelconfigEntity);
                    //
                    //***
                    
                    //在这里默认插入34个省份通道
                }
                db.Commit();
            }
        }
    }


        
}
