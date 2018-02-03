using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NFine.Web.Areas.ReportManage.Controllers
{
    public class DataAnalysisController : ControllerBase
    {
               public ActionResult DataAnalysisIndex()
        {
            return View();
        }

        public ActionResult SendDetailAnalysis()
        {
            return View();
        }

        //分析总数据，返回json结果
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetMainDataJson(string keyValue)
        {
            var costDIc = DAL.Sev_SendDateDetailDAL.Instance.GetAnaliseMainDataList(keyValue);
            return Content(costDIc.ToJson());
        }

        //分析成功率，返回json结果
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSuccessRateDataJson(string keyValue)
        {
            var date = DAL.Sev_FinalSendDetailDAL.Instance.GetSendSuccessRate(keyValue);
            return Content(date.ToJson());
        }

        //分析地区数据，返回json结果
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetAreaDataJson(string keyValue)
        {
            var date = DAL.Sev_SendDateDetailDAL.Instance.GetAreaSendData(keyValue);
            return Content(date.ToJson());
        }

        //分析运营商数据，返回json结果
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOperatorDataJson(string keyValue)
        {
             var OperatorDIc = DAL.Sev_SendDateDetailDAL.Instance.GetOperatorData(keyValue);
            return Content(OperatorDIc.ToJson());
        }

        //分析运营商数据，返回json结果
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSendDetailAnalysisJson(string keyValue)
        {
            Dictionary<string, decimal> Dic = new Dictionary<string, decimal>();
            decimal SubmitData= DAL.Sev_FinalSendDetailDAL.Instance.GetSubmitDate(keyValue,0);
            decimal RealSubmitData = DAL.Sev_FinalSendDetailDAL.Instance.GetSubmitDate(keyValue, 1);
            decimal ReissueSubmitData = DAL.Sev_FinalSendDetailDAL.Instance.GetSubmitDate(keyValue, 2);
            decimal BucklSubmitData = DAL.Sev_FinalSendDetailDAL.Instance.GetSubmitDate(keyValue, 3);//模拟成功数量
            decimal SuccessSubmitData =DAL.Sev_FinalSendDetailDAL.Instance.GetSubmitDate(keyValue,4);//实际成功数量
            decimal AllSuccessCount = BucklSubmitData + SuccessSubmitData;//总成功数量
            decimal RealSuccessRate = 0;//实际成功率
            decimal SuccessRate = 0;//总成功率
            if (SuccessSubmitData !=0)
                RealSuccessRate = SuccessSubmitData / SubmitData;//计算实际成功率
            if(AllSuccessCount!=0)
                SuccessRate = AllSuccessCount / SubmitData;//计算总成功率

            Dic.Add("总提交:", SubmitData);
            Dic.Add("实际提交:", RealSubmitData);
            Dic.Add("补发提交:", ReissueSubmitData);
            Dic.Add("实际成功:", SuccessSubmitData);
            Dic.Add("补发成功:", ReissueSubmitData);
            Dic.Add("模拟成功:", BucklSubmitData);
            Dic.Add("实际成功率:", RealSuccessRate * 100);
            Dic.Add("总成功率:", SuccessRate * 100);
            //总提交， 实际提交： 补发提交： 实际成功： 补发成功： 模拟成功： 实际成功率： 总成功率： -4： -3 无： DELIVRD: EXPIRED: UNDEVL: REJECTD: 11: 总提交 总成功率
            return Content(Dic.ToJson());
        }

    }
}
