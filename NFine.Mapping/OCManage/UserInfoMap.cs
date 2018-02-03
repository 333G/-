using NFine.Domain.Entity.OCManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.OCManage
{
    public class UserInfoMap : EntityTypeConfiguration<UserInfoEntity>
    {
        public UserInfoMap()
        {
            this.ToTable("OC_UserInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}
