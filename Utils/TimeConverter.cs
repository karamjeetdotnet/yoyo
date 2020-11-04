using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoYo.Utils
{
    public class TimeConverter
    {
        public static int GetTotalSeconds(string minuteSeconds) {
            string[] time = minuteSeconds.Split(":");
            if (time.Length != 2) {
                throw new FormatException("data does not contain valid format");
            }
            int minutes = 0, seconds = 0;
            if (!int.TryParse(time[0], out minutes)) { minutes = 0; }
            if (!int.TryParse(time[1], out seconds)) { seconds = 0; }
            return ((minutes * 60) + seconds);
        }
    }
}
