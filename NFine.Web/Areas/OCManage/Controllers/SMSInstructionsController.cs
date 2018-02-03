 

using NFine.Application.OCManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.OCManage;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;



namespace NFine.Web.Areas.OCManage.Controllers
{
    public class SMSInstructionsController : ControllerBase
    {
        public ActionResult GetFormJson()
        {
            return View();
        }
        public ActionResult GetNewSMSInstructionsJson()
        {
            return View();
        }
        public ActionResult GetAddFormJson()
        {
            return View();
        }

        private SMSInstructionsApp SMSInstructionsApp = new SMSInstructionsApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = SMSInstructionsApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = SMSInstructionsApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SMSInstructionsEntity SMSInstructionsEntity, string keyValue)
        {
            SMSInstructionsApp.SubmitForm(SMSInstructionsEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            SMSInstructionsApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}






