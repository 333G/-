using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL.Self
{
   public class SMC_RceiveSmsBLL
    {
        #region 单例模式
        private static SMC_RceiveSmsBLL instance;
        private static object _lock = new object();

        private SMC_RceiveSmsBLL()
        {
        }

        public static SMC_RceiveSmsBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMC_RceiveSmsBLL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        public List<NFine.Entity.Views.VSMCRceiveSms> GetReceiveList(Pagination pagination, string queryJson)
        {
            return DAL.Self.SMCRceiveSmsDAL.Instance.GetReceiveList(pagination, queryJson);
        }
    }
}
