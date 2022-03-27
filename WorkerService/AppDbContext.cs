using WorkerService.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkerService
{
    public class AppDbContext : DbContext
    {
        public DbSet<Visit> Visits { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}