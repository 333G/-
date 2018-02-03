using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL
{
    public partial class OC_RechargeRecordManager
    {
        #region 单例模式
        private static OC_RechargeRecordManager instance;
        private static object _lock = new object();

        private OC_RechargeRecordManager()
        {
        }

        public static OC_RechargeRecordManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OC_RechargeRecordManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        public List<OC_RechargeRecord> GetList(Pagination pagination, string queryJson)
        {
            return DAL.OC_RechargeRecordDAL.Instance.GetList(pagination, queryJson);
        }
    }
}
