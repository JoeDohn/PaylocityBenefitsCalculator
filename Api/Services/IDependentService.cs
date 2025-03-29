using Api.Dtos.Dependent;

namespace Api.Services
{
    public interface IDependentService
    {
        Task<GetDependentDto> GetDependentById(int id);

        Task<IEnumerable<GetDependentDto>> GetAllDependentsAsync();
    }
}
