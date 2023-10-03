namespace MVCProject.Services
{
    public interface IGamesService
    {
        Task Create(CreateGameFormViewModel model);
    }
}
