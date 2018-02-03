using NFine.Application.OCManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.OCManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFine.Entity;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Application.SystemSecurity;
using NFine.Application;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class ManualReviewController : ControllerBase
    {
        //
        // GET: /OCManage/ManualReview/
        private ManualReviewApp manualReviewApp = new ManualReviewApp();
        LogEntity AddReviewlogEntity = new LogEntity();
        LogEntity ChangeReviewlogEntity = new LogEntity();
        LogEntity DeleteReviewlogEntity = new LogEntity();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = manualReviewApp.GetList(pagination, queryJson),
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
            var data = manualReviewApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ManualReviewEntity manualReviewEntity, string keyValue)
        {
            try
            {
                manualReviewEntity.F_RootID = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == manualReviewEntity.F_UserID).F_RootId.ToInt();
            }
            catch
            {
                return Error("发生错误，请检查此用户账户是否存在");
            }
            manualReviewEntity.F_ParentID = BLL.Sys_UserManager.Instance.GetModel(manualReviewEntity.F_UserID).F_ParentId;
            AddReviewlogEntity.F_ModuleName = "添加人工免审模板";
            AddReviewlogEntity.F_Type = DbLogType.Create.ToString();
            ChangeReviewlogEntity.F_ModuleName = "修改人工免审模板";
            ChangeReviewlogEntity.F_Type = DbLogType.Update.ToString();
            ChangeReviewlogEntity.F_Account = AddReviewlogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            ChangeReviewlogEntity.F_NickName = AddReviewlogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            try
            {
                manualReviewApp.SubmitForm(manualReviewEntity, keyValue);
                AddReviewlogEntity.F_Result = true;
                AddReviewlogEntity.F_Description = "添加人工免审模板成功";
                ChangeReviewlogEntity.F_Result = true;
                ChangeReviewlogEntity.F_Description = "修改人工免审模板成功";
                if (string.IsNullOrEmpty(keyValue))
                    new LogApp().WriteDbLog(AddReviewlogEntity);
                else
                    new LogApp().WriteDbLog(ChangeReviewlogEntity);
            }
            catch (Exception ex)
            {
                AddReviewlogEntity.F_Result = false;
                AddReviewlogEntity.F_Description = "添加人工免审模板失败," + ex;
                ChangeReviewlogEntity.F_Result = false;
                ChangeReviewlogEntity.F_Description = "修改人工免审目标失败," + ex;
                if (string.IsNullOrEmpty(keyValue))
                    new LogApp().WriteDbLog(AddReviewlogEntity);
                else
                    new LogApp().WriteDbLog(ChangeReviewlogEntity);
                return Error("操作失败，请重试!" + ex);
            }
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            DeleteReviewlogEntity.F_ModuleName = "删除人工免审模板";
            DeleteReviewlogEntity.F_Type = DbLogType.Delete.ToString();
            DeleteReviewlogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            DeleteReviewlogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            try
            {
                manualReviewApp.DeleteForm(keyValue);
                DeleteReviewlogEntity.F_Result = true;
                DeleteReviewlogEntity.F_Description = "删除人工免审模板成功";
                new LogApp().WriteDbLog(DeleteReviewlogEntity);
            }
            catch (Exception ex)
            {
                DeleteReviewlogEntity.F_Result = false;
                DeleteReviewlogEntity.F_Description = "删除人工免审模板失败" + ex;
                new LogApp().WriteDbLog(DeleteReviewlogEntity);
                return Error("删除失败，请重试." + ex);
            }
            return Success("删除成功。");
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Export(string F_UserID, string F_RootID, string F_ParentID)
        {
            //后台封装
            string queryJson = "{\"F_UserID\":\"" + F_UserID + "\",\"F_RootID\":\"" + F_RootID + "\",\"F_ParentID\":\"" + F_ParentID + "\"}";

            List<OC_ManualReviewTemplete> list = NFine.BLL.OC_ManualReviewTempleteManager.Instance.GetList(queryJson);
            DataTable dt = ListToTable(list);
            NFine.Code.Excel.NPOIExcel helper = new Code.Excel.NPOIExcel();
            string path = "/Resource/ManualReviewTemplete-Export/" + DateTime.Now.ToString("yyyy/MM/dd");
            string fileName = Guid.NewGuid() + ".xls";
            if (!NFine.Code.FileHelper.IsExistDirectory(Server.MapPath(path)))
                NFine.Code.FileHelper.CreateDir(path);
            helper.ToExcel(dt, "人工免审模板", "sheet1", Server.MapPath(path + "/" + fileName));
            return DownFile(path + "/" + fileName, "人工免审模板.xls");

        }

        /// <summary>
        /// List对象转换成Table
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable ListToTable(List<OC_ManualReviewTemplete> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("用户id");
            dt.Columns.Add("祖宗id");
            dt.Columns.Add("父id");

            foreach (var model in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = model.F_UserID;
                dr[1] = model.F_RootID;
                dr[2] = model.F_ParentID;
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }
}
