using NFine.Application.OCManage;
using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;




namespace NFine.Web.Areas.OCManage.Controllers
{
    public class RechargeRecordController : ControllerBase
    {
        public ActionResult editForm()
        {
            return View();
        }
        public ActionResult GetFormJson()
        {
            return View();
        }
        public ActionResult GetNewRechargeRecordJson()
        {
            return View();
        }
        public ActionResult GetAddFormJson()
        {
            return View();
        }
        public ActionResult childRechargeForm()
        {
            return View();
        }

        private RechargeRecordApp RechargeRecordApp = new RechargeRecordApp();


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = RechargeRecordApp.GetList(pagination, queryJson),
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
            var data = RechargeRecordApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        //子账户充值填充账户名
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetChildFormJson(int id)
        {
            var data = BLL.Sys_UserManager.Instance.GetModel(id);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(RechargeRecordEntity RechargeRecordEntity,OC_UserInfo model_UI , string keyValue)
        {
            try//检测业务员是否存在
            {
                DAL.Sys_UserDAL.Instance.FindEntity(t => t.Id == RechargeRecordEntity.F_OperatorId);
                DAL.Sys_UserDAL.Instance.FindEntity(t => t.Id == RechargeRecordEntity.F_ManagerId);
            }
            catch { return Error("发生错误，请检查业务员，操作员ID是否正确！"); }
            RechargeRecordApp.SubmitForm(RechargeRecordEntity, model_UI, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            RechargeRecordApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        //获取付款方式下拉框
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetPayModeJson()
        {
            var data = DAL.Sys_ItemsDetailDAL.Instance.FindList(t => t.F_ItemId == "b2137d56-69b3-4821-9701-064f040a8a7f");//获取支付方式列表
            var treelist = new List<TreeSelectModel>();
            foreach (Sys_ItemsDetail item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_ItemCode;
                treeModel.text = item.F_ItemName;
                treeModel.parentId = "0";
                treeModel.data = item.F_Id;
                treelist.Add(treeModel);          
            }
            return Content(treelist.TreeSelectJson());
        }
    }
}





