using NFine.Domain.Entity.OCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.OCManage
{
    public class AutoReviewMap : EntityTypeConfiguration<AutoReviewEntity>
    {
        public AutoReviewMap()
        {
            this.ToTable("OC_AutoReviewTemplete");
            this.HasKey(t => t.F_Id);
        }
    }
}
