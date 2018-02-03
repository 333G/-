using NFine.Domain.Entity.OCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.OCManage
{
    public class SensitiveWordsMap : EntityTypeConfiguration<SensitiveWordsEntity>
    {
        public SensitiveWordsMap()
        {
            this.ToTable("SMS_SensitiveWords");
            this.HasKey(t => t.F_Id);
        }
    }
}
