using NFine.Domain.Entity.OCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.OCManage
{
    public class ManualReviewMap : EntityTypeConfiguration<ManualReviewEntity>
    {
        public ManualReviewMap()
        {
            this.ToTable("OC_ManualReviewTemplete");
            this.HasKey(t => t.F_Id);
        }
    }
}
