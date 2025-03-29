using Api.Dtos.Employee;
using Api.Repositories;
using AutoMapper;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<GetEmployeeDto> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            return _mapper.Map<GetEmployeeDto>(employee);
        }

        public async Task<IEnumerable<GetEmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            return _mapper.Map<IEnumerable<GetEmployeeDto>>(employees);
        }
    }
}
