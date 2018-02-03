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
    /// 通道列表管理
    /// </summary>
    public partial class OC_AutoReviewTempleteManager
    {

        #region 单例模式
        private static OC_AutoReviewTempleteManager instance;
        private static object _lock = new object();

        private OC_AutoReviewTempleteManager()
        {
        }

        public static OC_AutoReviewTempleteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OC_AutoReviewTempleteManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        public List<OC_AutoReviewTemplete> GetList(Pagination pagination, string queryJson)
        {
            return DAL.OC_AutoReviewTempleteDAL.Instance.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 查询所有可用的通道列表数组
        /// </summary>
        /// <param name="F_Mobile">手机号</param>
        /// <param name="F_Sign">名单类型</param>
        /// <param name="F_Level">级别</param>
        /// <returns></returns>
        public List<OC_AutoReviewTemplete> GetList(string queryJson)
        {
            return DAL.OC_AutoReviewTempleteDAL.Instance.GetList(queryJson); 
        }

        public OC_AutoReviewTemplete Model(int id)
        {
            return DAL.OC_AutoReviewTempleteDAL.Instance.FindEntity(a => a.F_Id == id.ToString());
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public List<bool> Delete(string[] ids,int operatorId)
        {
            List<OC_AutoReviewTemplete> list = new List<OC_AutoReviewTemplete>();
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
            return DAL.OC_AutoReviewTempleteDAL.Instance.UpdateRange(list);
        }
    
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(OC_AutoReviewTemplete model)
        {
            return DAL.OC_AutoReviewTempleteDAL.Instance.Update(model);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(OC_AutoReviewTemplete model)
        {
            object obj=DAL.OC_AutoReviewTempleteDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<object> Add(List<OC_AutoReviewTemplete> list)
        {
            return DAL.OC_AutoReviewTempleteDAL.Instance.Add(list);
        }
    }
}
