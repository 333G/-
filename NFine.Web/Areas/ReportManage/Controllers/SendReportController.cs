using NFine.Application.SMCManage;
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
    public class SendReportController : ControllerBase
    {
        //
        // GET: /QUEStatistics/SendReport/
        private SMCSendApp smcSendApp = new SMCSendApp();
        public ActionResult SendReportIndex()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            List<SMC_SendSms> mlist = new List<SMC_SendSms>();//创建一个空集合mlist，用于存放符合查询条件的数据并输出
            var data = new
            {
                rows = SMC_SendSmsManager.Instance.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };

            if (!queryParam["Operator"].IsEmpty())
            {
                for (int i = 0; i < data.rows.Count; i++)
                {
                    if (!queryParam["Operator"].IsEmpty())
                    {
                        string Operator = queryParam["Operator"].ToString();
                        DateTime now = System.DateTime.Now;
                        DateTime a = now.AddDays(-7);//前7天
                        DateTime b = now.AddDays(7);//后7天

                        if (Operator == "0")
                        {//+-7天内记录记录
                            if ((data.rows[i].F_SendTime >= a) & (data.rows[i].F_SendTime <= b))
                                mlist.Add(data.rows[i]);
                        }
                        else
                        {//7天外记录 
                            if ((data.rows[i].F_SendTime < a) || (data.rows[i].F_SendTime > b))
                                mlist.Add(data.rows[i]);
                        }
                    }
                }
                return Content(mlist.ToJson());
            }
            return Content(data.ToJson());
        }
    }
}
