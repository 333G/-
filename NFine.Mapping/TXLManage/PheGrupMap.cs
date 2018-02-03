using NFine.Domain.Entity.TXLManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.TXLManage
{
    public class PheGrupMap : EntityTypeConfiguration<PheGrupEntity>
    {
        public PheGrupMap()
        {
            this.ToTable("TXL_PhoneGroup");
            this.HasKey(t => t.F_Id);
        }
    }
}
