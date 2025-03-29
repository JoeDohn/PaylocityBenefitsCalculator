using Api.DbContext;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class DependentRepository : IDependentRepository
    {
        private readonly AppDbContext _context;

        public DependentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dependent> GetDependentById(int id)
        {
            return await _context.Dependents.FindAsync(id);
        }

        // TODO: add pagination
        public async Task<IEnumerable<Dependent>> GetAllDependentsAsync()
        {
            return await _context.Dependents.ToListAsync();
        }
    }
}
