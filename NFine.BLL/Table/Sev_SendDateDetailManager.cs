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
    public partial class Sev_SendDateDetailManager
    {

        #region 单例模式
        private static Sev_SendDateDetailManager instance;
        private static object _lock = new object();

        private Sev_SendDateDetailManager()
        {
        }

        public static Sev_SendDateDetailManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new Sev_SendDateDetailManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        public List<NFine.Entity.Views.VSevFinalSendDetail> GetReceiveList(Pagination pagination, string queryJson)
        {
            return DAL.Self.SevSendDateDetailDAL.Instance.GetReceiveList(pagination, queryJson);
        }
        public List<NFine.Entity.Views.VSevFinalSendDetail> BuffeDataGetList(Pagination pagination, string queryJson)
        {
            return DAL.Self.SevSendDateDetailDAL.Instance.BuffeDataGetList(pagination, queryJson);
        }
        public List<NFine.Entity.Views.VSevFinalSendDetail> DailyDataGetList(Pagination pagination, string queryJson)
        {
            return DAL.Self.SevSendDateDetailDAL.Instance.DailyDataGetList(pagination, queryJson);
        }

        public List<Sev_BusinessDataAnalysis> BusinessDataAnalysis(Pagination pagination, string queryJson)
        {
            return DAL.Self.SevSendDateDetailDAL.Instance.BusinessDataAnalysis(pagination, queryJson);
        }
        public List<Sev_DailyChannelData> ChannelDataAnalysis(Pagination pagination, string queryJson)
        {
            return DAL.Self.SevSendDateDetailDAL.Instance.ChannelDataAnalysis(pagination, queryJson);
        }
        //public List<Sev_FinalSendDetail> Sev_FinalSendDetailGetList(Pagination pagination, string queryJson)
        //{
        //    return DAL.Sev_SendDateDetailDAL.Instance.Sev_FinalSendDetailGetListGetList(pagination, queryJson);
        //}
        public List<Sev_SendDateDetail> GetList(Pagination pagination, string queryJson)
        {
            return DAL.Sev_SendDateDetailDAL.Instance.GetList(pagination, queryJson);
        }
        //public List<Sev_SendDateDetail> BuffeDataGetList(Pagination pagination, string queryJson)
        //{
        //    return DAL.Sev_SendDateDetailDAL.Instance.BuffeDataGetList(pagination, queryJson);
        //}
        //public List<Sev_SendDateDetail> DailyDataGetList(Pagination pagination, string queryJson)
        //{
        //    return DAL.Sev_SendDateDetailDAL.Instance.DailyDataGetList(pagination, queryJson);
        //}
        /// <summary>
        /// 查询所有发送数据详单数组
        /// </summary>

        public List<Sev_SendDateDetail> GetList(string F_UserId, string F_BlackWhite, string F_ChannelId, /*DateTime? F_SendTime,*/ string F_PhoneCode, string F_Operator, string F_Province, string F_Report, string F_Synchro, string F_Buckle, string F_Reissue, /*string F_Level,*/ string F_SmsContent)
        {
            return DAL.Sev_SendDateDetailDAL.Instance.FindList(F_UserId, F_BlackWhite, F_ChannelId, /*F_SendTime,*/ F_PhoneCode, F_Operator, F_Province, F_Report, F_Synchro, F_Buckle, F_Reissue, /*F_Level,*/ F_SmsContent);
        }

        public Sev_SendDateDetail Model(string id)
        {
            return DAL.Sev_SendDateDetailDAL.Instance.FindEntity(a => a.F_Id == id);
            //return DAL.Sev_SendDateDetailDAL.Instance.FindEntity(a => a.Id == id);
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public List<bool> Delete(string[] ids, int operatorId)
        {
            List<Sev_SendDateDetail> list = new List<Sev_SendDateDetail>();
            foreach (string id in ids)
            {
                var model = Model(id);
                if (model == null)
                    return null;
                model.F_CreatorTime = DateTime.Now;
                list.Add(model);
            }
            return DAL.Sev_SendDateDetailDAL.Instance.UpdateRange(list);
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(Sev_SendDateDetail model)
        {
            return DAL.Sev_SendDateDetailDAL.Instance.Update(model);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(Sev_SendDateDetail model)
        {
            object obj = DAL.Sev_SendDateDetailDAL.Instance.Add(model);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="list">数组</param>
        /// <returns></returns>
        public List<object> Add(List<Sev_SendDateDetail> list)
        {
            return DAL.Sev_SendDateDetailDAL.Instance.Add(list);
        }

        //public List<Sev_SendDateDetail> GetList(string f_UserId, string f_PhoneCode)
        //{
        //    throw new NotImplementedException();
        //}




        public Sev_FinalSendDetail Sev_FinalSendDetailModel(int id)
        {
            return DAL.Sev_FinalSendDetailDAL.Instance.FindEntity(a => a.Id == id);
        }
        public Sev_FinalSendDetail newSev_FinalSendDetailModel(string keyvalue)
        {
            return DAL.Sev_FinalSendDetailDAL.Instance.FindEntity(a => a.F_Id == keyvalue);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Sev_FinalSendDetailUpdate(Sev_FinalSendDetail model)
        {
            return DAL.Sev_FinalSendDetailDAL.Instance.Update(model);
        }
        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public List<bool> Sev_FinalSendDetailDelete(string[] ids, int operatorId)
        {
            List<Sev_FinalSendDetail> list = new List<Sev_FinalSendDetail>();
            foreach (string id in ids)
            {
                var model = Sev_FinalSendDetailModel(id.ToInt());
                if (model == null)
                    return null;
                list.Add(model);
            }
            return DAL.Sev_FinalSendDetailDAL.Instance.UpdateRange(list);
        }
    }
}
