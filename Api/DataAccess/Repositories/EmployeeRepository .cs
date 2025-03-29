using Api.DataAccess.Db;
using Api.Exceptions;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await _context.Employees.Include(x => x.Dependents).FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
            {
                throw new EntityNotFoundException($"Employee with id '{id}' was not found.");
            }

            return employee;
        }

        // TODO: add pagination
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _context.Employees.Include(x => x.Dependents).ToListAsync();
        }
    }
}
