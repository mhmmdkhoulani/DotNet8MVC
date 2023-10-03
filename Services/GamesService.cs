

namespace MVCProject.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly string _imagesPath;

        public GamesService(ApplicationDbContext db, IWebHostEnvironment webHostEnviroment)
        {
            _db = db;
            _webHostEnviroment = webHostEnviroment;
            _imagesPath = $"{_webHostEnviroment.WebRootPath}{FileSettings.ImagePath}";
        }
        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            var games = await _db.Games.Include(g => g.Category).Include(g => g.Devices).ThenInclude(d => d.Device).AsNoTracking().ToListAsync();
            return games;
        }
        public async Task<Game?> GetAsync(int id)
        {
            var game = await _db.Games.Include(g => g.Category).Include(g => g.Devices).ThenInclude(d => d.Device).AsNoTracking().SingleOrDefaultAsync(g => g.Id == id);
            return game;
        }

        public async Task Create(CreateGameFormViewModel model)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await model.Cover.CopyToAsync(stream);
            

            Game game = new()
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                Description = model.Description,
                CoverUrl = coverName,
                Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };

            await _db.Games.AddAsync(game);
            await _db.SaveChangesAsync();
        }

        
    }
}
