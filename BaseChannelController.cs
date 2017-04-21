using NFine.Application.OCManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Code.ViewModel;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using NFine.Entity.Views;
using NFine.BLL;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class BaseChannelController : ControllerBase
    {
        private BaseChannelApp BaseChannelApp = new BaseChannelApp();
        private ChannelRechargeRecordApp ChannelRechargeRecordApp = new ChannelRechargeRecordApp();
        public ActionResult RechargeForm()
        {
            return View();
        }
        public ActionResult ProvinceList()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                //rows = BaseChannelApp.GetList(pagination, queryJson),
                rows = OC_BaseChannelManager.Instance.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        //在省份页面自动显示数据库的相应字段信息
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetProvinceJson(string keyvalue, string queryJson)
        {
            var data = new
            {
                rows = DAL.OC_ChannelProvinceDAL.Instance.GetList(keyvalue, queryJson),
            };
            return Content(data.ToJson());
        }

        //在省份表内填充省份通道名
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetChannelName(int ChannelId)
        {
            var data= DAL.OC_BaseChannelDAL.Instance.FindList(t => t.Id == ChannelId);
            return Content(data.ToJson())
;        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(int keyValue)
        {
            var data = BLL.OC_BaseChannelManager.Instance.Model(keyValue);
            return Content(data.ToJson());
        }

        //获取第二个表单中的数据，keyvalue是F_ID
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSecondFormJson(string keyvalue)
        {
            var data = OC_ChannelConfigManager.Instance.Model(keyvalue);
            return Content(data.ToJson());
        }

        //提交表单
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult newSubmitForm(BaseChannelEntity basechannelEntity, ChannelProvinceEntity channelprovinceEntity, ChannelConfigEntity channelconfigEntity, string keyValue)
        {
            bool result = false;
            OC_BaseChannel model_BC = new OC_BaseChannel();
            OC_ChannelConfig model_CC = new OC_ChannelConfig();
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (!string.IsNullOrEmpty(keyValue))//修改通道
            {
                model_BC.Id = keyValue.ToInt();
                //取原来数据，然后修改不同，再传，不然会出错
                OC_BaseChannel oldmodel_BC = BLL.OC_BaseChannelManager.Instance.Model(model_BC.Id);
                model_BC = oldmodel_BC;
                OC_ChannelConfig oldmodel_CC = BLL.OC_ChannelConfigManager.Instance.Model(model_BC.F_Id);
                model_CC = oldmodel_CC;
                if (LoginInfo != null)
                {
                    model_BC.F_LastModifyUserId = LoginInfo.UserId;
                    model_CC.F_LastModifyUserId = LoginInfo.UserId;
                }
                model_BC.F_LastModifyTime = DateTime.Now;
                model_CC.F_LastModifyTime = DateTime.Now;
                ValueMove(model_CC, model_BC, basechannelEntity, channelconfigEntity);//修改model数据
                result = BLL.OC_BaseChannelManager.Instance.Update(model_BC);//往数据库方向的更新model_BC
                DAL.OC_ChannelConfigDAL.Instance.Update(model_CC);//往数据库方向插入model_CC
            }
            else//新建通道
            {
                model_BC.F_Id = Common.GuId();
                model_CC.F_Id = model_BC.F_Id;//保证F_Id相等,方便关联查询
                if (LoginInfo != null)
                {
                    model_BC.F_CreatorUserId = LoginInfo.UserId;
                    model_CC.F_CreatorUserId = LoginInfo.UserId;
                }
                model_BC.F_CreatorTime = DateTime.Now;
                model_CC.F_CreatorTime = DateTime.Now;
                model_BC.F_EnabledMark = true;
                model_BC.F_DeleteMark = false;
                model_CC.F_DeleteMark = false;//初始化flag，使新建通道默认开启
                ValueMove(model_CC, model_BC, basechannelEntity, channelconfigEntity);//给model赋值
                int ResultId = BLL.OC_BaseChannelManager.Instance.Add(model_BC);//往数据库方向的插入操作,返回主键Id值
                model_CC.F_ChannelId = ResultId;//绑定ChannelId
                DAL.OC_ChannelConfigDAL.Instance.Add(model_CC);//往数据库方向插入model_CC,返回自加的Id值
                RollProvinceList(model_BC, ResultId);//插入省份
                result = ResultId > 0;//判断是否成功
            }
            if (result)
                return Success("操作成功。");
            return Error("操作失败，请刷新页面重试");
            //BaseChannelApp.newSubmitForm(basechannelEntity, channelprovinceEntity, channelconfigEntity, keyValue);//这种数据库操作方法行不通，数据库一直报错，无法解决               
        }

        // 把BaseChannelEntity和ChannelConfigentity中的值赋给model
        private void ValueMove(OC_ChannelConfig model_CC, OC_BaseChannel model_BC, BaseChannelEntity basechannelEntity, ChannelConfigEntity channnelconfigEntity)
        {
            //给model_BC赋值
            model_BC.F_ChannelName = basechannelEntity.F_ChannelName;
            model_BC.F_Operator = basechannelEntity.F_Operator;
            model_BC.F_ChannelState = basechannelEntity.F_ChannelState;
            model_BC.F_CharacterCount = basechannelEntity.F_CharacterCount;
            model_BC.F_StartTime = basechannelEntity.F_StartTime;
            model_BC.F_EndTime = basechannelEntity.F_EndTime;
            model_BC.F_Price = basechannelEntity.F_Price;
            model_BC.F_ChaBalance = basechannelEntity.F_ChaBalance;
            model_BC.F_SurplusNum = basechannelEntity.F_SurplusNum;
            model_BC.F_SendedNum = basechannelEntity.F_SendedNum;
            model_BC.F_autograph = basechannelEntity.F_autograph;
            model_BC.F_unsubscribe = basechannelEntity.F_unsubscribe;
            model_BC.F_Description = basechannelEntity.F_Description;
            model_BC.F_LongSmsNumber = basechannelEntity.F_LongSmsNumber;
            model_BC.F_LongSmsSign = basechannelEntity.F_LongSmsSign;
            model_BC.F_ChargeNumber = basechannelEntity.F_ChargeNumber;
            model_BC.F_MonitorMobile = basechannelEntity.F_MonitorMobile;
            model_BC.F_MonitorState = basechannelEntity.F_MonitorState;
            model_BC.F_MonitorTime = basechannelEntity.F_MonitorTime;
            //给model_CC赋值            
            model_CC.F_GatewayIP = channnelconfigEntity.F_GatewayIP;
            model_CC.F_GatewayPort = channnelconfigEntity.F_GatewayPort;
            model_CC.F_ProtocolType = channnelconfigEntity.F_ProtocolType;
            model_CC.F_Url = channnelconfigEntity.F_Url;
            model_CC.F_UserName = channnelconfigEntity.F_UserName;
            model_CC.F_UserPwd = channnelconfigEntity.F_UserPwd;
            model_CC.F_CompanyCode = channnelconfigEntity.F_CompanyCode;
            model_CC.F_CompanyNodeCode = channnelconfigEntity.F_CompanyNodeCode;
            model_CC.F_AccessNumber = channnelconfigEntity.F_AccessNumber;
        }

        //新增通道的时候，在ChannelProvince中插入34条数据（默认全省份开通此通道）,keyvalue来判断是新建还是修改     
        private void RollProvinceList(OC_BaseChannel model_BC, int ChannelId)
        {
            AreaApp areaApp = new AreaApp();
            var data = areaApp.GetList();
            foreach (AreaEntity item in data)
            {
                if (item.F_Layers == 1)
                {
                    OC_ChannelProvince model = new OC_ChannelProvince();
                    //赋值
                    model.F_ProvinceName = item.F_FullName;
                    model.F_ProvinceId = item.F_Id.ToInt();
                    model.F_Id = model_BC.F_Id;
                    model.F_ChannelId = ChannelId;
                    //标记值
                    model.F_CreatorUserId = model_BC.F_CreatorUserId;
                    model.F_CreatorTime = model_BC.F_CreatorTime;
                    DAL.OC_ChannelProvinceDAL.Instance.Add(model);
                }
            }
        }

        /// <summary>
        /// 获取省份框中的内容keyvalue 是F_Id
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetProvinceName(string keyvalue)
        {
            string queryJson = null;
            var data = DAL.OC_ChannelProvinceDAL.Instance.GetList(keyvalue, queryJson);
            return Content(data.ToJson());
        }

        //往省份表更新修改的ChannelId
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult submitEditProvinceForm(List<ChannelProvinceEntity> channelprovinceentity, string keyvalue)//省份编辑框的后台,keyvalue是F_Id
        {
            int i = 0;
            foreach (ChannelProvinceEntity c in channelprovinceentity)
            {
                if (c.F_ChannelId != 0)
                {
                    OC_ChannelProvince model = new OC_ChannelProvince();
                    model = DAL.OC_ChannelProvinceDAL.Instance.Model(keyvalue, c.F_ProvinceId);//
                    //model.F_ChannelId = channelprovinceentity[i].F_ChannelId;//F_ChannelId不修改
                    model.F_SwitchChannelId = c.F_ChannelId;//只修改F_SwitchChannleId
                    model.F_LastModifyTime = DateTime.Now;
                    model.F_LastModifyUserId = OperatorProvider.Provider.GetCurrent().UserId;
                    DAL.OC_ChannelProvinceDAL.Instance.Update(model);//更新F_ChannelId,和switch
                    i++;
                }
            }
            return Success("修改成功");
        }

        /// <summary>
        /// 新增充值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Add(ChannelRechargeRecordAddParam model_CR, BaseChannelAddParam V_model_BC, int keyValue)
        {
            bool result = BLL.OC_ChannelRechargeRecordManager.Instance.AddTran(model_CR, keyValue);//keyvalue是ChannelId，与BaseChannel的Id(int)关联
            OC_BaseChannel model_BC = BLL.OC_BaseChannelManager.Instance.Model(keyValue.ToInt());//获取单个实体对象以更新
            model_BC.F_ChaBalance = model_CR.F_recharge + model_BC.F_ChaBalance;
            model_BC.F_SurplusNum = V_model_BC.F_SurplusNum;
            BLL.OC_BaseChannelManager.Instance.Update(model_BC);//提交model_BC以更新BaseChannel表中的余额
            return result ? Success("保存成功！") : Error("添加异常，请检查");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteForm(string keyValue)
        {
            BaseChannelApp.DeleteForm(keyValue);
            return Success("删除成功！");
        }

        //导出功能
        public ActionResult Export(string F_ChannelName, string F_Operator, string F_ChannelState)
        {
            List<OC_BaseChannel> list = NFine.BLL.OC_BaseChannelManager.Instance.GetList(F_ChannelName, F_Operator, F_ChannelState);
            DataTable dt = ListToTable(list);
            NFine.Code.Excel.NPOIExcel helper = new Code.Excel.NPOIExcel();
            string path = "/Resource/BaseChannel-Export/" + DateTime.Now.ToString("yyyy/MM/dd");
            string fileName = Guid.NewGuid() + ".xls";
            if (!NFine.Code.FileHelper.IsExistDirectory(Server.MapPath(path)))
                NFine.Code.FileHelper.CreateDir(path);
            helper.ToExcel(dt, "通道列表", "sheet1", Server.MapPath(path + "/" + fileName));
            return DownFile(path + "/" + fileName, "通道列表.xls");
        }

        /// <summary>
        /// List对象转换成Table
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable ListToTable(List<OC_BaseChannel> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("名称");
            dt.Columns.Add("运营商");
            dt.Columns.Add("状态");
            //dt.Columns.Add("类型");
            dt.Columns.Add("通道余额(元)");

            foreach (var model in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = model.F_ChannelName;
                dr[1] = model.F_Operator;
                if (model.F_Operator == 1)
                    dr[1] = "移动";
                else if (model.F_Operator == 2)
                    dr[1] = "联通";
                else
                    dr[1] = "电信";
                dr[2] = model.F_ChannelState == 1 ? "正常" : "暂停";
                //dr[3] = model.F_UrlType;
                dr[3] = model.F_ChaBalance;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //用于取前台select ChannelId的数据，keyvalue来区分运营商
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson(int keyvalue)
        {
           
            var data = BaseChannelApp.GetList();
            var treeList = new List<TreeSelectModel>();
            if (keyvalue == 0)
            {
                foreach (BaseChannelEntity item in data)
                {
                    //BaseChannel表中的Id就是ChannelProvince表中的F_ChannelId
                    TreeSelectModel treeModel = new TreeSelectModel();
                    treeModel.id = item.Id.ToString();
                    treeModel.text = item.F_ChannelName + "_" +item.Id;
                    treeModel.parentId = "0";
                    treeModel.data = item.Id;
                    treeList.Add(treeModel);
                }
                return Content(treeList.TreeSelectJson());
            }
            else 
            {
                foreach (BaseChannelEntity item in data)
                {
                    if (item.F_Operator == keyvalue)
                    {
                        //BaseChannel表中的Id就是ChannelProvince表中的F_ChannelId
                        TreeSelectModel treeModel = new TreeSelectModel();
                        treeModel.id = item.Id.ToString();
                        treeModel.text = item.F_ChannelName;
                        treeModel.parentId = "0";
                        treeModel.data = item.Id;
                        treeList.Add(treeModel);
                    }
                }
                return Content(treeList.TreeSelectJson());
            }

        }

        //根据CHannelId取得ChannelName
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetChannelNameJson(string keyValue)
        {
            var data = new
            {
                ChannelName= this.GetChannelList(keyValue)
            };
            return Content(data.ToJson());
        }
        private object GetChannelList(string keyValu)
        {
            var data = BaseChannelApp.GetForm(keyValu);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ChannelName", data.F_ChannelName);
            return dictionary;
        }
    }
}