using System;

using NFine.Domain.Entity.OCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.OCManage
{
    public class SMSInstructionsMap : EntityTypeConfiguration<SMSInstructionsEntity>
    {
        public SMSInstructionsMap()
        {
            this.ToTable("OC_SMSInstructions");
            this.HasKey(t => t.F_Id);
        }
    }
}
