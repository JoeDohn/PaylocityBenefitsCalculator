using Api.DbContext;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
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
            return await _context.Employees.FindAsync(id);
        }

        // TODO: add pagination
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.Include(x => x.Dependents).ToListAsync();
        }
    }
}
