
using NFine.Code;
using NFine.Domain.Entity.InterfaceTXT;
using NFine.Domain.IRepository.InterfaceTXT;
using NFine.Repository.InterfaceTXT;
using System;
using System.Collections.Generic;
using System.Data;

namespace NFine.Application.InterfaceTXT
{
    public class IFDocSyncaddressApp
    {
        private IIFDocSyncaddressRepository service = new IFDocSyncaddressRepository();
        public List<IFDocSyncaddressEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<IFDocSyncaddressEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["F_ReAscRptUrl"].IsEmpty())
            {
                string F_ReAscRptUrl = queryParam["F_ReAscRptUrl"].ToString();
                expression = expression.And(t => t.F_ReAscRptUrl.Equals(F_ReAscRptUrl));
            }
            
            return service.FindList(expression, pagination);
        }
        public IFDocSyncaddressEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(IFDocSyncaddressEntity IFDocSyncaddressEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                IFDocSyncaddressEntity.Modify(keyValue);
            }
            else
            {
                IFDocSyncaddressEntity.Create();
            }
            service.SubmitForm(IFDocSyncaddressEntity, keyValue);
        }
        public void UpdateForm(IFDocSyncaddressEntity IFDocSyncaddressEntity)
        {
            service.Update(IFDocSyncaddressEntity);
        }

       
    }
}
