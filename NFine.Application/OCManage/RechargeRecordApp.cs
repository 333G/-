using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using System;
using System.Collections.Generic;
using System.Data;
using NFine.Entity;

namespace NFine.Application.OCManage
{
    public class RechargeRecordApp
    {
        private IRechargeRecordRepository service = new RechargeRecordRepository();
        public List<RechargeRecordEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<RechargeRecordEntity>();
            var queryParam = queryJson.ToJObject();

            //查询条件 页面下拉栏字段待定

            if (!(queryParam["F_CreatorTimeFrom"].IsEmpty() || queryParam["F_CreatorTimeEnd"].IsEmpty()))
            {
                DateTime F_CreatorTimeFrom = (DateTime)queryParam["F_CreatorTimeFrom"];
                DateTime F_CreatorTimeEnd = (DateTime)queryParam["F_CreatorTimeEnd"];
                DateTime newF_CreatorTimeEnd = F_CreatorTimeEnd.AddDays(1);
                expression = expression.And(t => ((t.F_CreatorTime >= F_CreatorTimeFrom) && t.F_CreatorTime <= newF_CreatorTimeEnd));
            }
            if (!queryParam["F_UserId"].IsEmpty())
            {
                string F_UserId = queryParam["F_UserId"].ToString();
                expression = expression.And(t => t.F_UserId.Equals(F_UserId));
            }
            if (!queryParam["F_Account"].IsEmpty())
            {
                string F_Account = queryParam["F_Account"].ToString();
                expression = expression.And(t => t.F_Account.Equals(F_Account));
            }
            if (!queryParam["F_State"].IsEmpty())
            {
                string F_State = queryParam["F_State"].ToString();
                expression = expression.And(t => t.F_State.Equals(F_State));
            }

            if (!queryParam["F_PayMode"].IsEmpty())
            {
                int F_PayMode = queryParam["F_PayMode"].ToInt();
                expression = expression.And(t => t.F_PayMode.Equals(F_PayMode));
            }


            /*
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_CustInfo.Contains(keyvalue));
            }*/
            return service.FindList(expression, pagination);
        }
        public RechargeRecordEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(RechargeRecordEntity RechargeRecordEntity,OC_UserInfo model_UI, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                RechargeRecordEntity.Modify(keyValue);
                service.SubmitForm(RechargeRecordEntity, keyValue);
            }
            else
            {
                RechargeRecordEntity.Create();
                OC_UserInfo model= DAL.OC_UserInfoDAL.Instance.FindEntity(t=>t.F_UserId==model_UI.F_UserId);//获取UserInfo的Model
                model.F_Balance = RechargeRecordEntity.F_RechargeOver;//修改UserInfo的余额
                DAL.OC_UserInfoDAL.Instance.Update(model);//更新UserInfo
                service.SubmitForm(RechargeRecordEntity, keyValue);
            }
        }
        public void UpdateForm(RechargeRecordEntity RechargeRecordEntity)
        {
            service.Update(RechargeRecordEntity);
        }

        public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<RechargeRecordEntity>();
            var queryParam = queryJson.ToJObject();
            if (!(queryParam["F_CreatorTimeFrom"].IsEmpty() || queryParam["F_CreatorTimeEnd"].IsEmpty()))
            {
                DateTime F_CreatorTimeFrom = (DateTime)queryParam["F_CreatorTimeFrom"];
                DateTime F_CreatorTimeEnd = (DateTime)queryParam["F_CreatorTimeEnd"];
                expression = expression.And(t => (t.F_CreatorTime > F_CreatorTimeFrom));
            }
            if (!queryParam["F_UserId"].IsEmpty())
            {
                string F_UserId = queryParam["F_UserId"].ToString();
                expression = expression.And(t => t.F_UserId.Equals(F_UserId));
            }
            if (!queryParam["F_Account"].IsEmpty())
            {
                string F_Account = queryParam["F_Account"].ToString();
                expression = expression.And(t => t.F_Account.Equals(F_Account));
            }
            /*//查询条件 字段待定
             * if (!queryParam["F_StateId"].IsEmpty())
            {
                string F_StateId = queryParam["F_StateId"].ToString();
                expression = expression.And(t => t.F_StateId.Equals(F_StateId));
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_CustInfo.Contains(keyvalue));
            }
            */
            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_CreatorTime desc"));
            return getdatatable;
        }
    }
}
