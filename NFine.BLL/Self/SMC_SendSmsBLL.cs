using NFine.Code;
using NFine.Entity;
using NFine.Entity.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL.Self
{
    public class SMC_SendSmsBLL
    {
        #region 单例模式
        private static SMC_SendSmsBLL instance;
        private static object _lock = new object();

        private SMC_SendSmsBLL()
        {
        }

        public static SMC_SendSmsBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMC_SendSmsBLL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        public List<NFine.Entity.Views.VSMCSendSms> GetReceiveList(Pagination pagination, string queryJson)
        {
            return DAL.Self.SMCSendSmsDAL.Instance.GetReceiveList(pagination, queryJson);
        }
    }
}
