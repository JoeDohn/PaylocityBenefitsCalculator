using Api.Dtos.Dependent;
using Api.Repositories;
using AutoMapper;

namespace Api.Services
{
    public class DependentService : IDependentService
    {
        private readonly IDependentRepository _dependentRepository;
        private readonly IMapper _mapper;

        public DependentService(IDependentRepository dependentRepository, IMapper mapper)
        {
            _dependentRepository = dependentRepository;
            _mapper = mapper;
        }

        public async Task<GetDependentDto> GetDependentById(int id)
        {
            var dependent = await _dependentRepository.GetDependentById(id);

            return _mapper.Map<GetDependentDto>(dependent);
        }

        public async Task<IEnumerable<GetDependentDto>> GetAllDependentsAsync()
        {
            var dependents = await _dependentRepository.GetAllDependentsAsync();

            return _mapper.Map<IEnumerable<GetDependentDto>>(dependents);
        }
    }
}
