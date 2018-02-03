using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL
{
    /// <summary>
    /// 通道列表管理
    /// </summary>
    public partial class OC_BaseChannelManager
    {
        #region 单例模式

        private static OC_BaseChannelManager instance;
        private static object _lock = new object();

        private OC_BaseChannelManager()
        {
        }

        public static OC_BaseChannelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OC_BaseChannelManager();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion 单例模式

        public List<OC_BaseChannel> GetList(Pagination pagination, string queryJson)
        {
            return DAL.OC_BaseChannelDAL.Instance.GetList(pagination, queryJson);
        }

        /// <summary>
        /// 查询所有可用的通道列表数组
        /// </summary>
        /// <returns></returns>
        //public List<OC_BaseChannel> GetList(Entity.Views.VBaseChannelParam query)
        //{
        //    return DAL.OC_BaseChannelDAL.Instance.GetList(query);
        //}
        public List<OC_BaseChannel> GetList(string F_ChannelName, string F_Operator, string F_ChannelState)
        {
            return DAL.OC_BaseChannelDAL.Instance.GetList(F_ChannelName, F_Operator, F_ChannelState);
        }

        //获取单个实例
        public OC_BaseChannel Model(int keyValue)
        {
            return DAL.OC_BaseChannelDAL.Instance.FindEntity(a => a.Id == keyValue);
        }

        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public List<bool> Delete(string[] keyValue, int operatorId)
        {
            List<OC_BaseChannel> list = new List<OC_BaseChannel>();
            foreach (string Id in keyValue)
            {
                var model = Model(Id.ToInt());
                if (model == null)
                    return null;
                model.F_DeleteMark = true;
                model.F_DeleteTime = DateTime.Now;
                model.F_DeleteUserId = operatorId.ToString();
                list.Add(model);
            }
            return DAL.OC_BaseChannelDAL.Instance.UpdateRange(list);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(OC_BaseChannel model)
        {
            return DAL.OC_BaseChannelDAL.Instance.Update(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(OC_BaseChannel model)
        {
            object obj = DAL.OC_BaseChannelDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<object> Add(List<OC_BaseChannel> list)
        {
            return DAL.OC_BaseChannelDAL.Instance.Add(list);
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="userAccount">用户账号</param>
        /// <returns></returns>
        public bool AddTran(Entity.Views.BaseChannelAddParam model)
        {
            var currentUser = NFine.Code.OperatorProvider.Provider.GetCurrent();
            return DAL.OC_BaseChannelDAL.Instance.AddTran(model, currentUser.UserCode);
        }
    }
}