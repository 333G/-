using NFine.Domain.Entity.SiteMailManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SiteMailManage
{
    public class SMailMessageMap : EntityTypeConfiguration<SMailMessageEntity>
    {
        public SMailMessageMap()
        {
            this.ToTable("SMail_Message");
            this.HasKey(t => t.F_Id);
        }
    }
}
