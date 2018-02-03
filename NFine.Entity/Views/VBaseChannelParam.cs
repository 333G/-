namespace NFine.Entity.Views
{
    /// <summary>
    /// 通道查询参数
    /// </summary>
    public class VBaseChannelParam
    {
        /// <summary>
        /// 通道名称
        /// </summary>
        public string F_ChannelName { get; set; }

        /// <summary>
        /// 运营商
        /// </summary>
        public int F_Operator { get; set; }

        /// <summary>
        ///通道状态（1=启用；0=暂停；）
        /// </summary>
        public int? F_ChannelState { get; set; }

        /// <summary>
        /// 类型（HTTP=1;CCMP=2;SMGP=3）
        /// </summary>
        public string F_UrlType { get; set; }

        /// <summary>
        ///通道余额(元)  最大值
        /// </summary>
        public decimal? F_ChaBalance_Up { get; set; }

        /// <summary>
        /// 通道余额(元)  最小值
        /// </summary>
        public decimal? F_ChaBalance_Down { get; set; }
    }
}