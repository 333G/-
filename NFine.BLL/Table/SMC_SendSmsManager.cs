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
    /// 发送短信管理
    /// </summary>
    public partial class SMC_SendSmsManager
    {

        #region 单例模式
        private static SMC_SendSmsManager instance;
        private static object _lock = new object();

        private SMC_SendSmsManager()
        {
        }

        public static SMC_SendSmsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMC_SendSmsManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
    
         public List<SMC_SendSms> GetList(Pagination pagination, string queryJson)
        {
            return DAL.SMC_SendSmsDAL.Instance.GetList(pagination, queryJson);
        }


        public List<SMC_SendSms> newGetList(Pagination pagination, string queryJson)
        {
            return DAL.SMC_SendSmsDAL.Instance.newGetList(pagination, queryJson);
        }

        public SMC_SendSms Model(int id)
        {
            return DAL.SMC_SendSmsDAL.Instance.FindEntity(a => a.F_Id == id.ToString());
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public List<bool> Delete(string[] ids, int operatorId)
        {
            List<SMC_SendSms> list = new List<SMC_SendSms>();
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
            return DAL.SMC_SendSmsDAL.Instance.UpdateRange(list);
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(SMC_SendSms model)
        {
            return DAL.SMC_SendSmsDAL.Instance.Update(model);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(SMC_SendSms model)
        {
            object obj = DAL.SMC_SendSmsDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<object> Add(List<SMC_SendSms> list)
        {
            return DAL.SMC_SendSmsDAL.Instance.Add(list);
        }
    }
}
