using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SqlSugar;

namespace NFine.DAL
{
    /// <summary>
    /// 发送数据详单
    /// </summary>
    public partial class Sev_SendDateDetailDAL
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<Sev_SendDateDetail> GetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<Sev_SendDateDetail>();
                var queryParam = queryJson.ToJObject();
                if (!queryParam["F_UserId"].IsEmpty())
                {
                    int F_UserId = queryParam["F_UserId"].ToInt();
                    queryable.Where(a => a.F_UserId == F_UserId);
                }
                if (!queryParam["F_BlackWhite"].IsEmpty())
                {
                    int F_BlackWhite = queryParam["F_BlackWhite"].ToInt();
                    queryable.Where(a => a.F_BlackWhite == F_BlackWhite);
                }
                if (!queryParam["F_ChannelId"].IsEmpty())
                {
                    int F_ChannelId = queryParam["F_ChannelId"].ToInt();
                    queryable.Where(a => a.F_ChannelId == F_ChannelId);
                }
                if (!(queryParam["F_CreatorTimeFrom"].IsEmpty() || queryParam["F_CreatorTimeEnd"].IsEmpty()))
                {
                    DateTime F_CreatorTimeFrom = (DateTime)queryParam["F_CreatorTimeFrom"];
                    DateTime F_CreatorTimeEnd = (DateTime)queryParam["F_CreatorTimeEnd"];
                    queryable.Where(t => ((t.F_SendTime >= F_CreatorTimeFrom) && t.F_SendTime <= F_CreatorTimeEnd));
                }
                if (!queryParam["F_PhoneCode"].IsEmpty())
                {
                    string F_PhoneCode = queryParam["F_PhoneCode"].ToString();
                    queryable.Where(a => a.F_PhoneCode == F_PhoneCode);
                }
                if (!queryParam["F_SmsContent"].IsEmpty())
                {
                    string F_SmsContent = queryParam["F_SmsContent"].ToString();
                    queryable.Where(a => a.F_SmsContent.Contains(F_SmsContent));
                }
                if (!queryParam["F_Province"].IsEmpty())
                {
                    string F_Province = queryParam["F_Province"].ToString();
                    string str = F_Province.Substring(0, 2);//截取省邮编的前两个字符用来匹配该省的所有市
                    queryable.Where(a => a.F_Province.Contains(str));
                }
                //if (!queryParam["F_Report"].IsEmpty())
                //{
                //    int F_Report = queryParam["F_Report"].ToInt();
                //    queryable.Where(a => a.F_Report == F_Report);
                //}
                if (!queryParam["F_Operator"].IsEmpty())
                {
                    int F_Operator = queryParam["F_Operator"].ToInt();
                    queryable.Where(a => a.F_Operator == F_Operator);
                }
                if (!queryParam["F_Synchro"].IsEmpty())
                {
                    int F_Synchro = queryParam["F_Synchro"].ToInt();
                    queryable.Where(a => a.F_Synchro == F_Synchro);
                }
                if (!queryParam["F_Buckle"].IsEmpty())
                {
                    int F_Buckle = queryParam["F_Buckle"].ToInt();
                    queryable.Where(a => a.F_Buckle == F_Buckle);
                }
                //if (!queryParam["F_Reissue"].IsEmpty())
                //{
                //    int F_Reissue = queryParam["F_Reissue"].ToInt();
                //    queryable.Where(a => a.F_Reissue == F_Reissue);
                //}
                if (!queryParam["F_Level"].IsEmpty())
                {
                    int F_Level = queryParam["F_Level"].ToInt();
                    queryable.Where(a => a.F_Level == F_Level);
                }
                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }

        private DateTime[] GetDateInterval(string queryJson)
        {
            DateTime StartTime;
            DateTime EndTime;
            var queryParam = queryJson.ToJObject();
            string F_UserFId = queryParam["F_UserFId"].ToString();
            if (!queryParam["StartTime"].IsEmpty())
            {
                StartTime = queryParam["StartTime"].ToDate();
                if (StartTime < DateTime.Now.AddMonths(-3))
                {
                    StartTime = DateTime.Now.AddMonths(-3);//三个月数据限定
                }
                if (queryParam["EndTime"].IsEmpty())
                {
                    EndTime = DateTime.Now.AddDays(-1);//查询截止的时间是查询时间的前一天
                }
                else
                    EndTime = queryParam["EndTime"].ToDate().AddDays(-1);//查询截止的时间是查询时间的前一天

            }
            else
            {
                if (queryParam["EndTime"].IsEmpty())
                {
                    StartTime = DateTime.Now.AddDays(-6);//默认三天
                    EndTime = DateTime.Now.AddDays(-1);//查询截止的时间是查询时间的前一天
                }
                else
                {
                    StartTime = DateTime.Now.AddMonths(-3);//最多查询三个月数据
                    EndTime = queryParam["EndTime"].ToDate().AddDays(-1);// 查询截止的时间是查询时间的前一天
                }
            }
            DateTime[] DateInterval = { StartTime, EndTime };
            return DateInterval;
        }

        //数据分析总数据获取
        public Dictionary<string, Dictionary<string, decimal>> GetAnaliseMainDataList(string queryJson)
        {
            DateTime[] DateInterval= GetDateInterval(queryJson);
            DateTime StartTime= DateInterval[0];
            DateTime EndTime= DateInterval[1];
            var queryParam = queryJson.ToJObject();
            string F_UserFId = queryParam["F_UserFId"].ToString();
            TimeSpan DayCount= EndTime - StartTime;
            return GetReturnData(StartTime, EndTime, F_UserFId, DayCount.Days);
        }

        private Dictionary<string, Dictionary<string, decimal>> GetReturnData(DateTime StartTime,DateTime EndTime,string UserFId,int DayCounts)
        {
            Dictionary<string, decimal> MobileDic = new Dictionary<string, decimal>();
            Dictionary<string, decimal> UnicomDic = new Dictionary<string, decimal>();
            Dictionary<string, decimal> TelecomDic = new Dictionary<string, decimal>();
            Dictionary<string, decimal> CountDic = new Dictionary<string, decimal>();
            Dictionary<string, Dictionary<string, decimal>> Dic = new Dictionary<string, Dictionary<string, decimal>>();
            string STime = StartTime.ToDateString();
            string ETime = EndTime.ToDateString();
            try
            {
                using (var db = DBHelper.GetReadInstance())
                {
                    var MobileList = db.SqlQuery<Sev_SendDateDetail>("select F_Price ,F_SendTime from Sev_SendDateDetail where F_CreatorUserId='"+ UserFId
                        +"'and F_Operator=1 and F_SendTime>'"+ STime + "'and F_SendTime<'" + ETime + "' ");
                    var UnicomList = db.SqlQuery<Sev_SendDateDetail>("select F_Price ,F_SendTime from Sev_SendDateDetail where F_CreatorUserId='" + UserFId
                        + "'and F_Operator=2 and F_SendTime>'" + STime + "'and F_SendTime<'" + ETime + "' ");
                    var TelecomList = db.SqlQuery<Sev_SendDateDetail>("select F_Price ,F_SendTime from Sev_SendDateDetail where F_CreatorUserId='" + UserFId
                        + "'and F_Operator=3 and F_SendTime>'" + STime + "'and F_SendTime<'" + ETime + "' ");

                    for (int i = 0; i < DayCounts; i++)
                    {
                        string Key= StartTime.AddDays(i).ToDateString().Substring(5);
                        MobileDic.Add(Key, 0);
                        UnicomDic.Add(Key, 0);
                        TelecomDic.Add(Key, 0);
                        CountDic.Add(Key, 0);
                        for (int j = 0; j < MobileList.Count(); j++)
                        {
                            string SendTime = MobileList[j].F_SendTime.ToDateString().Substring(5);
                            if (MobileDic.Keys.Contains(SendTime))
                            {
                                MobileDic[SendTime] += MobileList[j].F_Price / 100;
                                CountDic[SendTime] += MobileList[j].F_Price / 100;
                            }
                        }
                        for (int j = 0; j < UnicomList.Count(); j++)
                        {
                            string SendTime = UnicomList[j].F_SendTime.ToDateString().Substring(5);
                            if (UnicomDic.Keys.Contains(SendTime))
                            {
                                UnicomDic[SendTime] += UnicomList[j].F_Price / 100;
                                CountDic[SendTime] += UnicomList[j].F_Price / 100;
                            }
                        }
                        for (int j = 0; j < TelecomList.Count(); j++)
                        {
                            string SendTime = TelecomList[j].F_SendTime.ToDateString().Substring(5);
                            if (TelecomDic.Keys.Contains(SendTime))
                            {
                                TelecomDic[SendTime] += TelecomList[j].F_Price / 100;
                                CountDic[SendTime] += TelecomList[j].F_Price / 100;
                            }
                        }
                    }
                    Dic.Add("移动", MobileDic);
                    Dic.Add("联通", UnicomDic);
                    Dic.Add("电信", TelecomDic);
                    Dic.Add("总金额", CountDic);
                }
            }
            catch (Exception ex){ return Dic; }
            return Dic;
        }

        //获取运营商总数据
        public Dictionary<string, decimal> GetOperatorData(string queryJson)
        {
            DateTime[] DateInterval = GetDateInterval(queryJson);
            DateTime StartTime = DateInterval[0];
            DateTime EndTime = DateInterval[1];
            var queryParam = queryJson.ToJObject();
            string F_UserFId = queryParam["F_UserFId"].ToString();
            return GetOperatorReturnData(StartTime, EndTime, F_UserFId);
        }

        private Dictionary<string, decimal> GetOperatorReturnData(DateTime StartTime, DateTime EndTime, string UserFId)
        {
            Dictionary<string, decimal> DataDic = new Dictionary<string, decimal>();
            string STime = StartTime.ToDateString();
            string ETime = EndTime.ToDateString();
            try
            {
                using (var db = DBHelper.GetReadInstance())
                {
                    var MobileCount = db.SqlQuery<decimal>("select count(*) from Sev_SendDateDetail where F_CreatorUserId='" + UserFId
                        + "' and F_SendTime>'" + STime + "'and F_SendTime<'" + ETime + "'and F_Operator=1 ");
                    var UnicomCount = db.SqlQuery<decimal>("select count(*) from Sev_SendDateDetail where F_CreatorUserId='" + UserFId
                        + "'and F_SendTime>'" + STime + "'and F_SendTime<'" + ETime + "' and F_Operator=2 ");
                    var TelecomCount = db.SqlQuery<decimal>("select count(*) from Sev_SendDateDetail where F_CreatorUserId='" + UserFId
                        + "' and F_SendTime>'" + STime + "'and F_SendTime<'" + ETime + "'and F_Operator=3 ");
                    var SumCount = db.SqlQuery<decimal>("select count(*) from Sev_SendDateDetail where F_CreatorUserId='" + UserFId
                        + "' and F_SendTime>'" + STime + "'and F_SendTime<'" + ETime + "' ");

                    DataDic.Add("移动", MobileCount[0]);
                    DataDic.Add("联通", UnicomCount[0]);
                    DataDic.Add("电信", TelecomCount[0]);
                    DataDic.Add("其他", SumCount[0] - MobileCount[0] - UnicomCount[0] - TelecomCount[0]);
                    return DataDic;
                }
            }
            catch (Exception ex){ return DataDic; }
        }

        //获取地区发送数据
        public List<decimal> GetAreaSendData(string queryJson)
        {
            DateTime[] DateInterval = GetDateInterval(queryJson);
            DateTime StartTime = DateInterval[0];
            DateTime EndTime = DateInterval[1];
            var queryParam = queryJson.ToJObject();
            string F_UserFId = queryParam["F_UserFId"].ToString();
            return GetAreaSendReturnData(StartTime, EndTime, F_UserFId);
        }
        private List<decimal> GetAreaSendReturnData(DateTime StartTime, DateTime EndTime, string UserFId)
        {
            var ProvinceList = Sys_AreaDAL.Instance.FindList(t => t.F_Layers == 1);
            List<decimal> ProvinceCount = new List<decimal>();
            using (var db = DBHelper.GetReadInstance())
            {
                for (int i = 0; i < ProvinceList.Count(); i++)
                {
                    try
                    {
                        decimal count = db.Queryable<Sev_SendDateDetail>().Where(t => t.F_CreatorUserId == UserFId && t.F_Province.Contains(ProvinceList[i].F_Id.Substring(0, 2)) && t.F_SendTime > StartTime && t.F_SendTime < EndTime).Count();
                        ProvinceCount.Add(count / 1000);
                    }
                    catch (Exception ex) { ProvinceCount.Add(0); }
                }
                decimal OtherCount = db.Queryable<Sev_SendDateDetail>().Where(t => t.F_CreatorUserId == UserFId && t.F_Province == "未知" && t.F_SendTime > StartTime && t.F_SendTime < EndTime).Count();
                if (OtherCount == 0)
                    ProvinceCount.Add(0);
                else
                    ProvinceCount.Add(OtherCount / 1000);
            }
            return ProvinceCount;
        }
        //缓冲数据监控list
        public List<Sev_SendDateDetail> BuffeDataGetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<Sev_SendDateDetail>();/*.Where(t => t.F_Report!=2);*/
                var queryParam = queryJson.ToJObject();
                if (!queryParam["F_UserId"].IsEmpty())
                {
                    int F_UserId = queryParam["F_UserId"].ToInt();
                    queryable.Where(a => a.F_UserId == F_UserId);
                }
               
                if (!queryParam["F_PhoneCode"].IsEmpty())
                {
                    string F_PhoneCode = queryParam["F_PhoneCode"].ToString();
                    queryable.Where(a => a.F_PhoneCode == F_PhoneCode);
                }
                if (!queryParam["F_SmsContent"].IsEmpty())
                {
                    string F_SmsContent = queryParam["F_SmsContent"].ToString();
                    queryable.Where(a => a.F_SmsContent.Contains(F_SmsContent));
                }
                if (!queryParam["F_SendState"].IsEmpty())
                {//此字段查询有待完善
                    int SendState = queryParam["F_SendState"].ToInt();
                    string F_ID = DAL.SMC_SendSmsDAL.Instance.FindEntity(t => t.F_SendState == SendState).F_Id;
                    queryable.Where(t => t.SMC_F_Id == F_ID);
                }

                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }
        //单日数据分析list
        public List<Sev_SendDateDetail> DailyDataGetList(Pagination pagination, string queryJson)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<Sev_SendDateDetail>();
                var queryParam = queryJson.ToJObject();
                //if (!queryParam["F_ResponseTime"].IsEmpty())
                //{
                //    DateTime F_ResponseTime = Convert.ToDateTime(queryParam["F_ResponseTime"]);
                //    queryable.Where(a => a.F_ResponseTime < F_ResponseTime.AddDays(1) && a.F_ResponseTime > F_ResponseTime.AddDays(-1));
                //}
                //else if (queryParam["F_ResponseTime"].IsEmpty() & queryParam["F_ChannelId"].IsEmpty() & queryParam["F_Operator"].IsEmpty() & queryParam["F_UserId"].IsEmpty() & queryParam["F_Province"].IsEmpty())//如果是头一次加载页面，默认查询当天的信息
                //{
                //    DateTime F_ResponseTime = DateTime.Now.AddDays(1); 
                //    queryable.Where(a => a.F_ResponseTime < F_ResponseTime && a.F_ResponseTime > F_ResponseTime.AddDays(-1));
                //}
                if (!queryParam["F_ChannelId"].IsEmpty())
                {
                    int F_ChannelId = queryParam["F_ChannelId"].ToInt();
                    queryable.Where(a => a.F_ChannelId == F_ChannelId);
                }
                if (!queryParam["F_Operator"].IsEmpty())
                {
                    int F_Operator = queryParam["F_Operator"].ToInt();
                    queryable.Where(t => t.F_Operator == F_Operator);
                }
                if (!queryParam["F_Province"].IsEmpty())
                {
                    string F_Province = queryParam["F_Province"].ToString();
                    string str = F_Province.Substring(0, 2);//截取省邮编的前两个字符用来匹配该省的所有市
                    queryable.Where(a => a.F_Province.Contains(str));
                }
                if (!queryParam["F_UserId"].IsEmpty())
                {
                    int F_UserId = queryParam["F_UserId"].ToInt();
                    queryable.Where(a => a.F_UserId == F_UserId);
                }

                var tempData = queryable.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.ToPageList(pagination.page, pagination.rows);
            }
        }

        //根据条件批量操作按条件查找修改的List
        public List<Sev_SendDateDetail> GetList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            var expression = ExtLinq.True<Sev_SendDateDetail>();
            if (!(queryParam["F_UserId"].ToString()==""))
            {
                int F_UserId = queryParam["F_UserId"].ToInt();
                expression = expression.And(t => t.F_UserId == F_UserId);
            }
            if (!(queryParam["F_BlackWhite"].ToString()==""))
            {
                int F_BlackWhite = queryParam["F_BlackWhite"].ToInt();
                expression = expression.And(t => t.F_BlackWhite == F_BlackWhite);
            }
            if (!(queryParam["F_ChannelId"].ToString()==""))
            {
                int F_ChannelId = queryParam["F_ChannelId"].ToInt();
                expression = expression.And(t => t.F_ChannelId==(F_ChannelId));
            }
            if (!((queryParam["F_CreatorTimeFrom"].ToString()=="" || queryParam["F_CreatorTimeEnd"].ToString()=="")))
            {
                DateTime F_CreatorTimeFrom = (DateTime)queryParam["F_CreatorTimeFrom"];
                DateTime F_CreatorTimeEnd = (DateTime)queryParam["F_CreatorTimeEnd"];
                expression = expression.And(t => ((t.F_SendTime >= F_CreatorTimeFrom) && t.F_SendTime <= F_CreatorTimeEnd));
            }
            if (!(queryParam["F_PhoneCode"].ToString()==""))
            {
                string F_PhoneCode = queryParam["F_PhoneCode"].ToString();
                expression = expression.And(a => a.F_PhoneCode == F_PhoneCode);
            }
            if (!(queryParam["F_SmsContent"].ToString()==""))
            {
                string F_SmsContent = queryParam["F_SmsContent"].ToString();
                expression = expression.And(a => a.F_SmsContent.Contains(F_SmsContent));
            }
            if (!(queryParam["F_Province"].ToString()==""))
            {
                string F_Province = queryParam["F_Province"].ToString();
                expression = expression.And(a => a.F_Province == F_Province);
            }
            //if (!(queryParam["F_Report"].ToString()==""))
            //{
            //    int F_Report = queryParam["F_Report"].ToInt();
            //    expression = expression.And(a => a.F_Report == F_Report);
            //}
            if (!(queryParam["F_Operator"].ToString() == ""))
            {
                int F_Operator = queryParam["F_Operator"].ToInt();
                expression = expression.And(a => a.F_Operator == F_Operator);
            }
            if (!(queryParam["F_Synchro"].ToString()==""))
            {
                int F_Synchro = queryParam["F_Synchro"].ToInt();
                expression = expression.And(a => a.F_Synchro == F_Synchro);
            }
            if (!(queryParam["F_Buckle"].ToString()==""))
            {
                int F_Buckle = queryParam["F_Buckle"].ToInt();
                expression = expression.And(a => a.F_Buckle == F_Buckle);
            }
            //if (!(queryParam["F_Reissue"].ToString()==""))
            //{
            //    int F_Reissue = queryParam["F_Reissue"].ToInt();
            //    expression = expression.And(a => a.F_Reissue == F_Reissue);
            //}
            //if (!(queryParam["F_Level"].ToString()==""))
            //{
            //    int F_Level = queryParam["F_Level"].ToInt();
            //    expression = expression.And(a => a.F_Level == F_Level);
            //}
           return DAL.Sev_SendDateDetailDAL.instance.FindList(expression);
        }

        // 查询所有发送数据详单数组
        public List<Sev_SendDateDetail> FindList(string F_UserId, string F_BlackWhite, string F_ChannelId, /*DateTime? F_SendTime,*/ string F_PhoneCode, string F_Operator, string F_Province, string F_Report, string F_Synchro, string F_Buckle, string F_Reissue,/* string F_Level, */string F_SmsContent)
        {
            using (var db = DBHelper.GetReadInstance())
            {
                var queryable = db.Queryable<Sev_SendDateDetail>();
                if (!F_UserId.IsEmpty())
                {
                    queryable.Where(a => a.F_UserId == F_UserId.ToInt());
                }
                if (!F_BlackWhite.IsEmpty())
                {
                    queryable.Where(a => a.F_BlackWhite == F_BlackWhite.ToInt());
                }
                if (!F_ChannelId.IsEmpty())
                {
                    queryable.Where(a => a.F_ChannelId == F_ChannelId.ToInt());
                }
                //if (!F_SendTime.IsEmpty())
                //{
                //    queryable.Where(a => a.F_SendTime == F_SendTime);
                //}
                if (!F_PhoneCode.IsEmpty())
                {
                    queryable.Where(a => a.F_PhoneCode == F_PhoneCode);
                }
                if (!F_Operator.IsEmpty())
                {
                    queryable.Where(a => a.F_Operator == F_Operator.ToInt());
                }
                if (!F_Province.IsEmpty())
                {
                    queryable.Where(a => a.F_Province == F_Province);
                }
                //if (!F_Report.IsEmpty())
                //{
                //    queryable.Where(a => a.F_Report == F_Report.ToInt());
                //}
                if (!F_Synchro.IsEmpty())
                {
                    queryable.Where(a => a.F_Synchro == F_Synchro.ToInt());
                }
                if (!F_Buckle.IsEmpty())
                {
                    queryable.Where(a => a.F_Buckle == F_Buckle.ToInt());
                }
                //if (!F_Reissue.IsEmpty())
                //{
                //    queryable.Where(a => a.F_Reissue == F_Reissue.ToInt());
                //}
                //if (!F_Level.IsEmpty())
                //{
                //    queryable.Where(a => a.F_Level == F_Level.ToInt());
                //}
                if (!F_SmsContent.IsEmpty())
                {
                    queryable.Where(a => a.F_SmsContent == F_SmsContent);
                }
                return queryable.ToList();
            }
        }
    }
}
