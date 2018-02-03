using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Entity
{
    public class Sev_BusinessDataAnalysis
    {
        public Int64 Id { get; set; }

        public DateTime F_SendDate { get; set; }
        
        public int F_UserId { get; set; }

        //【单次短信总价】用户的某一次发送短信总价格
        public decimal F_SmsPrice { get; set; }

        public int F_SubmitCount { get; set; }

        public int F_SendCount { get; set; }

        public int F_SuccessCount { get; set; }

        public int F_ErrorCount { get; set; }

        public int F_UnknownCount { get; set; }

        //【金额】当天用户发送扣除的总金额
        public decimal F_Cost { get; set; }

        //【显示金额】用户充值记录里面 符合当前查询时间范围内的金额总计
        public decimal F_ShowCost { get; set; }

        //【实际金额】用户充值记录里面 符合当前查询时间范围内的金额总计
        public decimal F_RealCost { get; set; }

        //返款金额
        public decimal F_BackCost { get; set; }

        //未支付金额
        public decimal F_UnPayCost { get; set; }

        public int F_MobileSendCount { get; set; }

        public int F_UnicomSendCount { get; set; }

        public int F_TelecomSendCount { get; set; }

        //通道利润
        public decimal F_ChanneProfit { get; set; }

        //实际利润
        public decimal F_RealProfit { get; set; }
    }
}
