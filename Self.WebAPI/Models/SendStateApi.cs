using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Self.WebAPI.Models
{
    public class SendStateApi
    {   //GuID
        public string GUId { get; set; }

        //Id
        public int Id { get; set; }

        //发送Id号
        public string SendId { get; set; }

        /// <summary>
        /// Desc:报告 (0=等待；1=成功；2=失败)
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int? Report { get; set; }

        //应答状态(0=未提交（等待）；1=已提交；2=成功；3=失败；4=未知)
        public int Response { get; set; }

        //回执时间（实际发送时间）
        public DateTime? ResponseTime { get; set; }

        //通道报告状态
        public string BackReport { get; set; }

        //回执状态码
        public string ResponseCode { get; set; }

        //任务创建时间
        public string CreatorJobTime { get; set; }

        //任务编号
        public int JobNum { get; set; }

        //处理状态
        public int DealState { get; set; }
    }
}