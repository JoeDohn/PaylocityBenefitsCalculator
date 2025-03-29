using Api.Models;

namespace Api.Repositories
{
    public interface IDependentRepository
    {
        Task<Dependent> GetDependentById(int id);

        Task<IEnumerable<Dependent>> GetAllDependentsAsync();
    }
}