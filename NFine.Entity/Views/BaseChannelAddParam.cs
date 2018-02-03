using System;

namespace NFine.Entity.Views
{
    /// <summary>
    /// 添加通道参数
    /// </summary>
    public class BaseChannelAddParam
    {
        /// <summary>
        /// 通道名称
        /// </summary>
        public string F_ChannelName { get; set; }

        /// <summary>
        /// 运营商（1=移动；2=联通；3=电信；默认是1）
        /// </summary>
        public int F_Operator { get; set; }

        /// <summary>
        /// 通道状态（1=启用；0=暂停；）
        /// </summary>
        public int F_ChannelState { get; set; }

        /// <summary>
        /// 启用时间
        /// </summary>
        public TimeSpan F_StartTime { get; set; }

        /// <summary>
        /// 禁用时间
        /// </summary>
        public TimeSpan F_EndTime { get; set; }

        /// <summary>
        /// 通道单价(分)
        /// </summary>
        public decimal F_Price { get; set; }

        /// <summary>
        /// 通道余额(元)
        /// </summary>
        public decimal F_ChaBalance { get; set; }

        /// <summary>
        /// 剩余条数
        /// </summary>
        public int F_SurplusNum { get; set; }

        /// <summary>
        /// 已用条数
        /// </summary>
        public int F_SendedNum { get; set; }

        /// <summary>
        /// 字符数
        /// </summary>
        public int CharacterCount { get; set; }

        /// <summary>
        /// 长短信标志（0=不支持；1=支持；默认是1）
        /// </summary>
        public bool LongSmsSign { get; set; }

        /// <summary>
        /// 最大长字符数
        /// </summary>
        public int LongSmsNumber { get; set; }

        /// <summary>
        /// 计费字数
        /// </summary>
        public int ChargeNumber { get; set; }

        /// <summary>
        /// 签名类型
        /// </summary>
        public int F_autograph { get; set; }

        /// <summary>
        /// 退订（0=不开启退订；1=开启退订；）
        /// </summary>
        public int F_unsubscribe { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string F_Description { get; set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        public int ProtocolType { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 网关IP
        /// </summary>
        public string GatewayIP { get; set; }

        /// <summary>
        /// 网关端口
        /// </summary>
        public string GatewayPort { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// 接入号
        /// </summary>
        public string AccessNumber { get; set; }

        /// <summary>
        /// 企业代码
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 企业节点代码
        /// </summary>
        public string CompanyNodeCode { get; set; }

        /// <summary>
        /// 监控状态(0=禁用；1=启用)
        /// </summary>
        public bool MonitorState { get; set; }

        /// <summary>
        /// 监控时长
        /// </summary>
        public int MonitorTime { get; set; }

        /// <summary>
        /// 监控人手机号
        /// </summary>
        public string MonitorMobile { get; set; }
    }
}