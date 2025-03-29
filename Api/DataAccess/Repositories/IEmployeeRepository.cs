using Api.Models;

namespace Api.DataAccess.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeById(int id);

        Task<IEnumerable<Employee>> GetAllEmployees();
    }
}