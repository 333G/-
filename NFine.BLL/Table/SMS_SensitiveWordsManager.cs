using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NFine.BLL
{
    /// <summary>
    /// 敏感词过滤库
    /// </summary>
    public partial class SMS_SensitiveWordsManager
    {
        #region 单例模式

        private static SMS_SensitiveWordsManager instance;
        private static object _lock = new object();

        private SMS_SensitiveWordsManager()
        {
        }

        public static SMS_SensitiveWordsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SMS_SensitiveWordsManager();
                        }
                    }
                }
                return instance;
            }
        }
        public List<SMS_SensitiveWords> GetList(Pagination pagination, string queryJson)
        {
            return DAL.SMS_SensitiveWordsDAL.Instance.GetList(pagination, queryJson);
        }
        public List<SMS_SensitiveWords> GetList(string F_SensitiveWords, string F_CreatorUserId)
        {
            return DAL.SMS_SensitiveWordsDAL.Instance.GetList(F_SensitiveWords, F_CreatorUserId);
        }
        public bool Update(SMS_SensitiveWords model)
        {
            return DAL.SMS_SensitiveWordsDAL.Instance.Update(model);
        }

        #endregion 单例模式

        /// <summary>
        /// 添加关键词
        /// </summary>
        /// <param name="content">添加敏感词</param>
        /// <returns></returns>
        public bool Add(string content)
        {
            SMS_SensitiveWords model = new SMS_SensitiveWords();
            model.F_SensitiveWords = content;
            model.F_CreatorTime = DateTime.Now;
            model.F_CreatorUserId = NFine.Code.OperatorProvider.Provider.GetCurrent().UserCode;
            return DAL.SMS_SensitiveWordsDAL.Instance.Add(model).ToInt() > 0;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public SMS_SensitiveWords Model(string id)
        {
            return DAL.SMS_SensitiveWordsDAL.Instance.FindEntity(a => a.F_Id == id);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="content">关键词</param>
        /// <returns></returns>
        public SMS_SensitiveWords ModelFromWords(string content)
        {
            return DAL.SMS_SensitiveWordsDAL.Instance.FindEntity(a => a.F_SensitiveWords == content);
        }

        /// <summary>
        /// 根据关键词删除实体
        /// </summary>
        /// <param name="content">关键词</param>
        /// <returns></returns>
        public bool DeleteFromWords(string content)
        {
            return DAL.SMS_SensitiveWordsDAL.Instance.Delete(a => a.F_SensitiveWords == content);
        }

        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public List<bool> Delete(string[] keyValue, int operatorId)
        {
            List<SMS_SensitiveWords> list = new List<SMS_SensitiveWords>();
            foreach (string F_Id in keyValue)
            {
                var model = Model(F_Id);
                if (model == null)
                    return null;
               
                model.F_DeleteTime = DateTime.Now;
                model.F_DeleteUserId = operatorId.ToString();
                list.Add(model);
            }
            return DAL.SMS_SensitiveWordsDAL.Instance.UpdateRange(list);
        }

        /// <summary>
        /// 所有
        /// </summary>
        /// <returns></returns>
        public List<SMS_SensitiveWords> AllList()
        {
            return DAL.SMS_SensitiveWordsDAL.Instance.FindList("SELECT F_Id,F_SensitiveWords FROM dbo.SMS_SensitiveWords(NOLOCK)");
        }
    }
}