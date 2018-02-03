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
    /// 通讯录用户管理
    /// </summary>
    public partial class TXL_PhoneInfoManager
    {

        #region 单例模式
        private static TXL_PhoneInfoManager instance;
        private static object _lock = new object();

        private TXL_PhoneInfoManager()
        {
        }

        public static TXL_PhoneInfoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new TXL_PhoneInfoManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        public List<TXL_PhoneInfo> GetList(Pagination pagination, string queryJson)
        {
            return DAL.TXL_PhoneInfoDAL.Instance.GetList(pagination, queryJson);
        }

        /// <summary>
        /// 查询所有可用的通讯录人员数组
        /// </summary>
        /// <param name="GroupId">分组ID</param> 
        /// <param name="Sex">性别</param> 
        /// <param name="Operator">运营商</param> 
        /// <param name="State">状态</param> 
        /// <param name="keyword">关键词</param> 
        /// <returns></returns>
        public List<TXL_PhoneInfo> GetList(string queryJson)
        {
            return DAL.TXL_PhoneInfoDAL.Instance.GetList(queryJson);
        }
        /*
                public TXL_PhoneInfo Model(int id)
                {
                    return DAL.TXL_PhoneInfoDAL.Instance.FindEntity(a => a.F_Id == id);
                }
                /// <summary>
                /// 批量
                /// </summary>
                /// <param name="ids"></param>
                /// <param name="operatorId"></param>
                /// <returns></returns>
                public List<bool> Delete(string[] ids,int operatorId)
                {
                    List<TXL_PhoneInfo> list = new List<TXL_PhoneInfo>();
                    foreach (string id in ids)
                    {
                        var model = Model(id.ToInt());
                        if (model == null)
                            return null;
                        model.F_DeleteMark = true;
                        model.F_DeleteTime = DateTime.Now;
                     //   model.F_DeleteUserId = operatorId;
                        list.Add(model);
                    }
                    return DAL.TXL_PhoneInfoDAL.Instance.UpdateRange(list);
                }
              */
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(TXL_PhoneInfo model)
        {
            return DAL.TXL_PhoneInfoDAL.Instance.Update(model);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(TXL_PhoneInfo model)
        {
            object obj = DAL.TXL_PhoneInfoDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<object> Add(List<TXL_PhoneInfo> list)
        {
            return DAL.TXL_PhoneInfoDAL.Instance.Add(list);
        }
    }
}
