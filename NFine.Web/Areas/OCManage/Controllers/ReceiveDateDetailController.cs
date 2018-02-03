using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class ReceiveDateDetailController : ControllerBase
    {
        //
        // GET: /OCManage/ReceiveDateDetail/

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = BLL.Sev_SendDateDetailManager.Instance.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        //多表联查返回
        public ActionResult GetReceiveGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = BLL.Self.SMC_RceiveSmsBLL.Instance.GetReceiveList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        }
}
