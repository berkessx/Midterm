
using Microsoft.EntityFrameworkCore;
using Midterm.Models;

namespace Midterm.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usage> Usages { get; set; }
    }
}
