using Api.DataAccess.Repositories;
using Api.Dtos.Dependent;
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

        public async Task<IEnumerable<GetDependentDto>> GetAllDependents()
        {
            var dependents = await _dependentRepository.GetAllDependents();

            return _mapper.Map<IEnumerable<GetDependentDto>>(dependents);
        }
    }
}
