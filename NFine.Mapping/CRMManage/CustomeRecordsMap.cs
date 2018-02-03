using NFine.Domain.Entity.CRMManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.CRMManage
{
    public class CustomeRecordsMap : EntityTypeConfiguration<CustomeRecordsEntity>
    {
        public CustomeRecordsMap()
        {
            this.ToTable("CRM_CustomeRecords");
            this.HasKey(t => t.F_Id);
        }
    }
}
