using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoYo.Models
{
    public class ShuttleInfo
    {
        public int AccumulatedShuttleDistance { get; set; } = 0;
        public int SpeedLevel { get; set; } = 0;
        public int ShuttleNo { get; set; } = 0;
        public string Speed { get; set; } = "0.0";
        public string LevelTime { get; set; } = "0.0";
        public string CommulativeTime { get; set; } = "00:00";
        public string StartTime { get; set; } = "00:00";
        public int StartTimeSeconds { get { return Utils.TimeConverter.GetTotalSeconds(StartTime); } }
        public string ApproxVo2Max { get; set; } = "0.0";
    }
}
