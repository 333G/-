using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Self.WebAPI.Controllers
{
    public class SearchBalanceController : ApiController
    {
        [HttpGet]
        public async Task<string> SearchResultBack(int UserId)
        {
            Task<string> Result = SearchBalanceApi(UserId);
            return await Result;
        }

        private async Task<string> SearchBalanceApi(int UserId)
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
                    return "{ \"Status\":\"0\",\"Description\":\"发生错误:\",\"Data\":\" 没有找到此用户！" + ex + "\"}";
                }
            });
        }
    }
}
