using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFine.Code;
using NFine.Application.OCManage;
using NFine.Entity;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Application.SystemSecurity;
using NFine.Application;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class UserChannlController : ControllerBase
    {
        GroupChannelApp groupchannelapp = new GroupChannelApp();
        LogEntity ChangeGroupChannellogEntity = new LogEntity();
        LogEntity DeleteGroupChannellogEntity = new LogEntity();
        public ActionResult editChannelForm()
        {
            return View();
        }
        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetGridJson(Pagination pagination, string queryJson)
        //{
        //    var data = new
        //    {
        //        rows = groupchannelapp.getlist(pagination, queryJson),
        //        total = pagination.total,
        //        page = pagination.page,
        //        records = pagination.records
        //    };
        //    return Content(data.ToJson());
        //}

        public ActionResult GetFormJson(string keyValue)//keyvalue 是F_Id
        {
            var data = DAL.OC_GroupChannelDAL.Instance.FindEntity(t => t.F_ID == keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 获取发送界面的通道。返回当前用户的祖宗账号的组合通道
        /// </summary>
        /// <param name="keyvalue">传递当前用户的Id</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetChannel(int keyvalue)
        {
            try
            {
                int RootId = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == keyvalue).F_RootId.ToInt();//根据Id获取RootId
                var data = DAL.OC_GroupChannelDAL.Instance.FindList((t => t.F_UserId == RootId));
                return Content(data.ToJson());
            }
            catch
            {
                var data = 0;
                return  Content(data.ToJson());
            }
        }

        //修改组合通道
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitGropuChannelForm(OC_GroupChannel Model_GroupChannel, string keyValue)
        {
            Model_GroupChannel.F_ID = keyValue;//重新赋一遍F_ID值，用于实现更新操作，实则F_ID值并未变化！
            ChangeGroupChannellogEntity.F_ModuleName = "修改组合通道";
            ChangeGroupChannellogEntity.F_Type = DbLogType.Update.ToString();
            ChangeGroupChannellogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            ChangeGroupChannellogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            try
            {
                DAL.OC_GroupChannelDAL.Instance.Update(Model_GroupChannel);
                ChangeGroupChannellogEntity.F_Result = true;
                ChangeGroupChannellogEntity.F_Description = "修改组合通道成功";
                new LogApp().WriteDbLog(ChangeGroupChannellogEntity);
            }
            catch (Exception ex)
            {
                ChangeGroupChannellogEntity.F_Result = false;
                ChangeGroupChannellogEntity.F_Description = "修改组合通道失败";
                new LogApp().WriteDbLog(ChangeGroupChannellogEntity);
                return Error("出现错误，请重试或联系管理员！" + ex);
            }
            return Success("操作成功。");
        }

        //用于取前台select ChannelId的数据
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson(int keyvalue)
        {
            List<OC_GroupChannel> data = DAL.OC_GroupChannelDAL.Instance.FindList(t => t.F_UserId == keyvalue);
            var treeList = new List<TreeSelectModel>();
            //往treelist中添加空的节点。
            TreeSelectModel nullModel = new TreeSelectModel();
            nullModel.id = "1";
            nullModel.text = "==请选择==";
            nullModel.parentId = "0";
            nullModel.data = null;
            treeList.Add(nullModel);
            foreach (OC_GroupChannel item in data)
            {
                //BaseChannel表中的Id就是ChannelProvince表中的F_ChannelId
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_ID;
                treeModel.text = item.F_ChannelName;
                treeModel.parentId = "0";
                treeModel.data = item.F_ID;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            DeleteGroupChannellogEntity.F_ModuleName = "删除组合通道";
            DeleteGroupChannellogEntity.F_Type = DbLogType.Delete.ToString();
            DeleteGroupChannellogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            DeleteGroupChannellogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            try
            {
                DAL.OC_GroupChannelDAL.Instance.Delete(t => t.F_ID == keyValue);//删除用户通道(如果有的话）
                DeleteGroupChannellogEntity.F_Result = true;
                DeleteGroupChannellogEntity.F_Description = "删除组合通道成功";
                new LogApp().WriteDbLog(DeleteGroupChannellogEntity);
            }
            catch(Exception ex)
            {
                DeleteGroupChannellogEntity.F_Result = true;
                DeleteGroupChannellogEntity.F_Description = "删除组合通道失败" + ex;
                new LogApp().WriteDbLog(DeleteGroupChannellogEntity);
                return Error("出现错误，请重试" + ex);
            }
            return Success("删除成功。");
        }


    }
}
