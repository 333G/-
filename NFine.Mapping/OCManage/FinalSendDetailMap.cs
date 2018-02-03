using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Domain.Entity.OCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.OCManage
{
   public class FinalSendDetailMap : EntityTypeConfiguration<FinalSendDetailEntity>
    {
        public FinalSendDetailMap()
        {
            this.ToTable("Sev_FinalSendDetail");
            this.HasKey(t => t.F_Id);
        }
    }
}
