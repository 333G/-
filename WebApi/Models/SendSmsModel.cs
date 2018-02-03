using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class SendSmsModel
    {

        /// <summary>
        /// Desc:收件人号码字符串，分隔符‘，’ 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string MobileList { get; set; }

        /// <summary>
        /// Desc:短信号码数量 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? MobileCount { get; set; }

        /// <summary>
        /// Desc:短信内容 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string SmsContent { get; set; }

        /// <summary>
        /// Desc:是否定时（默认是false,false=及时发送；true=定时发送） 
        /// Default:((0)) 
        /// Nullable:False 
        /// </summary>
        public Boolean IsTime { get; set; }

        /// <summary>
        /// Desc:发送时间，用于定时发送 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// Desc:短信标记中文 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string SendSign { get; set; }

        /// <summary>
        /// Desc:创建时间 
        /// Default:(getdate()) 
        /// Nullable:True 
        /// </summary>
        public DateTime? CreatorTime { get; set; }

        /// <summary>
        /// Desc:创建人 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// Desc:短信标记英文 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string SmbSign { get; set; }

        public string GroupChannelId { get; set; }

    }
}