using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NFine.BLL.Self
{
    public class CustomersMessageBLL
    {
        
        #region 单例模式
        private static CustomersMessageBLL instance;
        private static object _lock = new object();

        private CustomersMessageBLL()
        {
        }

        public static CustomersMessageBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new CustomersMessageBLL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式


        public List<NFine.Entity.Views.VCustomersMessage> GetList(string queryJson)
        {
            return DAL.Self.CustomersMessageDAL.Instance.GetList(queryJson);
        }

        public bool Delete(string keyValue)
        {
            return DAL.Self.CustomersMessageDAL.Instance.Delete(keyValue);
        }
    }
}
