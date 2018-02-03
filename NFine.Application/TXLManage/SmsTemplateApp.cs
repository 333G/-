using NFine.Code;
using NFine.Domain.Entity.TXLManage;
using NFine.Domain.IRepository.TXLManage;
using NFine.Repository.TXLManage;
using System;
using System.Collections.Generic;
using System.Data;

namespace NFine.Application.TXLManage
{
    public class SmsTemplateApp
    {
        private ISmsTemplateRepository service = new SmsTemplateRepository();
        public List<SmsTemplateEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<SmsTemplateEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["F_TplContent"].IsEmpty())
            {
                string F_TplContent = queryParam["F_TplContent"].ToString();
                expression = expression.And(t => t.F_TplContent.Contains(F_TplContent));
            }

            return service.FindList(expression, pagination);
        }
        public SmsTemplateEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(SmsTemplateEntity SmsTemplateEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SmsTemplateEntity.Modify(keyValue);
            }
            else
            {
                SmsTemplateEntity.Create();
            }
            service.SubmitForm(SmsTemplateEntity, keyValue);
        }
        public void UpdateForm(SmsTemplateEntity SmsTemplateEntity)
        {
            service.Update(SmsTemplateEntity);
        }

        public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<SmsTemplateEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["F_TplContent"].IsEmpty())
            {
                string F_TplContent = queryParam["F_TplContent"].ToString();
                expression = expression.And(t => t.F_TplContent.Equals(F_TplContent));
            }
            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_CreatorTime desc"));
            return getdatatable;
        }
    }
}
