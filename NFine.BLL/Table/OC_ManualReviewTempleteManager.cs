using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NFine.Entity;
using System.Linq.Expressions;

namespace NFine.BLL
{
    /// <summary>
    /// 人工免审模板管理
    /// </summary>
    public partial class OC_ManualReviewTempleteManager
    {

        #region 单例模式
        private static OC_ManualReviewTempleteManager instance;
        private static object _lock = new object();

        private OC_ManualReviewTempleteManager()
        {
        }

        public static OC_ManualReviewTempleteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OC_ManualReviewTempleteManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        public List<OC_ManualReviewTemplete> GetList(Pagination pagination, string queryJson)
        {
            return DAL.OC_ManualReviewTempleteDAL.Instance.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 查询所有可用的人工免审模板数组
        /// </summary>
        /// <param name="queryJson"></param> 
        /// <returns></returns>
        public List<OC_ManualReviewTemplete> GetList(string queryJson)
        {
            return DAL.OC_ManualReviewTempleteDAL.Instance.GetList(queryJson); 
        }

        public OC_ManualReviewTemplete Model(int id)
        {
            return DAL.OC_ManualReviewTempleteDAL.Instance.FindEntity(a => a.F_Id == id.ToString());
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public List<bool> Delete(string[] ids,int operatorId)
        {
            List<OC_ManualReviewTemplete> list = new List<OC_ManualReviewTemplete>();
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
            return DAL.OC_ManualReviewTempleteDAL.Instance.UpdateRange(list);
        }
    
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(OC_ManualReviewTemplete model)
        {
            return DAL.OC_ManualReviewTempleteDAL.Instance.Update(model);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(OC_ManualReviewTemplete model)
        {
            object obj=DAL.OC_ManualReviewTempleteDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<object> Add(List<OC_ManualReviewTemplete> list)
        {
            return DAL.OC_ManualReviewTempleteDAL.Instance.Add(list);
        }
    }
}
