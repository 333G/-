 
using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL
{
    /// <summary>
    /// 站内信管理
    /// </summary>
    public partial class SMail_MessageManager
    {
        #region 单例模式

        private static SMail_MessageManager instance;
        private static object _lock = new object();

        private SMail_MessageManager()
        {
        }

        public static SMail_MessageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMail_MessageManager();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion 单例模式

        /*
        public List<SMail_Message> GetList(Pagination pagination, string queryJson)
        {
          return DAL.SMail_MessageDAL.Instance.GetList(pagination, queryJson);
        }
        */
        /// <summary>
        /// 查询所有可用的站内信数组
        /// </summary>
        /// <returns></returns>
   /*     public List<SMail_Message> GetList(Entity.Views.VBaseChannelParam query)
        {
            return DAL.SMail_MessageDAL.Instance.GetList(query);
        }
*/
        public SMail_Message Model(int id)
        {
            return DAL.SMail_MessageDAL.Instance.FindEntity(a => a.F_Id == id.ToString());
        }

        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="ids"></param> 
        /// <returns></returns>
        public List<bool> Delete(string[] ids)
        {
            List<SMail_Message> list = new List<SMail_Message>();
            foreach (string id in ids)
            {
                var model = Model(id.ToInt());
                if (model == null)
                    return null; 
                list.Add(model);
            }
            return DAL.SMail_MessageDAL.Instance.UpdateRange(list);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(SMail_Message model)
        {
            return DAL.SMail_MessageDAL.Instance.Update(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(SMail_Message model)
        {
            object obj = DAL.SMail_MessageDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<object> Add(List<SMail_Message> list)
        {
            return DAL.SMail_MessageDAL.Instance.Add(list);
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="userAccount">用户账号</param>
        /// <returns></returns>
        /*    public bool AddTran(Entity.Views.BaseChannelAddParam model)
            {
                var currentUser = NFine.Code.OperatorProvider.Provider.GetCurrent();
                return DAL.SMail_MessageDAL.Instance.AddTran(model, currentUser.UserCode);
            }
        */
    }
}