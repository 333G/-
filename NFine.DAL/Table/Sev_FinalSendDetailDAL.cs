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
    public partial class Sev_FinalSendDetailDAL
    {
        //根据条件批量操作按条件查找修改的List
        public List<Sev_FinalSendDetail> GetList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            var expression = ExtLinq.True<Sev_FinalSendDetail>();
            //if (!(queryParam["F_UserId"].ToString() == ""))
            //{
            //    int F_UserId = queryParam["F_UserId"].ToInt();
            //    expression = expression.And(t => t.F_UserId == F_UserId);
            //}
            //if (!(queryParam["F_BlackWhite"].ToString() == ""))
            //{
            //    int F_BlackWhite = queryParam["F_BlackWhite"].ToInt();
            //    expression = expression.And(t => t.F_BlackWhite == F_BlackWhite);
            //}
            //if (!(queryParam["F_ChannelId"].ToString() == ""))
            //{
            //    int F_ChannelId = queryParam["F_ChannelId"].ToInt();
            //    expression = expression.And(t => t.F_ChannelId == (F_ChannelId));
            //}
            if (!((queryParam["F_CreatorTimeFrom"].ToString() == "" || queryParam["F_CreatorTimeEnd"].ToString() == "")))
            {
                DateTime F_CreatorTimeFrom = (DateTime)queryParam["F_CreatorTimeFrom"];
                DateTime F_CreatorTimeEnd = (DateTime)queryParam["F_CreatorTimeEnd"];
                expression = expression.And(t => ((t.F_SendTime >= F_CreatorTimeFrom) && t.F_SendTime <= F_CreatorTimeEnd));
            }
            //if (!(queryParam["F_PhoneCode"].ToString() == ""))
            //{
            //    string F_PhoneCode = queryParam["F_PhoneCode"].ToString();
            //    expression = expression.And(a => a.F_PhoneCode == F_PhoneCode);
            //}
            if (!(queryParam["F_SmsContent"].ToString() == ""))
            {
                string F_SmsContent = queryParam["F_SmsContent"].ToString();
                expression = expression.And(a => a.F_SmsContent.Contains(F_SmsContent));
            }
            //if (!(queryParam["F_Province"].ToString() == ""))
            //{
            //    string F_Province = queryParam["F_Province"].ToString();
            //    expression = expression.And(a => a.F_Province == F_Province);
            //}
            //if (!(queryParam["F_Report"].ToString()==""))
            //{
            //    int F_Report = queryParam["F_Report"].ToInt();
            //    expression = expression.And(a => a.F_Report == F_Report);
            //}
            //if (!(queryParam["F_Operator"].ToString() == ""))
            //{
            //    int F_Operator = queryParam["F_Operator"].ToInt();
            //    expression = expression.And(a => a.F_Operator == F_Operator);
            //}
            //if (!(queryParam["F_Synchro"].ToString() == ""))
            //{
            //    int F_Synchro = queryParam["F_Synchro"].ToInt();
            //    expression = expression.And(a => a.F_Synchro == F_Synchro);
            //}
            //if (!(queryParam["F_Buckle"].ToString() == ""))
            //{
            //    int F_Buckle = queryParam["F_Buckle"].ToInt();
            //    expression = expression.And(a => a.F_Buckle == F_Buckle);
            //}
            if (!(queryParam["F_Reissue"].ToString() == ""))
            {
                int F_Reissue = queryParam["F_Reissue"].ToInt();
                expression = expression.And(a => a.F_Reissue == F_Reissue);
            }
            if (!(queryParam["F_Level"].ToString() == ""))
            {
                int F_Level = queryParam["F_Level"].ToInt();
                expression = expression.And(a => a.F_Level == F_Level);
            }
            return DAL.Sev_FinalSendDetailDAL.Instance.FindList(expression);
        }


        //获取发送成功率数据
        public Dictionary<string, Dictionary<string, decimal>> GetSendSuccessRate(string queryJson)
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
            TimeSpan DayCount = EndTime - StartTime;
            return GetReturnData(StartTime, EndTime, F_UserFId, DayCount.Days);
        }


        private Dictionary<string, Dictionary<string, decimal>> GetReturnData(DateTime StartTime, DateTime EndTime, string UserFId, int DayCounts)
        {
            Dictionary<string, decimal> SueecssDic = new Dictionary<string, decimal>();
            Dictionary<string, decimal> FailedDic = new Dictionary<string, decimal>();
            Dictionary<string, decimal> UnknownDic = new Dictionary<string, decimal>();
            Dictionary<string, Dictionary<string, decimal>> Dic = new Dictionary<string, Dictionary<string, decimal>>();
            try
            {
                using (var db = DBHelper.GetReadInstance())
                {
                    for (DateTime STime = StartTime ; STime < EndTime; STime= STime.AddDays(1))
                    {

                        var DailySuccessCount = db.SqlQuery<decimal>("select  count(*) from Sev_FinalSendDetail where  F_CreatorUserId='" + UserFId
                        + "' and F_SendTime  between '" +STime + "'and '"+STime.AddDays(1).ToShortDateString()+"' and (F_Report=1 or F_Report=3)");
                        var DailyFailedCount = db.SqlQuery<decimal>("select  count(*) from Sev_FinalSendDetail where F_CreatorUserId='" + UserFId
                        + "' and F_SendTime  between '" +STime + "'and '" + STime.AddDays(1).ToShortDateString() + "' and (F_Report=2 or F_Report=5) ");
                        var DailyUnknownCount = db.SqlQuery<decimal>("select  count(*) from Sev_FinalSendDetail where F_CreatorUserId='" + UserFId
                        + "' and F_SendTime  between '" +STime + "'and '" + STime.AddDays(1).ToShortDateString() + "' and F_Report=0 ");
                        decimal SumCount = DailySuccessCount[0] + DailyFailedCount[0] + DailyUnknownCount[0];
                        string Key = STime.ToDateString().Substring(5);
                        if (SumCount == 0)
                        {
                            SueecssDic.Add(Key, 0);
                            FailedDic.Add(Key, 0);
                            UnknownDic.Add(Key, 0);
                        }
                        else
                        {
                            SueecssDic.Add(Key, Math.Round(((DailySuccessCount[0] / SumCount)) * 100,2));
                            FailedDic.Add(Key, Math.Round((DailyFailedCount[0] / SumCount) * 100,2));
                            UnknownDic.Add(Key, Math.Round((DailyUnknownCount[0] / SumCount) * 100,2));
                        }
                    }
                    Dic.Add("成功", SueecssDic);
                    Dic.Add("失败", FailedDic);
                    Dic.Add("未知", UnknownDic);
                }
            }
            catch (Exception ex) { return Dic; }
            return Dic;
        }

        public decimal GetSubmitDate(string queryJson,int DataType)
        {
            var queryParam = queryJson.ToJObject();
            try
            {
                using (var db = DBHelper.GetReadInstance())
                {
                    var queryble = db.Queryable<Sev_FinalSendDetail>()
               .JoinTable<Sev_SendDateDetail>((s1, s2) => s1.F_SendId == s2.F_Id)
                .Select<Sev_SendDateDetail, Entity.Views.VSevFinalSendDetail>
                ((s1, s2) => new Entity.Views.VSevFinalSendDetail { F_Id = s1.F_Id, F_ChannelId = s2.F_ChannelId, F_Operator = s2.F_Operator, F_Province = s2.F_Province, F_UserId = s2.F_UserId, F_SendTime = s1.F_SendTime, F_Report = s1.F_Report, F_Response = s1.F_Response, F_LongsmsCount = s2.F_LongsmsCount, F_SendId = s1.F_SendId, F_Reissue = s2.F_Reissue })
               ;
                    //;第二次修改后用的联查

                    //发送数据详单只分析当日的数据

                    DateTime SendTime = DateTime.Now.Date;
                        DateTime newDateTime = SendTime.AddDays(1);
                        queryble.Where(s1 => s1.F_SendTime >= SendTime && s1.F_SendTime < newDateTime);
                    if (DataType == 0)
                    { }
                    else if (DataType == 1)
                        queryble.Where(s2 => s2.F_Reissue == 0);
                    else if (DataType == 2)
                        queryble.Where(s2 => s2.F_Reissue > 0);
                    else if (DataType == 3)
                        queryble.Where(s1 => s1.F_Report == "3");
                    else if (DataType==4)
                        queryble.Where(s1 => s1.F_Report == "1");
                    if (!queryParam["F_ChannelId"].IsEmpty())
                    {
                        int ChannelId = queryParam["F_ChannelId"].ToInt();
                        if (ChannelId != 0)
                        queryble.Where(s2 => s2.F_ChannelId == ChannelId);
                    }
                    if (!queryParam["F_Operator"].IsEmpty())
                    {
                        int Operator = queryParam["F_Operator"].ToInt();
                        if (Operator != 0)
                        queryble.Where(s2 => s2.F_Operator == Operator);
                    }
                    if (!queryParam["F_Province"].IsEmpty())
                    {
                        string ProvinceId = queryParam["F_Province"].ToString();
                        if (ProvinceId != "0")
                        {
                            string Pro = ProvinceId.Substring(0, 2);
                            queryble.Where(s2 => s2.F_Province.Contains(Pro));
                        }
                        queryble.Where(s2 => s2.F_Province == ProvinceId);
                    }
                    if (!queryParam["F_UserId"].IsEmpty())
                    {
                        int UserId = queryParam["F_UserId"].ToInt();
                        queryble.Where(s2 => s2.F_UserId == UserId);
                    }
                    return queryble.Count();
                }
            }
            catch
            { return 0; }
        }
    }
}

