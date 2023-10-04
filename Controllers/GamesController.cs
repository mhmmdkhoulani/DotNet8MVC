
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

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var game = await _gamesService.GetAsync(id);
            if(game is null)
            {
                return NotFound();
            }

            EditGameFormViewModel viewMode = new()
            {
                Id = game.Id,
                Name = game.Name,
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectList(),
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(g => g.DeviceId).ToList(),
                Description = game.Description,
                CoverUrl = game.CoverUrl,
            };

            return View(viewMode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesService.GetSelectList();
                return View(model);
            }

            //Update game to database
            var game = await _gamesService.UpdateAsync(model);
            if(game is null)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gamesService.Delete(id);
            
            return isDeleted ?  Ok() : BadRequest();

        }

    }
}
