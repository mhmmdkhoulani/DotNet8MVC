
namespace MVCProject.Controllers
{
    public class GamesController : Controller
    {
       
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGamesService _gamesService;
        public GamesController(ICategoriesService categoriesService, IDevicesService devicesService, IGamesService gamesService)
        {

            _categoriesService = categoriesService;
            _devicesService = devicesService;
            _gamesService = gamesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var games = await _gamesService.GetAllAsync();
            return View(games);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gamesService.GetAsync(id);
            if(game is null)
            {
                return NotFound();
            }
            return View(game);
        }

        [HttpGet]
        public IActionResult Create()
        {
           
            var viewModel = new CreateGameFormViewModel
            {
                Categories =_categoriesService.GetSelectList(),
                Devices =_devicesService.GetSelectList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesService.GetSelectList();
                return View(model);
            }

            //Save game to database
            await _gamesService.Create(model);

            return RedirectToAction(nameof(Index));
        }

        
    }
}
