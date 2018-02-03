using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.OCManage
{
   public class ChannelProvinceEntity : IEntity<ChannelProvinceEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }

        public string F_DeleteUserId { get; set; }

        public DateTime? F_DeleteTime { get; set; }
       
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }




        public int Id { get; set; }

        public int F_ProvinceId { get; set; }

        public string F_ProvinceName { get; set; }

       
        public int F_ChannelId { get; set; }

        
        public int F_SwitchChannelId { get; set; }

      
        public DateTime F_CreateTime { get; set; }

        //*********************************************************//
    }
}
