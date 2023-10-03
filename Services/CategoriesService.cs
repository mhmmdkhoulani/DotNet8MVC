namespace MVCProject.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext _db;

        public CategoriesService(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _db.Categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).OrderBy(c => c.Text).AsNoTracking().ToList();
            
        }
    }
}
