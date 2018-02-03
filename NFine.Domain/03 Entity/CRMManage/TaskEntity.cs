using System;
using System.Collections.Generic;



namespace NFine.Domain.Entity.CRMManage
{
    public class TaskEntity : IEntity<TaskEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
      //  public List<TaskEntity> taskList { get; set; }
        public string F_Id { get; set; }
        public string F_TaskInfo { get; set; }
        public string F_MobilePhone { get; set; }
        public DateTime? F_TaskTime { get; set; }
        public string F_DoUser { get; set; }
        public string F_StatId { get; set; }
        public int F_SortCode { get; set; } 
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
    }
}

