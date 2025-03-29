using Api.DataAccess.Db;
using Api.Exceptions;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DataAccess.Repositories
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
            var depenedent = await _context.Dependents.FirstOrDefaultAsync(x => x.Id == id);

            if (depenedent == null)
            {
                throw new EntityNotFoundException($"Dependent with id '{id}' was not found.");
            }

            return depenedent;
        }

        // TODO: add pagination
        public async Task<IEnumerable<Dependent>> GetAllDependents()
        {
            return await _context.Dependents.ToListAsync();
        }
    }
}
