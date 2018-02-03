using NFine.Domain.Entity.OCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.OCManage
{
    public class RechargeRecordMap : EntityTypeConfiguration<RechargeRecordEntity>
    {
        public RechargeRecordMap()
        {
            this.ToTable("OC_RechargeRecord");
            this.HasKey(t => t.F_Id);
        }
    }
}

 

