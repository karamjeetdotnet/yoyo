using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YoYo.Interfaces;
using YoYo.Models;

namespace YoYo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlayerService _playerService;

        public HomeController(ILogger<HomeController> logger, IPlayerService playerService)
        {
            _logger = logger;
            _playerService = playerService;
        }

        public IActionResult Index()
        {
            PlayerViewModel playerView = new PlayerViewModel();
            if (_playerService.StartTime != DateTime.MinValue)
            {
                return RedirectToAction("Start");
            }
            _playerService.Init();
            playerView.Players = _playerService.Players;
            return View("Index", playerView);
        }
        public IActionResult Start()
        {
            PlayerViewModel playerView = new PlayerViewModel();
            playerView.IsStart = true;
            _playerService.Start();
            TimeSpan timeSpan = (DateTime.Now - _playerService.StartTime);
            playerView.ElapsedTime = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
            playerView.Players = _playerService.Players;
            return View("Index", playerView);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
