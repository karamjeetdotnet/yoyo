using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using YoYo.Interfaces;
using YoYo.Models;
using YoYo.Utils;

namespace YoYo.Services
{
    public class PlayerService : IPlayerService
    {
        public List<Player> Players { get; set; } = new List<Player>();
        public DateTime StartTime { get; set; }

        public void Init()
        {
            string playerData = System.IO.File.ReadAllText("wwwroot/players.json");
            Players = JsonSerializer.Deserialize<List<Player>>(playerData);
        }

        public void Start()
        {
            if (DateTime.MinValue == StartTime)
            {
                StartTime = DateTime.Now;
            }
        }
    }
}
