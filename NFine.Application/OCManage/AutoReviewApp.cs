
using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using System;
using System.Collections.Generic;
using System.Linq;


namespace NFine.Application.OCManage
{
    public class AutoReviewApp
    {
        private IAutoReviewRepository service = new AutoReviewRepository();
        public List<AutoReviewEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public List<AutoReviewEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<AutoReviewEntity>();
            var queryParam = queryJson.ToJObject();
            //查询条件 
            if (!queryParam["F_UserID"].IsEmpty())
            {
                int F_UserID = queryParam["F_UserID"].ToInt();
                expression = expression.And(t => t.F_UserID == (F_UserID));
            }
            if (!queryParam["F_RootID"].IsEmpty())
            {
                int F_RootID = queryParam["F_RootID"].ToInt();
                expression = expression.And(t => t.F_RootID == (F_RootID));
            }
            if (!queryParam["F_ParentID"].IsEmpty())
            {
                int F_ParentID = queryParam["F_ParentID"].ToInt();
                string F_ID = DAL.Sys_UserDAL.Instance.FindEntity(t => t.Id == F_ParentID).F_Id;
                expression = expression.And(t => t.F_ParentID == F_ID);
            }
            if (!queryParam["F_SourceSms"].IsEmpty())
            {
                string F_SourceSms = queryParam["F_SourceSms"].ToString();
                expression = expression.And(t => t.F_SourceSms.Contains(F_SourceSms));
            }

            return service.FindList(expression, pagination);
        }
        public AutoReviewEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(AutoReviewEntity autoReviewEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                autoReviewEntity.Modify(keyValue);
            }
            else
            {
                autoReviewEntity.Create();//F_Id和CreatorUserId
            }
            service.SubmitForm(autoReviewEntity, keyValue);
        }
        public void UpdateForm(AutoReviewEntity AutoReviewEntity)
        {
            service.Update(AutoReviewEntity);
        }
    }
}
