using NFine.Application.CRMManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.CRMManage;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.CRMManage.Controllers
{
   
    public class TaskController : ControllerBase
    {

        private TaskApp taskApp = new TaskApp();
        public ActionResult NewTaskForm()
        {
            return View();
        }
         
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = taskApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]

        //获取F_Id字段为keyValue值的数据并返回JSon类型
        public ActionResult GetFormJson(string keyValue)
        {
            var data = taskApp.GetForm(keyValue);

            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        //单个数据提交
        //taskEntity:待提交数据
        //keyValue:区分提交方式标志，为空：添加数据，否则修改数据
        public ActionResult SubmitForm(TaskEntity taskEntity, string keyValue)
        {
            taskApp.SubmitForm(taskEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        //任务进度更新
        //遗留问题：进度字段读取出错，待修改？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？
        public ActionResult UpdateForm(List<TaskEntity> updateList)   
        {
            int n = 0;
            foreach (TaskEntity t in updateList)
            {
                taskApp.SubmitForm(t,t.F_Id);
                n++;
            } 
            return Success("成功更新" + n + "项任务进度。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken] //防伪表单字段不存在问题
        //批量数据提交，直接添加数据
       //taskList:待提交数据 
        public ActionResult BatchSubmitForm(List<TaskEntity> taskList)    
        {
            int n = 0;
            foreach (TaskEntity t in taskList)
           {
                taskApp.SubmitForm(t, "");
                n++;
            } 

            return Success("成功添加"+n+"项任务。");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        //删除F_Id字段为keyValue值的数据
        public ActionResult DeleteForm(string keyValue)
        {
            taskApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
    }
}

