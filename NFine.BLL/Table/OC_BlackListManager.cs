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
    /// 黑白名单管理
    /// </summary>
    public partial class OC_BlackListManager
    {

        #region 单例模式
        private static OC_BlackListManager instance;
        private static object _lock = new object();

        private OC_BlackListManager()
        {
        }

        public static OC_BlackListManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OC_BlackListManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        public List<OC_BlackList> GetList(Pagination pagination, string queryJson)
        {
            return DAL.OC_BlackListDAL.Instance.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 查询所有可用的黑白名单数组
        /// </summary>
        /// <param name="F_Mobile">手机号</param>
        /// <param name="F_Sign">名单类型</param>
        /// <param name="F_Level">级别</param>
        /// <returns></returns>
        public List<OC_BlackList> GetList(string F_Mobile, string F_Sign, string F_Level)
        {
            return DAL.OC_BlackListDAL.Instance.GetList(F_Mobile,F_Sign,F_Level);
        }

        public OC_BlackList Model(int id)
        {
            return DAL.OC_BlackListDAL.Instance.FindEntity(a => a.Id == id);
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public List<bool> Delete(string[] ids,int operatorId)
        {
            List<OC_BlackList> list = new List<OC_BlackList>();
            foreach (string id in ids)
            {
                var model = Model(id.ToInt());
                if (model == null)
                    return null;
                model.F_DeleteMark = true;
                model.F_DeleteTime = DateTime.Now;
                model.F_DeleteUserId = operatorId;
                list.Add(model);
            }
            return DAL.OC_BlackListDAL.Instance.UpdateRange(list);
        }
        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        public bool CheckMobile(string mobile)
        {
            return DAL.OC_BlackListDAL.Instance.Exists(a => a.F_DeleteMark==false&&a.F_EnabledMark==true && a.F_Mobile == mobile);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(OC_BlackList model)
        {
            return DAL.OC_BlackListDAL.Instance.Update(model);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(OC_BlackList model)
        {
            object obj=DAL.OC_BlackListDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<object> Add(List<OC_BlackList> list)
        {
            return DAL.OC_BlackListDAL.Instance.Add(list);
        }
    }
}
