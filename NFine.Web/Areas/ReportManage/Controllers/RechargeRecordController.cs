using NFine.BLL;
using NFine.Code;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.ReportManage.Controllers
{
    public class RechargeRecordController : ControllerBase
    {
        //
        // GET: /QUEStatistics/RechargeRecord/

        public ActionResult RechargeRecordIndex()
        {
            return View();
        }

        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            List<OC_RechargeRecord> mlist = new List<OC_RechargeRecord>();
            var data = new
            {
                rows = DAL.OC_RechargeRecordDAL.Instance.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };

            if (!queryParam["Operator"].IsEmpty())
            {
                for (int i = 0; i < data.rows.Count(); i++)
                {
                    if (!queryParam["Operator"].IsEmpty())
                    {
                        string Operator = queryParam["Operator"].ToString();
                        DateTime now = System.DateTime.Now;
                        DateTime a = now.AddDays(-7);//前7天
                        DateTime b = now.AddDays(7);//后7天

                        if (Operator == "0")
                        {//+-7天内记录记录
                            if ((data.rows[i].F_CreatorTime >= a) & (data.rows[i].F_CreatorTime <= b))
                                mlist.Add(data.rows[i]);
                        }
                        else
                        {//7天外记录 
                            if ((data.rows[i].F_CreatorTime < a) || (data.rows[i].F_CreatorTime > b))
                                mlist.Add(data.rows[i]);
                        }
                    }
                }
                return Content(mlist.ToJson());
            }
            return Content(data.ToJson());
        }

        //根据F_UserId获取F_Balance
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetUserF_Balance(int UserId)
        {
            var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == UserId);
            return data.F_Balance.ToString();
        }
    }
}
