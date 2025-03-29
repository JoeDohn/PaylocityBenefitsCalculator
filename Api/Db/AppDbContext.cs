using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.DbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Dependent> Dependents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between Employee and Dependent
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Dependents)
                .WithOne()
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if employee is removed
        }
    }
}