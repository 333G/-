using NFine.Domain.Entity.TXLManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.TXLManage
{
    public class PheInfoMap : EntityTypeConfiguration<PheInfoEntity>
    {
        public PheInfoMap()
        {
            this.ToTable("TXL_PhoneInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
