using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class AppDbContext : DbContext
    {
        public DbSet<Visit> Visits { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}