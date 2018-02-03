using AutoMapper;
using NFine.Code;
using NFine.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.DAL
{
    /// <summary>
    /// 站内信管理
    /// </summary>
    public partial class SMail_MessageDAL
    {
        /// <summary>
        /// 查询所有可用的站内信数组
        /// </summary>
        /// <param name="queryJson">查询</param>
        /// <returns></returns>
   /*     public List<SMail_Message> GetList(Entity.Views.VBaseChannelParam query)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<SMail_Message>();

                if (!query.F_ChannelName.IsEmpty())
                    queryable.Where(a => a.F_ChannelName == query.F_ChannelName);
                 
                return queryable.ToList();
            }
        }
        */
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="userAccount">用户账号</param>
        /// <returns></returns>
  /*      public bool AddTran(Entity.Views.BaseChannelAddParam model, string userAccount)
        {
            using (var db = DBHelper.GetWriteInstance())
            {
                bool result = false;
                db.CommandTimeOut = 3000;//设置超时时间
                try
                {
                    db.BeginTran(); //开启事务
                    SMail_Message channelModel = new SMail_Message();
                    //映射实体
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Entity.Views.BaseChannelAddParam, SMail_Message>());
                    var mapper = config.CreateMapper();
                    channelModel = mapper.Map<SMail_Message>(model);

                    channelModel.F_IsShared = false;
                    channelModel.F_SharedSign = "";

                    channelModel.F_DeleteMark = false;
                    channelModel.F_EnabledMark = true;
                    channelModel.F_CreatorTime = DateTime.Now;
                    channelModel.Creator = userAccount;
                    //添加通道基础配置
                    int channelId = db.Insert(channelModel).ToInt();

                    OC_ChannelConfig channelConfigModel = new OC_ChannelConfig();
                    //映射实体
                    var config2 = new MapperConfiguration(cfg => cfg.CreateMap<Entity.Views.BaseChannelAddParam, OC_ChannelConfig>());
                    var mapper2 = config2.CreateMapper();
                    channelConfigModel = mapper2.Map<OC_ChannelConfig>(model);
                    channelConfigModel.ChannelId = channelId;
                    channelConfigModel.CreateTime = DateTime.Now;
                    channelConfigModel.Creator = userAccount;
                    //添加通道接口配置
                    db.Insert(channelConfigModel);

                    db.CommitTran();//提交事务
                    result = true;
                }
                catch (Exception ex)
                {
                    db.RollbackTran();
                    result = false;
                    NFine.Log.LogHelper.GetLogManager.Error(ex.StackTrace);
                    throw ex;
                }
                return result;
            }
        }
        */
    }
}