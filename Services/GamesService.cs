

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
            var coverName = await SaveCover(model.Cover);
            

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

        public async Task<Game?> UpdateAsync(EditGameFormViewModel model)
        {
            var game = await _db.Games.Include(g => g.Devices).FirstOrDefaultAsync(g => g.Id == model.Id);
           
            if (game == null)
            {
                return null;
            }
            else
            {
                var hasNewCover = model.Cover is not null;
                var oldGameCover = game.CoverUrl;
                if (hasNewCover)
                {
                    game.CoverUrl = await SaveCover(model.Cover!);
                     
                }
                game.Name = model.Name;
                game.CategoryId = model.CategoryId; 
                game.Description = model.Description;
                game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

                var effectedRows =  await _db.SaveChangesAsync();   
                if(effectedRows > 0)
                {
                    if (hasNewCover)
                    {
                        var cover = Path.Combine(_imagesPath, oldGameCover);
                        File.Delete(cover);
                    }
                    return game;
                }
                else
                {
                    var cover = Path.Combine(_imagesPath, game.CoverUrl);
                    File.Delete(cover);
                    return null;
                }
            }
           
            
        }

        private async Task<string> SaveCover(IFormFile file)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await file.CopyToAsync(stream);
            return coverName;
        }
    }
}
