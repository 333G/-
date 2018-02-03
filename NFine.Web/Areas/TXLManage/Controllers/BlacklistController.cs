using NFine.Code;
using NFine.Domain.Entity.TXLManage;
using NFine.Mapping.TXLManage;
using System.Web.Mvc;
using NFine.Application.TXLManage;

namespace NFine.Web.Areas.TXLManage.Controllers
{
    public class BlacklistController : ControllerBase
    {
        private BlacklistApp BlacklistApp = new BlacklistApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = BlacklistApp.GetList(pagination, queryJson),
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
            var data = BlacklistApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(BlacklistEntity BlacklistEntity, string keyValue)
        {
            BlacklistApp.SubmitForm(BlacklistEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            BlacklistApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

      

    }
}
