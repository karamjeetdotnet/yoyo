using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoYo.Interfaces;
using YoYo.Models;

namespace YoYo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IPlayerService _playerService;

        public PlayerController(ILogger<PlayerController> logger, IPlayerService playerService)
        {
            _logger = logger;
            _playerService = playerService;
        }

        [HttpPost]
        [Route("save")]
        public IActionResult UpdatePlayers([FromBody]Player player)
        {
            Player _player = _playerService.Players.Where(x => x.Id == player.Id).FirstOrDefault();
            if(_player == null) { return BadRequest("Player not found"); }
            foreach (var item in player.Statuses)
            {
                _player.Statuses.Add(item);
            }
            _player.SuccessFleet = player.SuccessFleet;
            return Ok(_player);
        }
    }
}
