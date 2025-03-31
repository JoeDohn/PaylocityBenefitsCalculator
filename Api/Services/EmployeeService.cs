using Api.DataAccess.Repositories;
using Api.Dtos.Employee;
using AutoMapper;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPaycheckCalculator _paycheckCalculator;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IPaycheckCalculator paycheckCalculator, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _paycheckCalculator = paycheckCalculator;
            _mapper = mapper;
        }

        public async Task<GetEmployeeDto> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            return _mapper.Map<GetEmployeeDto>(employee);
        }

        public async Task<IEnumerable<GetEmployeeDto>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();

            return _mapper.Map<IEnumerable<GetEmployeeDto>>(employees);
        }

        public async Task<GetPaycheckDto> CalculatePaycheck(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            return _paycheckCalculator.CalculatePaycheck(employee);
        }
    }
}
