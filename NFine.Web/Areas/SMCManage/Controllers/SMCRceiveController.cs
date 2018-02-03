using NFine.Application.SMCManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.SMCManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
using NFine.Entity;
using NFine.Application.SystemManage; 


namespace NFine.Web.Areas.SMCManage.Controllers
{
    public class SMCRceiveController : ControllerBase
    {

        private SMCRceiveApp smcRceiveApp = new SMCRceiveApp();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = smcRceiveApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            smcRceiveApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SMCRceiveEntity smcRceiveEntity, string keyValue)
        {
            smcRceiveApp.SubmitForm(smcRceiveEntity, keyValue);
            return Success("操作成功。");
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        
        public ActionResult Export(string F_Mobile, string F_SmsContent, string GroupId, string F_RceiveTime, bool F_TA)

        {
            string queryJson = "{\"F_Mobile\":\"" + F_Mobile + "\",\"F_SmsContent\":\"" + F_SmsContent + "\",\"GroupId\":\"" + GroupId + "\",\"F_RceiveTime\":\"" + F_RceiveTime + "\",\"F_TA\":\"" + F_TA + "\"}";
            List<SMC_ReplyMessage> list = NFine.BLL.SMC_RceiveSmsManager.Instance.GetList(queryJson);
            DataTable dt = ListToTable(list);
            NFine.Code.Excel.NPOIExcel helper = new Code.Excel.NPOIExcel();
            string path = "/Resource/SMC_RceiveSms-Export/" + DateTime.Now.ToString("yyyy/MM/dd");
            string fileName = Guid.NewGuid() + ".xls";
            if (!NFine.Code.FileHelper.IsExistDirectory(Server.MapPath(path)))
                NFine.Code.FileHelper.CreateDir(path);
            helper.ToExcel(dt, "收件列表", "sheet1", Server.MapPath(path + "/" + fileName));
            return DownFile(path + "/" + fileName, "收件列表.xls");
        }

        /// <summary>
        /// List对象转换成Table
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable ListToTable(List<SMC_ReplyMessage> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("手机号");
            dt.Columns.Add("内容");
            dt.Columns.Add("类型");
            dt.Columns.Add("收件日期");
            dt.Columns.Add("发件人姓名");

            foreach (var model in list)
            {
                UserApp userApp = new UserApp();
                //主键值的类型与实体中定义的类型不匹配 待修改
                // string ReceiveName = userApp.GetForm(model.F_CreatorUserId).F_RealName;
                Sev_SendDateDetail sendmodel = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == model.MsgID);
                DataRow dr = dt.NewRow();
                dr[0] = model.mobile;
                dr[1] = model.receive_content;
                //组名待修改 dr[2] = model.GroupName;
                dr[2] = sendmodel.F_Operator ;
                dr[3] = model.receive_time;
                dr[4] = sendmodel.F_UserId;
                //dr[4] = ReceiveName;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    
        
    }
}
