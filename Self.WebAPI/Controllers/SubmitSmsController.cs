using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Self.WebAPI.Controllers
{
    public class SubmitSmsController : ApiController
    {
        [HttpPost]
        public async Task<string> SubmitResultBack(Models.SendSmsApiModel SendModel)
        {
            Task<string> Result = SendSmsApi(SendModel);
            return await Result;
        }
        private async Task<string> SendSmsApi(Models.SendSmsApiModel SendModel)//异步http请求？大概这样子，没研究透
        {
            return await Task.Factory.StartNew(() =>
            {
                if (!Request.Content.IsMimeMultipartContent("form-data")) // 检查是否是 multipart/form-data ，否则返回异常
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                var path = HttpContext.Current.Server.MapPath("~/File");//设置上传目录
                try
                {
                    object Mark;
                    SMC_SendSms SubmitModel = new SMC_SendSms();
                    SubmitModel.F_Id = Guid.NewGuid().ToString();
                    SubmitModel.F_CreatorTime = SendModel.CreatorTime;
                    SubmitModel.F_CreatorUserId = SendModel.CreatorUserId;
                    SubmitModel.F_GroupChannelId = SendModel.GroupChannelId;
                    SubmitModel.F_IsTimer = SendModel.IsTime;
                    SubmitModel.F_MobileCount = SendModel.MobileCount;
                    SubmitModel.F_MobileList = SendModel.MobileList;
                    SubmitModel.F_SendSign = SendModel.SendSign;
                    SubmitModel.F_SmbSign = SendModel.SmbSign;
                    SubmitModel.F_SendTime = SendModel.SendTime;
                    SubmitModel.F_SmsContent = SendModel.SmsContent;
                    SubmitModel.F_CreatorTime = DateTime.Now;

                    Mark = NFine.DAL.SMC_SendSmsDAL.Instance.Add(SubmitModel);

                    return "{ \"Status\":\"1\",\"Description\":\"提交成功,请记录发送标记\",\"Data\":\"" + Mark + "\"}";
                }
                catch (Exception ex)
                {
                    return "{ \"Status\":\"0\",\"Description\":\"发生错误:\",\"Data\":\"" + ex + "\"}";
                }
            });
        }

    }
}
