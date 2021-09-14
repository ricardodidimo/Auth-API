using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> users { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Link> links { get; set; }
        public DbSet<Artist> artists { get; set; }
        
    }
}