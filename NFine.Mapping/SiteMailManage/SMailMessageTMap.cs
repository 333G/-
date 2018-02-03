using NFine.Domain.Entity.SiteMailManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.SiteMailManage
{
    public class SMailMessageTMap : EntityTypeConfiguration<SMailMessageTEntity>
    {
        public SMailMessageTMap()
        {
            this.ToTable("SMail_MessageText");
            this.HasKey(t => t.F_Id);
        }
    }
}
