using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Entity
{
    public class Sev_DailyChannelData
    {
        public string F_Id { get; set; }

        public DateTime F_Time { get; set; }
        public int F_ChannelId { get; set; }

        public string F_ChannelFId { get; set; }

        public int F_SubmitCount { get; set; }

        public int F_SendCount { get; set; }

        public int F_RealSuccessCount { get; set; }

        public int F_BuckleSuccessCount { get; set; }

        public int F_ErrorCount { get; set; }

        public int F_UnknownCount { get; set; }

        public int F_ReissueCount { get; set; }

        public decimal F_Cost { get; set; }

        public decimal F_SuccessRate { get; set; }

        public int F_BuckleCount { get; set; }

    }
}
