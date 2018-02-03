

using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using System;
using System.Collections.Generic;
using System.Data;

namespace NFine.Application.OCManage
{
    public class SMSInstructionsApp
    {
        private ISMSInstructionsRepository service = new SMSInstructionsRepository();
        public List<SMSInstructionsEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<SMSInstructionsEntity>();
            var queryParam = queryJson.ToJObject();
            //查询条件 
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(
                    expression.And(t => t.F_UserID.Contains(keyvalue))
                    .Or(t => t.F_Instructions.Contains(keyvalue))
                    .Or(t => t.F_Money.ToString().Contains(keyvalue))
                    );
                //expression = expression.And(t => t.F_UserId.Contains(keyvalue));
                //expression = expression.Or(t => t.F_Instructions.Contains(keyvalue));
                //expression = expression.Or(t => t.F_Money.ToString().Contains(keyvalue));
            }
            return service.FindList(expression, pagination);
        }
        public SMSInstructionsEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(SMSInstructionsEntity SMSInstructionsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SMSInstructionsEntity.Modify(keyValue);
            }
            else
            {
                SMSInstructionsEntity.Create();
            }
            service.SubmitForm(SMSInstructionsEntity, keyValue);
        }
        public void UpdateForm(SMSInstructionsEntity SMSInstructionsEntity)
        {
            service.Update(SMSInstructionsEntity);
        }

        public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<SMSInstructionsEntity>();
            var queryParam = queryJson.ToJObject();

            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_UserID.Contains(keyvalue));
                expression = expression.Or(t => t.F_Instructions.Contains(keyvalue));
                expression = expression.Or(t => t.F_Money.ToString().Contains(keyvalue));
            }

            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_CallTime desc"));
            return getdatatable;
        }
    }
}
