using NFine.Domain.Entity.CRMManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.CRMManage
{
    public class CustomersMap : EntityTypeConfiguration<CustomersEntity>
    {
        public CustomersMap()
        {
            this.ToTable("CRM_Customers");
            this.HasKey(t => t.F_Id);
        }
    }
}
