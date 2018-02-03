namespace NFine.Entity.Views
{
    public class VChannelConfigParam
    {
        /// <summary>
        /// Desc:主键，自增长 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Desc:通道ID 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// Desc:协议类型（HTTP=1;CCMP=2;SMGP=3；默认是1） 
        /// Default:((1)) 
        /// Nullable:False 
        /// </summary>
        public int ProtocolType { get; set; }

        /// <summary>
        /// Desc:通道URL地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Desc:网关IP 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string GatewayIP { get; set; }

        /// <summary>
        /// Desc:网关端口 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string GatewayPort { get; set; }

        /// <summary>
        /// Desc:接入号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string AccessNumber { get; set; }

        /// <summary>
        /// Desc:企业代码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// Desc:账号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Desc:密码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string UserPwd { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string CompanyNodeCode { get; set; }
    }
}
