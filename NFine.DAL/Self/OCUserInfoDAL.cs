using NFine.Code;
using NFine.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.DAL.Self
{
    public partial class OCUserInfoDAL
    {
        #region 单例模式
        private static OCUserInfoDAL instance;
        private static object _lock = new object();

        private OCUserInfoDAL()
        {
        }

        public static OCUserInfoDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OCUserInfoDAL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        public bool AddCRM(string F_UserFId, long F_CRMID)
        {
            using (var db = DBHelper.GetWriteInstance())
            {
                return db.Update<Entity.OC_UserInfo>(new { F_CRMId = F_CRMID }, it => it.F_Id == F_UserFId);
            }
        }
    }
}
