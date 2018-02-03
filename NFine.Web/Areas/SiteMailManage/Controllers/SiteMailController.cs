using System;
using System.Collections.Generic;
using System.IO;
using NFine.Application.SiteMailManage;
using NFine.Code;
using NFine.Domain.Entity.SiteMailManage;
using System.Linq;
using System.Web.Mvc;
using NFine.Application.OCManage; 
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.OCManage; 
using System.Web; 
using NFine.Entity;

using NFine.Application.SystemManage;
using NFine.Application.SMCManage;  

namespace NFine.Web.Areas.SiteMailManage.Controllers
{
    public class SiteMailController : ControllerBase
    {
        public ActionResult SendMailForm()
        {
            return View();
        }
        public ActionResult ReplyForm()
        {
            return View();
        }
        public ActionResult Inbox()
        {
            return View();
        }

        private SMailMessageTApp smailMessageTApp = new SMailMessageTApp();
        private SMailMessageApp smailMessageApp = new SMailMessageApp();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetReceiveGridJson(Pagination pagination,string queryJson)
        {
            //登录信息对应字段查看LoginController.cs文件  
            var userId = OperatorProvider.Provider.GetCurrent().UserId;//当前登录账户ID ；
            /*
            var RoleId = OperatorProvider.Provider.GetCurrent().RoleId;//当前登录账户ID ；
            RoleApp roleApp = new RoleApp();

            string F_Type;
            try      //针对admin用户信息未完善无法获取F_Type值问题，待删除
            {
                F_Type = roleApp.GetForm(RoleId).F_Type; //1:系统2业务3其他;//获取角色
            }
            catch
            {
                F_Type = "1";
            }
            */
            var data = new
            {
                rows = BLL.Self.SMailMessageBLL.Instance.GetReceiveList(userId, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSendGridJson(Pagination pagination)
        {
            //登录信息对应字段查看LoginController.cs文件  
            var userId = OperatorProvider.Provider.GetCurrent().UserId;//当前登录账户ID ；
            var RoleId = OperatorProvider.Provider.GetCurrent().RoleId;//当前登录账户ID ；
            RoleApp roleApp = new RoleApp();
            string F_Type;
            try      //针对admin用户信息未完善无法获取F_Type值问题，待删除
            {
                F_Type = roleApp.GetForm(RoleId).F_Type; //1:系统2业务3其他;//获取角色
            }
            catch
            {
                F_Type = "1";
            }
            var data = new
            {
                rows = BLL.Self.SMailMessageBLL.Instance.GetSendList(userId),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = smailMessageTApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        public ActionResult SubmitForm(SMailMessageTEntity smailMessageTEntity, string keyValue)  //keyValue：收件人F_Id字符串
        {
            smailMessageTEntity.F_AppendixPath = UploadAppendix();
            smailMessageTEntity.F_DeleteMark = false;
            smailMessageTApp.SubmitForm(smailMessageTEntity, "");//提交到SMail_MessageText表

            var userId = OperatorProvider.Provider.GetCurrent().UserId; //当前登录账户id 
            var F_MessageID = smailMessageTEntity.F_Id; //当前发送短信F_Id

            string[] re = keyValue.Split(',');
            int i = 0;
            for (i = 0; i < re.Length; i++)
            {
                SMailMessageEntity sm = new SMailMessageEntity
                {
                    F_RecID = re[i],
                    F_SendID = userId,
                    F_MessageID = F_MessageID, 
                };
                smailMessageApp.SubmitForm(sm, ""); //对应信息提交到SMail_Message表
            }

            return Success("操作成功。" + smailMessageTEntity.F_AppendixPath);

        }

        private string UploadAppendix()
        {
            HttpPostedFileBase postedFile = Request.Files["fileField"];//获取上传信息对象
            string filePath;
            if (postedFile != null)
            {
                filePath = postedFile.FileName;//获取上传的文件路径
                string path;   //上传文件服务器上路径
                string[] filetypes = { ".gif", ".jpg", ".bmp", ".png", ".zip", ".rar", ".txt", ".doc", ".docx", ".xls", ".xlsx", ".TXT" };
                string fileExt = filePath.Substring(filePath.LastIndexOf("."));
                bool isLegal = false;
                for (int i = 0; i < filetypes.Length; i++)
                {
                    if (filetypes[i] == fileExt)
                    {
                        isLegal = true;
                        break;
                    }
                }
                if (isLegal)//有效文件格式
                {
                    try
                    {
                        //服务器上传文件夹路径
                        string upfilePath = Server.MapPath("~/FileUpLoad/upload/");
                        if (!Directory.Exists(upfilePath))
                        {
                            Directory.CreateDirectory(upfilePath);
                        }
                        //文件在服务器上的路径
                        path = System.IO.Path.Combine(Server.MapPath("~/FileUpLoad/upload/"), System.IO.Path.GetFileName(DateTime.Now.ToString("yyyymmddhhMMss") + fileExt));
                        postedFile.SaveAs(path);//保存

                        //删除服务器上的上传的文件
                        //if (System.IO.File.Exists(path))
                        //{
                        //    System.IO.File.Delete(path);
                        //}
                        return path;
                    }
                    catch
                    {
                        return "文件上传失败！";
                    }
                }
                else
                {
                    return "文件格式不正确！";
                }

            }
            return "";
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            smailMessageApp.DeleteForm(keyValue);
            return Success("删除成功。"); 
        }
        //标记已读
        public ActionResult MarkRead(string keyValue)
        {
            string[] ids = keyValue.Split(',');
            int i = 0, j = 0;
            foreach (string id in ids)
            {
                SMailMessageEntity s = smailMessageApp.GetForm(id);
                //可为空的对象必须有一个值
                if (s.F_Statue.ToString() == "True")
                { j++; }
                else
                {
                    s.F_Statue = true;
                    i++;
                }
                smailMessageApp.SubmitForm(s, id);
            }
            return Success("共选中" + (i + j) + "条短信，其中成功标记" + i + "条短信已读," + j + "条短信无需标记。");
        }
        //标记未读
        public ActionResult MarkNotRead(string keyValue)
        {
            string[] ids = keyValue.Split(',');
            int i = 0, j = 0; 
            foreach (string id in ids)
            {
                SMailMessageEntity s = smailMessageApp.GetForm(id);
                //可为空的对象必须有一个值
                if (s.F_Statue.ToString() == "False")
                { j++; }
                else
                {
                    s.F_Statue = false;
                    i++;
                }  
                smailMessageApp.SubmitForm(s, id);
            }
            return Success("共选中" + (i + j) + "条短信，其中成功标记" + i + "条短信未读," + j + "条短信无需标记。");
        }
        [HttpGet]
        [HandlerAjaxOnly]

        //获取F_Id字段为keyValue值的数据并返回JSon类型
        public ActionResult GetFormJson(string keyValue)
        {
            var data = smailMessageApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        //回复
        public ActionResult GetReplyJson(string keyValue)
        {
            var data = BLL.Self.SMailMessageBLL.Instance.GetReply(keyValue);
            
            return Content(data[0].ToJson());
        }
    }
}
