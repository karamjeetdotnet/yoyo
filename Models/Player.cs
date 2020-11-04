using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoYo.Enums;

namespace YoYo.Models
{
    public class Player
    {
        public Player()
        {
            //enums are not working due to system.text.json lib using string as workaround for storage
            Statuses = new HashSet<string>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ShuttleInfo SuccessFleet { get; set; }
        public HashSet<string> Statuses { get; set; }
    }
}
