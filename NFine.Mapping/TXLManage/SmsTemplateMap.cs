using NFine.Domain.Entity.TXLManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.TXLManage
{
    public class SmsTemplateMap : EntityTypeConfiguration<SmsTemplateEntity>
    {
        public SmsTemplateMap()
        {
            this.ToTable("TXL_CusSmsTemplate");
            this.HasKey(t => t.F_Id);
        }
    }
}
