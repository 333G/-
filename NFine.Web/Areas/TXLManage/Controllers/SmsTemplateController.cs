using NFine.Application.TXLManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.TXLManage;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.TXLManage.Controllers
{
    public class SmsTemplateController : ControllerBase
    {
        private SmsTemplateApp SmsTemplateApp = new SmsTemplateApp();
        public ActionResult SmsIndex()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = SmsTemplateApp.GetList(pagination, queryJson),
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
            var data = SmsTemplateApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SmsTemplateEntity SmsTemplateEntity, string keyValue)
        {
            SmsTemplateApp.SubmitForm(SmsTemplateEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            SmsTemplateApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }


    }
}
