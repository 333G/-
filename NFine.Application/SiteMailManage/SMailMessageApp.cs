
using NFine.Code;
using NFine.Domain.Entity.SiteMailManage;
using NFine.Domain.IRepository.SiteMailManage;
using NFine.Repository.SiteMailManage;
using System;
using System.Collections.Generic;
using System.Data;

namespace NFine.Application.SiteMailManage
{
    public class SMailMessageApp
    {
        private ISMailMessageRepository service = new SMailMessageRepository();
        public List<SMailMessageEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<SMailMessageEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["F_SendID"].IsEmpty())
            {
                string F_SendID = queryParam["F_SendID"].ToString();
                expression = expression.And(t => t.F_SendID.Equals(F_SendID));
            }

            return service.FindList(expression, pagination);
        }


        public SMailMessageEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(SMailMessageEntity SMailMessageEntity, string keyValue)
        {
            //针对对象引用问题
            if (string.IsNullOrEmpty(keyValue))
                SMailMessageEntity.F_Id = Common.GuId();
            service.SubmitForm(SMailMessageEntity, keyValue);
        }
        public void UpdateForm(SMailMessageEntity SMailMessageEntity)
        {
            service.Update(SMailMessageEntity);
        }

       
    }
}
