namespace MVCProject.Services
{
    public interface IGamesService
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetAsync(int id);
        Task<Game?> UpdateAsync(EditGameFormViewModel model);

        Task Create(CreateGameFormViewModel model);
        bool Delete(int id);
    }
}
