using NFine.Code;
using NFine.Domain.Entity.CRMManage;
using NFine.Domain.IRepository.CRMManage;
using NFine.Repository.CRMManage;
using System;
using System.Collections.Generic;
using System.Data; 

namespace NFine.Application.CRMManage
{
    public class TaskApp
    {
        private ITaskRepository service = new TaskRepository();
        public List<TaskEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<TaskEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_TaskInfo.Contains(keyvalue));
                expression = expression.Or(t => t.F_DoUser.Contains(keyvalue));
                //完成进度项查询 待定
                //expression = expression.Or(t => t.DoUser.Contains(keyvalue));
            }
            //时间比较,例子，
            if (!queryParam["CreatorTime"].IsEmpty())
            {
                DateTime? CreatorTime = Convert.ToDateTime(queryParam["CreatorTime"]); 
                expression = expression.And(t => t.F_CreatorTime.Value.Day ==CreatorTime.Value.Day);
            }
            return service.FindList(expression, pagination);
        }
        public TaskEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
 
            public void SubmitForm(TaskEntity TaskEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                TaskEntity.Modify(keyValue);
            }
            else
            {
                TaskEntity.Create();
            }
            service.SubmitForm(TaskEntity, keyValue);
        }
        public void UpdateForm(TaskEntity TaskEntity)
        {
            service.Update(TaskEntity);
        }

        public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<TaskEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_TaskInfo.Contains(keyvalue));
                // expression = expression.Or(t => t.F_CreatorTime.Contains(keyvalue)); 时间
                expression = expression.Or(t => t.F_DoUser.Contains(keyvalue));
                //完成进度项查询 待定
                //expression = expression.Or(t => t.DoUser.Contains(keyvalue));

            }
            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_TaskTime desc"));
            return getdatatable;
        }
    }
}
