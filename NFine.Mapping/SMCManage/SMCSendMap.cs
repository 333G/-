using NFine.Domain.Entity.SMCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SMCManage
{
    public class SMCSendMap : EntityTypeConfiguration<SMCSendEntity>
    {
        public SMCSendMap()
        {
            this.ToTable("SMC_SendSms");
            this.HasKey(t => t.F_Id);
        }
    }
}
