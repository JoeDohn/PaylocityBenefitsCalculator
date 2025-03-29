using Api.Models;

namespace Api.DataAccess.Repositories
{
    public interface IDependentRepository
    {
        Task<Dependent> GetDependentById(int id);

        Task<IEnumerable<Dependent>> GetAllDependents();
    }
}