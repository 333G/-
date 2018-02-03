
using NFine.Domain.Entity.CRMManage;
using System.Data.Entity.ModelConfiguration; 

namespace NFine.Mapping.CRMManage
{
    public class TaskMap : EntityTypeConfiguration<TaskEntity>
    {
        public TaskMap()
        {
            this.ToTable("CRM_Task");
            this.HasKey(t => t.F_Id);
        }
    }
     
}
