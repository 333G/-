using NFine.Application.OCManage;
using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFine.Domain.Entity;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class SendDateDetailController : ControllerBase
    {
        private SendDateDetailApp sendDateDetailApp = new SendDateDetailApp();
        private GroupChannelApp groupChannelApp = new GroupChannelApp();
        //
        // GET: /OCManage/SendDateDetail/
        public ActionResult SendDateDetailIndex()
        {
            return View();
        } 

        public ActionResult EditForm()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                //rows = BLL.Sev_SendDateDetailManager.Instance.Sev_FinalSendDetailGetList(pagination, queryJson),
                rows = BLL.Sev_SendDateDetailManager.Instance.GetReceiveList(pagination, queryJson),
                //rows = sendDateDetailApp.GetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            Dictionary<string, string> AreaDic = new Dictionary<string, string>();
            for (int i = 0; i < data.rows.Count(); i++)//换省份名
            {
                if (AreaDic.ContainsKey(data.rows[i].F_Province))
                    data.rows[i].F_Province = AreaDic[data.rows[i].F_Province];
                else
                {
                    try
                    {
                        string ParentId = DAL.Sys_AreaDAL.Instance.FindEntity(t => t.F_Id == data.rows[i].F_Province).F_ParentId;
                        string F_FullName = DAL.Sys_AreaDAL.Instance.FindEntity(t => t.F_Id == ParentId).F_FullName;
                        AreaDic.Add(data.rows[i].F_Province, F_FullName);
                        data.rows[i].F_Province = F_FullName;
                    }
                    catch
                    {
                        data.rows[i].F_Province ="未知";
                    }
                }
            }
            return Content(data.ToJson());
        }

        //public ActionResult GetReceiveGridJson(Pagination pagination, string queryJson)
        //{
        //    List<NFine.Entity.Views.VSevSendDateDetail> mlist = new List<NFine.Entity.Views.VSevSendDateDetail>();//创建一个空集合mlist，用于存放符合查询条件的数据并输出
        //    var queryParam = queryJson.ToJObject();
        //    var data = new
        //    {
        //        rows = BLL.Self.Sev_SendDateDetailBLL.Instance.GetReceiveList(pagination, queryJson),
        //        total = pagination.total,
        //        page = pagination.page,
        //        records = pagination.records
        //    };
        //    if (!queryParam["F_UserId"].IsEmpty() || !queryParam["F_PhoneCode"].IsEmpty())
        //    {
        //        for (int i = 0; i < data.rows.Count; i++)
        //        {
        //            //单个条件查询
        //            if (data.rows[i].F_UserId.ToString() == queryParam["F_UserId"].ToString() & queryParam["F_PhoneCode"].IsEmpty())
        //            {
        //                mlist.Add(data.rows[i]);  //将符合查询条件的集合放入新建的mlist集合里
        //            }
        //            if (data.rows[i].F_PhoneCode.ToString() == queryParam["F_PhoneCode"].ToString() & queryParam["F_UserId"].IsEmpty())
        //            {
        //                mlist.Add(data.rows[i]);
        //            }
        //            //多个条件查询
        //            if (data.rows[i].F_UserId.ToString() == queryParam["F_UserId"].ToString() & data.rows[i].F_PhoneCode.ToString() == queryParam["F_PhoneCode"].ToString())
        //            {
        //                mlist.Add(data.rows[i]);
        //            }
        //        }
        //        return Content(mlist.ToJson());
        //    }
        //    return Content(data.ToJson()); //多表联查返回的结果
        //}


        //根据编号取所有通道
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetAllChannelJson()
        {
            var sql = "select Id,F_Id,F_ChannelName,F_ChannelState from OC_BaseChannel";
            List<OC_BaseChannel> resultList = DAL.OC_BaseChannelDAL.Instance.FindList(sql);
            var treelist = getTreelist(resultList);
            return Content(treelist.TreeSelectJson());
        }

        private List<TreeSelectModel> getTreelist(List<OC_BaseChannel> list)
        {
            var treelist = new List<TreeSelectModel>();
            int length = list.Count;
            for (int i = 0; i < length; i++)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = list[i].Id.ToString();
                treeModel.text = list[i].F_ChannelName;
                treeModel.parentId = "0";
                treelist.Add(treeModel);
            }
            return treelist;
        }

        //根据编号取可用的通道
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetChannelJson()
        {
            var sql = "select Id,F_Id,F_ChannelName,F_ChannelState from OC_BaseChannel where F_ChannelState = '1'";//启用状态
            List<OC_BaseChannel> resultList = DAL.OC_BaseChannelDAL.Instance.FindList(sql);
            var treelist = getTreelist(resultList);
            return Content(treelist.TreeSelectJson());
        }


        //按条件更改基础通道
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult ChangeChannel(string queryJson, int keyvalue)//ID or RootId
        {
            try
            {
                List<Sev_SendDateDetail> listdata = new List<Sev_SendDateDetail>();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                Dictionary<string, object> dic = new Dictionary<string, object>();//新建字典类，判断批次<key:SMC_F_Id;value:EntityList>
                dic = jss.Deserialize<Dictionary<string, object>>(queryJson);

                foreach (var item in dic)
                {
                    var data = item.Value.ToJson();
                    Dictionary<string, List<Sev_SendDateDetail>> dic1 = new Dictionary<string, List<Sev_SendDateDetail>>();
                    dic1 = jss.Deserialize<Dictionary<string, List<Sev_SendDateDetail>>>(data);
                    foreach (var listitem in dic1)
                    {
                        listdata = listdata.Concat(listitem.Value).ToList();//拼接不同的list
                    }
                }
                DAL.Sev_SendDateDetailDAL.Instance.UpdateRange(listdata);//批量更新
            }
            catch (Exception ex){
                return Error("发生错误:" + ex + "请联系管理员");
            }
            return Success("修改通道成功");
        }


        //根据F_ChannelId获取通道名、移动、 联通、 电信、小灵通、发送率、成功率、优先级等
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetChannelInfo(string F_ChannelId)
        {
            Dictionary<string, OC_GroupChannel> map = new Dictionary<string, OC_GroupChannel>();
            OC_GroupChannel groupChannel = new OC_GroupChannel();
            var newdata = DAL.OC_GroupChannelDAL.Instance.FindEntity(t => t.F_ID == F_ChannelId);
            groupChannel.F_ChannelName = newdata.F_ChannelName;
            groupChannel.F_MobileChannel = newdata.F_MobileChannel;
            groupChannel.F_UnicomChannel = newdata.F_UnicomChannel;
            groupChannel.F_TelecomChannel = newdata.F_TelecomChannel;
            groupChannel.F_ChannelXLT = newdata.F_ChannelXLT;
            groupChannel.F_SendRate =  newdata.F_SendRate;
            groupChannel.F_SuccessRate = newdata.F_SuccessRate;
            groupChannel.F_Priority = newdata.F_Priority;
            
            map.Add(F_ChannelId, groupChannel);

            List<OC_GroupChannel> DictionaryToList = map.Values.ToList();//将Dictionary转为list
            return DictionaryToList.ToJson();
        }

        //按条件批量补发
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult Reissue(string queryJson)
        {
            List<Sev_SendDateDetail> datalist = new List<Sev_SendDateDetail>();
            try
            {
                datalist = DAL.Sev_SendDateDetailDAL.Instance.GetList(queryJson);//按条件选择需要补发的列表项
            }
            catch (Exception ex)
            {
                return Error("发生错误：" + ex + "，请联系管理员！");
            }
            if (datalist.Count == 0)
                return Error("没有需要补发的项，请检查补发条件!");
            foreach (var item in datalist)
            {
                //item.F_Report = 0;
                //item.F_Reissue++;
            }
            DAL.Sev_SendDateDetailDAL.Instance.UpdateRange(datalist);//批量更新
            return Success("处理成功！");
                   
        }

        //补发数据获取
        public ActionResult GetDateDetail(string queryJson)
        {
            List<Sev_SendDateDetail> datalist = new List<Sev_SendDateDetail>();
            try
            {
                datalist = DAL.Sev_SendDateDetailDAL.Instance.GetList(queryJson);//按条件选择需要补发的列表项
            }
            catch (Exception ex)
            {
                return Error("发生错误：" + ex + "，请联系管理员！");
            }
            Dictionary<string,Diclist> dic = new Dictionary<string,Diclist>();//新建字典类，判断批次<key:SMC_F_Id;value:EntityList>
            for (int i = 0; i < datalist.Count; i++)
            {
                if (!dic.ContainsKey(datalist[i].SMC_F_Id))
                {
                    dic.Add(datalist[i].SMC_F_Id, new Diclist());//添加新的键
                    dic[datalist[i].SMC_F_Id].senddatedetaillist.Add(datalist[i]);
                    //添加Value元素
                }
                else
                {
                    dic[datalist[i].SMC_F_Id].senddatedetaillist.Add(datalist[i]);   //List中添加新的元素
                }
                //if (!dic.ContainsKey(datalist[i].F_SendId))
                //{
                //    dic.Add(datalist[i].F_SendId, new Diclist());//添加新的键
                //    dic[datalist[i].F_SendId].senddatedetaillist.Add(datalist[i]);
                //    //添加Value元素
                //}
                //else
                //{
                //    dic[datalist[i].F_SendId].senddatedetaillist.Add(datalist[i]);   //List中添加新的元素
                //}
            }//获取dictionary键值对
            if (dic.Count() == 0)
                return Error("没有可以修改的项，请重新选择查询条件！");
            string Json = JsonConvert.SerializeObject(dic, Formatting.Indented);//序列化Dictionary
            return Content(Json);//返回批次的F_Id和Sev_SendDateDetail的实体的Json
        }

        //选中批量重发
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Retransmission(string keyValue)
        {
            string[] keys = keyValue.Split(',');
            int length = keyValue.Split(',').Length - 1;
            if (length == 0)
            {
                return Error("未选择要处理的数据，请检查！");
            }
            else
            {           
                for (int i = 0; i < length; i++)
                {
                    var resultSev_FinalSendDetail = BLL.Sev_SendDateDetailManager.Instance.newSev_FinalSendDetailModel(keys[i]);
                    var resultSev_SendDateDetail = BLL.Sev_SendDateDetailManager.Instance.Model(resultSev_FinalSendDetail.F_SendId);
                    resultSev_SendDateDetail.F_DealState = 0;
                    resultSev_SendDateDetail.F_Reissue = resultSev_SendDateDetail.F_Reissue + 1;
                    resultSev_SendDateDetail.F_CreatorTime = DateTime.Now;
                    BLL.Sev_SendDateDetailManager.Instance.Update(resultSev_SendDateDetail);
                    //var resultSev_FinalSendDetail = BLL.Sev_SendDateDetailManager.Instance.newSev_FinalSendDetailModel(keys[i]);
                    resultSev_FinalSendDetail.F_DealState = 1;
                    resultSev_FinalSendDetail.F_Report = "0";//默认未收到回复报告
                    resultSev_FinalSendDetail.F_Response = 0;//默认未收到应答
                    resultSev_FinalSendDetail.F_DeleteMark = true;
                    resultSev_FinalSendDetail.F_Reissue = resultSev_FinalSendDetail.F_Reissue + 1;//每重发一次，补发字段加1
                    BLL.Sev_SendDateDetailManager.Instance.Sev_FinalSendDetailUpdate(resultSev_FinalSendDetail);
                }
                return Success("重发成功");
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Export(string F_UserId, string F_BlackWhite, string F_ChannelId,/* DateTime? F_SendTime,*/ string F_PhoneCode, string F_Operator, string F_Province, string F_Report, string F_Synchro, string F_Buckle, string F_Reissue,/* string F_Level,*/ string F_SmsContent)
        {
            List<Sev_SendDateDetail> list = NFine.BLL.Sev_SendDateDetailManager.Instance.GetList(F_UserId, F_BlackWhite, F_ChannelId, /*F_SendTime,*/ F_PhoneCode, F_Operator, F_Province, F_Report, F_Synchro, F_Buckle, F_Reissue,/* F_Level,*/ F_SmsContent);
            DataTable dt = ListToTable(list);
            NFine.Code.Excel.NPOIExcel helper = new Code.Excel.NPOIExcel();
            string path = "/Resource/VSevSendDateDetail-Export/" + DateTime.Now.ToString("yyyy/MM/dd");
            string fileName = Guid.NewGuid() + ".xls";
            if (!NFine.Code.FileHelper.IsExistDirectory(Server.MapPath(path)))
                NFine.Code.FileHelper.CreateDir(path);
            helper.ToExcel(dt, "发送数据详单", "sheet1", Server.MapPath(path + "/" + fileName));
            return DownFile(path + "/" + fileName, "发送数据详单.xls");
        }

        /// <summary>
        /// List对象转换成Table
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private DataTable ListToTable(List<Sev_SendDateDetail> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("用户ID");
            dt.Columns.Add("黑白");
            dt.Columns.Add("通道");
            dt.Columns.Add("手机号");
            dt.Columns.Add("类型");
            dt.Columns.Add("省份");
            dt.Columns.Add("报告");
            dt.Columns.Add("同步");
            dt.Columns.Add("扣量");
            dt.Columns.Add("补发");
            //dt.Columns.Add("级别");
            dt.Columns.Add("发送内容");
            foreach (var model in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = model.F_UserId;
                dr[1] = model.F_BlackWhite;
                dr[2] = model.F_ChannelId;
                dr[3] = model.F_PhoneCode;
                dr[4] = model.F_Operator;
                dr[5] = model.F_Province;
                //dr[6] = model.F_Report;
                dr[7] = model.F_Synchro;
                dr[8] = model.F_Buckle;
                //dr[9] = model.F_Reissue;
                dr[10] = model.F_SmsContent;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
    class Diclist//封装一个List类,防止干扰
    {
        //List<Sev_SendDateDetail> SendDateDetaillist = new List<Sev_SendDateDetail>();
        //    public List<Sev_SendDateDetail> senddatedetaillist
        //{
        //    get
        //    {
        //        return SendDateDetaillist;
        //    }
        //}
        List<Sev_SendDateDetail> SendDateDetaillist = new List<Sev_SendDateDetail>();
        public List<Sev_SendDateDetail> senddatedetaillist
        {
            get
            {
                return SendDateDetaillist;
            }
        }
    }
}
