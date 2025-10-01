using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.Models;

namespace PlaygroundArenaApp.Infrastructure.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }
        

        public DbSet<Employee> Employees { get; set; }
    }
}
