using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL.Self
{
    public class PheGrupBLL
    {

        #region 单例模式
        private static PheGrupBLL instance;
        private static object _lock = new object();

        private PheGrupBLL()
        {
        }

        public static PheGrupBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new PheGrupBLL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        public List<NFine.Entity.Views.VPheGrup> GetList(string keyword)
        {
            return DAL.Self.PheGrupDAL.Instance.GetList( keyword);
        }
    }
}
