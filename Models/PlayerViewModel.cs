using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using YoYo.Utils;

namespace YoYo.Models
{
    public class PlayerViewModel
    {
        public PlayerViewModel()
        {
            Players = new List<Player>();
        }
        public List<Player> Players { get; set; }
        public bool IsStart { get; set; }
        public string ElapsedTime { get; set; } = "00:00";
        public int NextShuttle { get; set; } = 0;
        public int NextOrgSeconds { get; set; } = 0;
        public ShuttleInfo ReadShuttleInfo() {
            ShuttleInfo shuttleInfo = new ShuttleInfo();
            if (IsStart)
            {
                string bleepData = System.IO.File.ReadAllText("wwwroot/js/fitnessrating_beeptest.json");
                var serializeOptions = new JsonSerializerOptions();
                serializeOptions.Converters.Add(new StringToIntConverter());
                List<ShuttleInfo> shuttleItems = JsonSerializer.Deserialize<List<ShuttleInfo>>(bleepData, serializeOptions);
                int totalSeconds = TimeConverter.GetTotalSeconds(ElapsedTime);
                ShuttleInfo nextShuttle = shuttleItems.Where(x => x.StartTimeSeconds > totalSeconds).FirstOrDefault();
                shuttleInfo = shuttleItems.Where(x => x.StartTimeSeconds <= totalSeconds).LastOrDefault();
                if (nextShuttle != null)
                {
                    NextShuttle = TimeConverter.GetTotalSeconds(nextShuttle.StartTime) - totalSeconds;
                    NextOrgSeconds = TimeConverter.GetTotalSeconds(nextShuttle.StartTime) - TimeConverter.GetTotalSeconds(shuttleInfo.StartTime);
                }
            }
            return shuttleInfo;
        }
    }
}
