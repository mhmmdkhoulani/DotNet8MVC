namespace MVCProject.Services
{
    public interface IGamesService
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetAsync(int id);

        Task Create(CreateGameFormViewModel model);
    }
}
