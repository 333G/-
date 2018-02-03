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
    public class CustomeRecordsApp
    {
        private ICustomeRecordsRepository service = new CustomeRecordsRepository();


        public List<CustomeRecordsEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<CustomeRecordsEntity>();
            var queryParam = queryJson.ToJObject();

            /*
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
                    */
            return service.FindList(expression, pagination);
        }
        //查询客户F_Id为F_CustomeId值的所有记录并按降序排列
        public List<CustomeRecordsEntity> GetList(string queryJson)
        {
            var expression = ExtLinq.True<CustomeRecordsEntity>();
            var queryParam = queryJson.ToJObject();

            
                    if (!queryParam["F_CustomeId"].IsEmpty())
                    {
                        string F_CustomeId = queryParam["F_CustomeId"].ToString();
                        expression = expression.And(t => t.F_CustomeId.Equals(F_CustomeId));               
                    }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_Record.Contains(keyvalue));
            }
            return service.FindList(expression, "F_CreatorTime desc");
        }
        public CustomeRecordsEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(CustomeRecordsEntity CustomeRecordsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                CustomeRecordsEntity.Modify(keyValue);
            }
            else
            {
                CustomeRecordsEntity.Create();
            }
            service.SubmitForm(CustomeRecordsEntity, keyValue);
        }
        public void UpdateForm(CustomeRecordsEntity CustomeRecordsEntity)
        {
            service.Update(CustomeRecordsEntity);
        }

       public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<CustomeRecordsEntity>();
            var queryParam = queryJson.ToJObject();
            /*
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
            */
            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_CallTime desc"));
            return getdatatable;
        }
    }
}
