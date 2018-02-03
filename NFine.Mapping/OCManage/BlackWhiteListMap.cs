using NFine.Domain.Entity.OCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.OCManage
{
    public class BlackWhiteListMap : EntityTypeConfiguration<BlackWhiteListEntity>
    {
        public BlackWhiteListMap()
        {
            this.ToTable("OC_BlackList");
            this.HasKey(t => t.F_Id);
        }
    }
}
