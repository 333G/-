
using NFine.Code;
using NFine.Domain.Entity.TXLManage;
using NFine.Domain.IRepository.TXLManage;
using NFine.Repository.TXLManage;
using System.Collections.Generic;
using System.Data;

namespace NFine.Application.TXLManage
{
    public class BlacklistApp
    {
        private IBlacklistRepository service = new BlacklistRepository();
        public List<BlacklistEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<BlacklistEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["Mobile"].IsEmpty())
            {
                string Mobile = queryParam["Mobile"].ToString();
                expression = expression.And(t => t.Mobile.Equals(Mobile));
            }
            if (!queryParam["F_CreatorTime"].IsEmpty())
            {
                string F_CreatorTime = queryParam["F_CreatorTime"].ToString();
                expression = expression.And(t => t.F_CreatorTime.Equals(F_CreatorTime));
            }
            return service.FindList(expression, pagination);
        }
        public BlacklistEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(BlacklistEntity BlacklistEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                BlacklistEntity.Modify(keyValue);
            }
            else
            {
                BlacklistEntity.Create();
            }
            service.SubmitForm(BlacklistEntity, keyValue);
        }
        public void UpdateForm(BlacklistEntity BlacklistEntity)
        {
            service.Update(BlacklistEntity);
        }

        public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<BlacklistEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["Mobile"].IsEmpty())
            {
                string Mobile = queryParam["Mobile"].ToString();
                expression = expression.And(t => t.Mobile.Equals(Mobile));
            }
            if (!queryParam["F_CreatorTime"].IsEmpty())
            {
                string F_CreatorTime = queryParam["F_CreatorTime"].ToString();
                expression = expression.And(t => t.F_CreatorTime.Equals(F_CreatorTime));
            }
            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_CreatorTime desc"));
            return getdatatable;
        }
    }
}
