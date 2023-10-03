namespace MVCProject.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly ApplicationDbContext _db;

        public DevicesService(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _db.Devices.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).AsNoTracking().OrderBy(c => c.Text).ToList();
        }
    }
}
