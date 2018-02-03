using NFine.Domain.Entity.OCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.OCManage
{
    public class BaseChannelMap : EntityTypeConfiguration<BaseChannelEntity>
    {
        public BaseChannelMap()
        {
            this.ToTable("OC_BaseChannel");
            //this.HasKey(t => t.Id);
            this.HasKey(t => t.F_Id);
        }
    }
}
