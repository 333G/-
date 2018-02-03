using NFine.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Self.WebAPI.Controllers
{
    public class GetSendStateController : ApiController
    {
        [HttpGet]
        public async Task<string> SendStateBack(int UserId)
        {
            Task<string> Result = GetSendStateApi(UserId);
            return await Result;
        }
        private async Task<string> GetSendStateApi(int UserId)
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
                        List<Models.SendStateApi> BackList = new List<Models.SendStateApi>();
                        foreach (var Id in SendIdList)
                        {
                            List<Sev_FinalSendDetail> SendStateModelList = new List<Sev_FinalSendDetail>();
                            SendStateModelList = NFine.DAL.Sev_FinalSendDetailDAL.Instance.FindList(t => t.F_SendId == Id);
                            if (SendStateModelList.Count() != 0)
                            {
                                foreach (var item in SendStateModelList)
                                {
                                    Models.SendStateApi model = new Models.SendStateApi();
                                    // model.BackReport=item.F_Report

                                    BackList.Add(model);
                                }
                            }

                        }
                        Dictionary<string, Models.SendStateApi> dic = new Dictionary<string, Models.SendStateApi>();//新建字典类，判断批次<key:SMC_F_Id;value:EntityList>
                        for (int i = 0; i < BackList.Count; i++)
                        {
                            if (!dic.ContainsKey(BackList[i].GUId))
                            {
                                dic.Add(BackList[i].GUId, new Models.SendStateApi());//添加新的键
                                //添加Value元素
                                dic[BackList[i].GUId] = BackList[i];
                            }
                            else
                                dic[BackList[i].GUId] = BackList[i];
                        }//获取dictionary键值对  
                        return "{ \"Status\":\"1\",\"Description\":\"发送状态：\",\"Data\":\"" + dic + "\"}";
                    }
                    else
                        return "{ \"Status\":\"1\",\"Description\":\"发送状态：\",\"Data\":\"没有找到此用户发送短信！\"}";
                }
                catch (Exception ex)
                {
                    return "{ \"Status\":\"0\",\"Description\":\"发生错误:\",\"Data\":\" 没有找到此用户发送短信！" + ex + "\"}";
                }
            });
        }
    }
}
