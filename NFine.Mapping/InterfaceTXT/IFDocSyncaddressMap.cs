using NFine.Domain.Entity.InterfaceTXT;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.InterfaceTXT
{
    public class IFDocSyncaddressMap : EntityTypeConfiguration<IFDocSyncaddressEntity>
    {
        public IFDocSyncaddressMap()
        {
            this.ToTable("IFDoc_SyncAddress");
            this.HasKey(t => t.F_Id);
        }
    }
}
