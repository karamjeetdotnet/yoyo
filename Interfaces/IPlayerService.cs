using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoYo.Models;

namespace YoYo.Interfaces
{
    public interface IPlayerService
    {
        List<Player> Players { get; set; }
        DateTime StartTime { get; set; }
        void Init();
        void Start();
    }
}
