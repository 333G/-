using System;
using System.Data;

namespace NFine.Domain.Entity.OCManage
{
    public class PhoneNumAreaInfoEntity : IEntity<PhoneNumAreaInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_NumSegment { get; set; }
        public string F_Province { get; set; }
        public string F_City { get; set; }
        public string F_Operator { get; set; }
        public string F_AreaCode { get; set; }
        public string F_PostCode { get; set; }
        public string F_ReMark { get; set; }


        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }

        /// <summary>
        /// 删除实体的用户
        /// </summary>
        public string F_DeleteUserId { get; set; }

        /// <summary>
        /// 删除实体时间
        /// </summary>
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
    }
}
