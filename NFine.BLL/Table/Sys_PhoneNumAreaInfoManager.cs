using NFine.Code;
using NFine.Entity;
using NFine.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL
{
    public partial class Sys_PhoneNumAreaInfoManager
    {
        #region 单例模式

        private static Sys_PhoneNumAreaInfoManager instance;
        private static object _lock = new object();

        private Sys_PhoneNumAreaInfoManager()
        {
        }

        public static Sys_PhoneNumAreaInfoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Sys_PhoneNumAreaInfoManager();
                        }
                    }
                }
                return instance;
            }
        }
        public List<Sys_PhoneNumAreaInfo> GetList(Pagination pagination, string queryJson)
        {
            return DAL.Sys_PhoneNumAreaInfoDAL.Instance.GetList(pagination, queryJson);
        }
        #endregion 单例模式
    }
}
