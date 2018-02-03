using System;

namespace NFine.Domain.Entity.SiteMailManage
{
    public class SMailMessageEntity : IEntity<SMailMessageEntity>
    {
        public string F_Id { get; set; }
        public string F_SendID { get; set; }
        public string F_RecID { get; set; }
        public string F_MessageID { get; set; }
        public Boolean? F_Statue { get; set; }
        
    }
}
