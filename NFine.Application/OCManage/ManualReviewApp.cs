
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
    public class ManualReviewApp
    {
        private IManualReviewRepository service = new ManualReviewRepository();
        public List<ManualReviewEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<ManualReviewEntity>();
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
            return service.FindList(expression, pagination);
        }
        public ManualReviewEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(ManualReviewEntity manualReviewEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                manualReviewEntity.Modify(keyValue);
            }
            else
            {
                manualReviewEntity.Create();
            }
            service.SubmitForm(manualReviewEntity, keyValue);
        }
        public void UpdateForm(ManualReviewEntity ManualReviewEntity)
        {
            service.Update(ManualReviewEntity);
        }
    }
}
