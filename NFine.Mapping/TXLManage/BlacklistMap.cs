using NFine.Domain.Entity.TXLManage;
using System.Data.Entity.ModelConfiguration;
using System;

namespace NFine.Mapping.TXLManage
{
    public class BlacklistMap : EntityTypeConfiguration<BlacklistEntity>
    {
        public BlacklistMap()
        {
            this.ToTable("TXL_BlackList");
            this.HasKey(t => t.F_Id);
        }

       
    }
}
