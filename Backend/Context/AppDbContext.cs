using Microsoft.EntityFrameworkCore;
using Backend.Model;

namespace Backend.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Videos> Videos { get; set; }
        public DbSet<Users> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}