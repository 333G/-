using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Entity.Views
{
    public class VSMailMessage
    {
        public string F_Id { get; set; }

        public string F_Message { get; set; }

        public DateTime F_CreatorTime { get; set; }

        public string F_SendName { get; set; }
        public string F_ReceiveName { get; set; }

        public string F_Statue { get; set; }
        //收件箱回复字段
        public string ReceiveId { get; set; }
        public string ReceiveMessage { get; set; }
        public string ReceiveName { get; set; } 
    }
}
