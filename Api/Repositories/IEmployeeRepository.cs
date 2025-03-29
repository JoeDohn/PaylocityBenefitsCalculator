using Api.Models;

namespace Api.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeById(int id);

        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    }
}