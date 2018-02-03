using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Entity;
using NFine.Repository.OCManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.OCManage
{
    public class SendDateDetailApp
    {
        private ISendDateDetailRepository service = new SendDateDetailRepository();
        public List<SendDateDetailEntity> GetList(Pagination pagination, string queryJson)
        {

            var expression = ExtLinq.True<SendDateDetailEntity>();
            var queryParam = queryJson.ToJObject();

            //查询条件 
            if (!queryParam["F_UserId"].IsEmpty())
            {
                //int UserId = queryParam["F_UserId"].ToInt();
                //string F_ID = DAL.Sys_UserDAL.Instance.FindEntity(t => t.Id == UserId).F_Id;
                int F_UserId = queryParam["F_UserId"].ToInt();
                expression = expression.And(t => t.F_UserId == F_UserId);
            }
            if (!queryParam["F_PhoneCode"].IsEmpty())
            {
                string F_PhoneCode = queryParam["F_PhoneCode"].ToString();
                expression = expression.And(t => t.F_PhoneCode == F_PhoneCode);
            }
            //if (!queryParam["F_SmsContent"].IsEmpty())
            //{
            //    string keyvalue = queryParam["F_SmsContent"].ToString();
            //    expression = expression.And(t => t.F_SmsContent.Contains(keyvalue));
            //}
            //if (!(queryParam["F_CreatorTimeFrom"].IsEmpty() || queryParam["F_CreatorTimeEnd"].IsEmpty()))
            //{
            //    DateTime F_CreatorTimeFrom = (DateTime)queryParam["F_CreatorTimeFrom"];
            //    DateTime F_CreatorTimeEnd = (DateTime)queryParam["F_CreatorTimeEnd"];
            //    expression = expression.And(t => ((t.F_CreatorTime >= F_CreatorTimeFrom) && t.F_CreatorTime <= F_CreatorTimeEnd));
            //}
            //if (!queryParam["Province"].IsEmpty())
            //{
            //    string Province = queryParam["Province"].ToString();
            //    expression = expression.And(t => t.Province.Equals(Province));
            //}
            //if (!queryParam["F_ChannelName"].IsEmpty())
            //{
            //    string F_ChannelName = queryParam["F_Operator"].ToString();
            //    expression = expression.And(t => t.F_ChannelName.Equals(F_ChannelName));
            //}
            //if (!queryParam["F_Report"].IsEmpty())
            //{
            //    int F_Report = queryParam["F_Report"].ToInt();
            //    expression = expression.And(t => t.F_Report == F_Report);
            //}
            //if (!queryParam["F_Operator"].IsEmpty())
            //{
            //    int F_Operator = queryParam["F_Operator"].ToInt();
            //    expression = expression.And(t => t.F_Operator.Equals(F_Operator));
            //}
            //if (!queryParam["F_Synchro"].IsEmpty())
            //{
            //    int F_Synchro = queryParam["F_Synchro"].ToInt();
            //    expression = expression.And(t => t.F_Synchro == F_Synchro);
            //}
            //if (!queryParam["F_AD"].IsEmpty())
            //{
            //    int F_AD = queryParam["F_AD"].ToInt();
            //    expression = expression.And(t => t.F_AD == F_AD);
            //}
            //if (!queryParam["F_WB"].IsEmpty())
            //{
            //    int F_WB = queryParam["F_WB"].ToInt();
            //    expression = expression.And(t => t.F_WB == F_WB);
            //}
            //if (!queryParam["F_Reissue"].IsEmpty())
            //{
            //    int F_Reissue = queryParam["F_Reissue"].ToInt();
            //    expression = expression.And(t => t.F_Reissue == F_Reissue);
            //}
            return service.FindList(expression, pagination);
        }


        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
    }
}
