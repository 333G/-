using Newtonsoft.Json;
using NFine.Application.OCManage;
using NFine.Code;
using NFine.Entity;
using NFine.Entity.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFine.BLL.Table;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NFine.Web.Areas.OCManage.Controllers
{
    public class ReportCenterController : ControllerBase
    {
        static List<VSevFinalSendDetail> newDictionaryToList;//将单独用日期查询的数据放到静态变量里以便后期的分组查询
        //
        // GET: /OCManage/ReportCenter/
        private SendDateDetailApp sendDateDetailApp = new SendDateDetailApp();
        private FinalSendDetailApp finalSendDetailApp = new FinalSendDetailApp();
        public ActionResult BuffeDataMonitoring()
        {
            return View();
        }

        public ActionResult ChannelDataAnalysis()
        {
            return View();
        }

        public ActionResult DailyDataAnalysis()
        {
            return View();
        }
        public ActionResult BusinessDataAnalysis()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string queryJson)//缓冲数据监控
        {
            var data = new
            {
                rows = BLL.Sev_SendDateDetailManager.Instance.BuffeDataGetList(pagination, queryJson),
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
                        data.rows[i].F_Province = "未知";
                    }
                }
            }
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBusinessGridJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            var data = new
            {
                rows = BLL.Sev_SendDateDetailManager.Instance.BusinessDataAnalysis(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetChannelDataAnalysisGridJson(Pagination pagination, string queryJson)
        {
            var data = new
            {
                rows = BLL.Sev_SendDateDetailManager.Instance.ChannelDataAnalysis(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult DailyDataGetGridJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string newChannelId ;
            string newOperator ;
            string newProvince;
            string newUser ;
            var data = new
            {
                rows = BLL.Sev_SendDateDetailManager.Instance.DailyDataGetList(pagination, queryJson),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };

            if (queryJson == null)
            {
                 newChannelId = "-3";
                 newOperator = "-3";
                 newProvince = "-3";
                 newUser = "-3";
            }
            else
            {
                string ChannelId = queryParam["F_ChannelId"].ToString();
                string Operator = queryParam["F_Operator"].ToString();
                string Province = queryParam["F_Province"].ToString();
                string User = queryParam["F_User"].ToString();
                newChannelId = ChannelId;
                newOperator = Operator;
                newProvince = Province;
                newUser = User;
            }
        
            // return Content(data.ToJson());
            //重复数据进行合并       
            Dictionary<string, VSevFinalSendDetail> map = new Dictionary<string, VSevFinalSendDetail>();
            int length = data.rows.Count;  
            //是否是分组查询
            if (newChannelId == "0" || newOperator == "0" || newProvince == "0" || newUser == "0")
            {
                //Firstcheck(map, newChannelId, newOperator, newProvince, newUser, data.rows);
                Firstcheck(map, newChannelId, newOperator, newProvince, newUser, newDictionaryToList);
            }
          
            else//不是分组，单个查询统计
            {
                for (int i = 0; i < length; i++)
                {
                    VSevFinalSendDetail vdailyDataAnalysis = new VSevFinalSendDetail();
                    string F_ChannelId = data.rows[i].F_ChannelId.ToString();
                    string F_Operator = data.rows[i].F_Operator.ToString();
                    string F_Province = data.rows[i].F_Province;
                    string F_UserId = data.rows[i].F_UserId.ToString();
                    string str = F_ChannelId + F_Operator + F_Province + F_UserId;//将四个条件值相同的合并成一个字符串当做map的key

                    if (map.ContainsKey(str))
                    {
                        VSevFinalSendDetail OldvdailyDataAnalysis = map[str];//取map里key为str的旧值
                        OldvdailyDataAnalysis.F_ReissueCount = OldvdailyDataAnalysis.F_ReissueCount + data.rows[i].F_Reissue;//计算补发数
                        if (data.rows[i].F_Report == "1")
                        {
                            OldvdailyDataAnalysis.F_SendCount = OldvdailyDataAnalysis.F_SendCount + 1;//计算发送数
                        }
                        else if (data.rows[i].F_Report == "1" || data.rows[i].F_Response == 1)
                        {
                            OldvdailyDataAnalysis.F_SuccessCount = OldvdailyDataAnalysis.F_SuccessCount + 1;//计算实际成功数量
                        }
                        else if (data.rows[i].F_Report == "3")//**************有待商榷********************************************
                        {
                            OldvdailyDataAnalysis.F_SimulationSucceCount = OldvdailyDataAnalysis.F_SimulationSucceCount + 1;//计算模拟成功数量
                        }
                        else if (data.rows[i].F_Report == "0")
                        {
                            OldvdailyDataAnalysis.F_FailCount = OldvdailyDataAnalysis.F_FailCount + 1;//失败数量
                        }
                        else
                            OldvdailyDataAnalysis.F_UnknownCount = OldvdailyDataAnalysis.F_UnknownCount + 1;//未知数量
                        try
                        {
                            OldvdailyDataAnalysis.F_SuccessRate = (int)((OldvdailyDataAnalysis.F_SuccessCount + OldvdailyDataAnalysis.F_SimulationSucceCount) / OldvdailyDataAnalysis.F_SubmitCount);//计算成功率
                        }
                        catch
                        {
                            OldvdailyDataAnalysis.F_SuccessRate = 0;
                        }
                        OldvdailyDataAnalysis.F_Price = OldvdailyDataAnalysis.F_Price + (data.rows[i].F_LongsmsCount * data.rows[i].F_Price) / 100; //计算价格
                        map[str] = OldvdailyDataAnalysis;//更新覆盖key值为“str”的value值
                    }
                    else
                    {
                        vdailyDataAnalysis.F_SendId = data.rows[i].F_SendId;//后面分组查询会用到
                        try
                        {
                                var resultSev_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == data.rows[i].F_SendId);
                                var resultSMC = DAL.SMC_SendSmsDAL.Instance.FindEntity(t => t.F_Id == resultSev_SendData.SMC_F_Id);
                                vdailyDataAnalysis.F_ChannelId = data.rows[i].F_ChannelId;
                                vdailyDataAnalysis.F_Operator = data.rows[i].F_Operator;
                                vdailyDataAnalysis.F_Province = data.rows[i].F_Province;
                                vdailyDataAnalysis.F_UserId = data.rows[i].F_UserId;
                                vdailyDataAnalysis.F_SubmitCount = resultSMC.F_MobileCount;
                                vdailyDataAnalysis.F_ReissueCount = data.rows[i].F_Reissue;
                                vdailyDataAnalysis.F_Price = resultSMC.F_Price;
                        }
                        catch (Exception ex)
                        {
                            vdailyDataAnalysis.F_ChannelId = -2;
                            vdailyDataAnalysis.F_Operator = -2;
                            vdailyDataAnalysis.F_Province = "-2";
                            vdailyDataAnalysis.F_UserId = -2;
                            vdailyDataAnalysis.F_SubmitCount = 0;
                            vdailyDataAnalysis.F_SendCount = 0;
                            vdailyDataAnalysis.F_ReissueCount = 0;
                            vdailyDataAnalysis.F_Price = 0;
                        }
                        //当key值不存在字典类里时，初始化实际成功数量、模拟成功数量、失败数量、未知数量的值
                        if (data.rows[i].F_Report == "1")//发送数
                        {
                            vdailyDataAnalysis.F_SendCount = 1;
                            vdailyDataAnalysis.F_FailCount = 0;
                            vdailyDataAnalysis.F_UnknownCount = 0;
                        }
                        else if (data.rows[i].F_Report == "1" || data.rows[i].F_Response == 1)//实际成功数
                        {
                            vdailyDataAnalysis.F_SuccessCount = 1;
                            vdailyDataAnalysis.F_FailCount = 0;
                            vdailyDataAnalysis.F_UnknownCount = 0;
                        }
                        else if (data.rows[i].F_Report == "3")//模拟成功数********有待商榷********************************************
                        {
                            vdailyDataAnalysis.F_SendCount = 0;
                            vdailyDataAnalysis.F_SimulationSucceCount = 1;
                            vdailyDataAnalysis.F_FailCount = 0;
                            vdailyDataAnalysis.F_UnknownCount = 0;
                        }
                        else if (data.rows[i].F_Report == "0")//计算失败数
                        {
                            vdailyDataAnalysis.F_FailCount = 1;
                            vdailyDataAnalysis.F_SuccessCount = 0;
                            vdailyDataAnalysis.F_SimulationSucceCount = 0;
                            vdailyDataAnalysis.F_UnknownCount = 0;
                        }
                        else //未知数
                        {
                            vdailyDataAnalysis.F_UnknownCount = 1;
                            vdailyDataAnalysis.F_SuccessCount = 0;
                            vdailyDataAnalysis.F_SimulationSucceCount = 0;
                            vdailyDataAnalysis.F_FailCount = 0;
                        }
                        try
                        {
                            vdailyDataAnalysis.F_SuccessRate = (int)((vdailyDataAnalysis.F_SuccessCount + vdailyDataAnalysis.F_SimulationSucceCount) / vdailyDataAnalysis.F_SubmitCount);//计算成功率
                        }
                        catch
                        {
                            vdailyDataAnalysis.F_SuccessRate = 0;
                        }
                        vdailyDataAnalysis.F_Price = (data.rows[i].F_LongsmsCount * data.rows[i].F_Price) / 100;//计算金额，并转换成“元”
                        map.Add(str, vdailyDataAnalysis);
                    }
                }
            }
            List<VSevFinalSendDetail> DictionaryToList = map.Values.ToList();//将Dictionary转为list
            if (queryJson == null)
            {
                newDictionaryToList = DictionaryToList;//将首次进入页面时查询的数据放到全局变量里以便后期的分组查询              
            }
            else if ( newChannelId == "" & newOperator == "" & newProvince == "" & newUser == "")//将单独用日期查询的数据放到全局变量里以便后期的分组查询
            {
                newDictionaryToList = DictionaryToList;    
            }
            return Content(DictionaryToList.ToJson());
        }

        //分组
        public Dictionary<string, VSevFinalSendDetail> Firstcheck(Dictionary<string, VSevFinalSendDetail> map, string ChannelId, string Operator, string Province, string User, List<VSevFinalSendDetail> newDictionaryToList)
        {
            //var sql = "select * from Sev_FinalSendDetail ";
            //List<Sev_FinalSendDetail> result = DAL.Sev_FinalSendDetailDAL.Instance.FindList(sql);
            //int size = result.Count;
            int size = newDictionaryToList.Count;
            //int size = data.rows.Count;
            //单个分组查询
            if (Operator == "0" & ChannelId == "" & Province == "" & User == "")//运营商分组
            {
                for (int i = 0; i < size; i++)
                {
                    VSevFinalSendDetail vdailyDataAnalysis = new VSevFinalSendDetail();
                    try
                    {
                        var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == newDictionaryToList[i].F_SendId);
                        string strOperator = model_SendData.F_Operator.ToString();
                        vdailyDataAnalysis.F_Operator = model_SendData.F_Operator;
                        vdailyDataAnalysis.F_ChannelId = -1;
                        vdailyDataAnalysis.F_UserId = -1;
                        vdailyDataAnalysis.F_Province = "-1"; 
                        GroupCount(strOperator, vdailyDataAnalysis, map, newDictionaryToList, i);//运营商分组查询结果    
                    }
                    catch
                    {
                        vdailyDataAnalysis.F_ChannelId = -2;
                        vdailyDataAnalysis.F_Operator = -2;
                        vdailyDataAnalysis.F_Province = "-2";
                        vdailyDataAnalysis.F_UserId = -2;
                        vdailyDataAnalysis.F_SubmitCount = 0;
                        vdailyDataAnalysis.F_SendCount = 0;
                        vdailyDataAnalysis.F_ReissueCount = 0;
                        vdailyDataAnalysis.F_Price = 0;
                    }                          
                }
            }
            else if (Operator == "" & ChannelId == "0" & Province == "" & User == "")//产品分组
            {
                for (int i = 0; i < size; i++)
                {
                    VSevFinalSendDetail vdailyDataAnalysis = new VSevFinalSendDetail();
                    try
                    {                        
                        var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == newDictionaryToList[i].F_SendId);
                        string strChannelId = model_SendData.F_ChannelId.ToString();
                        vdailyDataAnalysis.F_ChannelId = model_SendData.F_ChannelId;
                        vdailyDataAnalysis.F_Operator = -1;
                        vdailyDataAnalysis.F_UserId = -1;
                        vdailyDataAnalysis.F_Province = "-1";
                        GroupCount(strChannelId, vdailyDataAnalysis, map, newDictionaryToList, i);//产品分组查询结果  
                    }                    
                     catch
                    {
                        vdailyDataAnalysis.F_ChannelId = -2;
                        vdailyDataAnalysis.F_Operator = -2;
                        vdailyDataAnalysis.F_Province = "-2";
                        vdailyDataAnalysis.F_UserId = -2;
                        vdailyDataAnalysis.F_SubmitCount = 0;
                        vdailyDataAnalysis.F_SendCount = 0;
                        vdailyDataAnalysis.F_ReissueCount = 0;
                        vdailyDataAnalysis.F_Price = 0;
                    }
                }
            }
            else if (Operator == "" & ChannelId == "" & Province == "0" & User == "")//省份分组
            {
                for (int i = 0; i < size; i++)
                {
                    VSevFinalSendDetail vdailyDataAnalysis = new VSevFinalSendDetail();
                    try
                    {
                        var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == newDictionaryToList[i].F_SendId);
                        string strProvince = model_SendData.F_Province.ToString();
                        vdailyDataAnalysis.F_Province = model_SendData.F_Province;
                        vdailyDataAnalysis.F_ChannelId = -1;
                        vdailyDataAnalysis.F_Operator = -1;
                        vdailyDataAnalysis.F_UserId = -1;
                        GroupCount(strProvince, vdailyDataAnalysis, map, newDictionaryToList, i);//省份分组查询结果 
                    }                    
                    catch
                    {
                        vdailyDataAnalysis.F_ChannelId = -2;
                        vdailyDataAnalysis.F_Operator = -2;
                        vdailyDataAnalysis.F_Province = "-2";
                        vdailyDataAnalysis.F_UserId = -2;
                        vdailyDataAnalysis.F_SubmitCount = 0;
                        vdailyDataAnalysis.F_SendCount = 0;
                        vdailyDataAnalysis.F_ReissueCount = 0;
                        vdailyDataAnalysis.F_Price = 0;
                    }
                }
            }
            else if (Operator == "" & ChannelId == "" & Province == "" & User == "0")//用户分组
            {
                for (int i = 0; i < size; i++)
                {
                    VSevFinalSendDetail vdailyDataAnalysis = new VSevFinalSendDetail();
                    try
                    {
                        var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == newDictionaryToList[i].F_SendId);
                        string strUser = model_SendData.F_UserId.ToString();
                        vdailyDataAnalysis.F_UserId = model_SendData.F_UserId;
                        vdailyDataAnalysis.F_ChannelId = -1;
                        vdailyDataAnalysis.F_Operator = -1;
                        vdailyDataAnalysis.F_Province = "-1";
                        GroupCount(strUser, vdailyDataAnalysis, map, newDictionaryToList, i);//省份分组查询结果 
                    }
                    catch
                    {
                        vdailyDataAnalysis.F_ChannelId = -2;
                        vdailyDataAnalysis.F_Operator = -2;
                        vdailyDataAnalysis.F_Province = "-2";
                        vdailyDataAnalysis.F_UserId = -2;
                        vdailyDataAnalysis.F_SubmitCount = 0;
                        vdailyDataAnalysis.F_SendCount = 0;
                        vdailyDataAnalysis.F_ReissueCount = 0;
                        vdailyDataAnalysis.F_Price = 0;
                    }
                }
            }

            //多个分组同时查询（按照要求，不包括用户分组查询）
            else if (Operator == "0" & ChannelId == "0" & Province == "")//两个分组同时查询
            {
                for (int i = 0; i < size; i++)
                {
                    VSevFinalSendDetail vdailyDataAnalysis = new VSevFinalSendDetail();
                    try
                    {
                        var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == newDictionaryToList[i].F_SendId);
                        vdailyDataAnalysis.F_ChannelId = model_SendData.F_ChannelId;
                        vdailyDataAnalysis.F_Operator = model_SendData.F_Operator;
                        string strOpeCha = vdailyDataAnalysis.F_ChannelId.ToString() + "-" + vdailyDataAnalysis.F_Operator.ToString();
                        vdailyDataAnalysis.F_Province = "-1";
                        vdailyDataAnalysis.F_UserId = -1;
                        GroupCount(strOpeCha, vdailyDataAnalysis, map, newDictionaryToList, i);//运营商分组、产品分组查询结果
                    }
                    catch
                    {
                        vdailyDataAnalysis.F_ChannelId = -2;
                        vdailyDataAnalysis.F_Operator = -2;
                        vdailyDataAnalysis.F_Province = "-2";
                        vdailyDataAnalysis.F_UserId = -2;
                        vdailyDataAnalysis.F_SubmitCount = 0;
                        vdailyDataAnalysis.F_SendCount = 0;
                        vdailyDataAnalysis.F_ReissueCount = 0;
                        vdailyDataAnalysis.F_Price = 0;
                    }
                }
            }
            else if (Operator == "0" & ChannelId == "" & Province == "0")//两个分组同时查询
            {
                for (int i = 0; i < size; i++)
                {
                    VSevFinalSendDetail vdailyDataAnalysis = new VSevFinalSendDetail();
                    try
                    {
                        var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == newDictionaryToList[i].F_SendId);
                        vdailyDataAnalysis.F_Operator = model_SendData.F_Operator;
                        vdailyDataAnalysis.F_Province = model_SendData.F_Province;
                        string strOpePro = vdailyDataAnalysis.F_Province + "-" + vdailyDataAnalysis.F_Operator.ToString();
                        vdailyDataAnalysis.F_ChannelId = model_SendData.F_ChannelId;
                        vdailyDataAnalysis.F_UserId = -1;
                        GroupCount(strOpePro, vdailyDataAnalysis, map, newDictionaryToList, i);//运营商分组、省份分组查询结果 
                    }
                    catch
                    {
                        vdailyDataAnalysis.F_ChannelId = -2;
                        vdailyDataAnalysis.F_Operator = -2;
                        vdailyDataAnalysis.F_Province = "-2";
                        vdailyDataAnalysis.F_UserId = -2;
                        vdailyDataAnalysis.F_SubmitCount = 0;
                        vdailyDataAnalysis.F_SendCount = 0;
                        vdailyDataAnalysis.F_ReissueCount = 0;
                        vdailyDataAnalysis.F_Price = 0;
                    }
                }
            }
            else if (Operator == "" & ChannelId == "0" & Province == "0")//两个分组同时查询
            {
                for (int i = 0; i < size; i++)
                {
                    VSevFinalSendDetail vdailyDataAnalysis = new VSevFinalSendDetail();
                    try
                    {
                        var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == newDictionaryToList[i].F_SendId);
                        vdailyDataAnalysis.F_ChannelId = model_SendData.F_ChannelId;
                        vdailyDataAnalysis.F_Province = model_SendData.F_Province;
                        string strChaPro = vdailyDataAnalysis.F_Province + "-" + vdailyDataAnalysis.F_ChannelId.ToString();
                        vdailyDataAnalysis.F_Operator = model_SendData.F_Operator;
                        vdailyDataAnalysis.F_UserId = -1;
                        GroupCount(strChaPro, vdailyDataAnalysis, map, newDictionaryToList, i);//产品分组、省份分组查询结果
                    }
                    catch
                    {
                        vdailyDataAnalysis.F_ChannelId = -2;
                        vdailyDataAnalysis.F_Operator = -2;
                        vdailyDataAnalysis.F_Province = "-2";
                        vdailyDataAnalysis.F_UserId = -2;
                        vdailyDataAnalysis.F_SubmitCount = 0;
                        vdailyDataAnalysis.F_SendCount = 0;
                        vdailyDataAnalysis.F_ReissueCount = 0;
                        vdailyDataAnalysis.F_Price = 0;
                    }
                }
            }
            else if (Operator == "0" & ChannelId == "0" & Province == "0")//三个分组同时查询
            {
                for (int i = 0; i < size; i++)
                {
                    VSevFinalSendDetail vdailyDataAnalysis = new VSevFinalSendDetail();
                    try
                    {
                        var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == newDictionaryToList[i].F_SendId);
                        vdailyDataAnalysis.F_ChannelId = model_SendData.F_ChannelId;
                        vdailyDataAnalysis.F_Province = model_SendData.F_Province;
                        vdailyDataAnalysis.F_Operator = model_SendData.F_Operator;
                        string strChaProOpe = vdailyDataAnalysis.F_Province + "-" + vdailyDataAnalysis.F_ChannelId.ToString() + "-" + vdailyDataAnalysis.F_Operator;
                        vdailyDataAnalysis.F_UserId = -1;
                        GroupCount(strChaProOpe, vdailyDataAnalysis, map, newDictionaryToList, i);//产品分组、省份分组查询结果 
                    }
                    catch
                    {
                        vdailyDataAnalysis.F_ChannelId = -2;
                        vdailyDataAnalysis.F_Operator = -2;
                        vdailyDataAnalysis.F_Province = "-2";
                        vdailyDataAnalysis.F_UserId = -2;
                        vdailyDataAnalysis.F_SubmitCount = 0;
                        vdailyDataAnalysis.F_SendCount = 0;
                        vdailyDataAnalysis.F_ReissueCount = 0;
                        vdailyDataAnalysis.F_Price = 0;
                    }
                }
            }
            return map;
        }

        //分组统计方法
        public Dictionary<string, VSevFinalSendDetail> GroupCount(string str, VSevFinalSendDetail vdailyDataAnalysis, Dictionary<string, VSevFinalSendDetail> map, List<VSevFinalSendDetail> result, int i)
        {
            if (map.ContainsKey(str))
            {
                VSevFinalSendDetail oldvdailyDataAnalysis = map[str];
                oldvdailyDataAnalysis.F_SuccessCount = oldvdailyDataAnalysis.F_SuccessCount + result[i].F_SuccessCount;//计算实际成功数量
                oldvdailyDataAnalysis.F_SimulationSucceCount = oldvdailyDataAnalysis.F_SimulationSucceCount + result[i].F_SimulationSucceCount;//计算模拟成功数量
                oldvdailyDataAnalysis.F_FailCount = oldvdailyDataAnalysis.F_FailCount + result[i].F_FailCount;//失败数量
                oldvdailyDataAnalysis.F_UnknownCount = oldvdailyDataAnalysis.F_UnknownCount + result[i].F_UnknownCount;//未知数量

                if (result[i].F_SubmitCount != null)
                    oldvdailyDataAnalysis.F_SubmitCount = oldvdailyDataAnalysis.F_SubmitCount + result[i].F_SubmitCount;//累加提交数
                else
                    oldvdailyDataAnalysis.F_SubmitCount = oldvdailyDataAnalysis.F_SubmitCount;
                if (result[i].F_Price != null)
                    oldvdailyDataAnalysis.F_Price = oldvdailyDataAnalysis.F_Price + result[i].F_Price;//累加价格
                else
                    oldvdailyDataAnalysis.F_Price = oldvdailyDataAnalysis.F_Price;
                if (result[i].F_Reissue != null)
                    oldvdailyDataAnalysis.F_ReissueCount = oldvdailyDataAnalysis.F_ReissueCount + result[i].F_Reissue;//计算补发数
                else
                    oldvdailyDataAnalysis.F_ReissueCount = oldvdailyDataAnalysis.F_ReissueCount;
                if (result[i].F_SendCount != null)
                    oldvdailyDataAnalysis.F_SendCount = oldvdailyDataAnalysis.F_SendCount + result[i].F_SendCount;//计算发送数
                else
                    oldvdailyDataAnalysis.F_SendCount = oldvdailyDataAnalysis.F_SendCount;
                try
                {
                    oldvdailyDataAnalysis.F_SuccessRate = (int)((oldvdailyDataAnalysis.F_SuccessCount + oldvdailyDataAnalysis.F_SimulationSucceCount) / oldvdailyDataAnalysis.F_SubmitCount);//计算成功率
                }
                catch
                {
                    oldvdailyDataAnalysis.F_SuccessRate = 0;
                }
                var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == result[i].F_SendId);
                oldvdailyDataAnalysis.F_Price = oldvdailyDataAnalysis.F_Price + (model_SendData.F_LongsmsCount * model_SendData.F_Price) / 100; //计算价格
                map[str] = oldvdailyDataAnalysis;
            }
            else
            {
                var resultSev_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == result[i].F_SendId);
                var newresult = DAL.SMC_SendSmsDAL.Instance.FindEntity(t => t.F_Id == resultSev_SendData.SMC_F_Id);
                vdailyDataAnalysis.F_SubmitCount = newresult.F_MobileCount;
                vdailyDataAnalysis.F_ReissueCount = result[i].F_Reissue;
                vdailyDataAnalysis.F_Price = newresult.F_Price;
                vdailyDataAnalysis.F_SuccessCount = result[i].F_SuccessCount;//计算实际成功数量
                vdailyDataAnalysis.F_SimulationSucceCount = result[i].F_SimulationSucceCount;//计算模拟成功数量
                vdailyDataAnalysis.F_FailCount = result[i].F_FailCount;//失败数量
                vdailyDataAnalysis.F_UnknownCount = result[i].F_UnknownCount;//未知数量
                vdailyDataAnalysis.F_SuccessRate = result[i].F_SuccessRate;
               
                //var model_SendData = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == result[i].F_SendId);
                //vdailyDataAnalysis.F_Price = (model_SendData.F_LongsmsCount * model_SendData.F_Price) / 100;//计算金额，并转换成“元”
                map.Add(str, vdailyDataAnalysis);
            }
            return map;
        }



        //根据编号取通道
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetBaseChannelJson()
        {
            var data = "select * from OC_BaseChannel";
            List<OC_BaseChannel> resultList = DAL.OC_BaseChannelDAL.Instance.FindList(data);
            var treelist = new List<TreeSelectModel>();
            int length = resultList.Count;
            for (int i = 0; i < length; i++)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = resultList[i].Id.ToString();
                treeModel.text = resultList[i].Id + "-" + resultList[i].F_ChannelName;
                treeModel.parentId = "0";
                treelist.Add(treeModel);
            }
            return Content(treelist.TreeSelectJson());
        }
        //根据编号取用户
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetUserInfoJson()
        {
            //    //var time = Request["F_ResponseTime"];
            //    var time = "2017/5/9 ";
            //    if (time == null)
            //    {
            //        DateTime now = DateTime.Now.Date;
            //        string newnow = now.ToString();
            //        var dataSev = "select * from Sev_SendDateDetail where F_SendTime like '%" + newnow.Split(' ')[0] + "%' ";
            //        List<Sev_SendDateDetail> resultTimeList = DAL.Sev_SendDateDetailDAL.Instance.FindList(dataSev);
            //        Dictionary<string, Sev_SendDateDetail> map = new Dictionary<string, Sev_SendDateDetail>();
            //        int resultTimeListCount = resultTimeList.Count;
            //        Sev_SendDateDetail sev_SendDateDetail = new Sev_SendDateDetail();
            //        for (int i = 0; i < resultTimeListCount; i++)
            //        {
            //            if (!map.ContainsKey(newnow))
            //            {
            //                map.Add((sev_SendDateDetail.F_SendTime.ToDate()).ToString(), sev_SendDateDetail);
            //            }
            //        }
            //        List<Sev_SendDateDetail> DictionaryToList = map.Values.ToList();//将Dictionary转为list
            //        var treelist = new List<TreeSelectModel>();
            //        int length = DictionaryToList.Count;
            //        for (int j = 0; j < length; j++)
            //        {
            //            TreeSelectModel treeModel = new TreeSelectModel();
            //            treeModel.id = DictionaryToList[j].F_Id;
            //            treeModel.text = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == DictionaryToList[j].F_UserId).F_Account;
            //            treeModel.parentId = "0";
            //            treelist.Add(treeModel);
            //        }
            //        return Content(treelist.TreeSelectJson());
            //    }
            //    else
            //    {
            //        var dataSev = "select * from Sev_SendDateDetail where F_SendTime like '%" + time.Split(' ')[0] + "%' ";
            //        List<Sev_SendDateDetail> resultTimeList = DAL.Sev_SendDateDetailDAL.Instance.FindList(dataSev);
            //        Dictionary<string, Sev_SendDateDetail> map = new Dictionary<string, Sev_SendDateDetail>();
            //        int resultTimeListCount = resultTimeList.Count;
            //        Sev_SendDateDetail sev_SendDateDetail = new Sev_SendDateDetail();
            //        for (int i = 0; i < resultTimeListCount; i++)
            //        {
            //            if (!map.ContainsKey(time))
            //            {
            //                map.Add((sev_SendDateDetail.F_SendTime.ToDate()).ToString(), sev_SendDateDetail);
            //            }

            //        }
            //        List<Sev_SendDateDetail> DictionaryToList = map.Values.ToList();//将Dictionary转为list
            //        var treelist = new List<TreeSelectModel>();
            //        int length = DictionaryToList.Count;
            //        for (int j = 0; j < length; j++)
            //        {
            //            TreeSelectModel treeModel = new TreeSelectModel();
            //            treeModel.id = DictionaryToList[j].F_Id;
            //            treeModel.text = DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == DictionaryToList[j].F_UserId).F_Account;
            //            treeModel.parentId = "0";
            //            treelist.Add(treeModel);
            //        }
            //        return Content(treelist.TreeSelectJson());
            //    }
            //}

            var data = "select * from OC_UserInfo ";
            List<OC_UserInfo> resultList = DAL.OC_UserInfoDAL.Instance.FindList(data);
            var treelist = new List<TreeSelectModel>();
            int length = resultList.Count;
            for (int j = 0; j < length; j++)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = resultList[j].F_Id;
                treeModel.text = resultList[j].F_Account;
                treeModel.parentId = "0";
                treelist.Add(treeModel);
            }
            return Content(treelist.TreeSelectJson());
        }

        //根据smc_F_Id取SMC_SendSms的处理状态
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetF_OperateState(string smc_F_Id)
        {
            var data = DAL.SMC_SendSmsDAL.Instance.FindEntity(t => t.F_Id == smc_F_Id);
            return data.F_OperateState.ToString();
        }
        //根据smc_F_Id取SMC_SendSms的发送状态
        [HttpGet]
        [HandlerAjaxOnly]
        public string GetF_SendState(string smc_F_Id)
        {
            var data = DAL.SMC_SendSmsDAL.Instance.FindEntity(t => t.F_Id == smc_F_Id);
            return data.F_SendState.ToString();
        }


        //选中删除
        public ActionResult DeleteForm(string keyValue)
        {
            //string[] idList = keyValue.Split(',');
            //int length = idList.Length - 1;
            //int j = 0;
            //if (length == 0)
            //{
            //    return Error("未选择要处理的数据，请检查！");
            //}
            //else
            //{
            //    for (int i = 0; i < length; i++)
            //    {
            //        finalSendDetailApp.DeleteForm(idList[i]);
            //        j++;
            //    }
            //    if (j > 0)
            //    {
            //        return Success("删除成功。");
            //    }
            //    else
            //        return Error("删除失败，请刷新重试");
            var userModel = NFine.Code.OperatorProvider.Provider.GetCurrent();
            string[] idList = keyValue.Split(',');
            int length = idList.Length;
            string[] resultidList = new string[length - 1];
            Array.Copy(idList, 0, resultidList, 0, length - 1);
            List<bool> result = BLL.Sev_SendDateDetailManager.Instance.Sev_FinalSendDetailDelete(resultidList, userModel.Id);
            if (result.Count > 0)
                return Success("删除成功。");
            return Error("删除失败，请刷新重试");
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
                    var resultSev_FinalSendDetail = BLL.Sev_SendDateDetailManager.Instance.Sev_FinalSendDetailModel(keys[i].ToInt());
                    var resultSev_SendDateDetail = BLL.Sev_SendDateDetailManager.Instance.Model(resultSev_FinalSendDetail.F_SendId);
                    resultSev_SendDateDetail.F_DealState = 0;
                    resultSev_SendDateDetail.F_Reissue = resultSev_SendDateDetail.F_Reissue + 1;
                    resultSev_SendDateDetail.F_CreatorTime = DateTime.Now;
                    BLL.Sev_SendDateDetailManager.Instance.Update(resultSev_SendDateDetail);

                    resultSev_FinalSendDetail.Id = resultSev_FinalSendDetail.Id;
                    resultSev_FinalSendDetail.F_DealState = 1;
                    resultSev_FinalSendDetail.F_Reissue = resultSev_FinalSendDetail.F_Reissue + 1;//每重发一次，补发字段加1
                    resultSev_FinalSendDetail.F_DeleteMark = true;
                    BLL.Sev_SendDateDetailManager.Instance.Sev_FinalSendDetailUpdate(resultSev_FinalSendDetail);
                }
                return Success("重发成功");
            }
        }

        //选中优先发送
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult PrioritySend(string keyValue)
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
                    var result = BLL.Sev_SendDateDetailManager.Instance.Sev_FinalSendDetailModel(keys[i].ToInt());
                    if (result.F_Level != 140)
                    {
                        result.F_Level = 140;//更改为优先级最高状态
                        BLL.Sev_SendDateDetailManager.Instance.Sev_FinalSendDetailUpdate(result);//更新Sev_SendDateDetail表的F_Level字段值
                    }
                }
                return Success("优先级设置完成！");
            }        
        }

        //显示通道的缓冲数
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetChannel()
        {

            List<string> SendChannelIdlist = new List<string>();
            SendChannelIdlist = DAL.Sev_FinalSendDetailDAL.Instance.FindSendChannelIdList();
            Dictionary<int, int> dic = new Dictionary<int, int> ();
            for (int i = 0; i < SendChannelIdlist.Count; i++)
            {
                int ChannelId = SendChannelIdlist[i].ToInt();
                if (dic.ContainsKey(ChannelId))
                { 
                    dic[ChannelId]++;  //元素++
                }
                else
                {
                    dic.Add(ChannelId, 1);//添加新的键
                                               //元素初始化为1
                }
            }//获取dictionary键值对
            if (dic.Count() == 0)
                return Error("所有通道都没有缓冲数据！");
            else
            {
                Dictionary<int, string> ChannelDic = new Dictionary<int, string>();
                foreach (var item in dic)
                {
                    try { string ChannelName = DAL.OC_BaseChannelDAL.Instance.FindEntity(t => t.Id == item.Key).F_ChannelName;
                        string ChannelInfo = "[" + ChannelName + "]" + "(" + item.Value + ")";
                        ChannelDic.Add(item.Key, ChannelInfo);
                    }
                    catch {
                        string ChannelInfo = "[ 此通道不存在或已经被删除]" + "(" + item.Value + ")";
                        ChannelDic.Add(item.Key, ChannelInfo);
                    }

                }
                string InfoJson = JsonConvert.SerializeObject(ChannelDic, Formatting.Indented);//序列化Dictionary
                return Content(InfoJson);//返回批次的F_Id和Sev_SendDateDetail的实体的Json
            }
        }
    }
}
