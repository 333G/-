using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NFine.Entity;
using NFine.Domain.Entity.OCManage;
using System.Web.WebPages;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Application.SystemSecurity;
using NFine.Application;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class UserController : ControllerBase
    {

        LogEntity AddUserlogEntity = new LogEntity();
        LogEntity ChangePasswordlogEntity = new LogEntity();
        private UserApp userApp = new UserApp();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        public ActionResult AddForm()
        {
            return View();
        }
        public ActionResult childRechargeForm()
        {
            return View();
        }
        //所有用户
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeUserJson()
        {
            var data = BLL.Sys_UserManager.Instance.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (Sys_User item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_RealName;
                treeModel.parentId = "0";
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        //所有业务员
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSalesJson()
        {
            var data = BLL.Sys_UserManager.Instance.GetList("1");
            var treeList = new List<TreeSelectModel>();

            foreach (Sys_User item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.Id.ToString();
                treeModel.text = item.F_RealName;
                treeModel.parentId = "0";
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = BLL.Sys_UserManager.Instance.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetChildGridJson(Pagination pagination, string keyword)
        {
            string userId = OperatorProvider.Provider.GetCurrent().UserId;
            var data = new
            {
                rows = BLL.Sys_UserManager.Instance.GetList(pagination, userId, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(int id)
        {
            //var data = userApp.GetForm(id);
            var data = BLL.Sys_UserManager.Instance.GetModel(id);
            return Content(data.ToJson());
        }

        //提交表单
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Sys_User userEntity, UserInfoEntity userInfoEntity, UserLogOnEntity userLogOnEntity, string keyValue)//keyValue是F_Id
        {
            try
            {
                userApp.SubmitForm(userEntity, userInfoEntity, userLogOnEntity, keyValue);//新用户表单
                return Success("操作成功。");
            }
            catch (Exception e)
            {
                return Error("操作失败，请检查输入信息!" + e);
            }
        }
        //添加普通用户页面的提交表单
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult NomalSubmitForm(Sys_User userEntity, UserInfoEntity userInfoEntity, UserLogOnEntity userLogOnEntity, string keyValue)//keyValue是F_Id
        {

            AddUserlogEntity.F_ModuleName = "添加用户";
            AddUserlogEntity.F_Type = DbLogType.Create.ToString();
            AddUserlogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            AddUserlogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            userEntity.F_IsAdministrator = false;
            userEntity.F_RoleId = "2DA8390B-61A4-4E6C-A6E7-8F6794C7EDCE";//给两个标记赋值，均设置为为普通用户
            try
            {
                userApp.SubmitForm(userEntity, userInfoEntity, userLogOnEntity, keyValue);//新用户表单
                AddUserlogEntity.F_Result = true;
                AddUserlogEntity.F_Description = "添加用户成功" ;
                new LogApp().WriteDbLog(AddUserlogEntity);
                return Success("操作成功。");
            }
            catch (Exception e)
            {
                AddUserlogEntity.F_Result = false;
                AddUserlogEntity.F_Description = "添加用户失败，" + e;
                new LogApp().WriteDbLog(AddUserlogEntity);
                if (e.HResult == -2146233088)
                {
                    return Error("不能创建重复账户，请检查表单信息！");
                }
                else
                    return Error("提交表单有误" + e);
            }
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(int keyValue)//F_Id?
        {
            DAL.Sys_UserDAL.Instance.Delete(t => t.Id == keyValue);
            try
            {
                DAL.OC_UserInfoDAL.Instance.Delete(t => t.F_UserId == keyValue);//一同删除OC_UserInfo中的数据（如果有的话）
                DAL.OC_GroupChannelDAL.Instance.Delete(t => t.F_UserId == keyValue);//一同删除用户通道（如果有的话)
            }
            catch { return Success("删除成功"); }
            return Success("联删成功。");
        }

        [HttpGet]
        public ActionResult RevisePassword()
        {
            return View();
        }

        //修改密码
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string userPassword, string keyValue, int Id, UserEntity userentity)//修改后的密码，和F_Id
        {
            ChangePasswordlogEntity.F_ModuleName = "修改密码";
            ChangePasswordlogEntity.F_Type = DbLogType.Create.ToString();
            ChangePasswordlogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            ChangePasswordlogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            userLogOnApp.RevisePassword(userPassword, keyValue);
            userentity.Id = Id;
            userApp.UpdateForm(userentity);
            ChangePasswordlogEntity.F_Result = true;
            ChangePasswordlogEntity.F_Description = "修改密码成功";
            new LogApp().WriteDbLog(ChangePasswordlogEntity);
            return Success("修改成功。");
        }

        //修改签名
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult SubmitMark(int Id, UserEntity userentity)
        {
            userentity.Id = Id;
            userApp.UpdateForm(userentity);
            return Success("签名修改成功。");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult CheckPwd(string username, string password)
        {
            try
            {
                UserEntity userEntity = new UserApp().CheckLogin(username, password);
                return Content(new AjaxResult { state = ResultType.success.ToString(), message = "密码正确。" }.ToJson());
            }
            catch (Exception ex)
            {
                return Content(new AjaxResult { state = ResultType.error.ToString(), message = ex.Message }.ToJson());
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitChangePassword(string userPassword, string userAccount)
        {
            userLogOnApp.RevisePassword(userPassword, userAccount);
            return Success("修改密码成功。");
        }

        //禁用
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string id)
        {
            if (id == "undefined")
                return Error("请选择要处理的数据！");
            else
            {
                var data = DAL.Sys_UserDAL.Instance.FindEntity(t => t.F_Id == id);
                if (data.F_EnabledMark == true)
                    data.F_EnabledMark = false;
                else
                    return Error("账户已禁用，无需修改！");
                DAL.Sys_UserDAL.Instance.Update(data);
                return Success("账户禁用成功。");
            }
        }
        //启用
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult EnabledAccount(string id)
        {
            if (id == "undefined")
                return Error("请选择要处理的数据！");
            else
            {
                var data = DAL.Sys_UserDAL.Instance.FindEntity(t => t.F_Id == id);
                if (data.F_EnabledMark == false)
                    data.F_EnabledMark = true;
                else
                    return Error("账户已启用，无需修改！");
                DAL.Sys_UserDAL.Instance.Update(data);
                return Success("账户启用成功。");
            }
        }

        ////禁用
        //[HttpPost]
        //[HandlerAjaxOnly]
        //[HandlerAuthorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult DisabledAccount(string id)
        //{
        //    UserEntity userEntity = new UserEntity();
        //    userEntity = setUserEntity(userEntity, id);
        //    userEntity.F_EnabledMark = false;
        //    userApp.UpdateForm(userEntity);
        //    return Success("账户禁用成功。");
        //}

        ////启用
        //[HttpPost]
        //[HandlerAjaxOnly]
        //[HandlerAuthorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult EnabledAccount(string id)
        //{
        //    UserEntity userEntity = new UserEntity();
        //    userEntity = setUserEntity(userEntity, id);
        //    userEntity.F_EnabledMark = true;
        //    userApp.UpdateForm(userEntity);
        //    return Success("账户启用成功。");
        //}

        //补全提交字段
        private UserEntity setUserEntity(UserEntity userEntity, string id)
        {
            List<Sys_User> data = BLL.Sys_UserManager.Instance.GetDepthList(id);
            userEntity.Id = data[0].Id;
            userEntity.F_Id = data[0].F_Id;
            userEntity.F_Account = data[0].F_Account;
            return userEntity;
        }

        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChildIndex()
        {
            return View();
        }

        //根据用户F_Id取得用户UserId
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetUserIDsJson(string F_ID)
        {
            try
            {
                var data = userApp.GetForm(F_ID);
                return data[0].Id.ToString();
            }
            catch
            {
                return "null";
            }

        }

        //根据用户UserId获取签名
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetUserSignature(int UserId)
        {
            var data = BLL.Sys_UserManager.Instance.GetModel(UserId);
            if (data.F_Signature != null)
                return data.F_Signature.ToString();
            else
                return "无签名";
        }

        //根据用户UserId获取余额是否提醒状态
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetUserBalanceReminder(int UserId)
        {
            var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == UserId);
            if (data.F_BalanceReminder != null)
            {
                if (data.F_BalanceReminder == false)
                {
                    return "否";
                }
                else
                    return "是";
            }
            else
                return "否";
        }

        //根据Id获取F_Balance
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetF_Balance(int Id)
        {
            //var data = BLL.OC_UserInfoManager.Instance.GetModel(Id);
            var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == Id);
            return data.F_Balance.ToString();
        }

        //转账提交
        //提交表单
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult childRechargeSubmitForm(int id)
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            try
            {
                decimal balance = Request.Form["F_TransferAmount"].AsDecimal();
                var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == LoginInfo.Id);//父账户实体
                if (balance <= data.F_Balance)
                {
                    var childData = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == id);//子账户实体
                    decimal newF_Balance = data.F_Balance - balance;//给子账户转账后父账户余额
                    data.F_Balance = newF_Balance;
                    DAL.OC_UserInfoDAL.Instance.Update(data);//更新父账户余额
                    childData.F_Balance = balance + childData.F_Balance;//原有的余额加上被转的余额
                    DAL.OC_UserInfoDAL.Instance.Update(childData);//更新子账户余额
                    return Success("操作成功。");
                }
                else
                    return Error("余额不足，请检查!");
            }
            catch (Exception e)
            {
                return Error("操作失败，请检查输入信息!" );
            }
        }
   }
}
