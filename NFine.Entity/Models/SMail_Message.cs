using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class SMail_Message
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:发件人ＩＤ 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_SendID {get;set;}

        /// <summary>
        /// Desc:收件人ＩＤ 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_RecID {get;set;}

        /// <summary>
        /// Desc:短信ＩＤ 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_MessageID {get;set;}

        /// <summary>
        /// Desc:标记，已读，未读 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_Statue {get;set;}

    }
}
