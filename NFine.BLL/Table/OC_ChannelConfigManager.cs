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
    /// 通道配置
    /// </summary>
     public partial class OC_ChannelConfigManager
    {
        #region 单例子模式
        private static OC_ChannelConfigManager instance;
        private static object _lock = new object();

        private OC_ChannelConfigManager()
        {
        }

        public static OC_ChannelConfigManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OC_ChannelConfigManager();

                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 根据F_Id查询单个实体（因为Id和BaseChannel中的Id不统一。F_Id是一致的。
        /// </summary>
        /// <param name="keyValue">F_Id</param>
        /// <returns></returns>
        public OC_ChannelConfig Model(string keyValue)
        {
            return DAL.OC_ChannelConfigDAL.Instance.FindEntity(a => a.F_Id == keyValue);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        public bool Update(OC_ChannelConfig model)
        {
            return DAL.OC_ChannelConfigDAL.Instance.Update(model);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        public int Add(OC_ChannelConfig model)
        {
            object obj = DAL.OC_ChannelConfigDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }
    }
}