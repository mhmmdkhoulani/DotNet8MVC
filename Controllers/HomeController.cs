using Microsoft.AspNetCore.Mvc;
using MVCProject.Models;
using System.Diagnostics;

namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGamesService _gamesService;

        public HomeController(ILogger<HomeController> logger, IGamesService gamesService)
        {
            _logger = logger;
            _gamesService = gamesService;
        }

        public async Task<IActionResult> Index()
        {
            var games = await _gamesService.GetAllAsync();
            return View(games);
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