using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NFine.Entity;
using NFine;
using System.Threading.Tasks;
using System.Web;
using NFine.Code;

namespace WebApi.Controllers
{
    public class MyAPIController : ApiController
    {
        private async Task<string> SendSmsApi(Models.SendSmsModel SendModel)//异步http请求？大概这样子，没研究透
        {
            return await Task.Factory.StartNew(() =>
           {
               if (!Request.Content.IsMimeMultipartContent("form-data")) // 检查是否是 multipart/form-data ，否则返回异常
                   throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
               var path = HttpContext.Current.Server.MapPath("~/File");//设置上传目录
               try
               {
                   object Mark;
                   SMC_SendSms SubmitModel= new SMC_SendSms();
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

                   return "{ \"Status\":\"1\",\"Description\":\"提交成功,请记录发送标记\",\"Data\":\"" + Mark +"\"}";
               }
               catch (Exception ex)
               {
                   return "{ \"Status\":\"0\",\"Description\":\"发生错误:\",\"Data\":\"" + ex + "\"}";
               }
           });
        }

        
        private async Task< string> SearchBalanceApi(int UserId)
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    string balance;
                    balance = NFine.DAL.OC_UserInfoDAL.Instance.FindEntity(t => t.F_UserId == UserId).F_Balance.ToString();
                    return "{ \"Status\":\"1\",\"Description\":\"余额\",\"Data\":\"" + balance + "\"}";
                }
                catch (Exception ex)
                {
                    return "{ \"Status\":\"0\",\"Description\":\"发生错误:\",\"Data\":\" 没有找到此用户！\"}";
                }
            });
        }

        private async Task< string> GetSendStateApi(int UserId)
        {
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    List<Sev_SendDateDetail> data = NFine.DAL.Sev_SendDateDetailDAL.Instance.FindList(t => t.F_UserId == UserId);
                    List<string> SendIdList = new List<string>();
                    if (data.Count() != 0)
                    {
                        foreach (var item in data)
                        {
                            SendIdList.Add(item.F_Id);
                        }
                    }
                    if (SendIdList.Count != 0)
                    {
                        List<Models.SendStateModel> BackList = new List<Models.SendStateModel>();
                        foreach (var Id in SendIdList)
                        {
                            List<Sev_FinalSendDetail> SendStateModelList = new List<Sev_FinalSendDetail>();
                            SendStateModelList = NFine.DAL.Sev_FinalSendDetailDAL.Instance.FindList(t => t.F_SendId == Id);
                            if (SendStateModelList.Count() != 0)
                            {
                                foreach (var item in SendStateModelList)
                                {
                                    Models.SendStateModel model = new Models.SendStateModel();
                                    // model.BackReport=item.F_Report

                                    BackList.Add(model);
                                }
                            }
                        
                        }
                        Dictionary<string, Models.SendStateModel> dic = new Dictionary<string, Models.SendStateModel>();//新建字典类，判断批次<key:SMC_F_Id;value:EntityList>
                        for (int i = 0; i < BackList.Count; i++)
                        {
                            if (!dic.ContainsKey(BackList[i].GUId))
                            {
                                dic.Add(BackList[i].GUId,new Models.SendStateModel());//添加新的键
                                //添加Value元素
                                dic[BackList[i].GUId] = BackList[i];
                            }
                            else
                                dic[BackList[i].GUId]=BackList[i];
                        }//获取dictionary键值对  
                        return "{ \"Status\":\"1\",\"Description\":\"发送状态：\",\"Data\":\"" + dic + "\"}";
                    }
                    else
                        return "{ \"Status\":\"1\",\"Description\":\"发送状态：\",\"Data\":\"没有找到此用户发送短信！\"}";
                }
                catch(Exception ex)
                {
                    return "{ \"Status\":\"0\",\"Description\":\"发生错误:\",\"Data\":\" 没有找到此用户发送短信！\"}";
                }
            });
        }

        [HttpPost]
        public async Task<string> SubmitResultBack(Models.SendSmsModel SendModel)
        {
            Task<string> Result = SendSmsApi(SendModel);
            return await Result;
        }

        [HttpGet]
        public async Task<string> SearchResultBack(int UserId)
        {
            Task<string> Result = SearchBalanceApi(UserId);
            return await Result;
        }

        [HttpGet]
        public async Task<string> SendStateBack(int UserId)
        {
            Task<string> Result = GetSendStateApi(UserId);
            return await Result;
        }
    }
}
