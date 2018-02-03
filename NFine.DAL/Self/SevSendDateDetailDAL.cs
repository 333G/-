using NFine.Code;
using NFine.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NFine.DAL.Self
{
    public partial class SevSendDateDetailDAL
    {
        private static SevSendDateDetailDAL instance;
        private static object _lock = new object();

        #region 单例模式
        private SevSendDateDetailDAL()
        {
        }

        public static SevSendDateDetailDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new SevSendDateDetailDAL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        //发送数据详单多表联查
        public List<NFine.Entity.Views.VSevFinalSendDetail> GetReceiveList(Pagination pagination, string queryJson)
        {

            //这里是一个原来用的多表联查，使用的sqlsuger，by lx
            var queryParam = queryJson.ToJObject();
            using (var db = DBHelper.GetReadInstance())//已经没有多表联查那种操作了，最新操作是数据库内的视图
            {
                //  原来的多表条件联查
                //var queryble = db.Queryable<Sev_FinalSendDetail>().Where(s1 => s1.F_DeleteMark != true)
                //   .JoinTable<Sev_SendDateDetail>((s1, s2) => s1.F_SendId == s2.F_Id)
                //   .Select<Sev_SendDateDetail, Entity.Views.VSevFinalSendDetail>
                //    ((s1, s2) => new Entity.Views.VSevFinalSendDetail { Id = s1.Id, F_Id = s1.F_Id, F_UserId = s2.F_UserId, F_BlackWhite = s2.F_BlackWhite, F_ChannelId = s2.F_ChannelId, F_SendTime = s1.F_SendTime, F_PhoneCode = s2.F_PhoneCode, F_Operator = s2.F_Operator, F_Province = s2.F_Province, F_Report = s1.F_Report, F_Synchro = s2.F_Synchro, F_Buckle = s2.F_Buckle, F_Reissue = s1.F_Reissue, F_Level = s1.F_Level, F_SmsContent = s1.F_SmsContent })
                //    .OrderBy(s2 => s2.F_PhoneCode)
                //    ;
                //var vlist = db.Queryable<Entity.Views.VSevFinalSendDetail>().Where(s1 => s1.F_DeleteMark != true);
                //Expression<Func<Entity.Views.VSevFinalSendDetail, bool>> where =PredicateExtensionses.
                string sqlcmd = "select * from ( select row_number() over(order by F_Id) as rownum, * from VSevFinalSendDetail where 1=1";
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!queryParam["F_UserId"].IsEmpty())
                {
                    int UserId = queryParam["F_UserId"].ToInt();
                   // queryble.Where(s2 => s2.F_UserId == UserId);
                    sqlcmd += "and F_UserId=" + UserId + "";
                }
                if (!queryParam["F_PhoneCode"].IsEmpty())
                {
                    string PhoneCode = queryParam["F_PhoneCode"].ToString(); //手机号
                   // queryble = queryble.Where(s2 => s2.F_PhoneCode == PhoneCode);
                    sqlcmd += " and F_PhoneCode='" + PhoneCode + "'";
                }
                if (!queryParam["F_SmsContent"].IsEmpty())
                {
                    string SmsContent = queryParam["F_SmsContent"].ToString();
                    // queryble.Where(s1 => s1.F_SmsContent.Contains(SmsContent));
                    sqlcmd += " and F_SmsContent like '%" + SmsContent + "%'";//可能的sql注入点
                    //parameters.Add(new SqlParameter("@SmsContent", "%" + SmsContent + "%"));//模糊查询参数化处理，避免SQL注入
                }
                if (!queryParam["F_CreatorTimeFrom"].IsEmpty() || !queryParam["F_CreatorTimeEnd"].IsEmpty())
                {
                    DateTime StartTime = queryParam["F_CreatorTimeFrom"].ToDate();
                    DateTime EndTime = queryParam["F_CreatorTimeEnd"].ToDate();
                    if (StartTime == DateTime.Parse("0001 / 1 / 1 0:00:00"))
                    {
                        StartTime = DateTime.Parse("2001/1/1 0:00:00");
                    }
                    if (EndTime == DateTime.Parse("0001/1/1 0:00:00"))
                    {
                        EndTime = DateTime.Parse("9999/1/1 0:00:00");
                    }
                   // queryble.Where(s1 => s1.F_SendTime > StartTime && s1.F_SendTime < EndTime);
                    sqlcmd += " and F_SendTime >'" + StartTime + "' and F_SendTime <'" + EndTime + "'";
                }
                if (!queryParam["F_Province"].IsEmpty())
                {
                    string ProvinceId = queryParam["F_Province"].ToString();
                    string Pro = ProvinceId.Substring(0, 2);
                    // queryble.Where(s2 => s2.F_Province.Contains(Pro));
                    //queryble.Where(s2 => s2.F_Province == ProvinceId);
                    sqlcmd += " and F_Province like '" + Pro + "%' ";
                   // parameters.Add(new SqlParameter("@Province", "%" + Pro + "%"));//模糊查询参数化处理，避免SQL注入
                }
                if (!queryParam["F_ChannelId"].IsEmpty())
                {
                    int ChannelId = queryParam["F_ChannelId"].ToInt();
                  //  queryble = queryble.Where(s2 => s2.F_ChannelId == ChannelId);
                    sqlcmd += " and F_ChannelId=" + ChannelId + "";
                }
                if (!queryParam["F_Report"].IsEmpty())
                {
                    string Report = queryParam["F_Report"].ToString();
                  //  queryble.Where(s1 => s1.F_Report == Report);
                    sqlcmd += " and F_Report='" + Report + "'";
                }
                if (!queryParam["F_Operator"].IsEmpty())
                {
                    int Operator = queryParam["F_Operator"].ToInt();
                  //  queryble.Where(s2 => s2.F_Operator == Operator);
                    sqlcmd += " and F_Operator=" + Operator + "";
                }
                if (!queryParam["F_Synchro"].IsEmpty())
                {
                    int Synchro = queryParam["F_Synchro"].ToInt();
                  //  queryble.Where(s2 => s2.F_Synchro == Synchro);
                    sqlcmd += " and F_Synchro=" + Synchro + "";
                }
                if (!queryParam["F_Buckle"].IsEmpty())
                {
                    int Buckle = queryParam["F_Buckle"].ToInt();
                 //   queryble.Where(s2 => s2.F_Buckle == Buckle);
                    sqlcmd += " and F_Buckle=" + Buckle + "";
                }
                if (!queryParam["F_BlackWhite"].IsEmpty())
                {
                    int BlackWhite = queryParam["F_BlackWhite"].ToInt();
                 //   queryble.Where(s2 => s2.F_BlackWhite == BlackWhite);
                    sqlcmd += " and F_BlackWhite=" + BlackWhite + "";
                }
                if (!queryParam["F_Reissue"].IsEmpty())
                {
                    int Reissue = queryParam["F_Reissue"].ToInt();
                 //   queryble.Where(s1 => s1.F_Reissue == Reissue);
                    sqlcmd += " and F_Reissue=" + Reissue + "";
                }
                int startrow = (pagination.page - 1) * pagination.rows;
                int endrow = pagination.page * pagination.rows;
                sqlcmd += ")aa where rownum between " + startrow + " and ";
                sqlcmd += "" + endrow + "";
                SqlParameter[] ppp = parameters.ToArray();
                pagination.records = db.Queryable<Sev_FinalSendDetail>().Count();
                //var vlist = db.Queryable<Entity.Views.VSevFinalSendDetail>().Where().Skip(startrow).Take(pagination.page);
                return db.SqlQuery<Entity.Views.VSevFinalSendDetail>(sqlcmd);
                // return queryble;
            }

        }


        //缓冲数据监控多表联查
        public List<NFine.Entity.Views.VSevFinalSendDetail> BuffeDataGetList(Pagination pagination, string queryJson)
        {
            List<NFine.Entity.Views.VSevFinalSendDetail> list = new List<NFine.Entity.Views.VSevFinalSendDetail>();
            var queryParam = queryJson.ToJObject();
            using (var db = DBHelper.GetReadInstance())//已经没有多表联查那种操作了，最新操作是数据库内的视图
            {
                //var queryble = db.Queryable<Sev_FinalSendDetail>().Where(a => a.F_Report != "1")
                //  .JoinTable<Sev_SendDateDetail>((s1, s2) => s1.F_SendId == s2.F_Id)   //s1和s2表关联
                //  .JoinTable<Sev_SendDateDetail, SMC_SendSms>((s1, s2, s3) => s3.F_Id == s2.SMC_F_Id)  //s2和s3表关联
                //  .OrderBy<Sev_SendDateDetail>((s1,s2)=>s1.F_SendTime)
                //  .OrderBy<Sev_SendDateDetail>((s1, s2) =>s2.F_PhoneCode)
                //  .Select<Sev_SendDateDetail, SMC_SendSms, Entity.Views.VSevFinalSendDetail>
                //   ((s1, s2, s3) => new Entity.Views.VSevFinalSendDetail { Id = s1.Id, F_Id = s1.F_Id, F_UserId = s2.F_UserId, F_BlackWhite = s2.F_BlackWhite, F_ChannelId = s2.F_ChannelId, F_SendTime = s1.F_SendTime, F_PhoneCode = s2.F_PhoneCode, F_Operator = s2.F_Operator, F_Report = s1.F_Report, F_Province = s2.F_Province, F_Buckle = s2.F_Buckle, F_Reissue = s1.F_Reissue, F_DealState = s1.F_DealState, F_Level = s1.F_Level, F_SmsContent = s1.F_SmsContent, F_SendState = s3.F_SendState })
                //  .ToList();
                //  ;
                string sqlcmd = "select * from ( select row_number() over(order by F_Id) as rownum, * from VSevFinalSendDetail where 1=1";
                if (!queryParam["F_UserId"].IsEmpty())
                {
                    int UserId = queryParam["F_UserId"].ToInt();
                    //queryble.Where(s2 => s2.F_UserId == UserId);
                    sqlcmd += " and F_UserId=" + UserId + "";
                }
                if (!queryParam["F_PhoneCode"].IsEmpty())
                {
                    string PhoneCode = queryParam["F_PhoneCode"].ToString(); //手机号
                    // queryble.Where(s2 => s2.F_PhoneCode == PhoneCode);
                    sqlcmd += " and F_PhoneCode='" + PhoneCode + "'";
                }
                if (!queryParam["F_SmsContent"].IsEmpty())
                {
                    string SmsContent = queryParam["F_SmsContent"].ToString();
                    //queryble.Where(s1 => s1.F_SmsContent.Contains(SmsContent));
                    sqlcmd += " and F_SmsContent like '%" + SmsContent + "%'";//可能的sql注入点
                }
                if (!queryParam["F_SendState"].IsEmpty())
                {
                    int SendState = queryParam["F_SendState"].ToInt();
                    // queryble.Where(s3 => s3.F_SendState == SendState);
                    sqlcmd += " and F_SendState=" + SendState + "";
                }
                int startrow = (pagination.page - 1) * pagination.rows;
                int endrow = pagination.page * pagination.rows;
                sqlcmd += ")aa where rownum between " + startrow + " and ";
                sqlcmd += "" + endrow + "";
                //var tempData = queryble.OrderBy(pagination.sidx);
                //pagination.records = tempData.Count();
                //return tempData.ToPageList(pagination.page, pagination.rows);
                pagination.records = db.Queryable<Sev_FinalSendDetail>().Count();
                return db.SqlQuery<Entity.Views.VSevFinalSendDetail>(sqlcmd);
                // return queryble.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToList();
            }
        }
    
        //单日数据分析多表联查
        public List<NFine.Entity.Views.VSevFinalSendDetail> DailyDataGetList(Pagination pagination, string queryJson)
        {
            List<NFine.Entity.Views.VSevFinalSendDetail> list = new List<NFine.Entity.Views.VSevFinalSendDetail>();
            var queryParam = queryJson.ToJObject();
            using (var db = DBHelper.GetReadInstance())
            {
                //list = db.Queryable<Sev_FinalSendDetail>().Where(a => a.F_DeleteMark != true)
                //   .JoinTable<Sev_SendDateDetail>((s1, s2) => s1.F_SendId == s2.F_Id)
                //   .Select<Sev_FinalSendDetail, Sev_SendDateDetail, NFine.Entity.Views.VSevFinalSendDetail>
                //    ((s1, s2) => new NFine.Entity.Views.VSevFinalSendDetail { F_Id = s1.F_Id, F_ChannelId = s2.F_ChannelId, F_Operator = s2.F_Operator, F_Province = s2.F_Province, F_UserId = s2.F_UserId, F_Report = s1.F_Report, F_Response = s1.F_Response, F_LongsmsCount = s2.F_LongsmsCount })
                //   .ToList();
                //return list;最初用的联查

                //var queryble = db.Queryable<Sev_FinalSendDetail>().Where(s1 => s1.F_Report != "1")
                //   .JoinTable<Sev_SendDateDetail>((s1, s2) => s1.F_SendId == s2.F_Id)
                //   .Select<Sev_SendDateDetail, Entity.Views.VSevFinalSendDetail>
                //    ((s1, s2) => new Entity.Views.VSevFinalSendDetail { F_Id = s1.F_Id, F_ChannelId = s2.F_ChannelId, F_Operator = s2.F_Operator, F_Province = s2.F_Province, F_UserId = s2.F_UserId, F_SendTime = s1.F_SendTime, F_Report = s1.F_Report, F_Response = s1.F_Response, F_LongsmsCount = s2.F_LongsmsCount, F_SendId = s1.F_SendId })
                //   .OrderBy(s1 => s1.F_SendTime)
                //   .OrderBy(s2 => s2.F_PhoneCode)
                //   ;第二次修改后用的联查

                //以下是最新的查询方式，为了提升性能
                string sqlcmd = "select * from ( select row_number() over(order by F_Id) as rownum, * from VSevFinalSendDetail where 1=1";
                if (queryParam["F_SendTime"].IsEmpty())//首次进入时自动查询当日的数据
                {
                    DateTime SendTime = DateTime.Now.Date;
                    DateTime newDateTime = SendTime.AddDays(1);
                    sqlcmd += " and F_SendTime >='" + SendTime + "' and F_SendTime <'" + newDateTime + "'";
                    //queryble.Where(s1 => s1.F_SendTime >= SendTime && s1.F_SendTime < newDateTime);
                }
                if (!queryParam["F_SendTime"].IsEmpty())
                {
                    DateTime SendStartTime = Convert.ToDateTime(queryParam["F_SendTime"]);
                    DateTime SendEndTime = SendStartTime.AddDays(1);
                    sqlcmd += " and F_SendTime >='" + SendStartTime + "' and F_SendTime <'" + SendEndTime + "'";
                    //queryble.Where(s1 =>( s1.F_SendTime >= SendStartTime && s1.F_SendTime < SendEndTime));
                }
                if (!queryParam["F_ChannelId"].IsEmpty())
                {
                    int ChannelId = queryParam["F_ChannelId"].ToInt();
                    if (ChannelId != 0)
                        sqlcmd += " and F_ChannelId=" + ChannelId + "";
                    //queryble.Where(s2 => s2.F_ChannelId == ChannelId);
                }
                if (!queryParam["F_Operator"].IsEmpty())
                {
                    int Operator = queryParam["F_Operator"].ToInt();
                    if (Operator != 0)
                        sqlcmd += " and F_Operator=" + Operator + "";
                    //queryble.Where(s2 => s2.F_Operator == Operator);
                }
                if (!queryParam["F_Province"].IsEmpty())
                {
                    string ProvinceId = queryParam["F_Province"].ToString();
                    if (ProvinceId != "0")
                    {
                        string Pro = ProvinceId.Substring(0, 2);
                        sqlcmd += " and F_Province like '" + Pro + "%' ";
                        //queryble.Where(s2 => s2.F_Province.Contains(Pro));
                    }
                    //queryble.Where(s2 => s2.F_Province == ProvinceId);
                }
                if (!queryParam["F_User"].IsEmpty())
                {
                    string UserId = queryParam["F_User"].ToString();
                    if (UserId != "0")//不是“用户分组”选项
                    {
                        var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_Id == UserId);
                        sqlcmd += " and F_Operator=" + data.F_UserId + "";
                        //queryble.Where(s2 => s2.F_UserId == data.F_UserId);
                    }
                }
                if (!queryParam["F_UserId"].IsEmpty())
                {
                    int UserId = queryParam["F_UserId"].ToInt();
                    sqlcmd += " and F_UserId=" + UserId + "";
                    //queryble.Where(s2 => s2.F_UserId == UserId);
                }
                int startrow = (pagination.page - 1) * pagination.rows;
                int endrow = pagination.page * pagination.rows;
                sqlcmd += ")aa where rownum between " + startrow + " and ";
                sqlcmd += "" + endrow + "";
                pagination.records = db.Queryable<Sev_FinalSendDetail>().Count();
                return db.SqlQuery<Entity.Views.VSevFinalSendDetail>(sqlcmd);
                //var tempData = queryble.OrderBy(pagination.sidx);
                //pagination.records = tempData.Count();
                //return tempData.Skip((pagination.page-1)*pagination.rows).Take(pagination.rows) .ToPageList(pagination.page, pagination.rows);
            }
        }

        //业务数据分析
        public List<Sev_BusinessDataAnalysis> BusinessDataAnalysis(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            using (var db = DBHelper.GetReadInstance())
            {
                var queryble = db.Queryable<Sev_BusinessDataAnalysis>();
                if (!queryParam["F_UserId"].IsEmpty())
                {
                    int UserId = queryParam["F_UserId"].ToInt();
                     queryble.Where(s => s.F_UserId == UserId);
                }
                if (!queryParam["F_SendTimeStart"].IsEmpty())
                {
                    DateTime F_SendTimeStart = queryParam["F_SendTimeStart"].ObjToDate();
                    if (F_SendTimeStart == DateTime.MinValue)
                    {
                        F_SendTimeStart = DateTime.Now.AddDays(-3);
                    }
                    queryble.Where(s => s.F_SendDate > F_SendTimeStart);
                }
                else
                {
                    DateTime F_SendTimeStart = DateTime.Now.AddDays(-3);
                    queryble.Where(s => s.F_SendDate > F_SendTimeStart);
                }
                if (!queryParam["F_SendTimeEnd"].IsEmpty())
                {
                    DateTime F_SendTimeEnd = queryParam["F_SendTimeEnd"].ObjToDate();
                    if (F_SendTimeEnd == DateTime.MinValue)
                    {
                        F_SendTimeEnd = DateTime.Now.Date.AddDays(1);
                    }
                    queryble.Where(s => s.F_SendDate < F_SendTimeEnd);
                }
                else
                {
                    DateTime F_SendTimeEnd = DateTime.Now.Date.AddDays(1);
                    queryble.Where(s => s.F_SendDate < F_SendTimeEnd);

                }
                var tempData = queryble.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.Skip((pagination.page-1)*pagination.rows).Take(pagination.rows) .ToPageList(pagination.page, pagination.rows);
            }
        }
        //业务数据分析
        public List<Sev_DailyChannelData> ChannelDataAnalysis(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            using (var db = DBHelper.GetReadInstance())
            {
                var queryble = db.Queryable<Sev_DailyChannelData>();
                if (!queryParam["F_ChannelId"].IsEmpty())
                {
                    int ChannelId = queryParam["F_UserId"].ToInt();
                    queryble.Where(s => s.F_ChannelId == ChannelId);
                }
                if (!queryParam["F_TimeStart"].IsEmpty())
                {
                    DateTime F_TimeStart = queryParam["F_TimeStart"].ObjToDate();
                    if (F_TimeStart == DateTime.MinValue)
                    {
                        F_TimeStart = DateTime.Now.AddDays(-3);
                    }
                    queryble.Where(s => s.F_Time > F_TimeStart);
                }
                else
                {
                    DateTime F_SendTimeStart = DateTime.Now.AddDays(-3);
                    queryble.Where(s => s.F_Time > F_SendTimeStart);
                }
                if (!queryParam["F_TimeEnd"].IsEmpty())
                {
                    DateTime F_TimeEnd = queryParam["F_TimeEnd"].ObjToDate();
                    if (F_TimeEnd == DateTime.MinValue)
                    {
                        F_TimeEnd = DateTime.Now.Date.AddDays(1);
                    }
                    queryble.Where(s => s.F_Time < F_TimeEnd);
                }
                else
                {
                    DateTime F_SendTimeEnd = DateTime.Now.Date.AddDays(1);
                    queryble.Where(s => s.F_Time < F_SendTimeEnd);

                }
                var tempData = queryble.OrderBy(pagination.sidx);
                pagination.records = tempData.Count();
                return tempData.Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToPageList(pagination.page, pagination.rows);
            }
        }
    }
}
