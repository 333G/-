using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Entity.Views
{
   public class VSMCRceiveSms
    {

        public string F_Id { get; set; }
        public string mobile { get; set; }
        public string receive_content { get; set; }
        public DateTime? receive_time { get; set; }
        public string MsgID { get; set; }
        public string sp_number { get; set; }

        public int F_UserId { get; set; }

        public string F_SmsContent { get; set; }
        public int F_Operator { get; set; }

        /// <summary>
        /// Desc:省 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Province { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_CreatorUserId { get; set; }

    }
}
