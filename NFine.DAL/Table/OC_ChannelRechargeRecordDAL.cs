using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Entity;
using AutoMapper;
using NFine.Code;

namespace NFine.DAL
{
    public partial class OC_ChannelRechargeRecordDAL
    {
        /// <summary>
        /// 事务提交
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="keyValue">通道ID</param>
        /// <returns></returns>
        public bool AddTran(Entity.Views.ChannelRechargeRecordAddParam model, int keyValue)
        {
            using (var db = DBHelper.GetWriteInstance())
            {
                bool result = false;
                db.CommandTimeOut = 3000;//设置超时时间
                try
                {
                    db.BeginTran(); //开启事务
                    OC_ChannelRechargeRecord channelModel = new OC_ChannelRechargeRecord();
                    //映射实体
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Entity.Views.ChannelRechargeRecordAddParam, OC_ChannelRechargeRecord>());
                    var mapper = config.CreateMapper();
                    var LoginInfo = OperatorProvider.Provider.GetCurrent();
                    channelModel = mapper.Map<OC_ChannelRechargeRecord>(model);
                   
                    channelModel.F_Id = Common.GuId();//通道编号
                    channelModel.F_ChannleId = keyValue;//通道ID
                    channelModel.F_CreatorTime = DateTime.Now;
                    channelModel.F_CreatorUserId = LoginInfo.UserId;
                    channelModel.F_LastModifyTime = DateTime.Now;
                    channelModel.F_LastModifyUserId = LoginInfo.UserId;                  
                    //插入充值记录到数据库
                    int channelId = db.Insert(channelModel).ToInt();
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
     }
}
