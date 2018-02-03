using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL.Self
{
    public class SMailMessageBLL
    {
        
        #region 单例模式
        private static SMailMessageBLL instance;
        private static object _lock = new object();

        private SMailMessageBLL()
        {
        }

        public static SMailMessageBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMailMessageBLL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        public List<NFine.Entity.Views.VSMailMessage> GetReply(string keyValue)
        {
            return DAL.Self.SMailMessageDAL.Instance.GetReply(keyValue);
        }
        public List<NFine.Entity.Views.VSMailMessage> GetReceiveList(string userId,  string queryJson)
        {
            return DAL.Self.SMailMessageDAL.Instance.GetReceiveList(userId,  queryJson);
        }
        public List<NFine.Entity.Views.VSMailMessage> GetSendList(string userId)
        {
            return DAL.Self.SMailMessageDAL.Instance.GetSendList(userId);
        }
        public bool Delete(string keyValue)
        {
            return DAL.Self.SMailMessageDAL.Instance.Delete(keyValue);
        }
    }
}
