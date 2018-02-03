using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Entity;

namespace NFine.BLL
{
    public partial class OC_ChannelRechargeRecordManager
    {
        private static OC_ChannelRechargeRecordManager instance;
        private static object _lock = new object();

        private OC_ChannelRechargeRecordManager()
        {
        }

        public static OC_ChannelRechargeRecordManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OC_ChannelRechargeRecordManager();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(OC_ChannelRechargeRecord model)
        {
            object obj = DAL.OC_ChannelRechargeRecordDAL.Instance.Add(model);
            return Convert.ToBoolean(obj);
        }
        public bool AddTran(Entity.Views.ChannelRechargeRecordAddParam model,int keyValue)
        {
            var currentUser = NFine.Code.OperatorProvider.Provider.GetCurrent();
            return DAL.OC_ChannelRechargeRecordDAL.Instance.AddTran(model, keyValue);
        }
    }
}
