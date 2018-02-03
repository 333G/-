using NFine.Domain.Entity.SMCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SMCManage
{
    public class SMCRceiveMap : EntityTypeConfiguration<SMCRceiveEntity>
    {
        public SMCRceiveMap()
        {
            this.ToTable("SMC_ReplyMessage");
            this.HasKey(t => t.F_Id);
        }
    }
}
