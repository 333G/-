using NFine.Entity;
using NFine.Application.OCManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.Entity.SystemManage;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using NFine.Web;
using NFine.Domain.Entity.SystemSecurity;
using NFine.Application.SystemSecurity;
using NFine.Application;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class UserInfoController : ControllerBase
    {
        private UserInfoApp userInfoApp = new UserInfoApp();
        LogEntity DeletelogEntity = new LogEntity();
        LogEntity MovelogEntity = new LogEntity();
        LogEntity ChangelogEntity = new LogEntity();
        LogEntity AddGroupChannellogEntity = new LogEntity();
        LogEntity DeleteGroupChannellogEntity = new LogEntity();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = BLL.Self.OC_UserInfoBLL.Instance.GetReceiveList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        //联查其他表字段信息
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetReceiveGridJson(Pagination pagination, string queryJson)
        {
            var data = new {
                rows = userInfoApp.GetList(pagination,queryJson),
                //rows = BLL.Self.OC_UserInfoBLL.Instance.GetReceiveList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        //F_CRMID的添加
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult AddCRM(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string F_UserFId = queryParam["F_UserFId"].ToString();
            long F_CRMID = queryParam["F_CRMId"].ToInt();
            if (DAL.Self.OCUserInfoDAL.Instance.AddCRM(F_UserFId, F_CRMID))
                return Success("添加成功！");
            else
                return Error("添加失败，请重试！");
        }

        public ActionResult AddForm()
        {
            return View();
        }
        public ActionResult EditMessageNumForm()
        {
            return View();
        }
        public ActionResult ChangePasswordForm()
        {
            return View();
        }
        public ActionResult ChangeManagerIdForm()
        {
            return View();
        }
        public ActionResult BindChannelForm()
        {
            return View();
        }
        public ActionResult SendMessageForm()
        {
            return View();
        }
        public ActionResult AddCRMForm()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)//keyvalue 是F_Id
        {
            var data = userInfoApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        //根据Id获取Form数据
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJsonById(int id)//Id
        {
            var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == id);
            return Content(data.ToJson());
        }

        //提交组合通道
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitGropuChannelForm(OC_GroupChannel Model_GroupChannel, int keyValue)
        {
            AddGroupChannellogEntity.F_ModuleName = "添加组合通道";
            AddGroupChannellogEntity.F_Type = DbLogType.Submit.ToString();
            AddGroupChannellogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            AddGroupChannellogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            GroupChannelApp groupchannelapp = new GroupChannelApp();
            try
            {
                groupchannelapp.SubmitForm(Model_GroupChannel, keyValue);
                AddGroupChannellogEntity.F_Result = true;
                AddGroupChannellogEntity.F_Description = "添加组合通道成功";
                new LogApp().WriteDbLog(AddGroupChannellogEntity);
            }
            catch(Exception ex)
            {
                AddGroupChannellogEntity.F_Result = false;
                AddGroupChannellogEntity.F_Description = "添加组合通道失败" + ex;
                new LogApp().WriteDbLog(AddGroupChannellogEntity);
            }

            return Success("操作成功。");
        }


        //提交修改资料表单
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(OC_UserInfo Model_UserInfo,UserEntity userentity, string keyValue /*int keyValue*/)
        {
            UserInfoApp userinfoapp = new UserInfoApp();
            UserApp userapp = new UserApp();
            var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_Id == keyValue);
            ChangelogEntity.F_ModuleName = "修改用户信息";
            ChangelogEntity.F_Type = DbLogType.Submit.ToString();
            ChangelogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            ChangelogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            //更新
            result.F_Account = Model_UserInfo.F_Account;
            result.F_Description = Model_UserInfo.F_Description;
            result.F_State = Model_UserInfo.F_State;
            result.F_Reviewed = Model_UserInfo.F_Reviewed;
            DAL.OC_UserInfoDAL.Instance.Update(result);
            userentity.Id = result.F_UserId.ToInt();
            try
            {
                userapp.UpdateForm(userentity);
                ChangelogEntity.F_Result = true;
                ChangelogEntity.F_Description = "修改用户信息成功";
                new LogApp().WriteDbLog(ChangelogEntity);
            }
            catch (Exception ex)
            {
                ChangelogEntity.F_Result = false;
                ChangelogEntity.F_Description = "修改用户信息失败," + ex;
                new LogApp().WriteDbLog(ChangelogEntity);
            }
            return Success("操作成功。");
        }

        //提交修改短信限制数表单
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult LimitSubmitForm(string keyValue)
        {
            var model = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_Id == keyValue);
            model.F_MessageNum = Request["F_MessageNum"].ToInt();//每手机24小时内最多接收短信
            model.F_OneCode = Request["F_OneCode"].ToInt();//1小时验证码限制
            model.F_TwentyFourCode = Request["F_TwentyFourCode"].ToInt();//24小时验证码限制
            DAL.OC_UserInfoDAL.Instance.Update(model);
            return Success("操作成功。");
        }


        //审核
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Review(string keyValue)
        {

            var review=userInfoApp.GetForm(keyValue);
            if (review.F_State == "正常")
            {
                return Success("正常状态，无需修改。");
            }
            else
            {
                review.F_State = "正常";
                userInfoApp.SubmitForm(review, keyValue);
                return Success("状态已改为正常。");
            }
        }

        //批量转移（批量重新绑定业务员ID）
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitBindChannelForm(OC_GroupChannel Model,string keyValue)
        {
            string[] F_UserIdArr = keyValue.Split(',');
            int[] F_UserIdAri = Array.ConvertAll(F_UserIdArr, new Converter<string, int>(StrToInt));//获取UserId数组
            List<OC_GroupChannel> list = new List<OC_GroupChannel>();
            MovelogEntity.F_ModuleName = "批量转移用户";
            MovelogEntity.F_Type = DbLogType.Update.ToString();
            MovelogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            MovelogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            try
            {
                foreach (int item in F_UserIdAri)
                {
                    OC_GroupChannel newModel = new OC_GroupChannel();
                    //newModel = Model;//类之间为引用，不能这么使用。直接引用的内存地址，会导致出错
                    //赋值
                    newModel.F_UserId = item;
                    newModel.F_AdminMark = Model.F_AdminMark;
                    newModel.F_ChannelName = Model.F_ChannelName;
                    newModel.F_ChannelPrice = Model.F_ChannelPrice;
                    newModel.F_ChannelType = Model.F_ChannelType;
                    newModel.F_ChannelXLT = Model.F_ChannelXLT;
                    newModel.F_hide = Model.F_hide;
                    newModel.F_MobileChannel = Model.F_MobileChannel;
                    newModel.F_Priority = Model.F_Priority;
                    newModel.F_SendRate = Model.F_SendRate;
                    newModel.F_SuccessRate = Model.F_SuccessRate;
                    newModel.F_TelecomChannel = Model.F_TelecomChannel;
                    newModel.F_UnicomChannel = Model.F_UnicomChannel;
                    newModel.F_UserMark = Model.F_UserMark;
                    if (DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == item).F_RootId == item)//判断是否为祖宗用户，如果是，就添加通道
                    {
                        newModel.F_ID = Common.GuId();
                        list.Add(newModel);
                    }
                }
                DAL.OC_GroupChannelDAL.Instance.Add(list);//批量生成
                MovelogEntity.F_Result = true;
                MovelogEntity.F_Description = "批量转移用户成功";
                new LogApp().WriteDbLog(MovelogEntity);
            }
            catch (Exception ex)
            {
                MovelogEntity.F_Result = false;
                MovelogEntity.F_Description = "批量转移用户失败" + ex;
                new LogApp().WriteDbLog(MovelogEntity);
                return Error("发生错误：" + ex + "，请联系管理员！");
            }
            return Success("操作成功！");
        }
        public ActionResult batchMove(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string keyValue = queryParam["F_UserId"].ToString();
            string[] F_UserIdArr = keyValue.Split(',');
            int[] F_UserIdAri = Array.ConvertAll(F_UserIdArr, new Converter<string, int>(StrToInt));
            List<OC_UserInfo> list = new List<OC_UserInfo>();
            MovelogEntity.F_ModuleName = "批量转移用户";
            MovelogEntity.F_Type = DbLogType.Update.ToString();
            MovelogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            MovelogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            try
            {
                foreach (int item in F_UserIdAri)
                {
                    var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == item);
                    data.F_ManagerId = queryParam["F_ManagerId"].ToInt();
                    list.Add(data);
                }
                DAL.OC_UserInfoDAL.Instance.UpdateRange(list);//批量更新
                MovelogEntity.F_Result = true;
                MovelogEntity.F_Description = "批量转移用户成功";
                new LogApp().WriteDbLog(MovelogEntity);
            }
            catch (Exception ex)
            {
                MovelogEntity.F_Result = false;
                MovelogEntity.F_Description = "批量转移用户失败" + ex;
                new LogApp().WriteDbLog(MovelogEntity);
                return Error("发生错误：" + ex + "，请联系管理员！");
            }
            return Success("操作成功!");
        }

        public static int StrToInt(string str)
        {
            return int.Parse(str);
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            string[] F_UserIdArr = keyValue.Split(',');
            int[] F_UserIdAri = Array.ConvertAll(F_UserIdArr, new Converter<string, int>(StrToInt));
            DeletelogEntity.F_ModuleName = "删除用户";
            DeletelogEntity.F_Type = DbLogType.Delete.ToString();
            DeletelogEntity.F_Account = OperatorProvider.Provider.GetCurrent().UserCode;
            DeletelogEntity.F_NickName = OperatorProvider.Provider.GetCurrent().UserName;
            foreach (int item in F_UserIdAri)
            {
                //DAL.OC_UserInfoDAL.Instance.Delete(t => t.F_UserId == item);
                var result = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == item);
                try
                {
                    DAL.Sys_UserDAL.Instance.Delete(t => t.Id == item);//联删Sys_User中的数据
                    DAL.OC_GroupChannelDAL.Instance.Delete(t => t.F_UserId == item);//删除用户通道(如果有的话）
                    var sql = "select * from SMC_SendSms where F_CreatorUserId = '" + result.F_UserFid + "'";
                    List<SMC_SendSms> list = DAL.SMC_SendSmsDAL.Instance.FindList(sql);
                    int length = list.Count;
                    for (int i=0; i<length; i++)
                    {
                        DAL.SMC_SendSmsDAL.Instance.Delete(t => t.F_CreatorUserId == list[i].F_CreatorUserId);//删除普通用户时同时删除其发送的短信
                    }
                    DeletelogEntity.F_Result = true;
                    DeletelogEntity.F_Description = "删除用户成功";
                    new LogApp().WriteDbLog(DeletelogEntity);
                }
                catch {
                    new LogApp().WriteDbLog(DeletelogEntity);
                    return Success("删除成功。");
                }
                DAL.OC_UserInfoDAL.Instance.Delete(t => t.F_UserId == item);
            }
            return Success("联删成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult InsertUserInfo(int id)
        {
            var data = BLL.Sys_UserManager.Instance.GetModel(id);
            string fid = data.F_Id;
            UserInfoEntity userInfoEntity = new UserInfoEntity();
            userInfoEntity.F_UserFid = fid;
            userInfoEntity.F_Account = data.F_Account;
            userInfoEntity.F_UserId = data.Id;
            userInfoEntity.F_RootId = data.Id;
            userInfoEntity.F_CreatorUserId = fid;
            userInfoEntity.F_CreatorTime = data.F_CreatorTime;
            userInfoEntity.F_Reviewed = 1;//默认条条需审核
            userInfoEntity.F_State = "正常";
            string keyValue = null;
            if (data.F_IsAdministrator == false)
            {
                userInfoApp.SubmitForm(userInfoEntity, keyValue);
            }
            return Success("操作成功。");
           // return Content(data.ToJson());
        }

        //根据用户UserId修改余额是否提醒状态

        public ActionResult BalanceReminder(string keyValue)
        {
            var model = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_Id == keyValue);
            if (model.F_BalanceReminder!=null)
            {
                if (model.F_BalanceReminder == false)
                    model.F_BalanceReminder = true;
                else
                    model.F_BalanceReminder = false;
                DAL.OC_UserInfoDAL.Instance.Update(model);
            }
            else
            {
                model.F_BalanceReminder = true;
                DAL.OC_UserInfoDAL.Instance.Update(model);
            }
            return Success("状态已修改！");
        }
        //根据用户UserId获取时间
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetUserTime(int UserId)
        {
            var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == UserId);
            if (data.F_CreatorTime != null)
                return data.F_CreatorTime.ToString();
            else
                return "待定";
        }

        //根据用户UserId获取最多接收短信数
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetUserMessageNum(int UserId)
        {
            var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == UserId);
            if (data.F_MessageNum != null)
                return data.F_MessageNum.ToString();
            else
                return "0";
        }

        //根据Id获取余额
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetF_Balance(int id)//Id
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            string sql = "select F_Id from OC_UserInfo ";
            List<OC_UserInfo> resultList = DAL.OC_UserInfoDAL.Instance.FindList(sql);
            int length = resultList.Count;
            Dictionary<string, string> map = new Dictionary<string, string>();
            for (int i = 0; i < length; i++)
            {
                OC_UserInfo oc_UserInfo = resultList[i];
                map.Add(oc_UserInfo.F_Id, oc_UserInfo.F_Id);
            }
            if (map.ContainsKey(LoginInfo.UserId)) //判断是否为普通用户,若是显示余额
            {
                var data = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == id);
                return data.F_Balance.ToString();//显示余额
            }
            else if (!map.ContainsKey(LoginInfo.UserId) & LoginInfo.UserCode != "admin")//当前登陆者为业务员，没有余额
                return "0";
            else//管理员登录，没有余额
                return "0";
        }
    }
}



