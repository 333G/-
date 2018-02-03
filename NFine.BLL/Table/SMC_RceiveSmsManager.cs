using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using NFine.Code; 

using NFine.Entity;
using System.Linq.Expressions;

namespace NFine.BLL
{
    /// <summary>
    /// 接收短信管理
    /// </summary>
    public partial class SMC_RceiveSmsManager
    {

        #region 单例模式
        private static SMC_RceiveSmsManager instance;
        private static object _lock = new object();

        private SMC_RceiveSmsManager()
        {
        }

        public static SMC_RceiveSmsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMC_RceiveSmsManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        public List<SMC_ReplyMessage> GetList(Pagination pagination, string queryJson)
        {
            return DAL.SMC_RceiveSmsDAL.Instance.GetList(pagination, queryJson);
        }
        public List<SMC_ReplyMessage> GetList(string queryJson)
        {
             
                return DAL.SMC_RceiveSmsDAL.Instance.GetList(queryJson);
        }


        public SMC_ReplyMessage Model(int id)
        {
            return DAL.SMC_RceiveSmsDAL.Instance.FindEntity(a => a.F_Id == id.ToString());
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public List<bool> Delete(string[] ids, int operatorId)
        {
            List<SMC_ReplyMessage> list = new List<SMC_ReplyMessage>();
            foreach (string id in ids)
            {
                var model = Model(id.ToInt());
                if (model == null)
                    return null;
                model.F_DeleteMark = true;
                model.F_DeleteTime = DateTime.Now;
                model.F_DeleteUserId = operatorId.ToString();
                list.Add(model);
            }
            return DAL.SMC_RceiveSmsDAL.Instance.UpdateRange(list);
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(SMC_ReplyMessage model)
        {
            return DAL.SMC_RceiveSmsDAL.Instance.Update(model);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(SMC_ReplyMessage model)
        {
            object obj = DAL.SMC_RceiveSmsDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<object> Add(List<SMC_ReplyMessage> list)
        {
            return DAL.SMC_RceiveSmsDAL.Instance.Add(list);
        }
    }
}
