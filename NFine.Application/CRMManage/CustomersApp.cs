using NFine.Code;
using NFine.Domain.Entity.CRMManage;
using NFine.Domain.IRepository.CRMManage;
using NFine.Repository.CRMManage;
using System;
using System.Collections.Generic;
using System.Data; 
using System.Linq;

namespace NFine.Application.CRMManage
{
    public class CustomersApp
    {
        private ICustomersRepository service = new CustomersRepository();
       

        public List<CustomersEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<CustomersEntity>();
            var queryParam = queryJson.ToJObject();

    
            if (!queryParam["F_StateId"].IsEmpty())
            {
                string F_StateId = queryParam["F_StateId"].ToString();
                expression = expression.And(t => t.F_StateId.Equals(F_StateId));               
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_CustInfo.Contains(keyvalue));
            }
            return service.FindList(expression, pagination);
        }
        public CustomersEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(CustomersEntity CustomersEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                CustomersEntity.Modify(keyValue);
            }
            else
            {
                CustomersEntity.Create();
            }
            service.SubmitForm(CustomersEntity, keyValue);
        }
        public void UpdateForm(CustomersEntity CustomersEntity)
        {
            service.Update(CustomersEntity);
        }

       public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<CustomersEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["F_StateId"].IsEmpty())
            {
                string F_StateId = queryParam["F_StateId"].ToString();
                expression = expression.And(t => t.F_StateId.Equals(F_StateId));
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_CustInfo.Contains(keyvalue));
            }
            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_CallTime desc"));
            return getdatatable;
        }
    }
}
