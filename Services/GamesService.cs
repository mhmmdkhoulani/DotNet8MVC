

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
