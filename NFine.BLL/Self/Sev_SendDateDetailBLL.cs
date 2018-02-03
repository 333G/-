using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL.Self
{
    public class Sev_SendDateDetailBLL
    {
        #region 单例模式
        private static Sev_SendDateDetailBLL instance;
        private static object _lock = new object();

        private Sev_SendDateDetailBLL()
        {
        }

        public static Sev_SendDateDetailBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Sev_SendDateDetailBLL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        public List<NFine.Entity.Views.VSevFinalSendDetail> GetReceiveList(Pagination pagination, string queryJson)
        {
            return DAL.Self.SevSendDateDetailDAL.Instance.GetReceiveList(pagination, queryJson);
        }
    }
}
