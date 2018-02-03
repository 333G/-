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
    /// 通道列表管理
    /// </summary>
    public partial class OC_BaseChannelDAL
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<OC_BaseChannel> GetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<OC_BaseChannel>().Where(t => t.F_DeleteMark != true && t.F_EnabledMark !=false);
                var queryParam = queryJson.ToJObject();
                if (!queryParam["F_ChannelName"].IsEmpty())
                {
                    string F_ChannelName = queryParam["F_ChannelName"].ToString();
                    queryable.Where(a => a.F_ChannelName == F_ChannelName);
                }
                if (!queryParam["F_Operator"].IsEmpty())
                {
                    int F_Operator = queryParam["F_Operator"].ToInt();
                    queryable.Where(t => t.F_Operator == F_Operator);
                }
                if (!queryParam["F_ChannelState"].IsEmpty())
                {
                    int? F_ChannelState = queryParam["F_ChannelState"].ToInt();
                    queryable.Where(t => t.F_ChannelState == F_ChannelState);
                }

                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }




        //查询所有可用的通道列表数组  （导出功能）
        public List<OC_BaseChannel> GetList(string F_ChannelName, string F_Operator, string F_ChannelState)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<OC_BaseChannel>();
                
                if (!F_ChannelName.IsEmpty())
                {                    
                    queryable.Where(a => a.F_ChannelName == F_ChannelName);
                }
                if (!F_Operator.IsEmpty())
                {                    
                    queryable.Where(t => t.F_Operator == F_Operator.ToInt());
                }
                if (!F_ChannelState.IsEmpty())
                {                    
                    queryable.Where(t => t.F_ChannelState == F_ChannelState.ToInt());
                }
                return queryable.ToList();
            }
        }
        /// <summary>
        /// 查询所有可用的通道列表数组
        /// </summary>
        /// <param name="queryJson">查询</param>
        /// <returns></returns>
        public List<OC_BaseChannel> GetList(Entity.Views.VBaseChannelParam query)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<OC_BaseChannel>();

                if (!query.F_ChannelName.IsEmpty())
                    queryable.Where(a => a.F_ChannelName == query.F_ChannelName);
                if (!query.F_Operator.IsEmpty())
                    queryable.Where(t => t.F_Operator == query.F_Operator);
                if (query.F_ChannelState != null)
                    queryable.Where(t => t.F_ChannelState == query.F_ChannelState.ToInt());

                //if (!query.F_UrlType.IsEmpty())
                //    queryable.Where(a => a. == query.F_UrlType);
                //if (query.F_ChaBalance_Up != null)
                //    queryable.Where(a => a.F_ChaBalance < query.F_ChaBalance_Up);
                //if (query.F_ChaBalance_Down != null)
                //    queryable.Where(a => a.F_ChaBalance > query.F_ChaBalance_Down);
                return queryable.ToList();
            }
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="userAccount">用户账号</param>
        /// <returns></returns>
        public bool AddTran(Entity.Views.BaseChannelAddParam model, string userAccount)
        {
            using (var db = DBHelper.GetWriteInstance())
            {
                bool result = false;
                db.CommandTimeOut = 3000;//设置超时时间
                try
                {
                    db.BeginTran(); //开启事务
                    OC_BaseChannel channelModel = new OC_BaseChannel();
                    //映射实体
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Entity.Views.BaseChannelAddParam, OC_BaseChannel>());
                    var mapper = config.CreateMapper();
                    channelModel = mapper.Map<OC_BaseChannel>(model);

                    channelModel.F_IsShared = false;
                    channelModel.F_SharedSign = "";

                    channelModel.F_DeleteMark = false;
                    channelModel.F_EnabledMark = true;
                    channelModel.F_CreatorTime = DateTime.Now;
                    channelModel.F_CreatorUserId = userAccount;
                    //添加通道基础配置

                    int channelId = db.Insert(channelModel).ToInt();                    
             
                    OC_ChannelConfig channelConfigModel = new OC_ChannelConfig();
                    //映射实体
                    var config2 = new MapperConfiguration(cfg => cfg.CreateMap<Entity.Views.BaseChannelAddParam, OC_ChannelConfig>());
                    var mapper2 = config2.CreateMapper();
                    channelConfigModel = mapper2.Map<OC_ChannelConfig>(model);
                    channelConfigModel.F_ChannelId = channelId;
                    channelConfigModel.F_CreatorTime = DateTime.Now;
                    channelConfigModel.F_CreatorUserId = userAccount;
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
    }
}